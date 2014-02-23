


Public Class RoleAttribute : Inherits AuthorizeAttribute
    Public Sub New()

    End Sub


    Protected Overrides Function AuthorizeCore(httpContext As HttpContextBase) As Boolean
        Dim db As New VCVOnlineEntities
        If httpContext.Request.IsAuthenticated = False Then
            Return False

        End If
        'get user role
        Dim role = db.Users.FirstOrDefault(Function(u) u.UserId = httpContext.User.Identity.Name)


        For Each definedRole As String In Me.Roles.Split(",")
            If role.UserType = definedRole.ToString.Trim Then
                Return True
            End If

        Next

        Return False


    End Function

    Public Overrides Sub OnAuthorization(filterContext As AuthorizationContext)
        Dim user = filterContext.HttpContext.User
        Dim db As New VCVOnlineEntities
       
        If HttpContext.Current.Request.IsAuthenticated = True Then
            'Your code to get the list of roles for the current user
            Dim user1 = db.Users.FirstOrDefault(Function(u) u.UserId = HttpContext.Current.User.Identity.Name)
            Dim role() As String = user1.UserType.Split(",")
            Dim formsIdentity = TryCast(filterContext.HttpContext.User.Identity, FormsIdentity)
            filterContext.HttpContext.User = New System.Security.Principal.GenericPrincipal(formsIdentity, role)
        End If




        MyBase.OnAuthorization(filterContext)
    End Sub

    Protected Overrides Sub HandleUnauthorizedRequest(filterContext As AuthorizationContext)
        filterContext.Result = New RedirectResult("/")
        MyBase.HandleUnauthorizedRequest(filterContext)

    End Sub

End Class



'Public Class CustomValidateAuthorization : Inherits AuthorizeAttribute
'    Public Sub New()

'    End Sub

'    Protected Overrides Function AuthorizeCore(ByVal httpContext As System.Web.HttpContextBase) As Boolean
'        If httpContext.Request.IsAuthenticated = False Then

'            Return False


'        End If
'        For Each definedRole As String In Me.Roles.Split(",")
'            Return True
'        Next

'        Return False



'        Return True
'    End Function
'End Class


