@Imports MvcMyVCV
@ModelType PagedList.IPagedList(Of Listing)
@Code
    ViewData("Title") = "Index"
End Code
<div>@Html.ActionLink("+ Create New Listing", "Create", "Listing", Nothing, New With {.class = "vcvbutton", .style = "background:#E8E8E8;color:#000;padding:5px 15px 5px 15px;text-decoration: none;float:right;"})</div>
@Using Html.BeginForm()
    @Html.AntiForgeryToken()
    @<fieldset>
    <legend>Sort By Expired Status</legend>
        Filter By Expired Status
         @Html.DropDownList("Status", TryCast(ViewBag.selectList, SelectList), New With {.onchange = "this.form.submit();"})
     </fieldset>       
End Using

@If Model.Count > 0 Then
    
    @:<div id="pageControl">
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
    
@:</div><br />

 @:<table id="alt" width="100%" >
@<tr>
        <th></th>
        <th></th>
        <th></th>
        <th></th>
        <th></th>
        <th colspan="2">Applicants</th>
     </tr>    
    @<tr style="background-color:white;">
        <th></th>
        <th></th>

        <th style="text-align:center;">Expiry Date</th>
        <th></th>
        <th></th>
        <th>New</th>
        <th>Total</th>
     </tr>
@For Each item In Model
    Dim currentItem = item
    Dim newApplicantCount As Integer = 0
    Dim totalApplicantCount As Integer = 0
   
    @<tr>
        <td style="padding-left:10px;">
            <div style="padding-bottom:10px;">@Html.ActionLink("Applicants", "Applicants", "Manage", New With {.id = currentItem.ListingId}, New With {.class = "vcvbutton", .style = "background:#E8E8E8;color:#000;padding:2px 12px 2px 12px;text-decoration: none;"})</div>
            @If item.ExpiryDate < Now() Then
                    @Html.ActionLink("Renew", "RenewListing", "Manage", New With {.id = currentItem.ListingId}, New With {.class = "vcvbutton", .style = "background:#E8E8E8;color:#000;padding:2px 25px 2px 25px;text-decoration: none;"})
            ElseIf item.Fulfilled = True And item.ExpiryDate > Now() Then
                @<div style="padding-bottom:10px;"><a class="vcvbutton" style="background:#E8E8E8;color:#000;padding:2px 19px 2px 19px;text-decoration: none;" onclick="changeFulfilledStatus(@item.ListingId, false, this)">Re-open</a></div>
            Else
                @<div style="padding-bottom:10px;"><a class="vcvbutton" style="background:#E8E8E8;color:#000;padding:2px 29px 2px 29px;text-decoration: none;" onclick="changeFulfilledStatus(@item.ListingId, true, this)">Close</a></div>
                @<div>@Html.ActionLink("Edit", "EditListing", "Manage", New With {.id = item.ListingId}, New With {.class = "vcvbutton", .style = "background:#E8E8E8;color:#000;padding:2px 35px 2px 35px;text-decoration: none;"})</div>
            End If
            
            
        </td>
        <td style="width:500px;">
            <h3>@Html.ActionLink(currentItem.ListingTitle, "ViewListing", "Listing", New With {.id = currentItem.ListingId}, Nothing)</h3><br />
             @Html.DisplayFor(Function(modelItem) currentItem.ListingSummary)
        </td>
        <td style="width:auto;">
            @CDate(item.ExpiryDate).ToShortDateString()
        </td>
        <td>
          <td>
          <img src="~/UploadedListings/@currentItem.Image1" width="100px" height ="100px" />
        </td>

        <td>
        
            @For Each la In ViewBag.allUsersApplicants
    If currentItem.ListingId = la.ListingId Then
        If la.IsViewed = False Then newApplicantCount += 1
        totalApplicantCount += 1
    End If
                Next
            @newApplicantCount
        </td>

        <td>
            @totalApplicantCount
        </td>
    </tr>
    
    Next



@:</table>
End If
<div id="noListings"></div>

<script>

    function changeFulfilledStatus(ListingId, Status, r) {
        $.ajax({
            url: '@Url.Action("ChangeFulfilledStatus", "Home")',
            type: "POST",
            data: { ListingId: ListingId, Status: Status },
            success: function (data) {
                var i = r.parentNode.parentNode.parentNode.rowIndex;
                document.getElementById("alt").deleteRow(i);
                refreshCSS()
                var rowCount = document.getElementById("alt").rows.length;
                if (rowCount == 2) {
                    var table = document.getElementById("alt");
                    table.parentNode.removeChild(table);

                    var pageControl = document.getElementById("pageControl");
                    pageControl.parentNode.removeChild(pageControl);

                    noListingsMessage();

                }
            }
        })
    }

    function noListingsMessage() {
        var rowCount = 0;
        if (document.getElementById("alt") != null) {
            rowCount = document.getElementById("alt").rows.length
        }
        if (rowCount < 3) {
            $("#noListings").append('<br /><p id="pNoListings">You have no ' + $("#Status").val().toString().toLowerCase() + ' listings.</p>');
        }

    }

    $(function () {
        noListingsMessage();
    });

</script>

<script>
    function refreshCSS() {
        $("#alt tr:odd").css("background-color", "#f7f7f7");
        $("#alt tr:even").css("background-color", "#ffffff");
        $("th").css("background-color", "#ffffff");
    }
    $(document).ready(function () {
        refreshCSS()
    });
</script>