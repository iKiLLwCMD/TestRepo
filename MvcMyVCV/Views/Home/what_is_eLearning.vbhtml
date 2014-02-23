@Code
    ViewData("Title") = "What is e-Learning?"
    ViewBag.Description = "Asking yourself what is eLearning, visit MyChatPack for the answer with the latest in eLearning technology, fun and easy  to use for school and home. "
    ViewBag.Keywords = "What is e Learning, e Learning, e learning technology, technology in the classroom, New Zealand"
End Code
<br />
<img src="/Images/FAQS.jpg" /><br />
<table style="height: 540px; margin-top: 0px;" >

<tr>
<td style="width: 70%; vertical-align:top;">
<h1>What is eLearning? | FAQs</h1>
<p>eLearning is anytime, anywhere learning, utilising the technology of the internet and the latest in software, such as MyChatPack. While some programmes can often be complicated, MyChatPack offers a new level of technology – providing supported communication while keeping it simple for students of any age, their teachers and their parents.</p>
<h3>How do we get it?</h3>
<p>Right here on our website – you can purchase it now. <a href="/Account/Register">Register Now</a></p>
<h3>Where can we use it?</h3>
<p>Anywhere with an internet connection. MyChatPack can be used locally and globally, as it web-based with the videos able to be sent in email format – without blocking your spam filters too. Easy to use, easy to access – My MyChatPack. Videos longer than a minute can be uploaded to Facebook, YouTube and Blogger.</p>

</td>
<td  style="vertical-align: top;">
@Html.Partial("_PartialSideBar")
</td>
</tr>
</table>

@Html.Partial("_PartialSignup")



<script>

    $(function () {

        document.getElementById("navFAQs").className = "navhighlited";
    })
</script>