
'------------------------------------------------------------------------------
' <auto-generated>
'    This code was generated from a template.
'
'    Manual changes to this file may cause unexpected behavior in your application.
'    Manual changes to this file will be overwritten if the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Collections.Generic


Partial Public Class Script

    Public Property ScriptId As Integer

    Public Property ScriptName As String

    Public Property ScriptContent As String

    Public Property UserId As String

    Public Property TimeStamp As Date

    Public Property Template As Boolean

    Public Property Hidden As Boolean



    Public Overridable Property User As User

    Public Overridable Property VCVs As ICollection(Of VCV) = New HashSet(Of VCV)


End Class
