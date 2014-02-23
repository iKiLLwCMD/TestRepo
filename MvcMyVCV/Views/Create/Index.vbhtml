<script>
    function keepSessionAlive() {
        $.ajax({
            url: '@Url.Action("Alive", "create")',
            type: "POST"
        })
    }

    $(function () { window.setInterval("keepSessionAlive()", 60000); });
</script>

@Code
    ViewData("Title") = "Home Page"
End Code
@section featured
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h2 style="text-align:center;padding-top:25px;" >@ViewData("Message")</h2>  
            </hgroup>
        </div>
    </section>
End Section
<br />
@Html.Hidden("Step", Session("LoadStep"))
<script type="text/javascript"></script>
<div id="tabs">
    <div style="text-align:right;">
        <button onclick="location.href='@Url.Action("Index", "Home")'">Exit</button>
    </div>
    <ul>
        <li><a href="#tabs-1">Step 1</a></li>
        <li><a href="/Create/Step2">Step 2</a></li>
        <li><a href="/Create/Step3">Step 3</a></li>
        <li><a href="/Create/Step4">Step 4</a></li>
        <li><a href="/Create/Step5">Step 5</a></li>
        <li><a href="/Create/Step6">Step 6</a></li>
    </ul>
    <hr style="height:1px;border-color:#999;background-color:#999;" />

    <div id="tabs-1">@Html.Action("Step1", "Create")</div>
    <div id="tabs-2"></div>
    <div id="tabs-3"></div>
    <div id="tabs-4"></div>
    <div id="tabs-5"></div>
    <div id="tabs-6"></div>
</div>