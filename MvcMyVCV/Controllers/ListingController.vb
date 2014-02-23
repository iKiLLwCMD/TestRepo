Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging
Imports PagedList
Imports System.Web.Script.Serialization
Imports System.Collections.Generic
Imports System.Runtime.Serialization.Json
Imports MvcMyVCV.PaymentExpress.PxPay

Public Class ListingController
    Inherits System.Web.Mvc.Controller

    '
    ' GET: /Job
    Private db As New VCVOnlineEntities

    'Function Index() As ActionResult
    '    Dim listings = db.Listings.OrderBy(Function(l) l.ListingId).ToList.Take(20)


    '    Return View(listings.ToList)


    'End Function

    Function Index(page As System.Nullable(Of Integer)) As ActionResult
        'Dim Count = (From Assignment In db.Assignments
        '          Join Classes In db.Classes On Assignment.ClassId Equals Classes.ClassId
        '          Where Classes.UserId = HttpContext.User.Identity.Name
        '          Select Assignment.AssignmentId, Assignment.AssignmentName).Count



        Dim selectlistCategories As New SelectList(db.Categories, "CategoryId", "CategoryName")
        Dim selectlistCities As New SelectList(db.Cities, "CityId", "CityName")


        ViewBag.CategoryId = selectlistCategories
        ViewBag.CityId = selectlistCities


        Dim listing = db.Listings.Where(Function(l) l.IsAdvertised = True).OrderBy(Function(l) l.ListingTitle)
        Dim pageSize As Integer = 5
        Dim pageNumber As Integer = (If(page, 1))
        Return View(listing.ToPagedList(pageNumber, pageSize))


        'Dim vcv = db.VCVs
        'Return View(vcv.ToList().Where(Function(u) u.AssignmentId = firstElement.AssignmentId And u.CurrentStep >= 3))

    End Function

    Public Function IsSystemTemplate(EmailTemplateId As Integer) As Boolean
        Dim et = db.EmailTemplates.Find(EmailTemplateId)
        If et.UserId = "system" Then
            Return 1
        Else
            Return 0
        End If
    End Function

    <ValidateInput(False)> _
    Public Function SaveNewEmailTemplate(templateName As String, shortListContent As String, unsuccessfulContent As String)
        Dim et As New EmailTemplate
        et.EmailTemplateName = templateName
        et.UserId = User.Identity.Name
        et.IsDefault = False
        et.TimeStamp = Now()

        db.EmailTemplates.Add(et)
        db.SaveChanges()


        Dim content() As String = {unsuccessfulContent, shortListContent}
        For i = 3 To 4
            Dim etd As New EmailTemplateDetail
            With etd
                .EmailTemplateDetailsContent = content(i - 3)
                .EmailTemplateDetailsName = "HardCodedName"
                .EmailTypeId = i
                .EmailTemplateId = et.EmailTemplateId
            End With
            db.EmailTemplateDetails.Add(etd)
            db.SaveChanges()
        Next

        Return et.EmailTemplateId

    End Function

    <ValidateInput(False)> _
    Public Sub SaveEmailTemplate(id As Integer, shortListContent As String, unsuccessfulContent As String)
        Dim unsuccessfulETD As EmailTemplateDetail = db.EmailTemplateDetails.FirstOrDefault(Function(etd) etd.EmailTypeId = 3 And etd.EmailTemplateId = id)
        Dim shortListETD As EmailTemplateDetail = db.EmailTemplateDetails.FirstOrDefault(Function(etd) etd.EmailTypeId = 4 And etd.EmailTemplateId = id)
        unsuccessfulETD.EmailTemplateDetailsContent = unsuccessfulContent
        shortListETD.EmailTemplateDetailsContent = shortListContent
        db.SaveChanges()
    End Sub

    Public Function GetEmailTemplate(id As Integer)
        Dim et As EmailTemplate = db.EmailTemplates.Find(id)

        If et.UserId = HttpContext.User.Identity.Name Or et.IsDefault = True Then
            Dim sETD = db.EmailTemplateDetails.FirstOrDefault(Function(e) e.EmailTemplateId = id And e.EmailTypeId = 4)
            Dim uETD = db.EmailTemplateDetails.FirstOrDefault(Function(e) e.EmailTemplateId = id And e.EmailTypeId = 3)
            Dim list As New List(Of KeyValuePair(Of String, String))
            list.Add(New KeyValuePair(Of String, String)("1", sETD.EmailTemplateDetailsContent))
            list.Add(New KeyValuePair(Of String, String)("1", uETD.EmailTemplateDetailsContent))

            Return New JavaScriptSerializer().Serialize(list)
        Else
            Return HttpNotFound()
        End If
    End Function

    Function ViewListing(id As Integer) As ActionResult
        Dim listing = db.Listings.FirstOrDefault(Function(l) l.ListingId = id)

        Dim count As Integer = db.WatchLists.Where(Function(w) w.ListingId = id And w.UserId = HttpContext.User.Identity.Name).Count
        If count > 0 Then
            ViewBag.Added = True
        Else
            ViewBag.Added = False
        End If


        Return View(listing)

    End Function

    <HttpPost()> _
    Function Index(page As System.Nullable(Of Integer), Optional ByVal CategoryId As Integer = 0, Optional ByVal CityId As Integer = 0) As ActionResult

        If CategoryId = 0 Then
            Dim selectlistCategories As New SelectList(db.Categories, "CategoryId", "CategoryName")
            ViewBag.CategoryId = selectlistCategories
        Else
            Dim selectlistCategories As New SelectList(db.Categories, "CategoryId", "CategoryName", CategoryId)
            ViewBag.CategoryId = selectlistCategories
        End If

        If CityId = 0 Then
            Dim selectlistCities As New SelectList(db.Cities, "CityId", "CityName")
            ViewBag.CityId = selectlistCities
        Else
            Dim selectlistCities As New SelectList(db.Cities, "CityId", "CityName", CityId)
            ViewBag.CityId = selectlistCities
        End If

        If CategoryId = 0 And CityId = 0 Then
            Dim listing = db.Listings.Where(Function(l) l.IsAdvertised = True).OrderBy(Function(l) l.ListingTitle)
            Dim pageSize As Integer = 5
            Dim pageNumber As Integer = (If(page, 1))
            Return View(listing.ToPagedList(pageNumber, pageSize))
        ElseIf CategoryId > 0 And CityId = 0 Then
            Dim listing = db.Listings.Where(Function(l) l.IsAdvertised = True And l.SubCategory.CategoryId = CategoryId).OrderBy(Function(l) l.ListingTitle)
            Dim pageSize As Integer = 5
            Dim pageNumber As Integer = (If(page, 1))
            Return View(listing.ToPagedList(pageNumber, pageSize))
        ElseIf CategoryId = 0 And CityId > 0 Then
            Dim Listing = db.Listings.Where(Function(l) l.IsAdvertised = True And l.Suburb.CityId = CityId).OrderBy(Function(l) l.ListingTitle)
            Dim pageSize As Integer = 5
            Dim pageNumber As Integer = (If(page, 1))
            Return View(Listing.ToPagedList(pageNumber, pageSize))
        Else
            Dim listing = db.Listings.Where(Function(l) l.IsAdvertised = True And l.Suburb.CityId = CityId And l.SubCategory.CategoryId = CategoryId).OrderBy(Function(l) l.ListingTitle)
            Dim pageSize As Integer = 5
            Dim pageNumber As Integer = (If(page, 1))
            Return View(listing.ToPagedList(pageNumber, pageSize))
        End If

    End Function

    Function Create() As ActionResult
        ViewBag.ListingTypeId = New SelectList(db.ListingTypes, "ListingTypeId", "ListingDescription")
        ViewBag.ListingPayId = New SelectList(db.ListingPays, "ListingPayId", "PayDescription")
        ViewBag.CategoryId = New SelectList(db.Categories, "CategoryId", "CategoryName")
        ViewBag.CityId = New SelectList(db.Cities, "CityId", "CityName")
        Dim qry = db.EmailTemplates.Where(Function(et) et.UserId = User.Identity.Name Or et.UserId = "system")
        Dim EmailTemplates As New SelectList(qry, "EmailTemplateId", "EmailTemplateName", 1)
        ViewBag.EmailTemplates = EmailTemplates
        Return View()
    End Function

    Function GetEmailSelection() As ActionResult
        Return View("EmailSelection")
    End Function

    Public Function EmployerListing(id As String, page As System.Nullable(Of Integer)) As ActionResult
        Try

            ViewBag.PublicId = id
            Session("PublicId") = id
            Dim l As Listing = db.Listings.FirstOrDefault(Function(li) li.PublicId = id)

            Dim userID As String = HttpContext.User.Identity.Name

            Dim usersApplicantsById = db.ListingApplications.Where(Function(la) la.ListingId = l.ListingId And la.ApplicationStatusId = 2).OrderBy(Function(la) la.ApplicationId)
            Dim pageSize As Integer = 5
            Dim pageNumber As Integer = (If(page, 1))
            Return View(usersApplicantsById.ToPagedList(pageNumber, pageSize))
        Catch ex As Exception
            Return HttpNotFound()
        End Try
    End Function

    '
    ' POST: 
    <HttpPost()> _
