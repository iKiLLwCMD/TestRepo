Imports System.ComponentModel.DataAnnotations



Public Class LoginViewModel

    <Display(Name:="UserId")> _
    <Required()> _
    Public Property UserId() As String
        Get
            Return m_UserId
        End Get
        Set(value As String)
            m_UserId = value
        End Set
    End Property
    Private m_UserId As String



    <Display(Name:="Password")> _
    <Required()> _
    Public Property UserPassword() As String
        Get
            Return m_UserPassword
        End Get
        Set(value As String)
            m_UserPassword = value
        End Set
    End Property
    Private m_UserPassword As String

End Class
