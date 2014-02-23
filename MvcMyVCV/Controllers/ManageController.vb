Imports System.Data.Entity
Imports System.Web.Security.Membership
Imports PagedList
Imports MvcMyVCV.PaymentExpress.PxPay
Imports System.IO
Imports System.Transactions
Imports System.Drawing

Public Class ManageController
    Inherits System.Web.Mvc.Controller

    Private db As New VCVOnlineEntities

    <Role(Roles:="Employer, Recruitment")> _
    Function Index(page As System.Nullable(Of Integer)) As ActionResult
        Dim userid As String = HttpContext.User.Identity.Name
        Dim usersListings = db.Listings.Where(Function(l) l.UserId = userid And l.Completed = True).OrderBy(Function(l) l.ListingTitle)
        Dim pageSize As Integer = 5
        Dim pageNumber As Integer = (If(page, 1))

        'New Applicant Count Logic
        Dim allUsersApplicants = db.ListingApplications.Where(Function(la) la.Listing.UserId = userid).OrderBy(Function(la) la.ApplicationId)
        ViewBag.allUsersApplicants = allUsersApplicants

        Return View(usersListings.ToPagedList(pageNumber, pageSize))
    End Function

    Public Function Applicants(id As System.Nullable(Of Integer), page As System.Nullable(Of Integer)) As ActionResult
        Return ApplicantView(0, id, page)
    End Function
    <HttpPost()> _
    <ValidateAntiForgeryToken()> _
    Public Function Applicants(col As FormCollection, id As System.Nullable(Of Integer), page As System.Nullable(Of Integer)) As ActionResult
        Return ApplicantView(CInt(col("ApplicationStatusId")), id, page)

    End Function

    Public Function ApplicantView(StatusId As Integer, id As System.Nullable(Of Integer), page As System.Nullable(Of Integer)) As ActionResult
        Dim userID As String = HttpContext.User.Identity.Name
        ViewBag.ApplicationStatusId = StatusId
        If Not id Is Nothing Then
            Dim l As Listing = db.Listings.Find(id)
            If l.UserId <> userID Then Return RedirectToAction("Index", "Home")
            If User.IsInRole("Recruitment") And l.PublicId = "" Then
                Dim PublicId As String = ""
                Do
                    PublicId = Guid.NewGuid().ToString.Substring(0, 7)
                Loop Until db.Listings.Where(Function(li) li.PublicId = PublicId).Count() = 0

                db.Listings.Find(id).PublicId = PublicId
                db.SaveChanges()
            End If

            Dim a As ApplicationStatus = db.ApplicationStatus1.Find(StatusId)
            If a.Status = "Short Listed" Then
                ViewBag.PublicLink = l.PublicId
            End If
        End If

        Dim usersApplicantsById = db.ListingApplications.Where(Function(la) la.Listing.UserId = userID And la.ListingId = id And la.ApplicationStatusId = StatusId).OrderBy(Function(la) la.IsViewed)
        Dim pageSize As Integer = 5
        Dim pageNumber As Integer = (If(page, 1))

        ViewBag.ApplicationId = id

        Dim query = From ApplicationStatusData In db.ApplicationStatus1
                   Select ApplicationStatusData.ApplicationStatusId, ApplicationStatusData.Status

        Dim selectList As New SelectList(query, "ApplicationStatusId", "Status", StatusId)

        ViewBag.selectList = selectList
        ViewBag.selectedIndex = StatusId

        Return View(usersApplicantsById.ToPagedList(pageNumber, pageSize))
    End Function

    Public Function RenewListing(id As System.Nullable(Of Integer)) As ActionResult
        Dim listing As Listing = db.Listings.Find(id)
        Session("NewListingId") = id
        Dim p As Product
        If listing.IsAdvertised Then p = db.Products.Find(3) Else p = db.Products.Find(2)
        Session("CompletePage") = "ListingRenewalComplete"
        If p.Price = CDec(0) Then Return RedirectToAction("ListingRenewalComplete", "Listing") Else Return RedirectToAction("Payment", "Listing")
    End Function

    Public Function EditListing(id As System.Nullable(Of Integer)) As ActionResult
        If id Is Nothing = False Then
            Dim listing = db.Listings.Find(id)
            If listing.UserId <> User.Identity.Name Then
                Return RedirectToAction("Index", "Home")
            End If
            Dim qry = db.EmailTemplates.Where(Function(et) et.UserId = User.Identity.Name Or et.UserId = "system")
            Dim EmailTemplates As New SelectList(qry, "EmailTemplateId", "EmailTemplateName", 1)
            ViewBag.EmailTemplates = EmailTemplates



            ViewBag.ListingTypeId = New SelectList(db.ListingTypes, "ListingTypeId", "ListingDescription", listing.ListingTypeId)
            ViewBag.ListingPayId = New SelectList(db.ListingPays, "ListingPayId", "PayDescription", listing.ListingPayId)

            If listing.SubCategory.CategoryId > 0 Then
                ViewBag.CategoryId = New SelectList(db.Categories, "CategoryId", "CategoryName", listing.SubCategory.CategoryId)
                If listing.SubCategoryId > 0 Then
                    ViewBag.SelectedSubCategory = listing.SubCategoryId
                End If

            Else
                ViewBag.CategoryId = New SelectList(db.Categories, "CategoryId", "CategoryName")
            End If

            If listing.Suburb.CityId > 0 Then
                ViewBag.CityId = New SelectList(db.Cities, "CityId", "CityName", listing.Suburb.CityId)
                If listing.SuburbId > 0 Then
                    ViewBag.SelectedSuburb = listing.SuburbId
                End If
            Else
                ViewBag.CityId = New SelectList(db.Cities, "CityId", "CityName")
            End If


            Return View(listing)
        Else : Return Nothing
        End If
    End Function
    <HttpPost> _
    Public Function EditListing(listing As Listing, id As Nullable(Of Integer), files As IEnumerable(Of HttpPostedFileBase)) As ActionResult

        Dim l As Listing = db.Listings.Find(id)
        l.ListingTitle = listing.ListingTitle
        l.Company = listing.Company
        l.ListingReference = listing.ListingReference
        l.ListingSummary = listing.ListingSummary
        l.ListingDescription = listing.ListingDescription
        l.IsAdvertised = listing.IsAdvertised
        l.ListingTypeId = listing.ListingTypeId
        l.ListingPayId = listing.ListingPayId
        l.SubCategoryId = listing.SubCategoryId
        l.Image1 = listing.Image1
        l.Image2 = listing.Image2
        l.Image3 = listing.Image3
        l.EmailNotificationsEnabled = listing.EmailNotificationsEnabled
        l.EmailTemplateId = listing.EmailTemplateId

        Try
            If ModelState.IsValid Then
                Dim x As Integer = 0
                For Each Listingfile As HttpPostedFileBase In files

                    If Listingfile IsNot Nothing AndAlso Listingfile.ContentLength > 0 Then
                        Dim fileName = Guid.NewGuid()
                        Dim filePath = "~/UploadedListings/" & fileName.ToString & ".jpg"
                        'Dim filePath = "~/UploadedListings/" & fileName.ToString & ".jpg"

                        'Dim photoFile As Bitmap = ResizeImage(Listingfile.InputStream, 800, 600)
                        Dim newsize As Size
                        If x = 0 Then newsize = New Size(250, 250) Else newsize = New Size(400, 400)

                        Using oldbmp As Bitmap = CType(Image.FromStream(Listingfile.InputStream), System.Drawing.Image)

                            If CSng(oldbmp.Width) > CSng(oldbmp.Height) Then        'landscape
                                newsize.Height = oldbmp.Height / (oldbmp.Width / newsize.Width)
                                Response.Write("Landscape: W=" & newsize.Width & " H=" & newsize.Height)
                            ElseIf CSng(oldbmp.Height) > CSng(oldbmp.Width) Then    'portrait
                                newsize.Width = oldbmp.Width / (oldbmp.Height / newsize.Height)
                                Response.Write("Portrait: W=" & newsize.Width & " H=" & newsize.Height)
                            ElseIf CSng(oldbmp.Width) = CSng(oldbmp.Height) Then    'square
                                If newsize.Width > newsize.Height Then              'get the shortest side
                                    newsize.Width = newsize.Height
                                Else
                                    newsize.Height = newsize.Width
                                End If
                                Response.Write("Square: W=" & newsize.Width & " H=" & newsize.Height)
                            End If

                            Using newbmp As New Bitmap(newsize.Width, newsize.Height)
                                Using newgraphics As Graphics = Graphics.FromImage(newbmp)
                                    newgraphics.Clear(Color.FromArgb(-1))
                                    newgraphics.DrawImage(oldbmp, 0, 0, newsize.Width, newsize.Height)
                                    newgraphics.Save()
                                    newgraphics.Dispose()
                                    Dim nPath = Server.MapPath(filePath)
                                    newbmp.Save(nPath, System.Drawing.Imaging.ImageFormat.Jpeg)
                                    newbmp.Dispose()
                                End Using
                            End Using

                            If x = 0 Then
                                l.Image1 = fileName.ToString & ".jpg"
                            ElseIf x = 1 Then
                                l.Image2 = fileName.ToString & ".jpg"
                            Else
                                l.Image3 = fileName.ToString & ".jpg"
                            End If


                        End Using
                    End If
                    x += 1
                Next

            End If

        Catch ex As Exception
            Dim i As Integer = 0
        End Try

        db.SaveChanges()

        Return RedirectToAction("Index", "Home")

        'Dim qry = db.EmailTemplates.Where(Function(et) et.UserId = User.Identity.Name Or et.UserId = "system")
        'Dim EmailTemplates As New SelectList(qry, "EmailTemplateId", "EmailTemplateName", 1)
        'ViewBag.EmailTemplates = EmailTemplates

        'ViewBag.ListingTypeId = New SelectList(db.ListingTypes, "ListingTypeId", "ListingDescription", listing.ListingTypeId)
        'ViewBag.ListingPayId = New SelectList(db.ListingPays, "ListingPayId", "PayDescription", listing.ListingPayId)

        'If l.SubCategory.CategoryId > 0 Then
        '    ViewBag.CategoryId = New SelectList(db.Categories, "CategoryId", "CategoryName", l.SubCategory.CategoryId)
        '    If listing.SubCategoryId > 0 Then
        '        ViewBag.SelectedSubCategory = listing.SubCategoryId
        '    End If

        'Else
        '    ViewBag.CategoryId = New SelectList(db.Categories, "CategoryId", "CategoryName")
        'End If

        'If l.Suburb.CityId > 0 Then
        '    ViewBag.CityId = New SelectList(db.Cities, "CityId", "CityName", l.Suburb.CityId)
        '    If listing.SuburbId > 0 Then
        '        ViewBag.SelectedSuburb = listing.SuburbId
        '    End If
        'Else
        '    ViewBag.CityId = New SelectList(db.Cities, "CityId", "CityName")
        'End If


    End Function

    Public Function Delete(id As String) As ActionResult
        Dim user As User = db.Users.Find(id)
        If user Is Nothing Then
            Return HttpNotFound()
        End If

        If user.UserId <> HttpContext.User.Identity.Name Then
            Return HttpNotFound()
        End If
        user.Removed = True
        db.SaveChanges()
        Return RedirectToAction("Classes")
    End Function


End Class