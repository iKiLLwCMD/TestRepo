Imports PagedList
Imports Facebook
Imports Google.Apis.Authentication
Imports Google.Apis.Authentication.OAuth2
Imports Google.Apis.Authentication.OAuth2.DotNetOpenAuth
Imports Google.Apis.Util
Imports Google.GData.Client
Imports Google.GData.Extensions
Imports Google.GData.YouTube
Imports Google.YouTube
Imports Google.GData.Extensions.MediaRss
Imports Google.Apis.Drive.v2
Imports Google.Apis.Drive.v2.Data
Imports Google.Apis
Imports System
Imports System.Diagnostics
Imports System.Configuration
Imports DotNetOpenAuth.OAuth2
Imports Google.Apis.Services
Imports System.Net.Mail
Imports System.IO

Public Class HomeController
    Inherits System.Web.Mvc.Controller

    Private db As New VCVOnlineEntities
    Public Shared AUTH_Code As String
    Private Shared oProvider As NativeApplicationClient
    Private Shared oAuth As OAuth2Authenticator(Of NativeApplicationClient)
    Private Shared oDriveService As DriveService
    '
    ' GET: /Home
    <RequireHttp> _
    Function Index(page As System.Nullable(Of Integer)) As ActionResult

        Dim userid As String = HttpContext.User.Identity.Name
        Dim user = db.Users.Find(HttpContext.User.Identity.Name)
        If Request.IsAuthenticated = True Then
            If user.UserType = "Demo" Or user.UserType = "Employee" Then
                Dim vcvs = db.VCVs.Where(Function(v) v.UserId = HttpContext.User.Identity.Name And v.CurrentStep >= 3 And v.Hidden = False).OrderByDescending(Function(v) v.VCVId)
                Dim pageSize As Integer = 5
                Dim pageNumber As Integer = (If(page, 1))
                'Return View(vcvs.ToList)view
                'vcvs.OrderBy(Function(v) v.VCVId)
                Session.Remove("NewVCVId")
                Return View(vcvs.ToPagedList(pageNumber, pageSize))

            Else
                Return ListingsView("Current", page)
            End If
        Else
            Return View()
        End If



    End Function
    <HttpPost> _
    Public Function Index(col As FormCollection, page As System.Nullable(Of Integer)) As ActionResult
        Dim currentUser As User = db.Users.Find(User.Identity.Name)
        If currentUser.UserType = "Employer" Or currentUser.UserType = "Recruitment" Then
            Return ListingsView(col("Status"), page)
        Else
            Return View()
        End If
    End Function

    Public Function ViewVCV(id As String, page As System.Nullable(Of Integer)) As ActionResult
        Try
            Dim vcvShared = db.VCVShareds.FirstOrDefault(Function(vcvs) vcvs.PublicId = id)

            If vcvShared IsNot Nothing Then
                Session("PublicId") = Nothing
                Session("VCVSharedPublicId") = vcvShared.PublicId
                Session("Attachment1Id") = vcvShared.Attachment1Id
                Session("Attachment2Id") = vcvShared.Attachment2Id
                Session("Attachment3Id") = vcvShared.Attachment3Id
                ViewBag.PublicId = id
                Return View(vcvShared)
            End If
            Return RedirectToAction("PageNotFound")
        Catch ex As Exception
            Return PageNotFound()
        End Try
    End Function

    Public Function PageNotFound() As ActionResult
        Return View()
    End Function

    Public Sub ChangeFulfilledStatus(ListingId As Integer, Status As Boolean)
        Dim listing As Listing = db.Listings.Find(ListingId)
        listing.Fulfilled = Status
        db.SaveChanges()
    End Sub

    Public Function ListingsView(status As String, page As System.Nullable(Of Integer)) As ActionResult
        Dim userid As String = User.Identity.Name
        Dim usersListings As IOrderedQueryable(Of Listing)
        If status = "Expired" Then
            usersListings = db.Listings.Where(Function(l) l.UserId = userid And l.Completed = True And (l.ExpiryDate < Now() Or l.Fulfilled = True)).OrderByDescending(Function(l) l.ListedDate)
        Else
            usersListings = db.Listings.Where(Function(l) l.UserId = userid And l.Completed = True And l.Fulfilled = False And l.ExpiryDate > Now()).OrderByDescending(Function(l) l.ListedDate)
        End If
        Dim pageSize As Integer = 5
        Dim pageNumber As Integer = (If(page, 1))

        Dim lst As New List(Of String)
        lst.Add("Current")
        lst.Add("Expired")
        Dim selectList As New SelectList(lst, status)
        ViewBag.selectList = selectList

        Dim allUsersApplicants = db.ListingApplications.Where(Function(la) la.Listing.UserId = userid).OrderBy(Function(la) la.ApplicationId)
        ViewBag.allUsersApplicants = allUsersApplicants

        Return View(usersListings.ToPagedList(pageNumber, pageSize))
    End Function

    Function ShortList(Optional id As String = "") As ActionResult
        Return RedirectToAction("EmployerListing", "Listing", New With {.controller = "Home", .id = id})
    End Function

    Public Sub UpdateApplicantWatchedStatus(id As Integer)
        Dim watchedListingApplication = db.ListingApplications.Find(id)
        watchedListingApplication.IsViewed = True
        db.SaveChanges()
    End Sub

    Public Function ReplacePlaceholders(body As String, la As ListingApplication) As String
        ReplacePlaceholders = body.Replace("[CompanyName]", la.Listing.Company)
        Dim baseUrl As String = Request.Url.GetLeftPart(UriPartial.Authority)
        ReplacePlaceholders = ReplacePlaceholders.Replace("[CompanyLogo]", "<center><img src=""" & baseUrl & "/UploadedListings/" & la.Listing.Image1 & """/></center>")
        ReplacePlaceholders = ReplacePlaceholders.Replace("[ApplicantName]", la.User.UserName)
        ReplacePlaceholders = ReplacePlaceholders.Replace("[ListingTitle]", la.Listing.ListingTitle)
        ReplacePlaceholders = ReplacePlaceholders.Replace("[ListingId]", la.Listing.ListingId)
        ReplacePlaceholders = "<html><body>" & ReplacePlaceholders & "</body></html>"
    End Function

    Public Sub MakeApplicationDecision(ApplicationStatusId As Integer, ApplicationId As Integer)
        Dim la As ListingApplication = db.ListingApplications.Find(ApplicationId)
        Dim l As Listing = la.Listing
        Dim appStatus As ApplicationStatus = db.ApplicationStatus1.Find(ApplicationStatusId)
        Dim et As EmailTemplateDetail
        If appStatus.Status = "Declined" Then
            et = db.EmailTemplateDetails.FirstOrDefault(Function(etd) etd.EmailTemplateId = l.EmailTemplateId And etd.EmailTypeId = 3)
        Else
            et = db.EmailTemplateDetails.FirstOrDefault(Function(etd) etd.EmailTemplateId = l.EmailTemplateId And etd.EmailTypeId = 4)
        End If
        SendEmail(la.User.UserEmail, ReplacePlaceholders(et.EmailTemplateDetailsContent, la), l.ListingTitle & " - #" & l.ListingId)

        la.ApplicationStatusId = ApplicationStatusId
        db.SaveChanges()
    End Sub

    Public Sub SendEmail(toEmail As String, body As String, subject As String)
        Dim mail As New MailMessage
        mail.To.Add(toEmail)
        mail.From = New MailAddress("no-reply@myvcv.co.nz")
        mail.Subject = subject
        mail.IsBodyHtml = True
        mail.Body = body

        Dim smtp As New SmtpClient("mail.licensing.my-vcv.com")
        smtp.Port = 49
        smtp.Timeout = 60000
        smtp.Credentials = New System.Net.NetworkCredential("vcv@licensing.my-vcv.com", "Count548")
        smtp.Send(mail)
    End Sub

    Public Sub FilterByApplicationStatus(status As Integer)

    End Sub

    Function online_cv() As ActionResult

        Return View()

    End Function



    Function technology_for_learning() As ActionResult

        Return View(New Global.MvcMyVCV.ContactViewModel)

    End Function

    <HttpPost> _
    Function technology_for_learning(contactVM As ContactViewModel)

        If (ModelState.IsValid = False) Then

            Return View(contactVM)
        End If

        Dim mail As New MailMessage

        'mail.To.Add("support@myvcv.co.nz")
        mail.To.Add("tcharleston@virtualtechnologies.co.nz")
        mail.CC.Add("justine@virtualtechnologies.co.nz")
        mail.CC.Add("tim@virtualtechnologies.co.nz")
        mail.From = New MailAddress(contactVM.From)
        mail.Subject = contactVM.Subject

        mail.Body = "<html><body><h3>MyChatPak Contact Form</h3><br /><b>Message From:</b><br />" & contactVM.sName & "<br /><b>Message:</b><br />" & contactVM.Message & "<br /><br /></body></html>"
        mail.IsBodyHtml = True



        Dim smtp As New SmtpClient("mail.licensing.my-vcv.com")
        smtp.Port = 49
        smtp.Timeout = 60000
        'smtp.Credentials = New NetworkCredential("username", "password")
        smtp.Credentials = New System.Net.NetworkCredential("vcv@licensing.my-vcv.com", "Count548")
        'smtp.EnableSsl = True

        smtp.Send(mail)


        Return RedirectToAction("ContactConfirm")

    End Function


    Function Teachers_Parents_Students() As ActionResult

        Return View()

    End Function





    Function Edit(id As Integer) As ActionResult

        Dim vcv As VCV = db.VCVs.Find(id)
        Dim teacher As String = ""

        If vcv.UserId = HttpContext.User.Identity.Name Then
            Session("LoadStep") = vcv.CurrentStep + 1
            Session("NewVCVName") = vcv.VCVName
            Session("NewVCVId") = vcv.VCVId
            Return (RedirectToAction("Index", "Create"))
        Else
            Return HttpNotFound()
        End If


    End Function

    Function Share(id As Integer) As ActionResult

        Dim vcv As VCV = db.VCVs.Find(id)

        If vcv.UserId = HttpContext.User.Identity.Name Then
            Return (View())
        Else
            Return HttpNotFound()
        End If



    End Function

    Public Function Remove(id As Integer)
        Dim vcv As VCV = db.VCVs.Find(id)

        If vcv.UserId = HttpContext.User.Identity.Name Then
            vcv.Hidden = True
            db.SaveChanges()
        Else

            Return HttpNotFound()
        End If

        Return True
    End Function

    <HttpPost> _
    <AsyncTimeout(300000)> _
    Public Function FacebookShare(FacebookId As String, Token As String, id As String)
        HttpContext.Server.ScriptTimeout = 600

        Dim accessToken = Token.ToString().Trim
        Dim client = New FacebookClient(accessToken)
        Dim vcvid As Integer = CInt(id.Trim)
        Dim vcv As VCV = db.VCVs.Find(vcvid)

        If vcv.UserId = HttpContext.User.Identity.Name Then
        Else
            Return HttpNotFound()
        End If

        Dim videopath = "C:\inetpub\wwwroot\MvcMyVCV\MvcMyVCV\Streams\" & vcv.VideoName & ".flv"


        Dim parameters As Object = New Dynamic.ExpandoObject()
        parameters.source = New FacebookMediaObject() With {.ContentType = "video/3gpp", .FileName = "video.3gp"}.SetValue(System.IO.File.ReadAllBytes(videopath))
        parameters.title = "My Chat Pak Video"
        parameters.description = StripTagsChar(vcv.Summary.SummaryContent.Replace("&nbsp;", ""))

        client.PostTaskAsync("/me/videos", parameters).Wait()



        Return True
    End Function


    <HttpPost> _
   <AsyncTimeout(300000)> _
    Public Function youtubeShare(Token As String, id As String, title As String, description As String, category As String, type As Integer)
        HttpContext.Server.ScriptTimeout = 600

        Dim accessToken = Token.ToString().Trim

        Dim vcvid As Integer = CInt(id.Trim)
        Dim vcv As VCV = db.VCVs.Find(vcvid)

        If vcv.UserId = HttpContext.User.Identity.Name Then
        Else
            Return False
        End If
        Dim settings = New YouTubeRequestSettings("MyChatPak", "AI39si4U_2snlM7g0ctg4kjfBTW1mi1uqZz5RUqcC1KptUd46HUomUJFlhVK46FfpGvH_MXkhTOSHwRfabqdMo3ejH5Dql78Uw", accessToken) With {.Timeout = -1}
        settings.Timeout = 1000000000

        Dim requestyoutube As New YouTubeRequest(settings)

        Dim newVideo As New Google.YouTube.Video

        newVideo.Title = title
        newVideo.Tags.Add(New MediaCategory(category, YouTubeNameTable.CategorySchema))
        newVideo.Keywords = "MyChatPack, Chatting, Classroom tool"
        newVideo.Description = description
        newVideo.Tags.Add(New MediaCategory("MyChatPack", YouTubeNameTable.DeveloperTagSchema))

        newVideo.YouTubeEntry.MediaSource = New MediaFileSource("C:\inetpub\wwwroot\MvcMyVCV\MvcMyVCV\Streams\" & vcv.VideoName & ".flv", "video/x-flv")
        If type = 1 Then
            newVideo.Private = False
        Else
            newVideo.Private = True
        End If

        Try
            'Dim createdVideo As Google.YouTube.Video = requestyoutube.Upload(newVideo)

            Dim task = requestyoutube.Upload(newVideo)

            Return True

        Catch ex As Exception

            Return False


        End Try

    End Function

    '  <HttpPost> _
    '<AsyncTimeout(300000)> _
    '  Public Function driveShare(Token As String, id As String)
    '      HttpContext.Server.ScriptTimeout = 600

    '      Dim accessToken = Token.ToString().Trim

    '      Dim vcvid As Integer = CInt(id.Trim)
    '      Dim vcv As VCV = db.VCVs.Find(vcvid)
    '      Dim teacher As String
    '      Try
    '          teacher = vcv.UserByVCV.Class.User.UserId
    '      Catch ex As Exception
    '          teacher = ""
    '      End Try
    '      If vcv.UserId = HttpContext.User.Identity.Name Or teacher = HttpContext.User.Identity.Name Then
    '      Else
    '          Return False
    '      End If






    '      AUTH_Code = accessToken



    '      Try
    '          oProvider = New NativeApplicationClient(GoogleAuthenticationServer.Description, "614827406472-vf02gggv105tspj1pepat7smj2l6aqk6.apps.googleusercontent.com", "5KwRiN1YTFh6ntUq45quJF3x")
    '          'oProvider = New NativeApplicationClient(GoogleAuthenticationServer.Description, "614827406472.apps.googleusercontent.com", "z00MjzqMhj6-CrSt0uyq5cTP")


    '          'Create OAuth2 autentication using the previously generated Auth Key
    '          oAuth = New OAuth2Authenticator(Of NativeApplicationClient)(oProvider, AddressOf GetAuthorization)

    '          'Initialize the DriveService Object
    '          Dim oBasicInit As New BaseClientService.Initializer()
    '          oBasicInit.Authenticator = oAuth
    '          oDriveService = New DriveService(oBasicInit)

    '          Dim body As New File()
    '          'body.Title = "MyChatPak" & Now.ToString("YYYY-MM-DD HHmmss")
    '          body.Title = "Test"
    '          body.Description = "A test"
    '          'body.MimeType = "video/x-flv"
    '          body.MimeType = "text/plain"

    '          'Dim byteArray As Byte() = System.IO.File.ReadAllBytes("C:\StreamRecordingApp\streams\" & vcv.VCVName & ".flv")
    '          Dim byteArray As Byte() = System.IO.File.ReadAllBytes("C:\StreamRecordingApp\streams\test.txt")
    '          Dim stream As New System.IO.MemoryStream(byteArray)

    '          Dim request As FilesResource.InsertMediaUpload = oDriveService.Files.Insert(body, stream, "text/plain")

    '          request.Upload()

    '          Dim file As File = request.ResponseBody
    '          Console.WriteLine("File id: " + file.Id)

    '          Return True

    '      Catch ex As Exception

    '          Return False


    '      End Try

    '  End Function

    Private Shared Function GetAuthorization(arg As NativeApplicationClient) As IAuthorizationState


        'Auth Scope (in this case Google Drive Read Only)
        Dim AuthState As IAuthorizationState = New AuthorizationState({DriveService.Scopes.Drive.GetStringValue()})
        'AuthState.Callback = New Uri(NativeApplicationClient.OutOfBandCallbackUrl)
        'AuthState.Callback = New Uri("postmessage")
        AuthState.Callback = New Uri("")
        'AuthState = arg.ProcessUserAuthorization(AUTH_Code, AuthState)
        Return arg.ProcessUserAuthorization(AUTH_Code, AuthState)

        'Return AuthState
    End Function

    Public Shared Function StripTagsChar(source As String) As String
        Dim array As Char() = New Char(source.Length - 1) {}
        Dim arrayIndex As Integer = 0
        Dim inside As Boolean = False

        For i As Integer = 0 To source.Length - 1
            Dim [let] As Char = source(i)
            If [let] = "<"c Then
                inside = True
                Continue For
            End If
            If [let] = ">"c Then
                inside = False
                Continue For
            End If
            If Not inside Then
                array(arrayIndex) = [let]
                arrayIndex += 1
            End If
        Next
        Return New String(array, 0, arrayIndex)
    End Function


    <HttpPost> _
    Public Function blogger(id As String) As JsonResult
        Dim vcvid As Integer = CInt(id.Trim)

        Dim vcv As VCV = db.VCVs.Find(vcvid)

        If vcv.UserId = HttpContext.User.Identity.Name Then
            Return Json(New With {.success = True, .VideoName = vcv.VideoName, .Summary = vcv.Summary.SummaryContent, .SummaryStrip = StripTagsChar(vcv.Summary.SummaryContent.Replace("&nbsp;", ""))})
        Else
            Return Json(New With {.success = False})
        End If





    End Function

    Public Function Create() As ActionResult

        Session.Remove("LoadStep")
        Session.Remove("NewVCVName")
        Session.Remove("NewVCVId")

        Return RedirectToAction("Index", "Create")
    End Function


    <HttpPost> _
    Public Function EmailSignup(sEmail As String)
        Try
            Dim addess As MailAddress = New MailAddress(sEmail)
            Dim Email As New EmailSignup
            Email.Email = sEmail
            Email.TimeStamp = Now()


            db.EmailSignups.Add(Email)
            db.SaveChanges()

            Return True
        Catch ex As Exception

            Return False

        End Try






    End Function






End Class