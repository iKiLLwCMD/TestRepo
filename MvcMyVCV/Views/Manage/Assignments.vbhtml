@ModelType PagedList.IPagedList(Of MvcMyVCV.VCV)

@Code
    ViewData("Title") = "Manage Assignments"
End Code
<div id="fb-root"></div>
@Html.Partial("_PartialSharing")

<br />



@Using Html.BeginForm()
    @Html.AntiForgeryToken()
    @<fieldset>
    <legend>Choose Assignment</legend>
            @*@Html.DropDownList("ClassId", TryCast(ViewBag.selectList, SelectList))*@
         @Html.DropDownList("AssignmentId", TryCast(ViewBag.selectList, SelectList), New With {.onchange = "this.form.submit();"})
           
    
     @*<p>
            <input type="submit" value="Create" />
        </p>*@
     </fieldset>       
End Using
<br /><br />
   @Html.ActionLink("Create New Assignment", "CreateAssignment")
   <br />

@If Model.Count > 0 Then
    

  @:<br /><br />
  @:<div>Page @(Model.PageNumber) of @(Model.PageCount)
     
    @If (Model.HasPreviousPage) Then
    
        @Html.ActionLink("<<", "Assignments", New With {.page = 1, .sortOrder = ViewBag.CurrentSort, .currentFilter = ViewBag.CurrentFilter})
        @Html.Raw(" ")
        @Html.ActionLink("< Prev", "Assignments", New With {.page = Model.PageNumber - 1, .sortOrder = ViewBag.CurrentSort, .currentFilter = ViewBag.CurrentFilter})
    
    End If

     
    @If (Model.HasNextPage) Then
    
        @Html.ActionLink("Next >", "Assignments", New With {.page = Model.PageNumber + 1, .sortOrder = ViewBag.CurrentSort, .currentFilter = ViewBag.CurrentFilter})
        @Html.Raw(" ")
        @Html.ActionLink(">>", "Assignments", New With {.page = Model.PageCount, .sortOrder = ViewBag.CurrentSort, .currentFilter = ViewBag.CurrentFilter})
    
    end if
@:</div>

    

 

@:<table>
    @:<tr>
        @:<th>
            
        @:</th>
@:<th></th>
@:<th></th>
        @:<th style="padding-right:10px;">
            @:Attachments
        @:</th>
        @:<th>
            @:Summary
        @:</th>
        
        
    @:</tr>
    @If Model Is Nothing = False Then
    @For Each item In Model
    Dim currentItem = item
    @<tr>

        <td>

@*<table>
<tr>
 <td>*@
 @currentItem.UserByVCV.UserName 
        </td>
<td>

   <a href="#"  onclick="doLogin(@currentItem.VCVId);return false;">Share to facebook</a><br />
   <a href="#"  onclick="doLoginyoutube(@currentItem.VCVId);return false;">Share to youtube</a><br />
   <a href="#"  onclick="doLoginDrive(@currentItem.VCVId);return false;">Share to Blogger (using drive)</a> 


</td>
<td>
    <a href="/Streams//@currentItem.VideoName.ToString
.flv"    class="myPlayer"
    style="display:block;width:320px;height:240px;margin:10px auto">
    </a><p align="center">@currentItem.Timestamp </p>
@*</td>
</tr>
</table>*@

   
        </td>
        <td>
            <a href="../Attachments/@currentItem.Attachment1Id" style="color:green;" target="_blank">@currentItem.Attachment1Name</a><br />
            <a href="../Attachments/@currentItem.Attachment2Id" style="color:green;" target="_blank">@currentItem.Attachment2Name</a><br />
            <a href="../Attachments/@currentItem.Attachment3Id" style="color:green;" target="_blank">@currentItem.Attachment3Name</a><br />
        </td>
        
        <td>
        @If currentItem.SummaryId Is Nothing = False Then
               @:<div style="overflow-y: scroll; height:240px; width: 100%;">@Html.Raw(currentItem.Summary.SummaryContent)</div> 
            End If
            
        </td>
        
        
    </tr>
        Next
        End If
@:</table>
Else
     @:<br /><br />No MyChatPacks for this Assignment.
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

    $(document).ready(function () {
        // $("table > tbody tr:odd").css("background-color", "#FFFFFF");
        $("tr:odd").css("background-color", "#f7f7f7");
        $("tr:even").css("background-color", "#ffffff");
        $("th").css("background-color", "#f7f7f7");
    });

</script>

<script src="~/Scripts/Share.js"></script>