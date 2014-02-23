@ModelType MvcMyVCV.Listing

@Code
    ViewData("Title") = "Create"
End Code
<h1>@ViewBag.Test</h1>
<h2>Create Listing</h2>
<br />
@Using Html.BeginForm(Nothing, Nothing, FormMethod.Post, New With {.enctype = "multipart/form-data"})
    @Html.AntiForgeryToken()
    @Html.ValidationSummary(True)

    @<fieldset>
        <legend>Listing</legend>
        @Html.TextBoxFor(Function(model) model.ListingTitle, New With {.class = "inputs", .placeholder = "Title"})
        @Html.ValidationMessageFor(Function(model) model.ListingTitle)
        <br />
        @Html.TextBoxFor(Function(model) model.Company, New With {.class = "inputs", .placeholder = "Company"})
        @Html.ValidationMessageFor(Function(model) model.Company)
        <br />
        @Html.DropDownList("CategoryId", Nothing, "--Select a category--", New With {.style = "width:290px;padding: 10px 5px;", .class = "inputs", .placeholder = "Category"})
        <select class="inputs" id="SubCategoryId" name="SubCategoryId" style="width:290px;padding: 10px 5px;"></select>
        @Html.ValidationMessageFor(Function(model) model.SubCategoryId)
        <br />
        @Html.DropDownListFor(Function(model) model.ListingTypeId, TryCast(ViewBag.ListingTypeId, SelectList), New With {.style = "width:290px;padding: 10px 5px;", .class = "inputs", .placeholder = "Listing Type"})
        @Html.ValidationMessageFor(Function(model) model.ListingTypeId)
        <br />
        @Html.DropDownListFor(Function(model) model.ListingPayId, TryCast(ViewBag.ListingPayId, SelectList), New With {.style = "width:290px;padding: 10px 5px;", .class = "inputs", .placeholder = "Listing Pay"})
        @Html.ValidationMessageFor(Function(model) model.ListingPayId)
        <br />
        @Html.TextBoxFor(Function(model) model.ListingReference, New With {.class = "inputs", .placeholder = "Listing Reference"})
        @Html.ValidationMessageFor(Function(model) model.ListingReference)
        <br />
        @Html.TextAreaFor(Function(model) model.ListingSummary, 12, 37, New With {.style = "width:563px;padding: 10px 10px;font:-webkit-small-control;", .class = "inputs", .placeholder = "Listing Summary"})
        @Html.ValidationMessageFor(Function(model) model.ListingSummary)
        <br />
        @Html.TextAreaFor(Function(model) model.ListingDescription, 12, 37, New With {.style = "width:563px;padding: 10px 10px;font:-webkit-small-control;", .class = "inputs", .placeholder = "Listing Description"})
        @Html.ValidationMessageFor(Function(model) model.ListingDescription)
        <div class="editor-field">
            @Html.CheckBoxFor(Function(model) model.IsAdvertised)
            @Html.ValidationMessageFor(Function(model) model.IsAdvertised)
            Check this if you want this job to be advertised
        </div>
        <br />
        @Html.DropDownList("CityId", Nothing, "--Select City--", New With {.style = "width:290px;padding: 10px 5px;", .class = "inputs", .placeholder = "Suburb"})
        <select class="inputs" id="SuburbId" name="SuburbId" style="width:290px;padding: 10px 5px;"></select>
        @Html.ValidationMessageFor(Function(model) model.SuburbId)
        <br />
        <table>
            <tr>
                <td>
                    <div id="divLogo" class="inputs">
                        <div class="inputWrapper" style="width: 100px;">Choose Logo<input class="fileInput" id="Image1" type="file" name="Files" value="" onchange="addFileNamesToUploadControl(id, 'divLogo')" accept="image/*"/></div>
                        <div id="logoContent"></div>
                        @Html.HiddenFor(Function(model) model.Image1, New With { .id = "logo" })
                    </div>
                </td>
                <td style="text-align: left;padding-left: 0.1rem;">
                    <span>@Html.ValidationMessageFor(Function(model) model.Image1, "Logo is required")</span>
                </td>
            </tr>
        </table>

        <div id="divListingImages" class="inputs">
            <div class="inputWrapper">Choose Listing Images<input class="fileInput" id="fileImages" type="file" name="Files" value="" multiple="multiple" onchange="addFileNamesToUploadControl(id, 'divListingImages')" accept="image/*"/></div>
        </div>
        <br />
        <div class="editor-field">
            @Html.CheckBoxFor(Function(model) model.EmailNotificationsEnabled)
            @Html.ValidationMessageFor(Function(model) model.EmailNotificationsEnabled)
            Check this if you want applicants to recieve an automatic email...
        </div>
        <div id="emailSelection" style="display:none;">
            <table>
                <tr class="inputs" style="border:solid;border-width:1px;border-color:grey;">
                    <td style="width:20%; vertical-align:top; padding-left: 10px;"><br />
                        @Html.DropDownListFor(Function(model) model.EmailTemplateId, TryCast(ViewBag.EmailTemplates, SelectList), New With {.style = "width:200px; height: 100%;", .size = 5, .id = "EmailTemplateId", .class = "inputs"})
                    </td>
                    <td id="emailData" style="width:80%;padding-bottom : 15px;"><br />
                        <div id="shortListData">
                            <div id="newTitle">@Html.TextBox("txtTemplateName", "", New With {.class = "inputs", .placeholder = "Template Name"})</div>
                            @Html.Label("Short listed Email Template")<br />
                            <div id="ShortListed" style="border-width: 1px; border-style: solid; border-color: black; width: 100%; float:right; height: 200px; overflow-y: scroll;background-color:white;"></div>
                        </div>
                        &nbsp;<br />
                        <div id="unsuccessfulData">
                            @Html.Label("Unsuccessful Email Template")<br />
                            <div id="Unsuccessful" style="border-width: 1px; border-style: solid; border-color: black; width: 100%; float:right; height: 200px; overflow-y: scroll;background-color:white;"></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        @*<a id="new" href="javascript:void(0)" onclick="newEmailTemplate()">New</a><br />*@
                        <button type="button" id="new" onclick="newEmailTemplate()">New</button>
                        <button type="button" id="edit" onclick="editEmailTemplate('saveEmailTemplate()')">Edit Selected</button>
                        <button type="button" id="save" onclick="saveEmailTemplate()">Save</button>
                        <button type="button" id="cancel" onclick="closeEdit()">Close</button>
                    </td>
                </tr>
            </table>
            @*@Html.DropDownList("EmailTemplates", TryCast(ViewBag.EmailTemplates, SelectList), New With { .style = "width:200px;", .size = 5})*@
        </div>
        <br />
        <p>
            <input class="vcvbutton" type="submit" value="Create" />
        </p>
    </fieldset>
            End Using

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
<script type="text/javascript" src="@Url.Content("~/Content/javascript/ckeditor/ckeditor.js")"></script>
<script>
    
