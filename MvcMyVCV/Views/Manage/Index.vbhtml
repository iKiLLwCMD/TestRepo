@Imports MvcMyVCV
@ModelType PagedList.IPagedList(Of Listing)


@If Model.Count > 0 Then
    

    @:<div>
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
@:</div>
            
    @<div>@Html.ActionLink("Create New Listing", "Create", "Listing", Nothing, Nothing)</div>
    
    @:<table id="alt" width="100%" >
@<tr>
        <th></th>
        <th></th>
        <th></th>
        <th></th>
        <th colspan="2">Applicants</th>
     </tr>    
    @<tr style="background-color:white;">
        <th></th>
        <th></th>
        <th></th>
        <th></th>
        <th>New</th>
        <th>Total</th>
     </tr>
@For Each item In Model
        Dim newApplicantCount As Integer = 0
        Dim totalApplicantCount As Integer = 0

    @<tr>
        <td>
            @*<button onclick="@("location.href='" + Url.Action("Applicants", New With {.id = item.ListingId}) + "'")">Applicants</button>
            <br />
            <button onclick="@("location.href='" + Url.Action("EditListing", New With {.id = item.ListingId}) + "'")">Edit</button>*@

            @Html.ActionLink("=>Applicants", "Applicants", "Manage", New With {.id = item.ListingId}, New With {.id = "vcvbutton"})
            <br />
            @Html.ActionLink("=>Edit", "EditListing", "Manage", New With {.id = item.ListingId}, New With {.id = "vcvbutton"})
        </td>
        <td>
            <h3>@Html.ActionLink(item.ListingTitle, "ViewListing", "Listing", New With {.id = item.ListingId}, Nothing)</h3><br /><br />
             @Html.DisplayFor(Function(modelItem) item.ListingSummary)
        </td>

        <td>
          <td>
          <img src="~/UploadedListings/@item.Image1" width="100px" height ="100px" />
        </td>

        <td>
        
            @For Each la In ViewBag.allUsersApplicants
                    If item.ListingId = la.ListingId Then
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
<script>

    $(document).ready(function () {
        // $("table > tbody tr:odd").css("background-color", "#FFFFFF");
        $("#alt tr:odd").css("background-color", "#f7f7f7");
        $("#alt tr:even").css("background-color", "#ffffff");
        $("th").css("background-color", "#ffffff");
    });
</script>
