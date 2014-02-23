@ModelType MvcMyVCV.user

@Code
    ViewData("Title") = "Create"
End Code

<h2>Create</h2>

@Using Html.BeginForm()
    @Html.AntiForgeryToken()
    @Html.ValidationSummary(True)

    @<fieldset>
        <legend>User</legend>

        <div class="editor-label">
            @Html.Label("User Id")
        </div>
        <div class="editor-field">
            @Html.EditorFor(Function(model) model.UserId)
            @Html.ValidationMessageFor(Function(model) model.UserId)
        </div>

        <div class="editor-label">
            @Html.Label("Name")
        </div>
        <div class="editor-field">
            @Html.EditorFor(Function(model) model.UserName)
            @Html.ValidationMessageFor(Function(model) model.UserName)
        </div>

        <div class="editor-label">
            @Html.LabelFor(Function(model) model.UserPassword)
        </div>
        <div class="editor-field">
            @Html.PasswordFor(Function(model) model.UserPassword)
            <a href="#" onclick="RandomPass()">Generate Random Password</a><label id="lblPass"></label>
            @Html.ValidationMessageFor(Function(model) model.UserPassword)
        </div>

        <div class="editor-label">
            @Html.LabelFor(Function(model) model.ClassId, "Class")
        </div>
        <div class="editor-field">
            @Html.DropDownList("ClassId")
            @Html.ValidationMessageFor(Function(model) model.ClassId)
        </div>

         @*  <div class="editor-field">
            @Html.EditorFor(Function(model) model.ClassId)
            @Html.ValidationMessageFor(Function(model) model.ClassId)
        </div>*@

        <p>
            <input type="submit" value="Create" />
        </p>
    </fieldset>
End Using

@*<div>
    @Html.ActionLink("Back to List", "Index")
</div>*@

<script>

    function RandomPass() {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXTZabcdefghiklmnopqrstuvwxyz";
        var string_length = 5;
        var randomstring = '';
        var charCount = 0;
        var numCount = 0;

        for (var i = 0; i < string_length; i++) {
            // If random bit is 0, there are less than 3 digits already saved, and there are not already 5 characters saved, generate a numeric value. 
            if ((Math.floor(Math.random() * 2) == 0) && numCount < 3 || charCount >= 5) {
                var rnum = Math.floor(Math.random() * 10);
                randomstring += rnum;
                numCount += 1;
            } else {
                // If any of the above criteria fail, go ahead and generate an alpha character from the chars string
                var rnum = Math.floor(Math.random() * chars.length);
                randomstring += chars.substring(rnum, rnum + 1);
                charCount += 1;
            }
        }

        document.getElementById("UserPassword").value = randomstring
        document.getElementById("lblPass").innerHTML = 'Random Password: ' + randomstring
        

    }



</script>

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
