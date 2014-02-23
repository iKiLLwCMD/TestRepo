@ModelType MvcMyVCV.ContactViewModel

@Code
    ViewData("Title") = "Technology for Learning "
    ViewBag.Description = "Call our team to find out how MyChatPack technology tool for learning can benefit the school and home, fun technology for learning for everyone."
    ViewBag.Keywords = "Technology for learning, technology for the classroom, technology in schools, e learning, New Zealand."
End Code
<br />
<img src="/Images/Contact.jpg" /><br />
<table style="height: 540px; margin-top: 0px;" >

<tr>
<td style="width: 70%; vertical-align:top;">

<h1>Technology for Learning | Contact Us</h1>
<p>Want to learn more about using technology for learning? Contact the MyChatPack team today.</p>

@Using Html.BeginForm()
    @Html.ValidationSummary(true) 
    @<fieldset> 
        <legend>Contact Us</legend> 

          <div class="editor-label"> 
            @Html.LabelFor(Function(model) model.sName) 
        </div> 
        <div class="editor-field"> 
            @Html.TextBoxFor(Function(model) model.sName) 
            @Html.ValidationMessageFor(Function(model) model.sName) 
        </div> 
    
        <div class="editor-label"> 
            @Html.LabelFor(Function(model) model.from) 
        </div> 
        <div class="editor-field"> 
            @Html.TextBoxFor(Function(model) model.From) 
            @Html.ValidationMessageFor(Function(model) model.From) 
        </div> 
        <div class="editor-label"> 
            @Html.LabelFor(Function(model) model.Subject) 
        </div> 
        <div class="editor-field"> 
            @Html.TextBoxFor(Function(model) model.Subject) 
            @Html.ValidationMessageFor(Function(model) model.Subject) 
        </div> 
        <div class="editor-label"> 
            @Html.LabelFor(Function(model) model.Message) 
        </div> 
        <div class="editor-field"> 
            @Html.EditorFor(Function(model) model.Message) 
            @Html.ValidationMessageFor(Function(model) model.Message) 
        </div> 
        <p> 
            <input type="submit" value="Send" /> 
        </p> 
    </fieldset> 
end using




</td>
<td  style="vertical-align: top;">
@Html.Partial("_PartialSideBar")
</td>
</tr>
</table>

@Html.Partial("_PartialSignup")




<script>
    $(function () {

        document.getElementById("navContact").className = "navhighlited ";
    })
</script>