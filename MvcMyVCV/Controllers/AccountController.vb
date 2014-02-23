Imports System.Data.Entity
Imports System.Security.Cryptography.SHA512
Imports System.Data
Imports System.Net
Imports System.IO
Imports System.Xml
Imports MvcMyVCV.PaymentExpress.PxPay
Imports System.Net.Mail

Public Class AccountController
    Inherits System.Web.Mvc.Controller

    Private db As New VCVOnlineEntities


    Function Reverse(ByVal value As String) As String
        ' Convert to char array.
        Dim arr() As Char = value.ToCharArray()
        ' Use Array.Reverse function.
        Array.Reverse(arr)
        ' Construct new string.
        Return New String(arr)
    End Function

    '
    ' GET: /Account/Register
    '<RequireHttps> _
    Function Register() As ActionResult

        Return View()
    End Function



    '
    ' POST: /Account/Register
    <HttpPost()> _
<ValidateAntiForgeryToken()> _
    Function Register(ByVal user As User, col As FormCollection) As ActionResult
        If ModelState.IsValid Then
            user.UserType = col("UserType")
            Dim encoder As Encoding = New UTF8Encoding()
            Dim sha As New System.Security.Cryptography.SHA512Managed()
            Dim passwordHash As Byte() = sha.ComputeHash(encoder.GetBytes(Reverse(user.UserId) & user.UserPassword))
            user.UserPassword = Convert.ToBase64String(passwordHash)

            If col("UserType") <> "Employee" Then user.Completed = True Else user.Completed = False

            If col("UserType") = "Demo" Then
                user.PurchaseDate = Now()
                user.NextBillingDate = Now.AddDays(7)
                user.LastBillingDate = Now()
            End If


            db.Users.Add(user)

            db.SaveChanges()
            Dim id As String = user.UserId
            Session("NewUserId") = id

            If col("UserType") = "Employee" Then
                Return RedirectToAction("Payment")
            Else
                Return RedirectToAction("SignupComplete")
            End If

        End If


        Return View(user)
    End Function

    Public Function SignupComplete() As ActionResult
        Return View()
    End Function




    '<RequireHttps> _
    Function Login() As ActionResult
        Return View()
    End Function

    '
    ' POST: /Account/Login

    <HttpPost()> _
    Function Login(ByVal user As LoginViewModel) As ActionResult
        If ModelState.IsValid Then
            Dim user__1 = db.Users.FirstOrDefault(Function(u) u.UserId.ToLower = user.UserId.ToLower)

            'Dim user__1 = db.Users.FirstOrDefault(Function(u) String.Compare(u.UserId, user.UserId, System.StringComparison.OrdinalIgnoreCase))

            If user__1 Is Nothing Then
                ModelState.AddModelError("", "That user name doesn't exist.")
                Return View(user)

            End If
            Dim sPassword As String
            'generate password hash
            If user__1.Activated = False And user__1.UserType = "Student" Then
                'check password unhashed
                sPassword = user.UserPassword
            Else
                'hash password
                Dim encoder As Encoding = New UTF8Encoding()
                Dim sha As New System.Security.Cryptography.SHA512Managed()
                Dim passwordHash As Byte() = sha.ComputeHash(encoder.GetBytes(Reverse(user__1.UserId) & user.UserPassword))
                sPassword = Convert.ToBase64String(passwordHash)
            End If


            If user__1.UserPassword <> sPassword Then

                ModelState.AddModelError("", "The password supplied is incorrect.")
                Return View(user)

            End If

            If user__1.Completed = False Or user__1.Completed Is Nothing Then
                ModelState.AddModelError("", "The account has not been completed.")
                Return View(user)

            End If


            If user__1.UserType = "Demo" Or user__1.UserType = "Employee" Then


                If Now() > user__1.NextBillingDate Or user__1.NextBillingDate Is Nothing Then
                    Session("RenewUserId") = user__1.UserId
                    Return RedirectToAction("Renew", "Account")
                    'ModelState.AddModelError("", "The account has expired.")
                    'Return View(user)

                End If

            End If




            If user__1.Removed = True Then
                ModelState.AddModelError("", "The account has been removed.")
                Return View(user)

            End If


            'UserPk = user__1.UserPk 
            FormsAuthentication.SetAuthCookie(user__1.UserId, False)
            'Roles.AddUserToRole(user.UserId, "Teacher")

            'HttpContext.User.Identity.Name


            Return RedirectToAction("Index", "Home")


        End If

        Return View(user)
    End Function

    Function LogOff() As ActionResult
        FormsAuthentication.SignOut()
        Return (RedirectToAction("Login", "Account"))

    End Function

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        db.Dispose()
        MyBase.Dispose(disposing)
    End Sub

    <HttpPost> _
    Public Function doesUserNameExist(UserId As String) As JsonResult

        Dim user = db.Users.FirstOrDefault(Function(u) u.UserId = UserId)

        Return Json(user Is Nothing)
    End Function

    <HttpPost> _
    Public Function doesOldPasswordMatch(OldPassword As String) As JsonResult
        Dim userid As String = HttpContext.User.Identity.Name
        Dim user__1 = db.Users.FirstOrDefault(Function(u) u.UserId.ToLower = userid.ToLower)
        Dim sPassword As String
        If user__1.UserType <> "Student" Then
            'hash password
            Dim encoder As Encoding = New UTF8Encoding()
            Dim sha As New System.Security.Cryptography.SHA512Managed()
            Dim passwordHash As Byte() = sha.ComputeHash(encoder.GetBytes(Reverse(user__1.UserId) & OldPassword))
            sPassword = Convert.ToBase64String(passwordHash)
        Else
            sPassword = OldPassword

        End If




        Dim user = db.Users.FirstOrDefault(Function(u) u.UserId.ToLower = userid.ToLower And u.UserPassword = sPassword)
        If user Is Nothing Then
            Return Json("Old password is incorrect. Please enter your current password.")
        Else
            Return Json(True)
        End If

    End Function

    '
    ' GET: /Account/Create
    <Authorize> _
    Function ChangePassword() As ActionResult

        Return View()


    End Function

    '
    ' POST: /Account/ChangePassword

    <HttpPost> _
    Function ChangePassword(col As FormCollection) As ActionResult
        Dim userid = HttpContext.User.Identity.Name
        Dim user__1 = db.Users.FirstOrDefault(Function(u) u.UserId.ToLower = userid.ToLower)

        Dim sPassword As String
        If user__1.UserType <> "Student" Then
            Dim encoder As Encoding = New UTF8Encoding()
            Dim sha As New System.Security.Cryptography.SHA512Managed()
            Dim passwordHash As Byte() = sha.ComputeHash(encoder.GetBytes(Reverse(user__1.UserId) & col("OldPassword")))
            sPassword = Convert.ToBase64String(passwordHash)
        Else
            sPassword = col("OldPassword")
        End If

        Dim user = db.Users.SingleOrDefault(Function(u) u.UserId = userid)
        If user.UserPassword <> sPassword Then
            ModelState.AddModelError("", "The old password doesn't match.")
            Return View()

        End If

        'hash password
        Dim encodernew As Encoding = New UTF8Encoding()
        Dim shanew As New System.Security.Cryptography.SHA512Managed()
        Dim passwordHashNew As Byte() = shanew.ComputeHash(encodernew.GetBytes(Reverse(user__1.UserId) & col("Password")))
        sPassword = Convert.ToBase64String(passwordHashNew)
        user.UserPassword = sPassword
        user.Activated = True
        db.Configuration.ValidateOnSaveEnabled = False
        db.SaveChanges()
        Return RedirectToAction("Index", "Home")
        ' End If


    End Function


    Function Payment() As ActionResult
        Dim userid As String = Session("NewUserId")
        Dim user = db.Users.SingleOrDefault(Function(u) u.UserId = userid)


        If user.UserType = "Employee" Then
            Dim p As Product = db.Products.Find(1)
            ViewBag.Product = p.ProductName
            ViewBag.Total = Format(p.Price, "0.00")
            ViewBag.Tax = Format((ViewBag.Total * 3) / 23, "0.00")
            ViewBag.SubTotal = ViewBag.Total - ViewBag.Tax
        End If

        'If user.UserType = "Employee" Then
        '    ViewBag.Product = "MyChatPack Teacher Edition"
        '    ViewBag.Total = CDbl(db.Products.Find(1).Price)
        'End If
        'If user.Country.CountryName.Trim = "New Zealand" Then
        '    ViewBag.Tax = Format((ViewBag.Total * 3) / 23, "0.00")
        'Else
        '    ViewBag.Tax = 0
        'End If
        'ViewBag.Subtotal = Format(ViewBag.Total - ViewBag.Tax, "0.00")
        'ViewBag.StartDate = Now.ToShortDateString
        'ViewBag.EndDate = Now.AddMonths(12).ToShortDateString


        Return View()
    End Function

    <HttpPost> _
    Function Payment(Recurring As Boolean) As ActionResult
        Dim userid As String = Session("NewUserId")
        Dim user = db.Users.SingleOrDefault(Function(u) u.UserId = userid)
        Dim price As Decimal

        'save transcation
        Dim tran As New Tran


        tran.UserId = user.UserId


        If user.UserType = "Employee" Then
            Dim p As Product = db.Products.Find(1)
            price = p.Price
            tran.TranType = p.ProductName
        End If



        tran.TranAmount = price
        tran.TimeStamp = Now()
        db.Trans.Add(tran)
        db.SaveChanges()

        Dim PxPayUserId As String = ConfigurationManager.AppSettings("PxPayUserId")
        Dim PxPayKey As String = ConfigurationManager.AppSettings("PxPayKey")

        Dim WS As New PxPay(PxPayUserId, PxPayKey)

        Dim input As New RequestInput()

        input.AmountInput = Math.Round(price, 2)
        input.CurrencyInput = "USD"
        input.MerchantReference = "Virtual Technologies"
        input.TxnType = "Purchase"
        input.UrlFail = Request.Url.ToString().Substring(0, Request.Url.ToString().LastIndexOf("/")) & "/" & "PaymentFailed"
        input.UrlSuccess = Request.Url.ToString().Substring(0, Request.Url.ToString().LastIndexOf("/")) & "/" & "PurchaseComplete"

        input.TxnData1 = userid
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

    Public Function PaymentFailed() As ActionResult
        Return View()
    End Function

    Function PurchaseComplete() As ActionResult
        Dim ResultQs As String = Request.QueryString("result")

        If Not String.IsNullOrEmpty(ResultQs) Then

            Dim PxPayUserId As String = ConfigurationManager.AppSettings("PxPayUserId")
            Dim PxPayKey As String = ConfigurationManager.AppSettings("PxPayKey")

            ' Obtain the transaction result 
            Dim WS As New PxPay(PxPayUserId, PxPayKey)

            Dim output As ResponseOutput = WS.ProcessResponse(ResultQs)

            ' Sending invoices/updating order status within database etc.


            If output.valid = "1" AndAlso output.Success = "1" Then
                ' TODO: Send emails, generate invoices, update order status etc.

                Try
                    Dim userid As String = output.TxnData1.ToString
                    Dim user = db.Users.SingleOrDefault(Function(u) u.UserId = userid)
                    user.PurchaseDate = Now()
                    user.LastBillingDate = Now()
                    user.NextBillingDate = Now.AddMonths(12)
                    user.Completed = True
                    user.RecurringBillingId = output.DpsBillingId
                    db.Configuration.ValidateOnSaveEnabled = False
                    db.SaveChanges()

                    'update transaction
                    Dim tranid As Integer = output.TxnData2
                    Dim tran = db.Trans.SingleOrDefault(Function(u) u.TranId = tranid)
                    tran.Completed = True
                    db.Configuration.ValidateOnSaveEnabled = True
                    db.SaveChanges()
                Catch ex As Exception

                End Try


            End If
        Else
            Return RedirectToAction("Error", "Account")
        End If



        Return View()
    End Function

    Function ChangeRequest() As ActionResult

        Return View()

    End Function

    <HttpPost()> _
    Function ChangeRequest(ByVal change As ForgotModel) As ActionResult

        Dim user = db.Users.SingleOrDefault(Function(u) u.UserId = change.UserId)
        If user Is Nothing = False Then

            'create guid and save datetime + 30min
            user.ForgotToken = System.Guid.NewGuid.ToString
            user.ForgotDateLimit = Now.AddMinutes(30)
            db.Configuration.ValidateOnSaveEnabled = False
            db.SaveChanges()
            'send email
            Dim mail As New MailMessage
            mail.To.Add(user.UserEmail.ToString)
            mail.From = New MailAddress("no-reply@MyChatPak.co.nz")
            mail.Subject = "MyChatPack Forgotten Password"
            mail.IsBodyHtml = True
            mail.Body = "<html><body><center><img src=""http://www.mychatpack.co.nz/Images/MyChatPack_logo.png"" boder=""0"" /></center><br /><br />Dear " & user.UserName & ",<br /><br />You have received this email because you have requested a password reset because you forgot your password.<br /><br />Click <a href=""http://www.mychatpack.co.nz/Account/ForgotPassword?" & user.ForgotToken & """>HERE</a> to change your password.<br/><br />If you did not request this then please ignore this email.<br /><br />Kind Regards, <br /> The MyChatPack Team</body></html>"

            'send the message
            Dim smtp As New SmtpClient("mail.licensing.my-vcv.com")
            smtp.Port = 49
            smtp.Timeout = 60000
            smtp.Credentials = New System.Net.NetworkCredential("vcv@licensing.my-vcv.com", "Count548")
            'smtp.EnableSsl = True


            smtp.Send(mail)

            Return RedirectToAction("TeacherReset", "Account")



        Else
            Return RedirectToAction("Error", "Account")
        End If

        Return View()


    End Function

    Function Terms() As ActionResult

        Return View()

    End Function

    Function StudentReset() As ActionResult

        Return View()

    End Function

    Function TeacherReset() As ActionResult

        Return View()

    End Function


    Function ForgotPassword() As ActionResult

        Return View()


    End Function

    '
    ' POST: /Account/ChangePassword

    <HttpPost> _
    Function ForgotPassword(col As FormCollection) As ActionResult
        Dim token As String = Request.QueryString.ToString
        Dim user = db.Users.FirstOrDefault(Function(u) u.ForgotToken = token And u.ForgotDateLimit > Now)
        Dim sPassword As String = col("Password")

        If user Is Nothing Then
            Return RedirectToAction("Error", "Account")
        Else
            'hash password
            Dim encodernew As Encoding = New UTF8Encoding()
            Dim shanew As New System.Security.Cryptography.SHA512Managed()
            Dim passwordHashNew As Byte() = shanew.ComputeHash(encodernew.GetBytes(Reverse(user.UserId) & col("Password")))
            sPassword = Convert.ToBase64String(passwordHashNew)
            user.UserPassword = sPassword
            db.Configuration.ValidateOnSaveEnabled = False
            db.SaveChanges()
            Return RedirectToAction("Login", "Account")
        End If



    End Function

    Function RenewComplete() As ActionResult
        Dim ResultQs As String = Request.QueryString("result")

        If Not String.IsNullOrEmpty(ResultQs) Then

            Dim PxPayUserId As String = ConfigurationManager.AppSettings("PxPayUserId")
            Dim PxPayKey As String = ConfigurationManager.AppSettings("PxPayKey")

            ' Obtain the transaction result 
            Dim WS As New PxPay(PxPayUserId, PxPayKey)

            Dim output As ResponseOutput = WS.ProcessResponse(ResultQs)

            ' Sending invoices/updating order status within database etc.

            If output.valid = "1" AndAlso output.Success = "1" Then
                ' TODO: Send emails, generate invoices, update order status etc.
                Try
                    Dim userid As String = output.TxnData1
                    Dim user = db.Users.SingleOrDefault(Function(u) u.UserId = userid)
                    user.LastBillingDate = Now()
                    user.NextBillingDate = Now.AddMonths(12)
                    user.RecurringBillingId = output.DpsBillingId
                    db.SaveChanges()

                    'update transaction
                    Dim tranid As Integer = output.TxnData2
                    Dim tran = db.Trans.SingleOrDefault(Function(u) u.TranId = tranid)
                    tran.Completed = True
                    db.SaveChanges()
                Catch ex As Exception

                End Try



            End If
        Else
            Return RedirectToAction("Error", "Account")
        End If



        Return RedirectToAction("Login", "Account")
    End Function

    Function Renew() As ActionResult
        Dim userid As String = Session("RenewUserId").ToString
        If userid Is Nothing Then Return RedirectToAction("Error", "Account")
        Dim user = db.Users.SingleOrDefault(Function(u) u.UserId = userid)



        ViewBag.Product = "MyChatPack Home Edition"
        ViewBag.Total = CDbl(db.Products.Find(1).Price)

        If user.Country.CountryName.Trim = "New Zealand" Then
            ViewBag.Tax = Format((CDbl(db.Products.Find(1).Price) * 3) / 23, "0.00")
        Else
            ViewBag.Tax = 0
        End If
        ViewBag.Subtotal = Format(ViewBag.Total - ViewBag.Tax, "0.00")
        ViewBag.StartDate = Now.ToShortDateString
        ViewBag.EndDate = Now.AddMonths(12).ToShortDateString

        If user.RecurringBillingId.ToString <> "" Then
            ViewBag.Recurring = False
        Else
            ViewBag.Recurring = True
        End If

        Return View(user)
    End Function

    <HttpPost> _
    Function Renew(Recurring As Boolean) As ActionResult
        'if recurring then try transaction
        Dim userid As String = Session("RenewUserId").ToString
        Dim user = db.Users.SingleOrDefault(Function(u) u.UserId = userid)

        If user.RecurringBillingId.ToString <> "" Then
            'try transaction using billing id


            'save transcation
            Dim tran As New Tran
            Dim price As Double

            tran.UserId = user.UserId


            price = CDbl(db.Products.Find(1).Price)
            tran.TranType = "Home Edition Renewal"

            tran.TranAmount = price
            tran.TimeStamp = Now()
            db.Trans.Add(tran)
            db.SaveChanges()

            Dim PxPayUserId As String = ConfigurationManager.AppSettings("PxPayUserId")
            Dim PxPayKey As String = ConfigurationManager.AppSettings("PxPayKey")

            Dim WS As New PxPay(PxPayUserId, PxPayKey)

            Dim input As New RequestInput()

            input.AmountInput = Math.Round(CDbl(db.Products.Find(1).Price), 2)
            input.CurrencyInput = "USD"
            input.MerchantReference = "Virtual Technologies"
            input.TxnType = "Purchase"
            input.UrlFail = Request.Url.ToString().Substring(0, Request.Url.ToString().LastIndexOf("/")) & "/" & "Error"
            input.UrlSuccess = Request.Url.ToString().Substring(0, Request.Url.ToString().LastIndexOf("/")) & "/" & "RenewComplete"
            input.TxnData1 = user.UserId
            input.TxnData2 = tran.TranId
            input.DpsBillingId = user.RecurringBillingId

            Dim output As RequestOutput = WS.GenerateRequest(input)

            If output.valid = "1" Then
                ' Redirect user to payment page 

                Return Redirect(output.Url)
            Else
                Return View()
            End If


        Else


            'save transcation
            Dim tran As New Tran
            Dim price As Double

            tran.UserId = user.UserId

            price = CDbl(db.Products.Find(1).Price)
            tran.TranType = "Home Edition Renewal"

            tran.TranAmount = price
            tran.TimeStamp = Now()
            db.Trans.Add(tran)
            db.SaveChanges()

            Dim PxPayUserId As String = ConfigurationManager.AppSettings("PxPayUserId")
            Dim PxPayKey As String = ConfigurationManager.AppSettings("PxPayKey")

            Dim WS As New PxPay(PxPayUserId, PxPayKey)

            Dim input As New RequestInput()

            input.AmountInput = Math.Round(CDbl(db.Products.Find(1).Price), 2)
            input.CurrencyInput = "USD"
            input.MerchantReference = "Virtual Technologies"
            input.TxnType = "Purchase"
            input.UrlFail = Request.Url.ToString().Substring(0, Request.Url.ToString().LastIndexOf("/")) & "/" & "Error"
            input.UrlSuccess = Request.Url.ToString().Substring(0, Request.Url.ToString().LastIndexOf("/")) & "/" & "RenewComplete"
            input.TxnData1 = user.UserId
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


        End If



    End Function




End Class