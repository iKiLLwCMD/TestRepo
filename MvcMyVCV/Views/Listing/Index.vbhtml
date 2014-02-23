@ModelType PagedList.IPagedList(Of MvcMyVCV.Listing)

@Code
    ViewData("Title") = "Index"
End Code

@Using Html.BeginForm()
    @Html.AntiForgeryToken()
    @Html.ValidationSummary(True)

    @<fieldset>
        <legend></legend>

        @Html.DropDownList("CityId", Nothing, "--Select City--")
        @Html.DropDownList("CategoryId", Nothing, "--Select Category--")
        <input type="submit" value="Filter" />
    </fieldset>
End Using


@If Model.Count > 0 Then
    

    @:<br /><br />
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
    

    @:<table id="alt">
@For Each item In Model
        Dim currentItem = item
    @<tr>
        <td>
            <h3>@Html.ActionLink(currentItem.ListingTitle, "ViewListing", New With {.id = currentItem.ListingId})</h3><br /><br />
             @Html.DisplayFor(Function(modelItem) currentItem.ListingSummary)
        </td>

        <td>
          <td>
          <img src="~/UploadedListings/@currentItem.Image1" width="100px" height ="100px" />
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
