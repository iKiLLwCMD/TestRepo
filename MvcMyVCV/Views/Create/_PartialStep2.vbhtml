@*@ModelType MvcMyVCV.VCV*@
<script src="~/Scripts/jquery.jcarousel.js"></script>
<script src="~/Scripts/jquery.jcarousel.min.js"></script>


<style>


.preload {
display: block;
position: absolute;

}
.jcarousel-skin-tango .jcarousel-container-horizontal {
    width: 100%;
}

.jcarousel-skin-tango .jcarousel-clip-horizontal {
    width: 100%;
}
</style>
<h2>Step 2 - Script</h2>
<br />
@Using Html.BeginForm()
    @<fieldset>
    <legend>Create Script</legend>
<button class="showNew">
    Create New
</button>
<button class="showExisting" id="">
    Use Existing Script
</button>
<button class="showTemplate">
    Use Script Template
</button>
<button class="showVideos">
    Use Existing Video
</button>
<br />


<link rel="stylesheet" type="text/css" href="../skins/tango/skin.css" />
<style type="text/css">
.inv
{
    display: none;
    visibility: hidden;
}
.vis
{
    display: inline;
    visibility: visible;
}
</style>
<script>
    flowplayer("a.myPlayer", { src: "http://releases.flowplayer.org/swf/flowplayer-3.2.16.swf", wmode: "opaque" }, {
        clip: {
            // these two configuration variables does the trick
            autoPlay: false,
            bufferLength: 5,
            autoBuffering: true // <- do not place a comma here
        },
        plugins: {
            controls: {
                // disable time/duration display and the scrubber
                time: false
            }
        }
    });
</script>

@Html.Hidden("Script", ViewBag.ScriptIdHid)
@If ViewBag.ExistingVideo Is Nothing = False Then
    @Html.Hidden("ExistingVideo", ViewBag.ExistingVideo)        
