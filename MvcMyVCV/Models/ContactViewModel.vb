Imports System.ComponentModel.DataAnnotations



Public Class ContactViewModel
    <Required> _
    <DataType(DataType.EmailAddress)> _
    <Display(Name:="Your Email Address")> _
    Public Property From() As String
        Get
            Return m_From
        End Get
        Set(value As String)
            m_From = value
        End Set
    End Property
    Private m_From As String

    <Required> _
    Public Property Subject() As String
        Get
            Return m_Subject
        End Get
        Set(value As String)
            m_Subject = value
        End Set
    End Property
    Private m_Subject As String

    <Required> _
    <DataType(DataType.MultilineText)> _
    Public Property Message() As String
        Get
            Return m_Message
        End Get
        Set(value As String)
            m_Message = value
        End Set
    End Property
    Private m_Message As String

    <Required> _
    <Display(Name:="Name")> _
    Public Property sName() As String
        Get
            Return m_sName
        End Get
        Set(value As String)
            m_sName = value
        End Set
    End Property
    Private m_sName As String
End Class

