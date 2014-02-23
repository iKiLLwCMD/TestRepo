@ModelType MvcMyVCV.Listing

@Code
    ViewData("Title") = "ViewListing"
End Code

<script type="text/javascript" src="/Scripts/jquery.timers-1.2.js"></script>
<script type="text/javascript" src="/Scripts/jquery.easing.1.3.js"></script>
<script type="text/javascript" src="/Scripts/jquery.galleryview-3.0-dev.js"></script>
<link type="text/css" rel="stylesheet" href="/Content/jquery.galleryview-3.0-dev.css" />
<script>
    $(function () {
               
        if ('@ViewBag.Added' == "True") {
            $('a#watchlist').attr('onclick', '').unbind('click');
            $('a#watchlist').text('Added to watchlist');
        }

	    $('#myGallery').galleryView({
	        panel_width: 350,
	        panel_height: 260,
	    });
	});

	function WatchListIt(id) {
	        // Save it!
	        $.ajax(
{
    url: '@Url.Action("WatchList", "Listing")',
    type: "POST",
    data: "id=" + id,
    success: function (data) {
        if (data == "True") {
            //$('a#watchlist').removeAttr('href');
            $('a#watchlist').attr('onclick', '').unbind('click');
            $('a#watchlist').text('Added to watchlist');
        }
        else {
            document.location = "../../Account/Login"

        }

    }


})
       


    }
</script>



<fieldset>
    <legend>Listing</legend>

     <div class="display-field">
       <h2>@Html.DisplayFor(Function(model) model.ListingTitle)</h2> 
    </div>
        @If Model.Image1 <> "" Or Model.Image2 <> "" Or Model.Image3 <> "" Then
        @:<div class="ListingImages">
        
     @: <ul id="myGallery">

    @If Model.Image1 <> "" Then
        @:<li><img src="/UploadedListings/@(Model.Image1)" alt="" />
    End If
       @If Model.Image2 <> "" Then
        @:<li><img src="/UploadedListings/@(Model.Image2)" alt="" />
    End If
      @If Model.Image3 <> "" Then
        @:<li><img src="/UploadedListings/@(Model.Image3)" alt="" />
    End If       
          

        @:</ul>

        @:</div>

End If
    <br />
 
    <div class="display-field">
        Company: @Html.DisplayFor(Function(model) model.Company)
    </div>
       
    <div class="display-field">
       Category: @Html.DisplayFor(Function(model) model.SubCategory.Category.CategoryName) - @Html.DisplayNameFor(Function(model) model.SubCategory.SubCategoryName) 
    </div>
      <div class="display-field">
        Location: @Html.DisplayFor(Function(model) model.Suburb.City.CityName) - @Html.DisplayFor(Function(model) model.Suburb.SuburbName)
    </div>

    <div class="display-field">
        Type: @Html.DisplayFor(Function(model) model.ListingType.ListingDescription)
    </div>

    <div class="display-field">
     <p class="ListingDescription">
        @Model.ListingDescription.ToString.Trim
    </p>
    </div>



</fieldset>
<p>

<a href="#"  id="watchlist" onclick="WatchListIt(@Model.ListingId);return false;">Add to Watchlist</a>  <br />


    @*@Html.ActionLink("Edit", "Edit", New With {.id = Model.ListingId}) |
    @Html.ActionLink("Back to List", "Index")*@
</p>
