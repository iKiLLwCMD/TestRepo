<h2>Add attachments</h2><br />

            <form id="uploaderAuto">
        <input id="fileInputAuto" type="file" multiple>
        <input type="submit" value="Upload CV" />
    </form>

   
          @Html.Label("", ViewBag.Auto.ToString, New With {.id = "AutoView", .style = "color: green"})

@If ViewBag.Auto.ToString <> "" Then
    
   @:<a href="Attachments/@ViewBag.AutoName.ToString" style="color:green;" target="_blank">Preview</a> 
End If

    
   
  
    <label id="Auto" name="Auto"  />
    

<br />
              <form id="uploaderOther1">
        <input id="fileInputOther1" type="file" multiple>
        <input type="submit" value="Upload Other 1" />
    </form> 
    
   
    @Html.Label("", ViewBag.Other1.ToString, New With {.id = "Other1View", .style = "color: green"})

    @If ViewBag.Other1.ToString <> "" Then
    
   @:<a href="Attachments/@ViewBag.Other1Name.ToString" style="color:green;" target="_blank">Preview</a> 
End If
     <label id="Other1" name="Other1"  />
   
<br />
              <form id="uploaderOther2">
        <input id="fileInputOther2" type="file" multiple>
        <input
         type="submit" value="Upload Other 2" />
    </form>
    @Html.Label("", ViewBag.Other2.ToString, New With {.id = "Other2View", .style = "color: green"})

    @If ViewBag.Other2.ToString <> "" Then
    
   @:<a href="Attachments/@ViewBag.Other2Name.ToString" style="color:green;" target="_blank">Preview</a> 
End If
    <label id="Other2" name="Other2"  />
    
<br />

<script>
       
        document.getElementById('uploaderAuto').onsubmit = function () {
            var fileInput = document.getElementById('fileInputAuto');
            var xhr = new XMLHttpRequest();
            xhr.open('POST', 'Create/Upload');
            xhr.setRequestHeader('Content-type', 'multipart/form-data');
    //Appending file information in Http headers
            xhr.setRequestHeader('X-File-Name', fileInput.files[0].name);
            xhr.setRequestHeader('X-File-Type', fileInput.files[0].type);
            xhr.setRequestHeader('X-File-Size', fileInput.files[0].size);
            xhr.setRequestHeader('X-Type', 'Auto');
    //Sending file in XMLHttpRequest
            xhr.send(fileInput.files[0]);
            xhr.onreadystatechange = function () {
                if (xhr.readyState == 4 && xhr.status == 200) {
                    $('#Auto').text(xhr.responseText.replace(/\"/g, ""));
                    document.getElementById("Auto").style.color = "green";
                    document.getElementById("AutoView").style.display = 'none';
                }
                else {
                    $('#Auto').text("File upload failed");
                    document.getElementById("Auto").style.color = "red";
                }
            }
            return false;
       
        }

        document.getElementById('uploaderOther1').onsubmit = function () {
            var fileInput = document.getElementById('fileInputOther1');
            var xhr = new XMLHttpRequest();
            xhr.open('POST', 'Create/Upload');
            xhr.setRequestHeader('Content-type', 'multipart/form-data');
            //Appending file information in Http headers
            xhr.setRequestHeader('X-File-Name', fileInput.files[0].name);
            xhr.setRequestHeader('X-File-Type', fileInput.files[0].type);
            xhr.setRequestHeader('X-File-Size', fileInput.files[0].size);
            xhr.setRequestHeader('X-Type', 'Other1');
            //Sending file in XMLHttpRequest
            xhr.send(fileInput.files[0]);
            xhr.onreadystatechange = function () {
                if (xhr.readyState == 4 && xhr.status == 200) {
                    $('#Other1').text(xhr.responseText.replace(/\"/g, ""));
                    document.getElementById("Other1").style.color = "green";
                    document.getElementById("Other1View").style.display = 'none';
                }
                else {
                    $('#Other1').text("File upload failed");
                    document.getElementById("Other1").style.color = "red";
                    document.getElementById("Other2View").style.display = 'none';

                }
            }
            return false;

        }

        document.getElementById('uploaderOther2').onsubmit = function () {
            var fileInput = document.getElementById('fileInputOther2');
            var xhr = new XMLHttpRequest();
            xhr.open('POST', 'Create/Upload');
            xhr.setRequestHeader('Content-type', 'multipart/form-data');
            //Appending file information in Http headers
            xhr.setRequestHeader('X-File-Name', fileInput.files[0].name);
            xhr.setRequestHeader('X-File-Type', fileInput.files[0].type);
            xhr.setRequestHeader('X-File-Size', fileInput.files[0].size);
            xhr.setRequestHeader('X-Type', 'Other2');
            //Sending file in XMLHttpRequest
            xhr.send(fileInput.files[0]);
            xhr.onreadystatechange = function () {
                if (xhr.readyState == 4 && xhr.status == 200) {
                    $('#Other2').text(xhr.responseText.replace(/\"/g, ""));
                    document.getElementById("Other2").style.color = "green";

                }
                else {
                    $('#Other2').text("File upload failed");
                    document.getElementById("Other2").style.color = "red";
                }
            }
            return false;

        }
</script>