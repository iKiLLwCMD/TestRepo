@ModelType MvcMyVCV.User
@Code
    ViewData("Title") = "Register"
End Code

<h2>Register</h2>
@Using Html.BeginForm()
    @Html.AntiForgeryToken()
    @Html.ValidationSummary(True)

    @<fieldset>
        <legend>Register</legend>
        <p></p><br />
        <div class="editor-field">
        @Html.RadioButton("UserType", "Demo", True)<span>Demo</span>
        @Html.RadioButton("UserType", "Employee")<span>Employee</span>
        @Html.RadioButton("UserType", "Employer")<span>Employer</span>
        @Html.RadioButton("UserType", "Recruitment")<span>Recruitment</span>
        
        </div><br />
        <div class="editor-field">
            @Html.TextBoxFor(Function(model) model.UserId, New With {.class = "inputs", .placeholder = "User ID"})
            @Html.ValidationMessageFor(Function(model) model.UserId)
        </div>
        <div class="editor-field">
            @Html.PasswordFor(Function(model) model.UserPassword, New With {.class = "inputs", .placeholder = "Password"})
            @Html.ValidationMessageFor(Function(model) model.UserPassword)
        </div>

        <div class="editor-field">
            @Html.TextBoxFor(Function(model) model.UserName, New With {.class = "inputs", .placeholder = "Name"})
            @Html.ValidationMessageFor(Function(model) model.UserName)
        </div>

        <div class="editor-field">
            @Html.TextBoxFor(Function(model) model.UserEmail, New With {.type = "email", .class = "inputs", .placeholder = "Email Address"})
            @Html.ValidationMessageFor(Function(model) model.UserEmail)
        </div>
        <br />
        <div class="editor-label">
            @Html.EditorFor(Function(model) model.Terms) Please Check if you Agree with the <a target="_blank" href="/Account/Terms">Terms</a>
            @Html.ValidationMessageFor(Function(model) model.Terms)
        </div><br />
        <p>
            <input type="submit" value="Create" />
        </p>
    </fieldset>
End Using



@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
