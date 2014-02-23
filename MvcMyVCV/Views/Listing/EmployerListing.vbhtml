@Imports MvcMyVCV
@ModelType PagedList.IPagedList(Of ListingApplication)

@Code
    ViewData("Title") = "Manage Applicants"
End Code
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

<table style="width: 100%;border-style:solid;border-width:1px;" id="alt">
    <tr style ="background-color:#f7f7f7;border-style:solid;border-width:1px">
        <th></th>
        <th></th>
        <th>Summary</th>
    </tr>

    @For Each la In Model
        Dim className As String = ""
        If la.IsViewed Then className = "applicantViewed" Else className = "applicantNotViewed"
        
        @<tr style="border-style:solid;border-width:1px" class="@className" id="@la.ApplicationId">
            <td style="text-align:center">
                @la.User.UserName
            </td>
            <td>
                <a href="/Streams/@(la.VideoName.ToString).flv" class="myPlayer" id="@la.ApplicationId"
                style="display:block;width:320px;height:240px;margin:10px auto"></a>
                <p align="center">@la.TimeStamp</p>
            </td>
            <td></td>
        </tr>
    Next

 </table>

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