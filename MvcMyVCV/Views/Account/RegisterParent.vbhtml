@ModelType MvcMyVCV.Class 
@Code
    ViewData("Title") = "Register Parent"
End Code

<h2>Register</h2>
@Using Html.BeginForm()
    @Html.AntiForgeryToken()
    '@Html.ValidationSummary(True)

    @<fieldset>
        <legend>Register</legend>

       <div class="editor-label">
            @Html.LabelFor(Function(model) model.CountryId, "Country")
        </div>

        <div class="editor-field">
            @Html.DropDownList("CountryId", String.Empty)
            @Html.ValidationMessageFor(Function(model) model.CountryId)
        </div>

        <div class="editor-label">
            @Html.LabelFor(Function(model) model.PostalCode)
        </div>
        <div class="editor-field">
            @Html.EditorFor(Function(model) model.PostalCode)
            @Html.ValidationMessageFor(Function(model) model.PostalCode)
        </div>

        <div class="editor-label">
            @Html.Label("Children Need Approval before they can Share their MyChatPacks?")
        </div>
        <div class="editor-field">
            @Html.EditorFor(Function(model) model.NeedsApprovalToShare)
            @Html.ValidationMessageFor(Function(model) model.NeedsApprovalToShare)
        </div>

         

        <p>
            <input type="submit" value="Create" />
        </p>
    </fieldset>
End Using



@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
