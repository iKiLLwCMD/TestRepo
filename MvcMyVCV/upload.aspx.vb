
Imports System.IO
Imports MvcMyVCV
Imports System.Linq

Partial Public Class upload
    Inherits System.Web.UI.Page

    Private db As New VCVOnlineEntities


    Protected Sub Page_Load(sender As Object, e As EventArgs)


        Dim sVideoname As String = Request.QueryString("ID").ToString
        Dim sToken As String = Request.QueryString("Access").ToString
        Dim slength As String = ""
        Try
            slength = Request.QueryString("Length").ToString
        Catch ex As Exception

        End Try


        Dim sql As String = "SELECT VCVName, TokenExpiryDate FROM VCV WHERE (VideoName = N'ID flash parameter') AND (TokenExpiryDate > { fn NOW() }) AND (VideoName <> N'""') AND (Token = N'Access flash parameter')"

        Dim count As Integer = (From VCVs In db.VCVs Where VCVs.VCVName = sVideoname And VCVs.TokenExpiryDate > DateTime.Now And VCVs.VideoName <> "" And VCVs.Token = sToken).Count()
        If count > 0 Then

            ' Get the data from the POST array
            Dim data As String = Request.Form("imageData")

            ' Decode the bytes from the Base64 string
            Dim bytes As Byte() = Convert.FromBase64String(data)

            ' Write the jpeg to disk
            Dim path As String = Server.MapPath("~/Snaps/" & Session("NewVCVName") & ".jpg")
            File.WriteAllBytes(path, bytes)

            Dim vcv As VCV = db.VCVs.Find(Session("NewVCVId"))

            ' set token and expiry
            vcv.Token = Guid.NewGuid.ToString
            vcv.TokenExpiryDate = Now.AddMinutes(30)
            If slength Is Nothing = False Then
                If slength <> "" Then
                    vcv.Length = slength
                End If

            End If

            db.SaveChanges()



            ' Clear the response and send a Flash variable back to the URL Loader
            Response.Clear()
            Response.ContentType = "text/plain"
            Response.Write("ok=ok")


        End If



    End Sub
End Class