End If


     <script>
         $(function () {

             
                       
             $('#mycarousel').jcarousel({
                 // Configuration goes here
                 visible: 3,
                 itemFallbackDimension: 220,
                 setupCallback: function (carousel, state) {
                     $("#div4").removeClass("preload");
                     $("#div4").addClass("inv");
             }
             });


           


             $("#ScriptId").change(function () {

                 // get script
                 $.ajax(
                 {
                     url: '@Url.Action("GetScript", "create")',
                     type: "POST",
                     data: "id= " + $('#ScriptId option:selected').val(),
                     success: function (data) {
                         // put data in textbox
                         //$('#Existing').text(data);

                         $("#Existing").html(data);



                     }


                 })
             });

             $("#TemplateId").change(function () {

                 // get script
                 $.ajax(
                 {
                     url: '@Url.Action("GetScript", "create")',
                     type: "POST",
                     data: "id= " + $('#TemplateId option:selected').val(),
                     success: function (data) {
                         // put data in textbox
                         //$('#Existing').text(data);

                         $("#Template").html(data);



                     }


                 })
              });


             //alert("In Function")

             id = $("#Script").val();
             if (id > 0) {
                 $("#div1").addClass("inv");
                 $("#div1").removeClass("vis");
                 $("#div2").addClass("vis");
                 $("#div2").removeClass("inv");
                 $('#ScriptId').val(id)
                 $('#ScriptId').val(id).change();
                 $("#edit").addClass("inv");
                 
                 // get script
             }
             else
             {
                 
                 $("#ScriptId").val($("#ScriptId option:first").val());
                 $('#ScriptId').change();
             }

             videoid = $("#ExistingVideo").val();

             if (videoid != undefined) {
                 $("#div1").addClass("inv");
                 $("#div1").removeClass("vis");
                 $("#div2").addClass("inv");
                 $("#div2").removeClass("vis");
                 $("#div4").addClass("vis");
                 $("#div4").removeClass("inv");
             }


                 
              
  
                
             

             

         });

         function SelectVideo(id) {
             //alert(id);

             $.ajax(
                {
                    url: '@Url.Action("SelectVideo", "create")',
                     type: "POST",
                     data: "id=" + id,
                     success: function (data) {
                         if (data = true) {
                             //change to tab 4
                             $('#tabs a[href="/Create/Step3"]').trigger('click');


                         }



                     }


                 })


         }

         function ScriptEdit() {

                          
             //var instance = CKEDITOR.instances['Create'];
             //if (instance) {
             //    CKEDITOR.remove(instance); //if existed then remove it
             //}
             //CKEDITOR.replace('Create'); // Create instance for 'editor'

//             var instance = CKEDITOR.instances['EditScript'];
//             if (instance) {
//                 CKEDITOR.remove('EditScript');
//             }
//CKEDITOR.replace('EditScript');
             


             
             CKEDITOR.instances['EditScript'].setData($("#Existing").html());
             $("#Existing").addClass("inv");
             $("#editScript").addClass("inv");
             $("#saveScript").removeClass("inv");
             $("#cancel").removeClass("inv");
             $('#ScriptId').attr('disabled', 'disabled');
             $("#edit").removeClass("inv");
             $("#edit").addClass("vis");
         }


         function RemoveScript() {
             // get script
             var optionToRemove = $('#ScriptId option:selected')
             $.ajax(
             {
                 url: '@Url.Action("RemoveScript", "create")',
                     type: "POST",
                     data: "id= " + $('#ScriptId option:selected').val(),
                     success: function (data) {
                        //refresh scripts
                         $("#ScriptId > option:selected").each(function () {
                             $(this).remove();
                         });



                     }


                 })


         }

         function ScriptSave() {
             var htmlout = CKEDITOR.instances['EditScript'].getData();
             var encoded = encodeURIComponent(htmlout);

             $.ajax(
                               {
                                   url: '@Url.Action("EditScript", "create")',
                                   type: "POST",
                                   data: "editor=" + encoded + "&ScriptId=" + $('#ScriptId option:selected').val(),
                                   //data: "Current= " + selected + "&Selected= " + ui.index,
                                   success: function (data) {
                                       if (data == "True") {
                                           //show div again
                                           $("#Existing").html(CKEDITOR.instances['EditScript'].getData());
                                           $("#Existing").removeClass("inv");
                                           $("#editScript").removeClass("inv");
                                           $("#saveScript").addClass("inv");
                                           $("#cancel").addClass("inv");
                                           $('#ScriptId').removeAttr('disabled');
                                           $("#edit").addClass("inv");
                                           $("#edit").removeClass("vis");
                                           

                                       }

                                   }

                               });



         }

         function CancelEdit(){

             $("#Existing").removeClass("inv");
             $("#editScript").removeClass("inv");
             $("#saveScript").addClass("inv");
             $("#cancel").addClass("inv");
             $('#ScriptId').removeAttr('disabled');
             $("#edit").addClass("inv");
             $("#edit").removeClass("vis");



         }


        




         
         //$(".selectOneListBox").live(function () {
         //    $(this).on.removeAttr('multiple');
         //});
              </script> 

   
        
   <div class="vis" id="div1">
    @Html.Label("Script Name:")
      @Html.TextBox("ScriptName", "", New With {.maxlength = 25, .class = "inputs"}) 
      @Html.Label("Script:")
@Html.TextArea("Create")
</div> 

        
          <div class="inv" id="div2">
<table>
<tr>
<td style="width:19%; vertical-align:top;">@Html.ListBox("ScriptId", TryCast(ViewBag.selectList, SelectList), New With {.class = "selectOneListBox", .Size = 10, .style = "width: 100%"})</td>
<td style="width:80%;">
@*@Html.TextArea("EditScript", New With {.class = "ckeditor", .style= "display: none;visibility: hidden;"})*@
<div id="edit">
@Html.TextArea("EditScript", New With {.style = "display: none;visibility: hidden;"})
 </div>
 
 <div id="Existing" style="border-width: 1px; border-style: solid; border-color: black; width: 100%; float:right; height: 200px; overflow-y: scroll;"></div>

 </td>
</tr>
<tr>
<td><input id="clickMe" type="button" value="Remove Script" onclick="RemoveScript()" /></td>
<td><input id="editScript" type="button" value="Edit Script" onclick="ScriptEdit()" /> <input id="saveScript" type="button" class="inv" value="Save Script" onclick="ScriptSave()" /> <input id="cancel" type="button" class="inv" value="Cancel" onclick="CancelEdit()" />  </td>

</tr>

</table>
 </div>

  

