

<h2>Step 3</h2>
@If ViewBag.ExistingVideo = False Then
   

    @:<div style="display:inline-block; width: 700px">
    @:<div id="ScriptScroll" style="overflow: scroll;height:200px;overflow-x:hidden;">
    @Html.Raw(Session("ScriptText"))
    @:</div>
    @:</div>

    @:<div style="display:inline-block; width: 60px; vertical-align: top; text-align: center; ">

    @:<label for="speed" style="color:#53AF45;">Speed</label>
    @:<div id="slider-speed" style="height: 160px; margin-left:auto; margin-right:auto;"></div>
    @:<input type="text" id="speed" style="border: 0; color: #f6931f; font-weight: bold; width: 10px;" />
    @:</div>

    @:<div style="display:inline-block; width: 70px; vertical-align: top; text-align: center;">

    @:<label for="size" style="color:#53AF45;">Font Size</label>
    @:<div id="slider-size" style="height: 160px; margin-left:auto; margin-right:auto;"></div>
    @:<input type="text" id="size" style="border: 0; color: #f6931f; font-weight: bold; width: 10px;" />
    @:</div>
End If

<center>

<div style="height: 800px;overflow-x:visible; width:90%">
  	<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000"
			id="CodeGenSample" width="100%" height="100%"
			codebase="http://fpdownload.macromedia.com/get/flashplayer/current/swflash.cab">

@If ViewBag.ExistingVideo = True Then
    @:<param name="movie" value="http://localhost:4494/MyChatPakSnap.swf" />
Else
    @:<param name="movie" value="http://localhost:4494/CodeGenSample.swf" />
End If
			
			<param name="quality" value="high" />
			<param name="bgcolor" value="#FFFFFF" />
            @*<param name="ID" value="@HttpContext.Current.Session("NewVCVId").ToString()" />*@
            <param name="ID" value="@HttpContext.Current.Session("NewVCVName").ToString()" />
            <param name="Access" value="@(ViewBag.Access)" />
            <param name="pic" value="@(ViewBag.Name)" />
            <param name="video" value="@(ViewBag.VideoName)" />
            <param name="FlashVars" value="ID=@HttpContext.Current.Session("NewVCVName").ToString()&Access=@(ViewBag.Access)&pic=@(ViewBag.Name)&video=@(ViewBag.VideoName)&Access=@(ViewBag.Access)" />
 			<param name="allowScriptAccess" value="sameDomain" />
@If ViewBag.ExistingVideo = True Then
    @:<embed src="http://localhost:4494/MyChatPakSnap.swf?ID=@HttpContext.Current.Session("NewVCVName").ToString()&pic=@(ViewBag.Name)&video=@(ViewBag.VideoName)&Access=@(ViewBag.Access)" quality="high" bgcolor="#FFFFFF"
           		@:width="100%" height="100%" name="CodeGenSample" align="middle"
				@:play="true"
				@:loop="false"
				@:quality="high"
				@:allowScriptAccess="sameDomain"
				@:type="application/x-shockwave-flash"
				@:pluginspage="http://www.adobe.com/go/getflashplayer" style="height:100%; width:100%">
			@:</embed>
    
    else
    
    @:<embed src="http://localhost:4494/CodeGenSample.swf?ID=@HttpContext.Current.Session("NewVCVName").ToString()&pic=@(ViewBag.Name)&video=@(ViewBag.VideoName)&Access=@(ViewBag.Access)" quality="high" bgcolor="#FFFFFF"
				@:width="100%" height="100%" name="CodeGenSample" align="middle"
				@:play="true"
				@:loop="false"
				@:quality="high"
				@:allowScriptAccess="sameDomain"
				@:type="application/x-shockwave-flash"
				@:pluginspage="http://www.adobe.com/go/getflashplayer" style="height:100%; width:100%">
			@:</embed>
    
End If


			
	</object>
</div>
</center>
   
<script type="text/javascript">



  
        var myTimer = null;
        function ScrollScript() {
          
            var divScroll = $("#ScriptScroll");
            myTimer = setInterval(function () {
                var pos = divScroll.scrollTop();
                divScroll.scrollTop(pos + $("#slider-speed").slider("value"));
            }, 500);
        }

        function ScrollScriptStop() {

            var divScroll = $('#ScriptScroll');
            divScroll.scrollTop(0);
        
           clearInterval(myTimer);
        }

        function ChangeTab() {

            $.ajax(
             {
                 url: '@Url.Action("VideoComplete", "create")',
                     type: "POST",
                     success: function () {
                        //change tab

                       $('#tabs a[href="/Create/Step4"]').trigger('click');



                     }


                 })

            
        }

    function getLocation() {
        return window.location;
    }

    $(function () {
        $("#slider-speed").slider({
            orientation: "vertical",
            range: "min",
            min: 1,
            max: 10,
            value: 5,
            slide: function (event, ui) {
                $("#speed").val(ui.value);
            }
        });
        $("#speed").val($("#slider-speed").slider("value"));

        $("#slider-size").slider({
            orientation: "vertical",
            range: "min",
            min: 1,
            max: 10,
            value: 5,
            slide: function (event, ui) {
                var fontsize = 7 + $("#slider-size").slider("value");
                $("#size").val(ui.value);
                $('#ScriptScroll').css('font-size', fontsize + 'pt');

                
            }
        });
        $("#size").val($("#slider-size").slider("value"));
    });
    
</script>

