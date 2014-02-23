Imports System.ComponentModel.DataAnnotations



Public Class CreateStudentViewModel

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



    <Display(Name:="Name")> _
    <Required()> _
    Public Property UserName() As String
        Get
            Return m_UserName
        End Get
        Set(value As String)
            m_UserName = value
        End Set
    End Property
    Private m_UserName As String

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

    <Display(Name:="Class")> _
    <Required()> _
    Public Property ClassId() As String
        Get
            Return m_ClassId
        End Get
        Set(value As String)
            m_ClassId = value
        End Set
    End Property
    Private m_ClassId As String


End Class
