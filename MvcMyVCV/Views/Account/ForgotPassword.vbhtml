@ModelType MvcMyVCV.ForgotPasswordChangeViewModel


@Code
    ViewData("Title") = "Change Password"
End Code

<hgroup class="title">
    <h1>@ViewBag.Title.</h1>
    <h2>Use this form to change your password.</h2>
</hgroup>

<p class="message-info">
    Passwords must be at least 6 characters long.
</p>

@Using Html.BeginForm()
    @Html.ValidationSummary()

    @<fieldset>
        <legend>Change Password Form</legend>
        <ol>
          @* @Html.HiddenFor(Function(model) model.UserId)
           @Html.HiddenFor(Function(model) model.ClassId)
           @Html.HiddenFor(Function(model) model.UserName)
           @Html.HiddenFor(Function(model) model.UserType)*@
          
            <li>
                @Html.LabelFor(Function(model) model.Password)
                @Html.PasswordFor(Function(model) model.Password)
                @Html.ValidationMessageFor(Function(model) model.Password)    
            </li>
            <li>
                @Html.LabelFor(Function(model) model.ConfirmPassword)
                @Html.PasswordFor(Function(model) model.ConfirmPassword) 
                @Html.ValidationMessageFor(Function(model) model.ConfirmPassword)               
            </li>
        </ol>
        <input type="submit" value="Change password" />
    </fieldset>
End Using

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section


