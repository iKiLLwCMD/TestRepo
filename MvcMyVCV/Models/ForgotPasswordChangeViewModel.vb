Imports System.ComponentModel.DataAnnotations

Public Class ForgotPasswordChangeViewModel

    <StringLength(10, MinimumLength:=6)> _
    <Display(Name:="New Password")> _
    Public Property Password() As String
        Get
            Return m_Password
        End Get
        Set(value As String)
            m_Password = value
        End Set
    End Property
    Private m_Password As String
    <Compare("Password")> _
    <Display(Name:="Confirm New Password")> _
    Public Property ConfirmPassword() As String
        Get
            Return m_ConfirmPassword
        End Get
        Set(value As String)
            m_ConfirmPassword = value
        End Set
    End Property
    Private m_ConfirmPassword As String

End Class
