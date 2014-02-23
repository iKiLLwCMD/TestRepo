'Namespace Chat.Models.Security


Public Class ChatRoleProvider : Inherits RoleProvider

    Public Sub New()

    End Sub

    Public Overrides Sub AddUsersToRoles(usernames As String(), roleNames As String())
        Throw New NotImplementedException()
    End Sub

    Public Overrides Property ApplicationName() As String
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As String)
            Throw New NotImplementedException()
        End Set
    End Property

    Public Overrides Sub CreateRole(roleName As String)
        Throw New NotImplementedException()
    End Sub

    Public Overrides Function DeleteRole(roleName As String, throwOnPopulatedRole As Boolean) As Boolean
        Throw New NotImplementedException()
    End Function

    Public Overrides Function FindUsersInRole(roleName As String, usernameToMatch As String) As String()
        Throw New NotImplementedException()
    End Function

    Public Overrides Function GetAllRoles() As String()
        Throw New NotImplementedException()
    End Function

    Public Overrides Function GetRolesForUser(username As String) As String()

        Dim db As New VCVOnlineEntities
        'Your code to get the list of roles for the current user
        Dim user1 = db.Users.FirstOrDefault(Function(u) u.UserId = username)
        Dim role() As String = user1.UserType.Split(",")

        Return role

    End Function

    Public Overrides Function GetUsersInRole(roleName As String) As String()
        Throw New NotImplementedException()
    End Function

    Public Overrides Function IsUserInRole(username As String, roleName As String) As Boolean
        Dim db As New VCVOnlineEntities
        If HttpContext.Current.Request.IsAuthenticated = False Then
            Return False

        End If
        'get user role
        Dim role = db.Users.FirstOrDefault(Function(u) u.UserId = username)

        If role.UserType = roleName.ToString.Trim Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Overrides Sub RemoveUsersFromRoles(usernames As String(), roleNames As String())
        Throw New NotImplementedException()
    End Sub

    Public Overrides Function RoleExists(roleName As String) As Boolean
        Throw New NotImplementedException()
    End Function




End Class

'End Namespace