</script>

<script>
    var isSystem;
    function isSystemTemplate() {
        $.ajax({
            url: '@Url.Action("IsSystemTemplate", "Listing")',
            type: "POST",
            data: { EmailTemplateId: $("#EmailTemplateId").val() },
            success: function (data) {
                if (data == "True") {
                    $("#edit").hide();
                    isSystem = true;
                } else {
                    $("#edit").show();
                }
            }
        });
    }

    var title = $("#newTitle");
    title.hide();
    function newEmailTemplate() {
        isNew = true
        title.show();
        shortListData = null;
        unsuccessfulData = null;
        editEmailTemplate("saveNewEmailTemplate()");
        isNew = false;
    }
    function editEmailTemplate(saveMethod) {
        $("#save").attr("onclick", saveMethod);
        $("#ShortListed").html(shortListData);
        $("#Unsuccessful").html(unsuccessfulData);
        $("#EmailTemplateId").attr("disabled", "disabled");
        $("#edit").hide();
        $("#new").hide();
        $("#save").show();
        $("#cancel").show();

        CKEDITOR.replace("ShortListed");
        CKEDITOR.replace("Unsuccessful");
    }
    var isNew;
    function saveNewEmailTemplate() {
        var templateName = $("#txtTemplateName").val();
        if (templateName != "") {
            var sData = (String)(CKEDITOR.instances['ShortListed'].getData());
            var uData = (String)(CKEDITOR.instances['Unsuccessful'].getData());
            $.ajax({
                url: '@Url.Action("SaveNewEmailTemplate", "Listing")',
                type: "POST",
                data: { templateName: templateName, shortListContent: sData, unsuccessfulContent: uData },
                success: function (data) {
                    refreshTemplateList(data, templateName)
                }
            });
            closeEdit();
        } else {
            alert("Please enter a Template Name");
        }
    }

    function refreshTemplateList(id, templateName) {
        $("#EmailTemplateId").append('<option value="' + id + '">' + templateName + '</option>');
        $("#EmailTemplateId").val(id);
        $("#EmailTemplateId").change();
    }

    function saveEmailTemplate() {
        var id = $('#EmailTemplateId option:selected').val();
        var slcontent = (String)(CKEDITOR.instances['ShortListed'].getData());
        var ucontent = (String)(CKEDITOR.instances['Unsuccessful'].getData());
        $.ajax({
            url: '@Url.Action("SaveEmailTemplate", "Listing")',
            type: "POST",
            data: { id: id, shortListContent: slcontent, unsuccessfulContent: ucontent },
            success: function (data) {

            }
        });

        shortListData = slcontent;
        unsuccessfulData = ucontent;
        closeEdit();
        $("#EmailTemplateId").val($("#EmailTemplateId").val());
        $("#EmailTemplateId").change();
    }

    function closeEdit() {
        CKEDITOR.instances['ShortListed'].destroy();
        CKEDITOR.instances['Unsuccessful'].destroy();

        $("#ShortListed").empty();
        $("#Unsuccessful").empty();
        if (shortListData && unsuccessfulData) {
            $("#ShortListed").html(shortListData);
            $("#Unsuccessful").html(unsuccessfulData);
        } else {
            //$("#EmailTemplateId").val($("#EmailTemplateId").val());
            //$("#EmailTemplateId").change();
        }

        if (isNew == true) {
            isSystem = false
            $("#EmailTemplateId").val($("#EmailTemplateId").val());
            $("#EmailTemplateId").change();
        }


        $("#EmailTemplateId").removeAttr("disabled");
        $("#edit").show();
        $("#new").show();
        $("#save").hide();
        $("#cancel").hide();
        title.hide();
        $("#txtTemplateName").val("");
        if (isSystem == true) {
            isSystem = false
            $("#EmailTemplateId").val($("#EmailTemplateId").val());
            $("#EmailTemplateId").change();
        }
    }

    var shortListData;
    var unsuccessfulData;
    $("#EmailTemplateId").change(function () {
        isSystemTemplate();
        $.ajax({
            url: '@Url.Action("GetEmailTemplate", "Listing")',
            type: "POST",
            data: { id: $('#EmailTemplateId option:selected').val() },
            success: function (data) {
                var etds = JSON.parse(data);
                var sData;
                var uData;
                for (i = 0; i < etds.length; i++) {
                    var etd = etds[i];
                    for (var key in etd) {
                        if (key == "Value") {
                            if (i == 0) {
                                sData = etd[key];
                            } else {
                                uData = etd[key];
                            }
                        }
                    }
                }
                shortListData = sData;
                unsuccessfulData = uData;
                $("#ShortListed").html(shortListData);
                $("#Unsuccessful").html(unsuccessfulData);
            }
        });
    });

