@Code
    ViewData("Title") = "New Technology in Education "
    ViewBag.Description = "Buy MyChatPack for exciting new technology in education and introduce eLearning to the school and home, MyChatPack makes new technology in education fun."
    ViewBag.Keywords = "New Technology in Education, eLearning, technology in education, technology for students, technology in schools, New Zealand"
End Code

<table style="height: 540px; margin-top: 0px;" >
<tr>
<td style="width: 70%; vertical-align:top;">
<h1>Email Sent</h1>
<p>Thank you for your interest in MyChatPack. We will be in touch shortly.</p>

</td>

<td  style="vertical-align: top;">
@Html.Partial("_PartialSideBar")
</td>

</table>


@Html.Partial("_PartialSignup")


