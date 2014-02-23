@ModelType IEnumerable(Of MvcMyVCV.User)

@Code
    ViewData("Title") = "Manage Classes"
End Code

<br />

@Using Html.BeginForm()
   
    @<fieldset>
    <legend>Choose Class</legend>
            @*@Html.DropDownList("ClassId", TryCast(ViewBag.selectList, SelectList))*@
         @Html.DropDownList("ClassId", TryCast(ViewBag.selectList, SelectList), New With {.onchange = "this.form.submit();"})
           
    
     @*<p>
            <input type="submit" value="Create" />
        </p>*@
     </fieldset>       
End Using

    @If User.IsInRole("Teacher") = True Then
        @Html.ActionLink("Create New Class", "CreateClass")
@Html.ActionLink("Create New Student", "CreateStudent")
@Html.ActionLink("Export Class List", "ExportClass", "Manage", "", New With {.id = "ExportClass"})
    End If
     
     <br />
    
   

<table>
    <tr>
        <th>
            @Html.DisplayNameFor(Function(model) model.UserId)
        </th>

        <th>
            @Html.DisplayNameFor(Function(model) model.UserName)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.UserPassword)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.Class.ClassName)
        </th>
        <th></th>
    </tr>

@For Each item In Model
    Dim currentItem = item
    @<tr>

        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.UserId)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.UserName)
        </td>
        <td>
        @If currentItem.Activated <> True Then
                @Html.DisplayFor(Function(modelItem) currentItem.UserPassword)
        Else
             @Html.ActionLink("Rest Password", "Reset", New With {.id = currentItem.UserId})
        End If
            
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) currentItem.Class.ClassName)
        </td>
        <td>
            @*@Html.ActionLink("Edit", "Edit", New With {.id = currentItem.UserId}) |
            @Html.ActionLink("Details", "Details", New With {.id = currentItem.UserId}) |*@
            @Html.ActionLink("Delete", "Delete", New With {.id = currentItem.UserId})
        </td>
    </tr>
Next
    
   
  
</table>

<br />
    <form id="uploadList">
        <input id="List" type="file">
        <input type="submit" value="Upload Class List" />
    </form> 
<br />
<a href ="../Content/ClassTemplate.csv">Download Class List Template</a><br /><br />
    <script>

        document.getElementById('uploadList').onsubmit = function () {
            var fileInput = document.getElementById('List');
            var xhr = new XMLHttpRequest();
            xhr.open('POST', 'Upload');
            xhr.setRequestHeader('Content-type', 'multipart/form-data');
            //Appending file information in Http headers
            xhr.setRequestHeader('X-File-Name', fileInput.files[0].name);
            xhr.setRequestHeader('X-File-Type', fileInput.files[0].type);
            xhr.setRequestHeader('X-File-Size', fileInput.files[0].size);
            xhr.setRequestHeader('X-ClassId', $('#ClassId option:selected').val() );
            //Sending file in XMLHttpRequest
            xhr.send(fileInput.files[0]);
            xhr.onreadystatechange = function () {
                if (xhr.readyState == 4 && xhr.status == 200) {                  
                    alert(xhr.responseText.replace(/\"/g, ""));
                    //document.getElementById("Auto").style.color = "green";
                    //document.getElementById("AutoView").style.display = 'none';
                }
                else {
                    //$('#Auto').text("File upload failed");
                    //alert('Upload failed');
                    //document.getElementById("Auto").style.color = "red";
                }
            }
            return false;

        }

 
        $('#ExportClass').click(function () {
        var classid= $('#ClassId').val(); 
      
        var path = '@Url.Content("~/Manage/ExportClass")' + "?classid=" + classid
        $(this).attr("href", path);
    });
 


</script>