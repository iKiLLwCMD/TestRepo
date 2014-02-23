Imports System.IO
Imports MvcMyVCV
Imports System.Web.Hosting
Imports System.Net.Mail


Public Class CreateController
    Inherits System.Web.Mvc.Controller

    Private db As New VCVOnlineEntities


    <Authorize> _
    Function Index() As ActionResult
        ViewData("Message") = "Create a VCV in 6 Simple Steps"

        If Session("LoadStep") Is Nothing And Session("NewVCVId") Is Nothing Then
            'Create empty vcv
            Dim newvcv As New VCV
            newvcv.UserId = HttpContext.User.Identity.Name
            newvcv.Timestamp = Now()
            newvcv.VCVName = System.Guid.NewGuid().ToString()
            newvcv.Length = "Under"
            Session("NewVCVName") = newvcv.VCVName
            'newvcv.Approved = vbNull
            db.VCVs.Add(newvcv)
            db.SaveChanges()
            Session("NewVCVId") = newvcv.VCVId
            Return View()
        Else
            Dim vcv As VCV = db.VCVs.Find(Session("NewVCVId"))
       
            If vcv.UserId = HttpContext.User.Identity.Name Then
                Session("LoadStep") = vcv.CurrentStep + 1
                Session("NewVCVName") = vcv.VCVName
                Session("NewVCVId") = vcv.VCVId
                If vcv.ScriptId Is Nothing = False Then
                    Session("ScriptText") = vcv.Script.ScriptContent
                End If

                Return View()
            Else
                Return HttpNotFound()
            End If

        End If


    End Function


    Public Function Step1() As ActionResult



        Dim vcv As VCV = db.VCVs.Find(Session("NewVCVId"))
        Dim teacher As String
       
        If IsNothing(vcv) Then
            Return HttpNotFound()
        End If

        If vcv.UserId = HttpContext.User.Identity.Name Or teacher = HttpContext.User.Identity.Name Then
        Else
            Return HttpNotFound()
        End If
       

        RemoveToken(Session("NewVCVId"))

        Return PartialView("_PartialStep1")
    End Function

    Public Function Step2() As ActionResult
        Dim vcv As VCV = db.VCVs.Find(Session("NewVCVId"))
        Dim query
        Dim currentscriptid As Integer
        Try
            currentscriptid = vcv.ScriptId
        Catch ex As Exception

        End Try

        If vcv.ScriptId Is Nothing = False Then
            query = From Existing In db.Scripts
            Where
            Existing.UserId = HttpContext.User.Identity.Name And Existing.Template = False And (Existing.Hidden = False Or Existing.ScriptId = currentscriptid)
            Select Existing.ScriptId, Existing.ScriptName Order By ScriptId Descending
        Else
            query = From Existing In db.Scripts
            Where
            Existing.UserId = HttpContext.User.Identity.Name And Existing.Template = False And Existing.Hidden = False
            Select Existing.ScriptId, Existing.ScriptName Order By ScriptId Descending

        End If



        If IsNothing(vcv) Then
            Return HttpNotFound()
        End If


        If vcv.UserId = HttpContext.User.Identity.Name Then
        Else
            Return HttpNotFound()
        End If

        'Dim selectlists As New SelectList(query, "ScriptId", "ScriptName", vcv.ScriptId)
        Dim selectlists As New SelectList(Query, "ScriptId", "ScriptName")

        ViewBag.selectList = selectlists

        'template selectlist

        Dim queryTemplate = From Existing In db.Scripts
        Where
        Existing.Template = True
        Select Existing.ScriptId, Existing.ScriptName

        Dim TemplateSelectlists As New SelectList(queryTemplate, "ScriptId", "ScriptName")

        ViewBag.selectTemplateList = TemplateSelectlists



        ViewBag.ScriptIdHid = vcv.ScriptId

        Dim videos = (From ExistingVideos In db.VCVs
       Where
       ExistingVideos.UserId = HttpContext.User.Identity.Name And ExistingVideos.CurrentStep >= 3
       Select ExistingVideos.VideoName).Distinct.ToList
        If vcv.VideoName <> "" Then
            If vcv.VideoName <> vcv.VCVName Then
                ViewBag.ExistingVideo = vcv.VideoName
            End If
        End If


        ViewBag.ExistingVideos = videos

        RemoveToken(Session("NewVCVId"))

        Return PartialView("_PartialStep2")

    End Function


    Public Function Step3() As ActionResult

        Dim vcv As VCV = db.VCVs.Find(Session("NewVCVId"))
       
        If IsNothing(vcv) Then
            Return HttpNotFound()
        End If


        If vcv.UserId = HttpContext.User.Identity.Name Then
        Else
            Return HttpNotFound()
        End If

        If VCV.VideoName <> VCV.VCVName Then
            ViewBag.ExistingVideo = True
        End If
        Dim filename = System.Web.Hosting.HostingEnvironment.MapPath(HttpContext.Request.ApplicationPath) & "/Snaps/" & vcv.VCVName & ".jpg"
        If System.IO.File.Exists(filename) = True Then
            ViewBag.Name = "yes"
        Else
            ViewBag.Name = "no"
        End If

        Dim filenameVideo = "C:\Program Files (x86)\Red5\webapps\oflaDemo\streams" & vcv.VCVName & ".flv"
        If System.IO.File.Exists(filename) = True Then
            ViewBag.VideoName = "yes"
        Else
            ViewBag.VideoName = "no"
        End If

        ' set token and expiry
        vcv.Token = Guid.NewGuid.ToString
        vcv.TokenExpiryDate = Now.AddMinutes(30)
        db.SaveChanges()
        ViewBag.Access = vcv.Token


        Return PartialView("_PartialStep3")
    End Function

    Public Sub RemoveToken(vcvid As Integer)
        Dim vcv As VCV = db.VCVs.Find(Session("NewVCVId"))
     
        If IsNothing(vcv) Then
            Exit Sub
        End If
        vcv.Token = Nothing
        vcv.TokenExpiryDate = Nothing
        db.SaveChanges()



    End Sub


    Public Function Step4() As ActionResult
        Dim vcv As VCV = db.VCVs.Find(Session("NewVCVId"))
        Dim currentsummaryid As Integer
        Try
            currentsummaryid = vcv.SummaryId
        Catch ex As Exception

        End Try
        Dim query
        If vcv.ScriptId Is Nothing = False Then
            query = From Existing In db.Summaries
                   Where
                   Existing.UserId = HttpContext.User.Identity.Name And (Existing.Hidden = False Or Existing.SummaryId = currentsummaryid)
                   Select Existing.SummaryId, Existing.SummaryName
        Else
            query = From Existing In db.Summaries
                  Where
                  Existing.UserId = HttpContext.User.Identity.Name And Existing.Hidden = False
                  Select Existing.SummaryId, Existing.SummaryName

        End If



       
        If IsNothing(vcv) Then
            Return HttpNotFound()
        End If


        If vcv.UserId = HttpContext.User.Identity.Name Then
        Else
            Return HttpNotFound()
        End If

        Dim selectlists As New SelectList(query, "SummaryId", "SummaryName")

        ViewBag.selectList = selectlists

        ViewBag.SummaryIdHid = vcv.SummaryId


        'RemoveToken(Session("NewVCVId"))


        Return PartialView("_PartialStep4")
    End Function
    Public Function Step5() As ActionResult

        Dim vcv As VCV = db.VCVs.Find(Session("NewVCVId"))
        Dim teacher As String
       
        If IsNothing(vcv) Then
            Return HttpNotFound()
        End If


        If vcv.UserId = HttpContext.User.Identity.Name Then
        Else
            Return HttpNotFound()
        End If
        If vcv.Attachment1Id <> "" Then
            ViewBag.Auto = vcv.Attachment1Name.ToString
            ViewBag.AutoName = vcv.Attachment1Id.ToString
        Else
            ViewBag.Auto = ""
            ViewBag.AutoName = ""
        End If

        If vcv.Attachment2Id <> "" Then
            ViewBag.Other1 = vcv.Attachment2Name.ToString
            ViewBag.Other1Name = vcv.Attachment2Id.ToString
        Else
            ViewBag.Other1 = ""
            ViewBag.Other1Name = ""
        End If

        If vcv.Attachment3Id <> "" Then
            ViewBag.Other2 = vcv.Attachment3Name.ToString
            ViewBag.Other2Name = vcv.Attachment3Id.ToString
        Else
            ViewBag.Other2 = ""
            ViewBag.Other2Name = ""
        End If
        RemoveToken(Session("NewVCVId"))

        Return PartialView("_PartialStep5")
    End Function

    Public Function Step6() As ActionResult
        Dim vcv As VCV = db.VCVs.Find(Session("NewVCVId"))

        ViewBag.CurrentUserId = vcv.UserId
        If IsNothing(vcv) Then
            Return HttpNotFound()
        End If



        If vcv.UserId = HttpContext.User.Identity.Name Then
        Else
            Return HttpNotFound()
        End If

        Dim onlineProfile As VCVShared = db.VCVShareds.FirstOrDefault(Function(v) v.UserId = vcv.UserId And v.IsEmail = False)
        Dim onlineProfileLink As String = ""
        If onlineProfile IsNot Nothing Then
            onlineProfileLink = String.Format("{0}:\\{1}{2}", Request.Url.Scheme, Request.Url.Authority, "\ViewVCV\" & onlineProfile.PublicId)
            ViewBag.onlineProfileLink = onlineProfileLink
        End If



        RemoveToken(Session("NewVCVId"))

        Return PartialView("_PartialStep6")
    End Function
    '<HttpPost()> _
    'Public Function Step6(col As FormCollection) As ActionResult
    '    'Dim converter As New Converter("./ffmpeg/ffmpeg.exe")
    '    'Dim oo As OutputPackage = converter.ConvertToWMV("")
    '    'Dim outStream As FileStream = System.IO.File.OpenWrite("./newVideo.flv")
    '    'oo.VideoStream.WriteTo(outStream)
    '    'outStream.Flush()
    '    'outStream.Close()
    '    'oo.PreviewImage.Save("./thumbnail.jpg")



    '    Return Content("Email Being sent")



    'End Function

    '<AcceptVerbs("POST")> _

    <HttpPost, ValidateInput(False)> _
    Public Function tabval(Current As String, Selected As String, col As String, editor As String, files As IEnumerable(Of HttpPostedFileBase), Scripttype As String, ScriptId As String, ScriptName As String, SummaryName As String, SummaryId As String, SummaryType As String)

        Dim allow As Boolean = True
        Dim qscoll As NameValueCollection = HttpUtility.ParseQueryString(col.Trim)



        'check if edit
        If Session("LoadStep") Is Nothing = False Then
            Session.Remove("LoadStep")
            Return True
        End If





        If Current = 0 Then ' save assignment
            Dim vcv As VCV = db.VCVs.Find(Session("NewVCVId"))

            If IsNothing(vcv) Then
                Return HttpNotFound()
            End If
           

            If vcv.UserId = HttpContext.User.Identity.Name Then
            Else
                Return HttpNotFound()
            End If
            'If qscoll("AssignmentId") <> "" Then
            '    vcv.AssignmentId = qscoll("AssignmentId")
            'Else
            '    vcv.AssignmentId = Nothing
            'End If
            If Current > vcv.CurrentStep Then
                vcv.CurrentStep = Current
            End If
            'vcv.Approved = vbNull
            db.SaveChanges()

        ElseIf Current = 1 Then ' script

            If Scripttype = "New" Then
                If ScriptName <> "" Then


                    Dim script As New Script
                    script.ScriptContent = editor
                    script.ScriptName = ScriptName
                    script.TimeStamp = Now()
                    script.UserId = HttpContext.User.Identity.Name
                    script.Template = False
                    db.Scripts.Add(script)
                    db.SaveChanges()

                    Dim scriptidNew As Integer = script.ScriptId

                    Dim vcv As VCV = db.VCVs.Find(Session("NewVCVId"))

                    If IsNothing(vcv) Then
                        Return HttpNotFound()
                    End If
                



                    If vcv.UserId = HttpContext.User.Identity.Name Then
                    Else
                        Return HttpNotFound()
                    End If
                    vcv.ScriptId = scriptidNew
                    If Current > vcv.CurrentStep Then
                        vcv.CurrentStep = Current
                    End If
                    vcv.VideoName = vcv.VCVName
                    'vcv.Approved = vbNull
                    db.SaveChanges()

                    Session("ScriptText") = editor

                Else
                    Return False
                End If
            ElseIf Scripttype = "Existing" Then
                Dim vcv As VCV = db.VCVs.Find(Session("NewVCVId"))

                If IsNothing(vcv) Then
                    Return HttpNotFound()
                End If
              



                If vcv.UserId = HttpContext.User.Identity.Name Then
                Else
                    Return HttpNotFound()
                End If
                If Current > vcv.CurrentStep Then
                    vcv.CurrentStep = Current
                End If
                vcv.VideoName = vcv.VCVName
                'vcv.Approved = vbNull
                vcv.ScriptId = ScriptId
                db.SaveChanges()

                Dim script As Script = db.Scripts.Find(vcv.ScriptId)

                Session("ScriptText") = script.ScriptContent

            ElseIf Scripttype = "Template" Then
                Return False
            ElseIf Scripttype = "ExistingVideo" Then
                Dim vcv As VCV = db.VCVs.Find(Session("NewVCVId"))

                If IsNothing(vcv) Then
                    Return HttpNotFound()
                End If


                If vcv.UserId = HttpContext.User.Identity.Name Then
                Else
                    Return HttpNotFound()
                End If
                If vcv.VideoName Is DBNull.Value = False Then
                    If vcv.VideoName <> "" Then
                        If vcv.VideoName <> vcv.VCVName Then
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return False

                    End If
                Else
                    Return False
                End If


            End If

        ElseIf Current = 3 Then
            If SummaryType = "New" Then
                If SummaryName <> "" Then


                    Dim summary As New Summary
                    summary.SummaryContent = editor
                    summary.SummaryName = SummaryName
                    summary.TimeStamp = Now()
                    summary.UserId = HttpContext.User.Identity.Name

                    db.Summaries.Add(summary)
                    db.SaveChanges()

                    Dim summaryidNew As Integer = summary.SummaryId

                    Dim vcv As VCV = db.VCVs.Find(Session("NewVCVId"))

                    If IsNothing(vcv) Then
                        Return HttpNotFound()
                    End If
                    



                    If vcv.UserId = HttpContext.User.Identity.Name Then
                    Else
                        Return HttpNotFound()
                    End If
                    If Current > vcv.CurrentStep Then
                        vcv.CurrentStep = Current
                    End If
                    'vcv.Approved = vbNull
                    vcv.SummaryId = summaryidNew
                    db.SaveChanges()

                    Session("SummaryText") = editor

                Else
                    Return False
                End If
            ElseIf SummaryType = "Existing" Then
                Dim vcv As VCV = db.VCVs.Find(Session("NewVCVId"))

                If IsNothing(vcv) Then
                    Return HttpNotFound()
                End If

                If vcv.UserId = HttpContext.User.Identity.Name Then
                Else
                    Return HttpNotFound()
                End If
                If Current > vcv.CurrentStep Then
                    vcv.CurrentStep = Current
                End If
                'vcv.Approved = vbNull
                vcv.SummaryId = SummaryId
                db.SaveChanges()

                'Dim script As Script = db.Scripts.Find(vcv.ScriptId)

                'Session("ScriptText") = script.ScriptContent


            End If




        ElseIf Current = 4 Then
            'upload 

            Dim vcv As VCV = db.VCVs.Find(Session("NewVCVId"))

            If IsNothing(vcv) Then
                Return HttpNotFound()
            End If
           



            If vcv.UserId = HttpContext.User.Identity.Name Then
            Else
                Return HttpNotFound()
            End If
            If Current > vcv.CurrentStep Then
                vcv.CurrentStep = Current
            End If
            'vcv.Approved = vbNull
            db.SaveChanges()



        End If


        'check if u can change to selected tab
        Dim vcv2 As VCV = db.VCVs.Find(Session("NewVCVId"))

        Dim currentTab As Integer = vcv2.CurrentStep

        If (Selected - currentTab) <= 1 Then
            allow = True
        Else
            allow = False
        End If

        If allow = True Then
            Return True
        Else
            Return False
        End If




    End Function

    Public Function CreateProfile(VCVId As String, CurrentUserId As String)
        If VCVId Is Nothing = False And CurrentUserId Is Nothing = False Then
            Dim profileCount = db.VCVShareds.Where(Function(v) v.UserId = CurrentUserId And v.IsEmail = False).Count()
            Dim vcvShared As VCVShared = Nothing
            If profileCount = 1 Then
                vcvShared = db.VCVShareds.FirstOrDefault(Function(v) v.UserId = CurrentUserId And v.IsEmail = False)
                Dim vcv = db.VCVs.Find(CInt(VCVId))
                Dim updatedVCVShared = CreateOrUpdateVCVShared(vcv, False, vcvShared)
                db.SaveChanges()
            Else
                Dim vcv = db.VCVs.Find(CInt(VCVId))
                vcvShared = CreateOrUpdateVCVShared(VCV, False)
                db.VCVShareds.Add(vcvShared)
                db.SaveChanges()
            End If
            
            Return vcvShared.PublicId
        Else
            Return False
        End If
    End Function

    Public Function CreateVCVEmail(VCVId As String, CurrentUserId As String) As VCVShared

        Dim vcv = db.VCVs.Find(CInt(VCVId))
        If vcv Is Nothing = False Then
            Dim vcvShared = CreateOrUpdateVCVShared(vcv, True)
            db.VCVShareds.Add(vcvShared)
            db.SaveChanges()
            Return vcvShared
        Else
            Return Nothing
        End If
    End Function

    Public Function CreateOrUpdateVCVShared(vcv As VCV, IsEmail As Boolean, Optional ByRef vcvShared As VCVShared = Nothing) As VCVShared
        If vcvShared Is Nothing Then
            vcvShared = New VCVShared()
            vcvShared.PublicId = GetPublicId()
        End If

        With vcvShared
            .UserId = vcv.UserId
            .VideoName = vcv.VideoName
            .Attachment1Id = vcv.Attachment1Id
            .Attachment1Name = vcv.Attachment1Name
            .Attachment2Id = vcv.Attachment2Id
            .Attachment2Name = vcv.Attachment2Name
            .Attachment3Id = vcv.Attachment3Id
            .Attachment3Name = vcv.Attachment3Name
            .TimeStamp = Now
            .SummaryContent = vcv.Summary.SummaryContent
            .IsEmail = IsEmail
        End With
        Return vcvShared
    End Function

    Public Function GetPublicId() As String
        Dim PublicId As String = ""
        Do
            PublicId = Guid.NewGuid().ToString.Substring(0, 7)
        Loop Until db.VCVShareds.Where(Function(vcvs) vcvs.PublicId = PublicId).Count() = 0
        Return PublicId
    End Function

    Public Sub SendEmail(CurrentUserId As String, toEmail As String, VCVId As String)
        Dim vcvShared = CreateVCVEmail(VCVId, CurrentUserId)
        Dim user = db.Users.Find(CurrentUserId)
        Dim vcv = db.VCVShareds.Find(vcvShared.VCVSharedId)

        Dim mail As New MailMessage
        mail.To.Add(toEmail)
        mail.From = New MailAddress(user.UserEmail)
        mail.Subject = "MyVCV Job Application - " & user.UserName
        mail.IsBodyHtml = True
        'Replace content in body!
        Dim body As String = db.Templates.Find(2).TemplateContent
        body = body.Replace("<Insert Name HERE>", user.UserName)
        body = body.Replace("<Insert Link HERE>", "<a href=""" & Request.Url.Scheme & ":\\" & Request.Url.Authority & "\Home\ViewVCV\" & vcv.PublicId & """>")
        body = body.Replace("<Insert Image HERE>", Request.Url.Scheme & ":\\" & Request.Url.Authority & "\Snaps\" & vcv.VideoName & ".jpg?id=" & vcv.PublicId)
        body = body.Replace("<Insert Summary HERE>", vcv.SummaryContent)

        mail.Body = body

        Dim smtp As New SmtpClient("mail.licensing.my-vcv.com")
        smtp.Port = 49
        smtp.Timeout = 60000
        smtp.Credentials = New System.Net.NetworkCredential("vcv@licensing.my-vcv.com", "Count548")
        smtp.Send(mail)
    End Sub
    


    <HttpPost, ValidateInput(False)> _
    Public Function Send(col As String, txtEmail As String, txtFromEmail As String, Optional id As Integer = 0)
        HttpContext.Server.ScriptTimeout = 300
        ' check permission
        Dim vcvid As Integer
        'Dim qscoll As NameValueCollection = HttpUtility.ParseQueryString(col.Trim)
        Dim vcv As VCV
        If id > 0 Then
            vcv = db.VCVs.Find(id)
            vcvid = id
        Else
            vcv = db.VCVs.Find(Session("NewVCVId"))
            vcvid = Session("NewVCVId")
        End If


        If IsNothing(vcv) Then
            Return HttpNotFound()
        End If
      

        If vcv.UserId = HttpContext.User.Identity.Name Then
        Else
            Return HttpNotFound()
        End If


        Dim emailqueue As New EmailQueue

        emailqueue.EmailFrom = txtFromEmail
        emailqueue.EmailTo = txtEmail
        emailqueue.IsProcessed = False
        emailqueue.QueueDate = Now()
        emailqueue.VCVId = vcvid
        db.EmailQueues.Add(emailqueue)
        db.SaveChanges()

        Return True

        'Try
        '    Dim converter As New Converter("F:\Work\Work\MvcMyVCV\MvcMyVCV\ffmpeg\ffmpeg.exe")
        '    Dim oo As OutputPackage = converter.ConvertToWMV("C:\StreamRecordingApp\streams\" & vcv.VideoName & ".flv")
        '    Dim outStream As FileStream = System.IO.File.OpenWrite("C:\StreamRecordingApp\streams\" & vcv.VideoName & ".wmv")
        '    'Dim oo As OutputPackage = converter.ConvertToWMV("C:\StreamRecordingApp\streams\e1afc74c-9ee1-4368-8be2-c4c99c2f5efc.flv")
        '    'Dim outStream As FileStream = System.IO.File.OpenWrite("C:\StreamRecordingApp\streams\e1afc74c-9ee1-4368-8be2-c4c99c2f5efc.wmv")
        '    oo.VideoStream.WriteTo(outStream)
        '    outStream.Flush()
        '    outStream.Close()
        'Catch ex As Exception
        '    Return False
        'End Try


        'oo.PreviewImage.Save("./thumbnail.jpg")





        ''send email

        'Dim FILE_NAME As String = Server.MapPath("~") & "\Content\Template.txt"
        'Dim sTemplate As String
        'Dim sStyles As String = ""

        'Dim TextLine As String = ""

        'If System.IO.File.Exists(FILE_NAME) = True Then

        '    'Dim objReader As New System.IO.StreamReader(FILE_NAME)

        '    'Do While objReader.Peek() <> -1
        '    '    TextLine = TextLine & objReader.ReadLine() & vbNewLine
        '    'Loop

        '    'sTemplate = TextLine

        '    Using reader As New StreamReader(FILE_NAME)
        '        Do While reader.Peek() <> -1
        '            TextLine = TextLine & reader.ReadLine() & vbNewLine
        '        Loop

        '        sTemplate = TextLine
        '    End Using

        'Else




        'End If

        'Dim mail As New MailMessage
        'Dim emails As String() = txtEmail.Split(";")

        'For Each email As String In emails
        '    Try
        '        mail.Bcc.Add(email.Trim)
        '    Catch ex As Exception

        '    End Try


        'Next

        'mail.From = New MailAddress(txtFromEmail)


        'mail.Subject = "MyChatPak Message"

        ''sPreview = sPreview.Insert(ibody, "Tim Long<br /><center><img src=""cid:Me.jpeg""><br />Click image to view VideoCV</center><br />")

        'Dim sStillFileName As String


        'sStillFileName = Server.MapPath("~") & "Snaps/" & vcv.VCVName.ToString & ".jpg"


        'Dim atchStill As New Attachment(sStillFileName)
        'atchStill.ContentId = "Me.jpeg"
        'atchStill.Name = "Me.jpeg"

        'Dim sVideoFilename As String


        'sVideoFilename = "C:\StreamRecordingApp\streams\" & vcv.VideoName.ToString & ".wmv"



        'Dim atchVideo As New Attachment(sVideoFilename)

        'atchVideo.ContentId = "MyChatPak"
        'atchVideo.Name = "MyChatPak.wmv"

        'mail.Attachments.Add(atchVideo)




        'Dim sPreview As String

        'Dim user = db.Users.Find(HttpContext.User.Identity.Name)

        'sPreview = sTemplate.Replace("<Insert CV HERE>", vcv.Summary.SummaryContent)

        'sPreview = sPreview.Replace("<Insert Name HERE>", user.UserName)

        'sPreview = sPreview.Replace("<Insert Directory HERE>", "http://licensing.my-vcv.com/email")

        'sPreview = sPreview.Replace("<Insert URL HERE>", "http://licensing.my-vcv.com/email")

        'sPreview = sPreview.Replace("<Insert Image HERE>", "cid:" + atchStill.ContentId + "")
        'sPreview = sPreview.Replace("<Insert Link HERE>", "<a href='cid:" + atchVideo.ContentId + "'" & ">").Replace("<Insert Close Link HERE>", "</a>")

        'sPreview = sPreview.Replace("<Insert styles HERE>", sStyles)





        'mail.Attachments.Add(atchStill)

        ''mail.Attachments.Add(atchSkills)

        ''cv

        'If vcv.Attachment1Id <> "" Then
        '    Try
        '        Dim atchCV As New Attachment(Server.MapPath("~") & "/Attachments/" & vcv.Attachment1Id)


        '        atchCV.Name = vcv.Attachment1Name
        '        mail.Attachments.Add(atchCV)
        '    Catch ex As Exception

        '    End Try
        'End If

        'If vcv.Attachment2Id <> "" Then
        '    Try
        '        Dim atch2 As New Attachment(Server.MapPath("~") & "/Attachments/" & vcv.Attachment2Id)


        '        atch2.Name = vcv.Attachment2Name
        '        mail.Attachments.Add(atch2)
        '    Catch ex As Exception

        '    End Try
        'End If

        'If vcv.Attachment3Id <> "" Then
        '    Try
        '        Dim atch3 As New Attachment(Server.MapPath("~") & "/Attachments/" & vcv.Attachment3Id)


        '        atch3.Name = vcv.Attachment3Name
        '        mail.Attachments.Add(atch3)
        '    Catch ex As Exception

        '    End Try
        'End If



        'mail.Body = sPreview

        'mail.IsBodyHtml = True


        ''send using company smtp server

        'Dim smtp As New SmtpClient("mail.licensing.my-vcv.com")
        'smtp.Port = 49
        'smtp.Timeout = 999999999

        'smtp.Credentials = New System.Net.NetworkCredential("vcv@licensing.my-vcv.com", "Count548")
        ''smtp.EnableSsl = True

        ''Dim userState As Object = mail


        'Try
        '    smtp.Send(mail)
        '    Return True
        'Catch ex As Exception
        '    Return False
        'End Try




    End Function

    '<HttpPost> _
    'Public Function Step5(files As IEnumerable(Of HttpPostedFileBase)) As ActionResult
    '    For Each var As System.Web.HttpPostedFileBase In files
    '        If var.ContentLength > 0 Then
    '            Dim fileName = Path.GetFileName(var.FileName)
    '            Dim path__1 = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName)
    '            var.SaveAs(path__1)
    '        End If
    '    Next

    'End Function

    'Public Function AsyncUpload(fileBase As HttpPostedFileBase, scriptData As String, ASPSESSID As String) As Guid
    '    'save file
    '    Dim _uploadsFolder As String = HostingEnvironment.MapPath("~/Attachments/")
    '    Dim identifier = Guid.NewGuid()
    '    Dim sfile = Request.Files(0)
    '    Dim ident As String = identifier.ToString
    '    sfile.SaveAs(_uploadsFolder & identifier.ToString)

    '    ' Dim ticket As FormsAuthenticationTicket = FormsAuthentication.De

    '    ' Dim identify = New FormsIdentity(ticket)

    '    'If identify.IsAuthenticated = True Then





    '    'save to db
    '    Dim id = Session("NewVCVId")

    '    Dim vcv As VCV = db.VCVs.Find(id)
    '    vcv.Attachment1Id = "test"
    '    vcv.Attachment1Name = sfile.FileName.ToString
    '    db.SaveChanges()



    '    Return identifier
    '    'Return _fileStore.SaveUploadedFile(Request.Files(0))





    'End Function

    <HttpPost> _
    Public Function Upload() As JsonResult
        'save to db
        Dim id = Session("NewVCVId")

        Dim vcv As VCV = db.VCVs.Find(id)
        Dim teacher As String
        If IsNothing(vcv) Then
            Return Json("File uploaded failed")
        End If



        If vcv.UserId = HttpContext.User.Identity.Name Then
        Else
            Return Json("File uploaded failed")
        End If





        Dim fileName As String = Request.Headers("X-File-Name")
        Dim fileType As String = Request.Headers("X-File-Type")
        Dim fileSize As Integer = Convert.ToInt32(Request.Headers("X-File-Size"))
        'File's content is available in Request.InputStream property
        Dim fileContent As System.IO.Stream = Request.InputStream
        'Creating a FileStream to save file's content
        Dim identifier = Guid.NewGuid()
        Dim fileExtension As String = fileName.Substring(fileName.LastIndexOf("."))


        Dim fileStream As System.IO.FileStream = System.IO.File.Create(Server.MapPath("~/Attachments/") & identifier.ToString & fileExtension)
        fileContent.Seek(0, System.IO.SeekOrigin.Begin)
        'Copying file's content to FileStream
        fileContent.CopyTo(fileStream)
        fileStream.Dispose()


        If Request.Headers("X-Type") = "Auto" Then
            VCV.Attachment1Id = identifier.ToString & fileExtension
            VCV.Attachment1Name = fileName

        ElseIf Request.Headers("X-Type") = "Other1" Then
            VCV.Attachment2Id = identifier.ToString & fileExtension
            VCV.Attachment2Name = fileName

        ElseIf Request.Headers("X-Type") = "Other2" Then
            VCV.Attachment3Id = identifier.ToString & fileExtension
            VCV.Attachment3Name = fileName

        End If


        db.SaveChanges()


        Return Json("File uploaded - " & fileName)
    End Function

    <HttpPost> _
    Public Function GetScript(id As Integer)
        Dim script As Script = db.Scripts.Find(id)

        If IsNothing(script) Then
            Return HttpNotFound()
        End If
       

        If script.UserId = HttpContext.User.Identity.Name Then
        Else
            Return HttpNotFound()
        End If

        Return script.ScriptContent

    End Function

    <HttpPost()> _
    Public Function RemoveScript(id As Integer)
        Dim script As Script = db.Scripts.Find(id)

        If IsNothing(script) Then
            Return False
        End If


        If script.UserId = HttpContext.User.Identity.Name Then
            ' update script
            script.Hidden = True
            db.SaveChanges()
        Else
            Return False
        End If


        Return True
    End Function

    <HttpPost(), ValidateInput(False)> _
    Public Function EditScript(editor As String, ScriptId As Integer)
        Dim script As Script = db.Scripts.Find(ScriptId)

        If IsNothing(script) Then
            Return False
        End If


        If script.UserId = HttpContext.User.Identity.Name Then
            '' update script
            script.ScriptContent = editor
            db.SaveChanges()
        Else
            Return False
        End If


        Return True
    End Function

    <HttpPost()> _
    Public Function RemoveSummary(id As Integer)
        Dim summary As Summary = db.Summaries.Find(id)

        If IsNothing(summary) Then
            Return False
        End If


        If summary.UserId = HttpContext.User.Identity.Name Then
            ' update script
            summary.Hidden = True
            db.SaveChanges()
        Else
            Return False
        End If


        Return True
    End Function

    <HttpPost(), ValidateInput(False)> _
    Public Function EditSummary(editor As String, SummaryId As Integer)
        Dim summary As Summary = db.Summaries.Find(SummaryId)

        If IsNothing(summary) Then
            Return False
        End If


        If summary.UserId = HttpContext.User.Identity.Name Then
            '' update script
            summary.SummaryContent = editor
            db.SaveChanges()
        Else
            Return False
        End If


        Return True
    End Function

    <HttpPost> _
    Public Function GetSummary(id As Integer)
        Dim summary As Summary = db.Summaries.Find(id)

        If IsNothing(summary) Then
            Return HttpNotFound()
        End If
       

        Return summary.SummaryContent

    End Function

    <HttpPost> _
    Public Function VideoComplete()

        Dim vcv As VCV = db.VCVs.Find(Session("NewVCVId"))

        If IsNothing(vcv) Then
            Return HttpNotFound()
        End If

        vcv.CurrentStep = 2
        db.SaveChanges()
        Return True

    End Function

    <HttpPost> _
    Public Function SelectVideo(id As String)
        Dim vcv As VCV = db.VCVs.Find(Session("NewVCVId"))

        If IsNothing(vcv) Then
            Return HttpNotFound()
        End If

        Dim vcvExisting = db.VCVs.Where(Function(v) v.VCVName = id).FirstOrDefault


        vcv.VideoName = id
        vcv.CurrentStep = 3
        vcv.Length = vcvExisting.Length
        db.SaveChanges()

        Return True

    End Function


    <HttpPost> _
    Public Function Alive() As ActionResult
        Return New ContentResult() With {.Content = "OK", .ContentType = "text/plain"}
    End Function
    <HttpPost> _
    Public Function EmailPreview(id As String)
        Dim sPreview As String

        Dim template = db.Templates.FirstOrDefault(Function(u) u.TemplateId = 1)

        sPreview = template.TemplateContent.ToString






        Dim user = db.Users.Find(HttpContext.User.Identity.Name)
        Dim vcv = db.VCVs.Find(CInt(id))

        If IsNothing(vcv) Then
            Return HttpNotFound()
        End If



        sPreview = sPreview.Replace("<Insert CV HERE>", vcv.Summary.SummaryContent)

        sPreview = sPreview.Replace("<Insert Name HERE>", user.UserName)

        sPreview = sPreview.Replace("<Insert Directory HERE>", "http://licensing.my-vcv.com/email")

        sPreview = sPreview.Replace("<Insert URL HERE>", "http://licensing.my-vcv.com/email")

        sPreview = sPreview.Replace("<Insert Image HERE>", "/Snaps/" & vcv.VCVName.ToString & ".jpg")
        sPreview = sPreview.Replace("<Insert Link HERE>", "")

        sPreview = sPreview.Replace("<Insert styles HERE>", "")


        Return sPreview
    End Function

    <HttpPost> _
    Public Function ListingPreview(id As String)
        Dim sPreview As String

        Dim listing = db.Listings.FirstOrDefault(Function(l) l.ListingId = id)



        sPreview = listing.ListingTitle & "<br /><br />"

        sPreview = sPreview & listing.ListingSummary


        Return sPreview
    End Function

    <HttpPost> _
    Public Function WatchListSelect()
        Dim sPreview As String

        Dim watchlist = db.WatchLists.Where(Function(w) w.UserId = HttpContext.User.Identity.Name).ToList
        sPreview = "<Table>"
        For Each wlisting As WatchList In watchlist
            sPreview = sPreview & "<tr><td><input type=""radio"" name=""WatchList"" value=""" & wlisting.ListingId & """></td><td>" & wlisting.Listing.ListingTitle & "</td><td>" & wlisting.Listing.ListingSummary & "</td><td>" & wlisting.Listing.Company & "</td></tr>"

        Next
        sPreview = sPreview & "</Table>"



        Return sPreview
    End Function

    <HttpPost> _
    Public Function Apply(VCVId As Integer, ListingId As Integer)
        Dim application As New ListingApplication
        Dim vcv As VCV = db.VCVs.FirstOrDefault(Function(v) v.VCVId = VCVId)

        Dim count As Integer = db.ListingApplications.Where(Function(l) l.UserId = HttpContext.User.Identity.Name And l.ListingId = ListingId).Count

        If count > 0 Then
            Return "You have already applied for this job."
        End If

        application.UserId = HttpContext.User.Identity.Name
        application.ListingId = ListingId
        application.TimeStamp = Now()
        application.SummaryContent = vcv.Summary.SummaryContent
        application.VideoName = vcv.VideoName
        application.Attachment1Id = vcv.Attachment1Id
        application.Attachment2Id = vcv.Attachment2Id
        application.Attachment3Id = vcv.Attachment3Id
        application.Attachment1Name = vcv.Attachment1Name
        application.Attachment2Name = vcv.Attachment2Name
        application.Attachment3Name = vcv.Attachment3Name
        db.ListingApplications.Add(application)
        db.SaveChanges()



        Return "Job Application Successfully Submited. Good luck!"

    End Function





End Class
