@If Request.IsAuthenticated Then
    @<p style="padding:0; margin:0;">
        Hello, @Html.ActionLink(User.Identity.Name, "ChangePassword", "Account", routeValues:=Nothing, htmlAttributes:=New With {.class = "username", .title = "Change password"})
       
        @*@If User.IsInRole("Employer") = True Or User.IsInRole("Recruitment") = True Then
                @:<img src="/Images/Manage.gif" />
            @Html.ActionLink("Manage", "Index", "Manage")
        end if*@

       <img src="/Images/Login.gif" />
        @Html.ActionLink("Log off", "LogOff", "Account")
       
    </p>
Else
    @<ul>
        <li><img src="/Images/Register.gif" />@Html.ActionLink("Register", "Register", "Account", routeValues:=Nothing, htmlAttributes:=New With {.id = "registerLink"})</li>
        <li><img src="/Images/Login.gif" />@Html.ActionLink("Log in", "Login", "Account", routeValues:=Nothing, htmlAttributes:=New With {.id = "loginLink"})</li>
    </ul>
End If
