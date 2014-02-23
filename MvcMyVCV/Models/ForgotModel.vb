Imports System.ComponentModel.DataAnnotations



Public Class ForgotModel

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


End Class