</script>

<script>

    $(function () {
        $(document).on("change", "#EmailNotificationsEnabled", function () {
            var isChecked = $("#EmailNotificationsEnabled").prop("checked")
            if (isChecked) {
                $("#EmailTemplateId").val($("#EmailTemplateId option:first").val());
                $("#EmailTemplateId").change();
                $("#save").hide();
                $("#cancel").hide();

                $("#emailSelection").show();
            } else {
                $("#emailSelection").hide();
            }
        });
    });
</script>

<script>
    $(function () {

        if ('@ViewBag.SelectedSubCategory' > 0) {

            $.ajax(
                    {
                        url: '@Url.Action("GetSubCategory", "Listing")',
                        type: "POST",
                        data: "CategoryId= " + $("#CategoryId").val(),
                        success: function (subCategories) {
                            $("#SubCategoryId").empty();
                            for (var i = 0; i < subCategories.length; i++) {

                                $("#SubCategoryId").append($('<option/>', {
                                    value: subCategories[i].Id, text: subCategories[i].Name
                                }));


                            }
                            $("#SubCategoryId").prop("disabled", false);
                            $("#SubCategoryId option[value='@ViewBag.SelectedSubCategory']").attr("selected", "selected");

                        }

                    });

                }


        if ('@ViewBag.SelectedSuburb' > 0) {
            $.ajax(
                               {
                                   url: '@Url.Action("GetSuburb", "Listing")',
                                   type: "POST",
                                   data: "CityId= " + $("#CityId").val(),
                                   success: function (suburbs) {
                                       $("#SuburbId").empty();
                                       for (var i = 0; i < suburbs.length; i++) {

                                           $("#SuburbId").append($('<option/>', {
                                               value: suburbs[i].Id, text: suburbs[i].Name
                                           }));


                                       }
                                       $("#SuburbId").prop("disabled", false);
                                       $("#SuburbyId option[value='@ViewBag.SelectedSuburb']").attr("selected", "selected");

                                   }

                               });

                           }





        $("#SubCategoryId").prop("disabled", true);
        $("#SuburbId").prop("disabled", true);
        $("#CategoryId").change(function () {
            if ($("#CategoryId").val() != "Please select") {
                $.ajax(
                    {
                        url: '@Url.Action("GetSubCategory", "Listing")',
                        type: "POST",
                        data: "CategoryId= " + $("#CategoryId").val(),
                        success: function (subCategories) {
                            $("#SubCategoryId").empty();
                            for (var i = 0; i < subCategories.length; i++) {

                                $("#SubCategoryId").append($('<option/>', {
                                    value: subCategories[i].Id, text: subCategories[i].Name
                                }));


                            }


                            $("#SubCategoryId").prop("disabled", false);
                        }

                    });


            }
        })

        $("#CityId").change(function () {
            if ($("#CityId").val() != "Please select") {
                $.ajax(
                    {
                        url: '@Url.Action("GetSuburb", "Listing")',
                        type: "POST",
                        data: "CityId= " + $("#CityId").val(),
                        success: function (suburbs) {
                            $("#SuburbId").empty();
                            for (var i = 0; i < suburbs.length; i++) {
                                //$("#SubCategoryId").append("<option>" + subCategories[i].Name + "</option>");
                                $("#SuburbId").append($('<option/>', {
                                    value: suburbs[i].Id, text: suburbs[i].Name
                                }));
                            }
                            $("#SuburbId").prop("disabled", false);
                        }

                    });


            }
        })


    });

</script>
<script>
    function addFileNamesToUploadControl(id, parentID) {
        // love the query selector
        var fileInput = document.querySelector("#" + id);
        var files = fileInput.files;
        // cache files.length 
        var fl = files.length;
        var i = 0;
        if (id == "Image1") {
            $("#logo").val(files[0].name);
            document.getElementById("logoContent").innerHTML = '<div class="divUpload">' + files[0].name + '</div>';
        } else {
            // localize file var in the loop
            var $fileUpload = $("#fileImages");
            if (parseInt($fileUpload.get(0).files.length) > 2) {
                alert("You can only upload a maximum of 2 files");
            } else {
                while (i < fl) {
                    var file = files[i];

                    $("#" + parentID).append('<div class="divUpload">' + file.name + '</div>');
                    i++;
                }
            }
        }
    }

</script>