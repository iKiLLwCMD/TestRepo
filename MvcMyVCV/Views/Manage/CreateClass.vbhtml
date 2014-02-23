@ModelType MvcMyVCV.Class 
@Code
    ViewData("Title") = "Register"
End Code

<h2>Register</h2>
@Using Html.BeginForm()
    @Html.AntiForgeryToken()
    @Html.ValidationSummary(True)

    @<fieldset>
        <legend>Register</legend>

        <div class="editor-label">
            @Html.LabelFor(Function(model) model.ClassName)
        </div>
        <div class="editor-field">
            @Html.EditorFor(Function(model) model.ClassName)
            @Html.ValidationMessageFor(Function(model) model.ClassName)
        </div>

        <div class="editor-label">
            @Html.LabelFor(Function(model) model.SchoolName)
        </div>
        <div class="editor-field">
            @Html.EditorFor(Function(model) model.SchoolName)
            @Html.ValidationMessageFor(Function(model) model.SchoolName)
        </div>

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
            @Html.LabelFor(Function(model) model.Grade, "Grade")
        </div>

        <div class="editor-field">
            @Html.DropDownList("GradeId", String.Empty)
            @Html.ValidationMessageFor(Function(model) model.GradeId)
        </div>

         <div class="editor-label">
            @Html.LabelFor(Function(model) model.Subject)
        </div>
        <div class="editor-field">
            @Html.EditorFor(Function(model) model.Subject)
            @Html.ValidationMessageFor(Function(model) model.Subject)
        </div>

          <div class="editor-label">
            @Html.LabelFor(Function(model) model.NeedsApprovalToShare)
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

<div>
    @Html.ActionLink("Back to List", "Index")
</div>

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
