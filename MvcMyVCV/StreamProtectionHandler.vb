Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.IO
Imports System.Web.SessionState

Public Class StreamProtectionHandler
    Implements IHttpHandler
    Implements IRequiresSessionState

    Private db As New VCVOnlineEntities
    '
    ' TODO: Add constructor logic here
    '
    Public Sub New()
    End Sub

    Private ReadOnly Property IHttpHandler_IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return True
        End Get
    End Property

    Private Sub IHttpHandler_ProcessRequest(context As HttpContext) Implements IHttpHandler.ProcessRequest
        '' Define your Domain Name Here
        Dim strDomainName As [String] = context.Session("DomainName").ToString()
        ' Add the RELATIVE folder where you keep your stuff here
        Dim strFolder As [String] = "~/Streams/"
        ' Add the RELATIVE PATH of your "no" image
        Dim strNoImage As [String] = ""
        ' if this is set to null or empty string then an empty response is return as per 'maxxnostra's comment on Codeproject. Thanks.

        Select Case context.Request.HttpMethod
            Case "GET"
                Dim strRequestedFile As [String] = context.Server.MapPath(context.Request.FilePath)
                Dim result As Boolean
                If context.Request.UrlReferrer IsNot Nothing Then
                    Dim strUrlRef As [String] = context.Request.UrlReferrer.ToString()
                    Dim strUrlImageFull As [String] = ResolveUrl(strFolder)
                    If strUrlRef.Contains(strUrlImageFull) Then
                        context = SendContentTypeAndFile(context, strNoImage)
                    ElseIf strUrlRef.StartsWith(strDomainName) Or strUrlRef = "http://releases.flowplayer.org/swf/flowplayer-3.2.16.swf" Then
                        result = CheckPermissions(strRequestedFile)

                        If result = True Then
                            context = SendContentTypeAndFile(context, strRequestedFile)
                        Else
                            context = SendContentTypeAndFile(context, strNoImage)
                        End If

                    Else
                        context = SendContentTypeAndFile(context, strNoImage)
                    End If
                Else
                    context = SendContentTypeAndFile(context, strNoImage)
                End If
                Exit Select
            Case "POST"
                context = SendContentTypeAndFile(context, strNoImage)
                Exit Select
        End Select
    End Sub

    Public Function CheckPermissions(ByVal file As String) As Boolean
        Dim UserId As String = HttpContext.Current.User.Identity.Name
        Dim filename As String = Path.GetFileNameWithoutExtension(file)
        Dim PublicId As String = HttpContext.Current.Session("PublicId")
        Dim VCVSharedPublicId As String = HttpContext.Current.Session("VCVSharedPublicId")

        If UserId <> "" Then
            If db.VCVs.Count(Function(v) v.VCVName = filename And v.UserId = UserId) > 0 Then Return True
        End If

        If PublicId IsNot Nothing Then
            Dim l As Listing = db.Listings.FirstOrDefault(Function(li) li.PublicId = PublicId)
            If l Is Nothing Then
                Return False
            Else
                If db.ListingApplications.Count(Function(lp) lp.VideoName = filename And lp.ListingId = l.ListingId) Then Return True
            End If
        End If

        If VCVSharedPublicId IsNot Nothing Then
            Dim vcvShared = db.VCVShareds.FirstOrDefault(Function(v) v.VideoName = filename And v.PublicId = VCVSharedPublicId)
            If vcvShared Is Nothing = False Then Return True
        End If

        Return False

    End Function



    Private Function SendContentTypeAndFile(context As HttpContext, strFile As [String]) As HttpContext
        If [String].IsNullOrEmpty(strFile) Then
            Return Nothing
        Else
            context.Response.ContentType = "video/x-flv"
            context.Response.TransmitFile(strFile)
            context.Response.[End]()
            Return context
        End If
    End Function

    ' NOTE:: I have not written this function. I found it on the web a while back. All credits for this function go to the author (whose name I cannot remember).
    Public Function ResolveUrl(originalUrl As String) As String
        If originalUrl Is Nothing Then
            Return Nothing
        End If
        ' *** Absolute path - just return   
        If originalUrl.IndexOf("://") <> -1 Then
            Return originalUrl
        End If
        ' *** Fix up image path for ~ root app dir directory    
        If originalUrl.StartsWith("~") Then
            Dim newUrl As String = ""
            If HttpContext.Current IsNot Nothing Then

                If HttpContext.Current.Request.ApplicationPath <> "/" Then
                    newUrl = HttpContext.Current.Request.ApplicationPath & originalUrl.Substring(1).Replace("//", "/")
                Else
                    newUrl = originalUrl.Substring(1).Replace("//", "/")
                End If

            Else
                ' *** Not context: assume current directory is the base directory        
                Throw New ArgumentException("Invalid URL: Relative URL not allowed.")
            End If
            Return newUrl
        End If
        ' *** Just to be sure fix up any double slashes        
        Return originalUrl
    End Function
End Class
