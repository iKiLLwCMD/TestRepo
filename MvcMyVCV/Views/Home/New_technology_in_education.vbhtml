@Code
    ViewData("Title") = "New Technology in Education "
    ViewBag.Description = "Buy MyChatPack for exciting new technology in education and introduce eLearning to the school and home, MyChatPack makes new technology in education fun."
    ViewBag.Keywords = "New Technology in Education, eLearning, technology in education, technology for students, technology in schools, New Zealand"
End Code
<br />
<img src="/Images/MyChatPack.jpg" /><br />

<table style="height: 540px; margin-top: 0px;" >
<tr>
<td style="width: 70%; vertical-align:top;">
<h1>New Technology in Education</h1>
<p>MyChatPack is new technology in education, providing an exciting new opportunity for schools, teachers, students and families to welcome a new online community of teaching and learning.</p>
<p>While new technology is frequent in education, MyChatPack has been thoroughly researched and developed by professionals in the education sector. MyChatPack can be used at any level of education, from early childhood / pre-school, through to the higher levels of tertiary education for assessment, distance learning, homework assignments, developing professional practice and more.</p>
<p>ESOL students can use it as a way to develop confidence in speaking and writing in another language with the strong support of the script and the teleprompter.  They can hear how they sound, hear quality examples that can be recorded by their teachers; and that is just the beginning.</p>
<p>Students who are away from school for any length of time, whether they are overseas or in hospital can use MyChatPak as a learning tool ensuring that they are not missing out on essential learning time.</p>  
</td>

<td  style="vertical-align: top;">
@Html.Partial("_PartialSideBar")
</td>

</table>


@Html.Partial("_PartialSignup")





<script>
    $(function () {

        document.getElementById("navMyChatPack").className = "navhighlited";
    })
</script>