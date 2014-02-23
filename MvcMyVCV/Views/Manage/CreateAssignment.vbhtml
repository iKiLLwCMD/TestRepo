@ModelType MvcMyVCV.Assignment

@Code
    ViewData("Title") = "Create Assignment"
End Code

<h2>CreateAssignment</h2>

@Using Html.BeginForm()
    @Html.AntiForgeryToken()
    @Html.ValidationSummary(True)

    @<fieldset>
        <legend>Assignment</legend>

        <div class="editor-label">
            @Html.LabelFor(Function(model) model.AssignmentName)
        </div>
        <div class="editor-field">
            @Html.EditorFor(Function(model) model.AssignmentName)
            @Html.ValidationMessageFor(Function(model) model.AssignmentName)
        </div>

        <div class="editor-label">
            @Html.LabelFor(Function(model) model.AssignmentDueDate)
        </div>
        <div class="editor-field">
            @Html.TextBoxFor(Function(model) model.AssignmentDueDate, New With {.id = "datepicker"})
            @Html.ValidationMessageFor(Function(model) model.AssignmentDueDate)
        </div>

        <div class="editor-label">
            @Html.LabelFor(Function(model) model.ClassId)
        </div>
        <div class="editor-field">
             @Html.DropDownList("ClassId")
            @Html.ValidationMessageFor(Function(model) model.ClassId)
        </div>

        <p>
            <input type="submit" value="Create" />
        </p>
    </fieldset>
End Using


@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section

<script>
    $(document).ready(function () { $("#datepicker").datepicker({ dateFormat: "MM/dd/yy", minDate: '0' }); });
</script>