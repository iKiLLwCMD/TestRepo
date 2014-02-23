
<div id="EmailPreview"></div>
@Html.Partial("_PartialSharing")
<script src="http://connect.facebook.net/en_US/all.js"></script>


<h2>Send and Share</h2>

<table>
<tr>
<td style="vertical-align:top;width:80px;border-right:dashed;border-width:1px;padding-right:20px;" >
<h2>Apply via email</h2>
@Html.Label("Destination Email Address")
<br />
@Html.TextBox("txtEmail", "", New With {.class = "inputs", .style = "width:200px;padding-bottom:10px;height:10px;"})
<a style="padding-top:10px;" href="#" onclick="EmailPreview('@Session("NewVCVId")', {error: function() { alert('Could not load form') }}); return false;">Preview Email</a>
<br />

<button class="mybtn" onclick="sendEmail()">Send</button>     




 
</td>

<td style="vertical-align:top;width:100px;padding-left:20px;border-right:dashed;border-width:1px;padding-right:10px;">
<h2>Apply for jobs advertised on MyVCV</h2>
<table>
<tr>
<td>Enter a Listing ID or <a style="color:darkblue;" href="javascript:void(0)" onclick="ChooseWatchList( '' , {error: function() { alert('Could not load form') }}); return false;">Send from watchlist</a><br /><br />@Html.TextBox("txtJobId", "", New With {.class = "inputs", .style = "height:10px;"}) 
<br />
<a href="#" onclick="ListingPreview( '' , {error: function() { alert('Could not load form') }}); return false;">Preview Listing</a>
<br />
<button id="mybtn" onclick="applyForListing($('#txtJobId').val())">Send</button>  
</td>
</tr>

</table>
</td>
<td id="createOnlineProfile" style="vertical-align:top;padding-left:20px;">
<h2>Create Online Profile</h2>

Create a online profile with a link that can be advertied in a CV or online Job Application form.
<br /><br />
<button id="btnProfile" class="mybtn" onclick="beginCreateProfile()">Create</button><br /><br />

    @If ViewBag.onlineProfileLink IsNot Nothing Then
        @<p>Your profile is viewable at</p>
        @Html.TextBox("txtOnlineProfile", ViewBag.onlineProfileLink, New With {.class = "inputs", .style = "width:250px;height:10px;font-size:8pt", .readonly = "readonly"})
    End If
</td>

</table>

<script>
    $(function () {
        if($("#txtOnlineProfile").val() != null) {
            $("#btnProfile").html("Update");
        }
    });
    function showprogressbar() {
        document.getElementById('divAnim').style.display = "block";
    }
    function stopProgressbar() {
        document.getElementById('divAnim').style.display = "none";
    }

    function beginCreateProfile() {
        
        if ($("#txtOnlineProfile").val() == null) {
            createProfile("You have successfully created your online profile.", "newProfile");
        } else {
            if (confirm("Are you sure you want to override your Online Profile?")) {
                createProfile("You have updated your profile.", "updateProfile");
            }
        }
    }

    function createProfile(msg, method) {
        $.ajax({
            url: '@Url.Action("CreateProfile", "Create")',
            type: "POST",
            data: { VCVId: '@Session("NewVCVId")', CurrentUserId: '@ViewBag.CurrentUserId' },
            success: function (data) {
                if (data != 'False') {
                    alert(msg);
                    if(method == "newProfile") {
                        $("#createOnlineProfile").append("<p>Your profile is viewable at</p>");
                        $("#createOnlineProfile").append('<input class="inputs" id="txtOnlineProfile" name="txtOnlineProfile" readonly="readonly" style="width:250px;height:10px;font-size:8pt" type="text" value="@(String.Format("{0}:\\\\{1}{2}", Request.Url.Scheme, Request.Url.Authority, "\\ViewVCV\\"))' + data + '">');
                        $("#btnProfile").html("Update");
                    }
                } else {
                    alert("Something went wrong.");
                }
            }
        });
    }

    function sendEmail() {
        $.ajax({
            url: '@Url.Action("SendEmail", "Create")',
            type: "POST",
            data: {CurrentUserId : '@ViewBag.CurrentUserId', toEmail : $("#txtEmail").val(), VCVId: '@Session("NewVCVId")'},
            success: function (data) {
                alert("You have successfully sent your VCV!");
            }
        });
    }

    function EmailPreview(id, options) {
        //var tag = $("<div></div>");
       
        $.ajax({
            url: '@Url.Action("EmailPreview", "Create")',
            type: "POST",
            data: "id=" + id,
            success: function (data) {
                $("#EmailPreview").html(data)
                $("#EmailPreview").dialog({ modal: true, width: 850, title: "Email Preview" }).dialog('open');
            }
        });
    }

    function ListingPreview(id, options) {
        //var tag = $("<div></div>");
        id = document.getElementById('txtJobId').value;
        $.ajax({
            url: '@Url.Action("ListingPreview", "Create")',
            type: "POST",
            data: "id=" + id,
            success: function (data) {
                $("#EmailPreview").html(data)
                $("#EmailPreview").dialog({ modal: true, width: 850, title: "Listing Preview" }).dialog('open');
            }
        });
    }

    function ChooseWatchList(id, options) {
        //var tag = $("<div></div>");
        
         $.ajax({
            url: '@Url.Action("WatchListSelect", "Create")',
             type: "POST",
             success: function (data) {
                $("#EmailPreview").html(data)
                $("#EmailPreview").dialog({
                    modal: true, width: 850,
                    title: "Choose From Watchlist",
                    buttons: { "Send": function () {
                        var ListingId = $('input[name=WatchList]:checked').val();
                        applyForListing(ListingId);
                        $(this).dialog("close");
                    },
                            cancel: function () { $(this).dialog("close"); }
                    }
                }).dialog('open');
            }
        });
    }

    function applyForListing(ListingId) {
        $.ajax({
            url: '@Url.Action("Apply", "Create")',
            type: "POST",
            data: {VCVId : @Session("NewVCVId") + "", ListingId : ListingId},
            success: function (data) {
                alert(data);
            }
        })
    }

   
</script>

<script src="~/Scripts/Share.js"></script>