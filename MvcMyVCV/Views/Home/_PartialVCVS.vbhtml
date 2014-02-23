
@If Model.Count > 0 Then
    @:<br /><br />
    @:<div>
@:Page @(Model.PageNumber) of @(Model.PageCount)
     
    @If (Model.HasPreviousPage) Then
    
        @Html.ActionLink("<<", "Index", New With {.page = 1, .sortOrder = ViewBag.CurrentSort, .currentFilter = ViewBag.CurrentFilter})
        @Html.Raw(" ")
        @Html.ActionLink("< Prev", "Index", New With {.page = Model.PageNumber - 1, .sortOrder = ViewBag.CurrentSort, .currentFilter = ViewBag.CurrentFilter})
    
     
    End If

     
    @If (Model.HasNextPage) Then
    
        @Html.ActionLink("Next >", "Index", New With {.page = Model.PageNumber + 1, .sortOrder = ViewBag.CurrentSort, .currentFilter = ViewBag.CurrentFilter})
        @Html.Raw(" ")
        @Html.ActionLink(">>", "Index", New With {.page = Model.PageCount, .sortOrder = ViewBag.CurrentSort, .currentFilter = ViewBag.CurrentFilter})
    
    End If
@:</div>
    


    @:<table style="width: 100%" id="alt">
@:<tr style ="background-color:#f7f7f7;">
        @:<th>
            
       @: </th>
@:<th></th>
        @:<th>
            @:Attachments
        @:</th>
        @:<th>
           @: Summary
        @:</th>
        
        
    @:</tr>

@For Each item In Model
        Dim currentItem = item
    @<tr>

      

<td>
@Html.ActionLink("Edit", "Edit", "Home", New With {.id = currentItem.VCVId}, New With {.id = currentItem.VCVId, .class = "vcvbutton2", .style = "padding-left:16px;padding-right:16px;"})<br />

<br /><a href="javascript:void(0);" class="vcvbutton2"  onclick="removeIt(@currentItem.VCVId);return false;">Remove</a>  <br />
</td>
<td>
    <a href="/Streams/@(currentItem.VideoName.ToString).flv" class="myPlayer"
    style="display:block;width:320px;height:240px;margin:10px auto">
    </a><p align="center">@currentItem.Timestamp</p> 
</td>
        <td>
            <a href="Attachments/@currentItem.Attachment1Id" style="color:green;" target="_blank">@currentItem.Attachment1Name</a><br />
            <a href="Attachments/@currentItem.Attachment2Id" style="color:green;" target="_blank">@currentItem.Attachment2Name</a><br />
            <a href="Attachments/@currentItem.Attachment3Id" style="color:green;" target="_blank">@currentItem.Attachment3Name</a><br />
        </td>
        
        <td>
        @If currentItem.SummaryId Is Nothing = False Then
           @:<div style="overflow-y: scroll; height:240px; width: 100%;">@Html.Raw(currentItem.Summary.SummaryContent)</div>
        End If

        </td>
        
        
    </tr>
    Next
@:</table>

   

End If

<script>
    flowplayer("a.myPlayer", { src: "http://releases.flowplayer.org/swf/flowplayer-3.2.16.swf", wmode: "opaque" }, {
        clip: {
            // these two configuration variables does the trick
            autoPlay: false,
            bufferLength: 5,
            autoBuffering: true // <- do not place a comma here
        }
    });
</script>

<script src="~/Scripts/Share.js"></script>


<script>

    $(document).ready(function () {
        // $("table > tbody tr:odd").css("background-color", "#FFFFFF");
        $("#alt tr:odd").css("background-color", "#f7f7f7");
        $("#alt tr:even").css("background-color", "#ffffff");
        $("th").css("background-color", "#ffffff");
    });

    function removeIt(id) {
        if (confirm('Are you sure you Want to Remove this MyChatPack?')) {
            // Save it!
            $.ajax(
{
    url: '@Url.Action("Remove", "Home")',
    type: "POST",
    data: "id=" + id,
    success: function () {
        document.location.reload(true);


    }


})
        } else {
            // Do nothing!
        }


    }


    document.getElementById("navHome").className = "navhighlited";


</script>

