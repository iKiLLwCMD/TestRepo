Public Class RequireHttp
    Inherits ActionFilterAttribute
    Public Overrides Sub OnActionExecuting(filterContext As ActionExecutingContext)
        ' If the request has arrived via HTTPS...
        If filterContext.HttpContext.Request.IsSecureConnection Then
            filterContext.Result = New RedirectResult(filterContext.HttpContext.Request.Url.ToString().Replace("https:", "http:"))
            ' Go on, bugger off "s"!
            filterContext.Result.ExecuteResult(filterContext)
        End If
        MyBase.OnActionExecuting(filterContext)
    End Sub
End Class
