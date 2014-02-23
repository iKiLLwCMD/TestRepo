@ModelType MvcMyVCV.ForgotModel
@Code
    ViewData("Title") = "Forgot Passowrd"
End Code

<h2>Forgot Password</h2>
@Using Html.BeginForm()
    @Html.AntiForgeryToken()
    @Html.ValidationSummary(True)

    @<fieldset>
        <legend>Forgot Password</legend>
        <p></p>
       

        <div class="editor-label">
            @Html.LabelFor(Function(model) model.UserId)
        </div>
        <div class="editor-field">
            @Html.TextBoxFor(Function(model) model.UserId)
            @Html.ValidationMessageFor(Function(model) model.UserId)
        </div>
       

        <p>
            <input type="submit" value="Request Password Change" />
        </p>
    </fieldset>
End Using