<div class="inv" id="div3">
@Html.ListBox("TemplateId", TryCast(ViewBag.selectTemplateList, SelectList), New With {.class = "selectOneListBox", .Size = 10, .style = "width: 19%"})
 <div id="Template"  style="border-width: 1px; border-style: solid; border-color: black; width: 80%; float:right; height: 200px; overflow-y: scroll;"></div><br />
 <button class="btnUse">
    Use Template
</button>
</div>
<br />
<div class="preload" id="div4" style="width: 100%;">

 <div>
    <div id="mycarousel" class="jcarousel-skin-tango">
    <ul>
    
    
    
    @For Each item As Object In ViewBag.ExistingVideos
            If ViewBag.ExistingVideo = item Then
                 @:<li><center><a href="#" onclick="SelectVideo(this.id);" id="@(item)" >SELECTED</a></center><a href="/Streams/@(item).flv" class="myPlayer" style="display:block;width:220px;height:180px;margin:10px auto"></a></li>
            Else
                 @:<li><center><a href="#" onclick="SelectVideo(this.id);" id="@(item)" >SELECT</a></center><a href="/Streams/@(item).flv" class="myPlayer" style="display:block;width:220px;height:180px;margin:10px auto"></a></li>
            End If
            
         
    Next

    
      
        
    </ul>
</div>
  
    
</div> 
   
</div>
    



     </fieldset>       
    End Using





<script type="text/javascript">
    $(function () {
        

        $(".showNew").click(function () {
            $("#div1").addClass("vis");
            $("#div1").removeClass("inv");

            $("#div2").addClass("inv");
            $("#div2").removeClass("vis");

            $("#div3").addClass("inv");
            $("#div3").removeClass("vis");

            $("#div4").addClass("inv");
            $("#div4").removeClass("vis");

              
            return false;
        });
        $(".showExisting").click(function () {
            $("#div1").addClass("inv");
            $("#div1").removeClass("vis");

            $("#div2").addClass("vis");
            $("#div2").removeClass("inv");

            $("#div3").addClass("inv");
            $("#div3").removeClass("vis");

            $("#div4").addClass("inv");
            $("#div4").removeClass("vis");
            $("#edit").addClass("inv");
            $("#edit").removeClass("vis");
           
            return false;
        });
        $(".showTemplate").click(function () {
            $("#div1").addClass("inv");
            $("#div1").removeClass("vis");

            $("#div2").addClass("inv");
            $("#div2").removeClass("vis");

            $("#div3").addClass("vis");
            $("#div3").removeClass("inv");

            $("#div4").addClass("inv");
            $("#div4").removeClass("vis");

            $("#TemplateId").val($("#TemplateId option:first").val());
            $('#TemplateId').change();
            return false;
        });

        $(".showVideos").click(function () {
            $("#div1").addClass("inv");
            $("#div1").removeClass("vis");

            $("#div2").addClass("inv");
            $("#div2").removeClass("vis");

            $("#div3").addClass("inv");
            $("#div3").removeClass("vis");

            $("#div4").addClass("vis");
            $("#div4").removeClass("inv");

            return false;
        });

        $(".btnUse").click(function () {
            $("#div1").addClass("vis");
            $("#div1").removeClass("inv");

            $("#div2").addClass("inv");
            $("#div2").removeClass("vis");

            $("#div3").addClass("inv");
            $("#div3").removeClass("vis");

            // $("#ckeditor").html($("#Template").html());
            CKEDITOR.instances['Create'].setData($("#Template").html())


            return false;
        });



    });

    
</script>

<script type="text/javascript" src="@Url.Content("~/Content/javascript/ckeditor/ckeditor.js")"></script>
<script>
    CKEDITOR.replace('Create');
    CKEDITOR.replace('EditScript');
      </script>

@*<link rel="Stylesheet" type="text/css" href="@Url.Content("~/Content/jHtmlArea/jHtmlArea.css")" />
<script type="text/javascript" src="~/Scripts/jHtmlArea-0.7.5.js"></script>

<script type="text/javascript">
    $(function(){
        $('#editor').htmlarea();
    });
</script>*@

@*<script type="text/javascript">
    $(document).ready(function () {
        $(this).parents('form').submit(function () {
            for (instance in CKEDITOR.instances) {
                CKEDITOR.instances[instance].updateElement();
            }
        });
    });
</script>*@
