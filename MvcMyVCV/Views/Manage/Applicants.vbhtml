@Imports MvcMyVCV
@ModelType PagedList.IPagedList(Of ListingApplication)

@Code
    ViewData("Title") = "Manage Applicants"
End Code
@If ViewBag.ApplicationStatusId = 2 And User.IsInRole("Recruitment") Then
    @<h3 style="float:right;">Public URL: @Html.TextBox("txtPublicLink", String.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~/ShortList/") & ViewBag.PublicLink), New With { .style = "width:250px;", .class = "inputs" } )</h3>    
End If

@If Model.Count > 0 Then
    @If (Model.HasPreviousPage) Then
    
        @Html.ActionLink("<<", "Index", New With {.page = 1, .sortOrder = ViewBag.CurrentSort, .currentFilter = ViewBag.CurrentFilter})
        @Html.Raw(" ")
        @Html.ActionLink("< Prev", "Index", New With {.page = Model.PageNumber - 1, .sortOrder = ViewBag.CurrentSort, .currentFilter = ViewBag.CurrentFilter})
   
    End If
    
    @:Page @(Model.PageNumber) of @(Model.PageCount)

    @If (Model.HasNextPage) Then
    
        @Html.ActionLink("Next >", "Index", New With {.page = Model.PageNumber + 1, .sortOrder = ViewBag.CurrentSort, .currentFilter = ViewBag.CurrentFilter})
        @Html.Raw(" ")
        @Html.ActionLink(">>", "Index", New With {.page = Model.PageCount, .sortOrder = ViewBag.CurrentSort, .currentFilter = ViewBag.CurrentFilter})
    End If
    @<br />@<br />
End If

@Using Html.BeginForm()
    @Html.AntiForgeryToken()
    @<fieldset>
    <legend>Sort By Applicatiion Status</legend>
        Filter By Application Status
         @Html.DropDownList("ApplicationStatusId", TryCast(ViewBag.selectList, SelectList), New With {.onchange = "this.form.submit();"})
     </fieldset>       
End Using
<table style="width: 100%;border-style:solid;border-width:1px;" id="alt">
    <tr style ="background-color:#f7f7f7;border-style:solid;border-width:1px">
        @If ViewBag.selectedIndex = 0 Then
            @<th></th>
        End If
        <th></th>
        <th></th>
        <th>Attachments</th>
        <th>Summary</th>
    </tr>

    @For Each la In Model
        Dim className As String = ""
        If la.IsViewed Then className = "applicantViewed" Else className = "applicantNotViewed"
        
        @<tr style="border-style:solid;border-width:1px" class="@className" id="@la.ApplicationId">
            @If ViewBag.selectedIndex = 0 Then
                @<td>
                    <button style="width: 100px;" onclick="makeApplicationDecision(2, @la.ApplicationId, this)">Short List</button><br />
                    <button style="width: 100px;" onclick="makeApplicationDecision(1, @la.ApplicationId, this)">Decline</button>
                </td>
            End If
            <td style="text-align:center">
                @la.User.UserName
            </td>
            <td>
                <a href="/Streams/@(la.VideoName.ToString).flv" class="myPlayer" id="@la.ApplicationId"
                style="display:block;width:320px;height:240px;margin:10px auto"></a>
                <p align="center">@la.TimeStamp</p>
            </td>
            <td>
                <a href="/Attachments/@(la.Attachment1Id)">@la.Attachment1Name</a><br />
                <a href="/Attachments/@(la.Attachment2Id)">@la.Attachment2Name</a><br />
                <a href="/Attachments/@(la.Attachment3Id)">@la.Attachment3Name</a>
            </td>
            <td></td>
        </tr>
    Next

 </table>

<script>
    function makeApplicationDecision(ApplicationStatusId, ApplicationId, r) {
        try {
            $.ajax({
                url: '@Url.Action("MakeApplicationDecision", "Home")',
                type: "POST",
                data: { ApplicationStatusId: ApplicationStatusId, ApplicationId: ApplicationId },
                success: function (data) {

                }
            })
        } catch (err) {
            alert(err.message);
        }

        var i = r.parentNode.parentNode.rowIndex;
        document.getElementById("alt").deleteRow(i);
    }

</script>

<script>
    flowplayer("a.myPlayer", { src: "http://releases.flowplayer.org/swf/flowplayer-3.2.16.swf", wmode: "opaque" }, {

        clip: {
            autoPlay: false,
            bufferLength: 5,
            autoBuffering: true,
            onCuepoint: [[10000], function (event) {
                try {
                    var applicationID = this.id();
                    $.ajax({
                        url: '@Url.Action("UpdateApplicantWatchedStatus", "Home")',
                        type: "POST",
                        data: { id: applicationID },
                        success: function (data) {
                            $("#" + applicationID).removeClass("applicantNotViewed");
                            $("#" + applicationID).addClass("applicantViewed");
                        }
                    })
                } catch (err) {
                    alert(err.message);
                }
            }]
        }
    });
</script>

<script src="~/Scripts/Share.js"></script>

<script>
    function removeIt(id) {
        if (confirm('Are you sure you Want to Remove this MyChatPack?')) {
            // Save it!
            $.ajax({
                url: '@Url.Action("Remove", "Home")',
                type: "POST",
                data: "id=" + id,
                success: function () { document.location.reload(true); }
            })
        }
    }

    document.getElementById("navHome").className = "navhighlited";
</script>