<ValidateAntiForgeryToken()> _
    Function Create(ByVal listing As Global.MvcMyVCV.Listing, files As IEnumerable(Of HttpPostedFileBase), Optional CategoryId As Integer = 0, Optional CityId As Integer = 0) As ActionResult
        Dim qry = db.EmailTemplates.Where(Function(et) et.UserId = User.Identity.Name Or et.UserId = "system")
        Dim EmailTemplates As New SelectList(qry, "EmailTemplateId", "EmailTemplateName", 1)
        ViewBag.EmailTemplates = EmailTemplates

        ViewBag.ListingTypeId = New SelectList(db.ListingTypes, "ListingTypeId", "ListingDescription", listing.ListingTypeId)
        ViewBag.ListingPayId = New SelectList(db.ListingPays, "ListingPayId", "PayDescription", listing.ListingPay)

        listing.Completed = False
        listing.Fulfilled = False
        listing.ListedDate = Now()
        listing.ExpiryDate = Now.AddMonths(1)

        If CategoryId > 0 Then
            ViewBag.CategoryId = New SelectList(db.Categories, "CategoryId", "CategoryName", CategoryId)
            If listing.SubCategoryId > 0 Then
                ViewBag.SelectedSubCategory = listing.SubCategoryId
            End If

        Else
            ViewBag.CategoryId = New SelectList(db.Categories, "CategoryId", "CategoryName")
        End If

        If CityId > 0 Then
            ViewBag.CityId = New SelectList(db.Cities, "CityId", "CityName", CityId)
            If listing.SuburbId > 0 Then
                ViewBag.SelectedSuburb = listing.SuburbId
            End If
        Else
            ViewBag.CityId = New SelectList(db.Cities, "CityId", "CityName")
        End If

        If ModelState.IsValid = False Then
            Return View(listing)
        End If
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
                                listing.Image1 = fileName.ToString & ".jpg"
                            ElseIf x = 1 Then
                                listing.Image2 = fileName.ToString & ".jpg"
                            Else
                                listing.Image3 = fileName.ToString & ".jpg"
                            End If

                            x += 1
                        End Using


                    End If
                Next

                listing.UserId = HttpContext.User.Identity.Name

                db.Listings.Add(listing)
                db.SaveChanges()

                Session("NewListingId") = listing.ListingId
                Session("CompletePage") = "ListingComplete"
                Dim p As Product
                If listing.IsAdvertised Then p = db.Products.Find(3) Else p = db.Products.Find(2)
                If p.Price = CDec(0) Then Return RedirectToAction("ListingComplete") Else Return RedirectToAction("Payment")
            End If




        Catch ex As Exception
            Dim i As Integer = 0
        End Try

        Return View()
    End Function

    Public Function Payment() As ActionResult
        Dim ListingId As Integer = Session("NewListingId")
        Dim listing = db.Listings.Find(ListingId)
        Dim p As Product
        If listing.IsAdvertised Then p = db.Products.Find(3) Else p = db.Products.Find(2)

        ViewBag.Product = p.ProductName
        ViewBag.Total = Format(p.Price, "0.00")
        ViewBag.Tax = Format((ViewBag.Total * 3) / 23, "0.00")
        ViewBag.SubTotal = ViewBag.Total - ViewBag.Tax

        Return View()
    End Function

    <HttpPost> _
    Public Function Payment(Recurring As Boolean) As ActionResult
        Dim ListingId As Integer = CInt(Session("NewListingId"))
        Dim listing = db.Listings.Find(ListingId)
        Dim p As Product
        If listing.IsAdvertised Then p = db.Products.Find(3) Else p = db.Products.Find(2)
        'save transcation
        Dim tran As New Tran


        tran.UserId = User.Identity.Name
        tran.TranType = p.ProductName

        tran.TranAmount = p.Price
        tran.TimeStamp = Now()
        db.Trans.Add(tran)
        db.SaveChanges()

        Dim PxPayUserId As String = ConfigurationManager.AppSettings("PxPayUserId")
        Dim PxPayKey As String = ConfigurationManager.AppSettings("PxPayKey")

        Dim WS As New PxPay(PxPayUserId, PxPayKey)

        Dim input As New RequestInput()

        input.AmountInput = Math.Round(CDec(p.Price), 2)
        input.CurrencyInput = "USD"
        input.MerchantReference = "Virtual Technologies"
        input.TxnType = "Purchase"
        input.UrlFail = Request.Url.ToString().Substring(0, Request.Url.ToString().LastIndexOf("/")) & "/" & "PaymentFailed"
        input.UrlSuccess = Request.Url.ToString().Substring(0, Request.Url.ToString().LastIndexOf("/")) & "/" & Session("CompletePage").ToString()

        input.TxnData1 = User.Identity.Name
        input.TxnData2 = tran.TranId
        If Recurring = True Then
            input.EnableAddBillCard = 1
        End If


        Dim output As RequestOutput = WS.GenerateRequest(input)

        If output.valid = "1" Then
            ' Redirect user to payment page 

            Return Redirect(output.Url)
        Else
            Return View()
        End If
    End Function

    Public Function ListingComplete() As ActionResult
        Dim listing = db.Listings.Find(CInt(Session("NewListingId")))
        listing.Completed = True
        db.SaveChanges()
        Return View()
    End Function

    Public Function ListingRenewalComplete() As ActionResult
        Dim listing = db.Listings.Find(CInt(Session("NewListingId")))
        listing.ExpiryDate = Now.AddMonths(1)
        db.SaveChanges()
        Return View()
    End Function

    Public Function GetSubCategory(CategoryId As String) As JsonResult
        If CategoryId.Trim = "" Then Return Json(0)
        Dim subcategories = db.SubCategories.Where(Function(c) c.CategoryId = CategoryId).Select(Function(c) New With {.Id = c.SubCategoryId, .Name = c.SubCategoryName}).ToList()
        Return Json(subcategories)
    End Function

    Public Function GetSuburb(CityId As String) As JsonResult
        If CityId.Trim = "" Then Return Json(0)
        Dim suburbs = db.Suburbs.Where(Function(c) c.CityId = CityId).Select(Function(c) New With {.Id = c.SuburbId, .Name = c.SuburbName}).ToList()
        Return Json(suburbs)
    End Function

    Public Function ResizeImage(stream As Stream, width As System.Nullable(Of Integer), height As System.Nullable(Of Integer)) As Bitmap
        Dim bmpOut As System.Drawing.Bitmap = Nothing
        Const defaultWidth As Integer = 800
        Const defaultHeight As Integer = 600
        Dim lnWidth As Integer = If(width Is Nothing, defaultWidth, CInt(width))
        Dim lnHeight As Integer = If(height Is Nothing, defaultHeight, CInt(height))
        Try
            Dim loBMP As New Bitmap(stream)
            Dim loFormat As ImageFormat = loBMP.RawFormat

            Dim lnRatio As Decimal
            Dim lnNewWidth As Integer = 0
            Dim lnNewHeight As Integer = 0

            '*** If the image is smaller than a thumbnail just return it
            If loBMP.Width < lnWidth AndAlso loBMP.Height < lnHeight Then
                Return loBMP
            End If

            If loBMP.Width > loBMP.Height Then
                lnRatio = CDec(lnWidth) / loBMP.Width
                lnNewWidth = lnWidth
                Dim lnTemp As Decimal = loBMP.Height * lnRatio
                lnNewHeight = CInt(Math.Truncate(lnTemp))
            Else
                lnRatio = CDec(lnHeight) / loBMP.Height
                lnNewHeight = lnHeight
                Dim lnTemp As Decimal = loBMP.Width * lnRatio
                lnNewWidth = CInt(Math.Truncate(lnTemp))
            End If

            bmpOut = New Bitmap(lnNewWidth, lnNewHeight)
            Dim g As Graphics = Graphics.FromImage(bmpOut)
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic
            g.FillRectangle(Brushes.White, 0, 0, lnNewWidth, lnNewHeight)
            g.DrawImage(loBMP, 0, 0, lnNewWidth, lnNewHeight)
            loBMP.Dispose()
        Catch
            Return Nothing
        End Try
        Return bmpOut
    End Function

    Private Shared Function GetEncoderInfo(ByVal format As ImageFormat) As ImageCodecInfo
        Dim j As Integer
        Dim encoders() As ImageCodecInfo
        encoders = ImageCodecInfo.GetImageEncoders()

        j = 0
        While j < encoders.Length
            If encoders(j).FormatID = format.Guid Then
                Return encoders(j)
            End If
            j += 1
        End While
        Return Nothing

    End Function 'GetEncoderInfo

    Public Function Watchlist(id As Integer)
        If HttpContext.User.Identity.Name <> "" Then
            Dim count As Integer = db.WatchLists.Where(Function(w) w.UserId = HttpContext.User.Identity.Name And w.ListingId = id).Count

            If count = 0 Then
                Dim watchlisting As New WatchList

                watchlisting.ListingId = id
                watchlisting.Status = 0
                watchlisting.UserId = HttpContext.User.Identity.Name
                watchlisting.Timestamp = Now
                db.WatchLists.Add(watchlisting)
                db.SaveChanges()

                Return "True"
            Else
                Return "True"
            End If

        Else
            Return "redirect"

        End If


    End Function

End Class