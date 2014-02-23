<h2>Step 4 - Personal Summary</h2>

@Using Html.BeginForm()
    @<fieldset>
    <legend>Create Summary</legend>
<button class="showNewSummary">
    Create New
</button>
<button class="showExistingSummary" id="">
    Use Existing
</button>

<br />



<style type="text/css">
.inv
{
    display: none ;
    visibility: hidden;
}
.vis
{
    display: inline;
    visibility: visible;
}
</style>

@Html.Hidden("Summary", ViewBag.SummaryIdHid)

     <script>
         $(function () {

             $("#SummaryId").change(function () {

                 // get script
                 $.ajax(
                 {
                     url: '@Url.Action("GetSummary", "create")',
                     type: "POST",
                     data: "id= " + $('#SummaryId option:selected').val(),
                     success: function (data) {
                         // put data in textbox
                         //$('#Existing').text(data);

                         $("#ExistingSummary").html(data);



                     }


                 })
             });

            
             //alert("In Function")
             id = $("#Summary").val();
             if (id > 0) {
                 $("#divNew").addClass("inv");
                 $("#divNew").removeClass("vis");
                 $("#divExisting").addClass("vis");
                 $("#divExisting").removeClass("inv");
                 $('#SummaryId').val(id)
                 $('#SummaryId').val(id).change();
                 $('#editsum').removeClass("vis");
                 $('#editsum').addClass("inv");
                 
              
             }
             else
             {
                 
                 $("#SummaryId").val($("#SummaryId option:first").val());
                 $('#SummaryId').change();
                 }

                 
              
  
                
             

             

         });

         //$(".selectOneListBox").live(function () {
         //    $(this).on.removeAttr('multiple');
         //});
              </script> 

   
        
   <div class="vis" id="divNew">
    @Html.Label("Summary Name:")
      @Html.TextBox("SummaryName", "", New With {.maxlength = 25}) 
      @Html.Label("Summary:")
@Html.TextArea("CreateSummary")
</div> 

        
          <div class="inv" id="divExisting">
<table>
<tr>
<td style="width:19%; vertical-align:top;">
 @Html.ListBox("SummaryId", TryCast(ViewBag.selectList, SelectList), New With {.class = "selectOneListBox", .Size = 10, .style = "width: 100%"})</td>
 <td style="width:80%;">
 <div id="editsum">
 @Html.TextArea("EditSummary", New With {.style = "display: none;visibility: hidden;"})
 </div>
 <div id="ExistingSummary"  style="border-width: 1px; border-style: solid; border-color: black; width: 100%; height: 200px; overflow-y: scroll; float:right;"></div>
  
   </td>
</tr>
<tr>
<td><input id="clickMe" type="button" value="Remove" onclick="RemoveSummary()" /></td>
<td><input id="editSummary" type="button" value="Edit" onclick="SummaryEdit()" /> <input id="saveSummary" type="button" class="inv" value="Save" onclick="SummarySave()" /> <input id="cancel" type="button" class="inv" value="Cancel" onclick="CancelEdit()" />  </td>

</tr>

</table>  
 </div>
    



     </fieldset>       
End Using





<script type="text/javascript">
    $(function () {
        $(".showNewSummary").click(function () {
            $("#divNew").addClass("vis");
            $("#divNew").removeClass("inv");

            $("#divExisting").addClass("inv");
            $("#divExisting").removeClass("vis");


            return false;
        });
        $(".showExistingSummary").click(function () {
            $("#divNew").addClass("inv");
            $("#divNew").removeClass("vis");

            $("#divExisting").addClass("vis");
            $("#divExisting").removeClass("inv");
            $('#editsum').addClass("inv");
            $('#editsum').removeClass("vis");
            return false;
        });
       
    });

    function SummaryEdit() {

        //var instance = CKEDITOR.instances['CreateSummary'];
        //if (instance) {
        //    CKEDITOR.remove(instance); //if existed then remove it 
        //    CKEDITOR.replace('EditSummary');
        //}


        CKEDITOR.instances['EditSummary'].setData($("#ExistingSummary").html());
        $("#ExistingSummary").addClass("inv");
        $("#editSummary").addClass("inv");
        $("#saveSummary").removeClass("inv");
        $("#cancel").removeClass("inv");
        $('#SummaryId').attr('disabled', 'disabled');
        $('#editsum').removeClass("inv");
        $('#editsum').addClass("vis");
    }


    function RemoveSummary() {
        // get script
        var optionToRemove = $('#SummaryId option:selected')
        $.ajax(
        {
            url: '@Url.Action("RemoveSummary", "create")',
                 type: "POST",
                 data: "id= " + $('#SummaryId option:selected').val(),
                 success: function (data) {
                     //refresh scripts
                     $("#SummaryId > option:selected").each(function () {
                         $(this).remove();
                     });



                 }


             })


         }

         function SummarySave() {
             var htmlout = CKEDITOR.instances['EditSummary'].getData();
             var encoded = encodeURIComponent(htmlout);

             $.ajax(
                               {
                                   url: '@Url.Action("EditSummary", "create")',
                                   type: "POST",
                                   data: "editor=" + encoded + "&SummaryId=" + $('#SummaryId option:selected').val(),
                                   //data: "Current= " + selected + "&Selected= " + ui.index,
                                   success: function (data) {
                                       if (data == "True") {
                                           //show div again
                                           $("#ExistingSummary").html(CKEDITOR.instances['EditSummary'].getData());
                                           $("#ExistingSummary").removeClass("inv");
                                           $("#editSummary").removeClass("inv");
                                           $("#saveSummary").addClass("inv");
                                           $("#cancel").addClass("inv");
                                           $('#SummaryId').removeAttr('disabled');
                                           $('#editsum').addClass("inv");
                                           $('#editsum').removeClass("vis");


                                       }

                                   }

                               });



         }

         function CancelEdit() {

             $("#ExistingSummary").removeClass("inv");
             $("#editSummary").removeClass("inv");
             $("#saveSummary").addClass("inv");
             $("#cancel").addClass("inv");
             $('#SummaryId').removeAttr('disabled');
             $('#editsum').addClass("inv");
             $('#editsum').removeClass("vis");



         }

    
</script>

<script type="text/javascript" src="@Url.Content("~/Content/javascript/ckeditor/ckeditor.js")"></script>
<script>
    CKEDITOR.replace('CreateSummary');
    CKEDITOR.replace('EditSummary');
 </script>