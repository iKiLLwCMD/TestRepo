<style>

    #container {
    margin: 0 auto;
    position: relative;
}

#topnav {
    text-align: left;
}

#session {
    cursor: pointer;
    display: inline-block;
    height: 20px;
    padding: 10px 0px;
    vertical-align: top;
    white-space: nowrap;
}

#session.active, #session:hover {
    background: rgba(255,255,255,0.1);
    color: fff;
}



.signin-dropdown {
    background-color: #202020;
    border-bottom-left-radius: 5px;
    border-bottom-right-radius: 5px;
    box-shadow: 0 1px 2px #666666;
    -webkit-box-shadow: 0 1px 2px #666666;
    min-height: 160px;
    min-width: 160px;
    position: absolute;
    right: 0;
    display: none;
     padding: 10px;
}

.signin-dropdown form {
    cursor: pointer;
    padding: 10px;
    text-align: left;
}

.signin-dropdown .textbox span {
    color: #BABABA;
}

.signin-dropdown .textbox input {
    width: 200px;
}



.signin-dropdown .textbox label {
    display: block;
    padding-bottom: 7px;
}

.signin-dropdown .textbox span {
    display: block;
}

.signin-dropdown p, .signin-dropdown span {
    color: #999;
    font-size: 11px;
    line-height: 18px;
}

.signin-dropdown .textbox input {
    background: #666666;
    border-bottom: 1px solid #333;
    border-left: 1px solid #000;
    border-right: 1px solid #333;
    border-top: 1px solid #000;
    color: #fff;
    -moz-border-radius: 3px;
    -webkit-border-radius: 3px;
    font: 13px Arial, Helvetica, sans-serif;
    padding: 6px 6px 4px;
}

 a.declinebut
    {
        color:#ffffff;
    }




</style>
@If Model.Count > 0 Then
    

  @:<br /><br />
  @:<div> Page @(Model.PageNumber) of @(Model.PageCount)
    
     
    @If (Model.HasPreviousPage) Then
    
        @Html.ActionLink("<<", "Approval", New With {.page = 1, .sortOrder = ViewBag.CurrentSort, .currentFilter = ViewBag.CurrentFilter})
        @Html.Raw(" ")
        @Html.ActionLink("< Prev", "Approval", New With {.page = Model.PageNumber - 1, .sortOrder = ViewBag.CurrentSort, .currentFilter = ViewBag.CurrentFilter})
    
      
    End If

     
    @If (Model.HasNextPage) Then
    
        @Html.ActionLink("Next >", "Approval", New With {.page = Model.PageNumber + 1, .sortOrder = ViewBag.CurrentSort, .currentFilter = ViewBag.CurrentFilter})
        @Html.Raw(" ")
        @Html.ActionLink(">>", "Approval", New With {.page = Model.PageCount, .sortOrder = ViewBag.CurrentSort, .currentFilter = ViewBag.CurrentFilter})
    
     
    end if
@:</div>



@:<table>
    @:<tr>
        @:<th>
            
        @:</th>
@:<th></th>
        @:<th style="padding-right:10px;">
            @:Attachments
        @:</th>
        @:<th>
            @:Summary
        @:</th>
        
        
    @:</tr>

@For Each item In Model
    Dim currentItem = item
    @<tr>

        <td>

@*<table>
<tr>
<td>*@
@*<a href="#"  onclick="approve(@currentItem.VCVId);return false;">Approve</a><br />*@
@Html.ActionLink("Approve", "approve", New With {.id = currentItem.VCVId})
<div id="container">
<div id="topnav">
<div class="active-links" onclick="javascript:show(@(currentItem.VCVId))">
    <div id="session">
    <a id="signin-link" href="#">
    Decline 
    </a>
    </div>
    <div id="signin-dropdown@(currentItem.VCVId)" class="signin-dropdown">    
       
        <fieldset class="textbox">
        <label class="username">
        <span>Reason to Decline</span><br />
        <input id="decline@(currentItem.VCVId)"   name="decline" value="" type="text"  maxlength="50">
        </label>
       
        @Html.ActionLink("Decline", "decline", New With {.id = currentItem.VCVId}, New With {.id = currentItem.VCVId, .class = "declinebut"})
        </fieldset>
      
        
    </div>

</div>
</div>
</div>
</td>
<td>
    <a href="http://www.mychatpack.co.nz/streams/@(currentItem.VCVName.ToString)
.flv"    class="myPlayer"
    style="display:block;width:320px;height:240px;margin:10px auto">
    </a><p align="center">@currentItem.Timestamp</p> 
@*</td>
</tr>
</table>*@

  
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
Else
    @:<br /><br />No MyChatPacks waiting Approval.
End If


<script>
        flowplayer("a.myPlayer", "http://releases.flowplayer.org/swf/flowplayer-3.2.16.swf", {
            clip: {
                // these two configuration variables does the trick
                autoPlay: false,
                autoBuffering: true // <- do not place a comma here
            }
        });
</script>

<script>
    //$(document).ready(function () {
        
        //Allow to hide the dropdown box if you click anywhere on the document.
        $('.signin-dropdown').click(function(e) {
            e.stopPropagation();
        });
    //    $(document).click(function() {
    //        $('#signin-dropdown').hide();
    //        $('#session').removeClass('active');
    //    });
    //});

        $(function () {
            $('.declinebut').click(function (e) {
            var id = e.target.id;
            var reason = $('#decline' + id).val();
            this.href = this.href + '?reason=' + encodeURIComponent(reason);
        });
        });

        $(document).ready(function () {
            // $("table > tbody tr:odd").css("background-color", "#FFFFFF");
            $("tr:odd").css("background-color", "#f7f7f7");
            $("tr:even").css("background-color", "#ffffff");
            $("th").css("background-color", "#f7f7f7");
            
        });

    function show(theLink) {

        id = theLink;
        //Conditional states allow the dropdown box appear and disappear 
        if ($('#signin-dropdown' + id).is(":visible")) {
            $('#signin-dropdown' + id).hide()
            $('#session').removeClass('active'); // When the dropdown is not visible removes the class "active"
        } else {
            $('#signin-dropdown' + id).show()
            $('#session').addClass('active'); // When the dropdown is visible add class "active"
        }
        return false;



    }


</script>



