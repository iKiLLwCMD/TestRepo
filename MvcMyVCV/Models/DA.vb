Imports System.ComponentModel.DataAnnotations
Imports System.Net.Http
Imports DataAnnotationsExtensions


<MetadataType(GetType(UserMD))>
Partial Public Class User
End Class

<MetadataType(GetType(ClassMD))>
Partial Public Class [Class]
End Class

<MetadataType(GetType(AssignmentMD))>
Partial Public Class Assignment
End Class

<MetadataType(GetType(ListingMD))>
Partial Public Class Listing
End Class


Public Class UserMD

    '<Remote("doesUserNameExist", "Account", HttpMethod:="POST", ErrorMessage:="User Id already exists. Please enter a different user Id.")>
    Private _UserId As Object
    <Required()> _
    <Display(Name:="User Id")> _
    <UserNameValidation(ErrorMessage:="User Id already exists. Please enter a different user Id.")>
    Public Property UserId() As Object
        Get
            Return _UserId
        End Get
        Set(ByVal value As Object)
            _UserId = value
        End Set
    End Property

    Private _UserName As Object
    <Required()> _
    <Display(Name:="Name")> _
    Public Property UserName() As Object
        Get
            Return _UserName
        End Get
        Set(ByVal value As Object)
            _UserName = value
        End Set
    End Property

    Private _UserEmail As Object
    <Required()> _
    <Display(Name:="Email Address")> _
    <DataType(DataType.EmailAddress)> _
    <Email> _
    Public Property UserEmail() As Object
        Get
            Return _UserEmail
        End Get
        Set(ByVal value As Object)
            _UserEmail = value
        End Set
    End Property

    Private _UserPassword As Object
    <Required()> _
    <Display(Name:="Password")> _
    <DataType(DataType.Password)> _
    Public Property UserPassword() As Object
        Get
            Return _UserPassword
        End Get
        Set(ByVal value As Object)
            _UserPassword = value
        End Set
    End Property

    Private _Terms As Object
    <Required()> _
     <CustomValidation(GetType(BoolValidation), "ValidateBool")> _
    Public Property Terms() As Object
        Get
            Return _Terms
        End Get
        Set(ByVal value As Object)
            _Terms = value
        End Set
    End Property





End Class


Public Class ClassMD

    Private _ClassName As Object
    <Required()> _
    <Display(Name:="Class Name")> _
    Public Property ClassName() As Object
        Get
            Return _ClassName
        End Get
        Set(ByVal value As Object)
            _ClassName = value
        End Set
    End Property

    Private _SchoolName As Object
    <Required()> _
    <Display(Name:="School Name")> _
    Public Property SchoolName() As Object
        Get
            Return _SchoolName
        End Get
        Set(ByVal value As Object)
            _SchoolName = value
        End Set
    End Property

    Private _CountryId As Object
    <Required()> _
    <Display(Name:="Country")> _
    Public Property CountryId() As Object
        Get
            Return _CountryId
        End Get
        Set(ByVal value As Object)
            _CountryId = value
        End Set
    End Property

    Private _GradeId As Object
    <Required()> _
    <Display(Name:="Grade")> _
    Public Property GradeId() As Object
        Get
            Return _GradeId
        End Get
        Set(ByVal value As Object)
            _GradeId = value
        End Set
    End Property

    Private _Subject As Object
    <Required()> _
    <Display(Name:="Class Subject")> _
    Public Property Subject() As Object
        Get
            Return _Subject
        End Get
        Set(ByVal value As Object)
            _Subject = value
        End Set
    End Property

    Private _NeedsApprovalToShare As Object
    <Required()> _
    <Display(Name:="Students Need Approval To Share?")> _
    Public Property NeedsApprovalToShare() As Object
        Get
            Return _NeedsApprovalToShare
        End Get
        Set(ByVal value As Object)
            _NeedsApprovalToShare = value
        End Set
    End Property

    Private _PostalCode As Object
    <Display(Name:="Postal Code")> _
    <Required(ErrorMessage:="Postal Code is required")> _
    Public Property PostalCode() As Object
        Get
            Return _PostalCode
        End Get
        Set(ByVal value As Object)
            _PostalCode = value
        End Set
    End Property


End Class


Public Class AssignmentMD

    Private _AssignmentName As Object
    <Required()> _
    <Display(Name:="Assignment Name")> _
    <StringLength(50, ErrorMessage:="No more than 50 characters.")>
    Public Property AssignmentName() As Object
        Get
            Return _AssignmentName
        End Get
        Set(ByVal value As Object)
            _AssignmentName = value
        End Set
    End Property

    Private _AssignmentDueDate As Object
    <Display(Name:="Assignment Due Date")> _
    <DisplayFormat(ApplyFormatInEditMode:=True, DataFormatString:="{MM/dd/yyyy}")>
    Public Property AssignmentDueDate() As Object
        Get
            Return _AssignmentDueDate
        End Get
        Set(ByVal value As Object)
            _AssignmentDueDate = value
        End Set
    End Property

    Private _ClassId As Object
    <Required()> _
    <Display(Name:="Class")> _
    Public Property ClassId() As Object
        Get
            Return _ClassId
        End Get
        Set(ByVal value As Object)
            _ClassId = value
        End Set
    End Property


End Class

Public Class ListingMD

    Public Property ListingId As Integer
    <Required()> _
    Public Property ListingTitle As String
    <Required()> _
    Public Property Company As String
    <Required()> _
    <Range(1, Integer.MaxValue, ErrorMessage:="The Sub Category field is Required")>
    Public Property SubCategoryId As Integer
    <Required()> _
    Public Property ListingTypeId As Integer
    <Required()> _
    Public Property ListingPayId As Integer
    Public Property ListingReference As String
    <Required()> _
    <StringLength(300, ErrorMessage:="No more than 300 characters.")>
    Public Property ListingSummary As String
    <Required()> _
    Public Property ListingDescription As String
    <Required()> _
    Public Property IsAdvertised As Boolean
    Public Property UserId As String
    <Required()> _
    Public Property SuburbId As Integer

End Class









