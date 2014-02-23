Imports System.ComponentModel.DataAnnotations

Public Class UserNameValidation
    Inherits ValidationAttribute

    Private db As New VCVOnlineEntities
 Public Overrides Function IsValid(value As Object) As Boolean
        Dim userid = value.ToString
        Dim user = db.Users.FirstOrDefault(Function(u) u.UserId = userid)

        If user Is Nothing Then
            Return True
        Else
            Return False
        End If

    End Function
End Class
