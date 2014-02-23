<div class ="SignUp">
<table style="width: 980px; Margin-left:auto;Margin-right:auto; padding-bottom: 0px;">
<tr>
<td>
<h3>Get MyChatPack</h3>
<hr />

</td>
<td>
<h3>Sign up for our Newsletter</h3>
<hr />
</td>

</tr>


<tr>

<td width="65%" style="vertical-align:top;">
<p  style="padding-top: 0px; margin-top: 0px;"><img src="/Images/Classroom.gif" border="0" style="float:left;margin:0 5px 0 0;"" />Get the latest in eLearning software with <a href="/Home/eLearning_software">MyChatPack</a>. Don’t be left behind – keep up with best practice in eLearning with software that is easy to get, simple to use and a great tool for students of any age. 
</p>

</td>

<td style="text-align:left; vertical-align:top;">
Get latest news & more on MyChatPack.co.nz<br />
 <p><a href="https://www.facebook.com/pages/MyChatPak/207153549308611" class="facebook" target="_blank">Facebook</a><a href="/Blog/"><img border="0" src="/Images/Blog.gif" /></a>Follow our <a href="/Blog/">Blog</a></p>
                        @*<a href="http://twitter.com" class="twitter">Twitter</a><br /><br />*@
@Using Html.BeginForm()
 @:<div style="align:left" >@Html.TextBox("txtEmailSignup", "", New With {.placeholder = "Enter your email"})<a href="#" onclick="EmailSignup()"><img border="0" src="/Images/signup.gif" style="vertical-align:bottom;" /></a></div>
 
End Using

</td>



       </tr>


</table>
</div>
<div style="height: 210px;"></div>
<script>

    $(function(){

        //$('#body').css('background', '#fff');
        $('body').css('background', '#fff');
    })


    function EmailSignup() {
        $.ajax(
    {
        url: '@Url.Action("EmailSignup", "Home")',
        type: "POST",
        data: "sEmail=" + $('#txtEmailSignup').val(),
        success: function (data) {
            if (data == "True") {
                
                alert('Thank you for signing up for our Newsletter!')
                requestcomplete = true


            }
            else {
                 alert('Error Registering Email');
                requestcomplete = false


            }


        }


    });



    }

</script>
