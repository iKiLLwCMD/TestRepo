@ModelType MvcMyVCV.LoginViewModel


@Code
    ViewData("Title") = "Log in"
End Code

@Using Html.BeginForm(New With {.ReturnUrl = ViewBag.ReturnUrl})
    @Html.ValidationSummary(true, "Log in was unsuccessful. Please correct the errors and try again.")
    
    @<fieldset>
        <legend>Log in Form</legend>
        <table>
            <tr>
                <td><h2>Use this form to enter your user name and password.</h2><br /></td>
            </tr>
            <tr>
                <td>@Html.TextBoxFor(Function(m) m.UserId, New With {.data_val = False, .class = "inputs", .placeholder = "User ID"})<br /></td>
            </tr>
            <tr>
                <td>@Html.PasswordFor(Function(m) m.UserPassword, New With { .class = "inputs", .placeholder = "Password"})<br /></td>
                <td>@Html.ValidationMessageFor(Function(m) m.UserPassword)</td>
            </tr>
        </table>

        <input class="vcvbutton" type="submit" value="Log in"  />
    </fieldset>
    @<br />
    @<p>
        @Html.ActionLink("Register", "Register") | @Html.ActionLink("Forgot Password?", "ChangeRequest")
    </p>
End Using



@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
