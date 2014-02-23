<!doctype html>
<html lang="en">
<head>
     <title>Video CV | New Zealand</title>

 <META NAME="Description" Content="Use the MyVcv Video CV template to create a Video CV that is easy to create with professional results and will help you stand out from the competition.">
 <META NAME="Keywords" CONTENT="Video CV, video cvs, video curriculum vitae, modern cv, easy cv, North Island, South Island, New Zealand, NZ">
    
<script src="http://code.jquery.com/jquery-1.9.1.js"></script>
<script src="http://code.jquery.com/ui/1.9.2/jquery-ui.js"></script>
        <link href="~/Content/jquery.css" rel="stylesheet" />

@*<link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css" rel="stylesheet" type="text/css"/>*@
        <script src="~/Scripts/jquery-validate.js"></script>
        <script src="~/Scripts/jquery-validate.min.js"></script>
        <script src="http://releases.flowplayer.org/js/flowplayer-3.2.12.min.js"></script>
         <script>
             var requestcomplete = false



             $(function () {





                 $("#tabs").tabs({

                     load: function (event, ui) {
                         //alert('in load function');
                         var $tabs = $('#tabs').tabs();
                         var selected = $tabs.tabs('option', 'selected');
                         
                         //if (selected == 1) {
                         //    var instance = CKEDITOR.instances['Create'];
                         //    if (instance) {
                         //        CKEDITOR.remove(instance); //if existed then remove it
                         //    }
                         //    CKEDITOR.replace('Create'); // Create instance for 'editor'

                         //}

                         //if (selected == 3) {

                         //    var instanceSummary = CKEDITOR.instances['CreateSummary'];
                         //    if (instanceSummary) {
                         //        CKEDITOR.remove(instanceSummary); //if existed then remove it
                         //    }
                         //    CKEDITOR.replace('CreateSummary'); // Create instance for 'editor' 
                         //}


                     },
                     select: function (event, ui) {
                         //alert('validating tab ');



                         var valid = true;


                         var $tabs = $('#tabs').tabs();
                         var selected = $tabs.tabs('option', 'selected');
                         if (selected == 4) {
                             if ($("#Auto").text() == "" && $("#AutoView").text() == "") {
                                 alert("Please upload a CV");
                                 valid = false;
                             }
                         }
                         // do your validating here...
                         // determine validity

                         if ($("#div1").hasClass("vis") == true) {
                             if (selected == 1) {
                                 //check textbox has text
                                 var scriptname = $('#ScriptName').val();
                                 if (scriptname == "") {
                                     alert("Please give your script a name");
                                     valid = false;
                                 }

                             }

                         }



                         if (!valid) {

                             event.preventDefault();
                         }

                         else {

                             //alert('valid');

                             if (requestcomplete == true) {
                                 requestcomplete = false
                             }

                             else {
                                 event.preventDefault();








                                 //var url = ui.tab.href
                                 //var url2 = url.replace("http://localhost:56176/", "");



                                 if (selected == 1) {
                                     var instance = CKEDITOR.instances['Create'];

                                     if (instance) {
                                         var htmlout = CKEDITOR.instances['Create'].getData();
                                         var encoded = encodeURIComponent(htmlout);
                                     }
                                     else {
                                         var htmlout = "";
                                     }
                                 }

                                 if (selected == 3) {
                                     var instance = CKEDITOR.instances['CreateSummary'];

                                     if (instance) {
                                         var htmlout = CKEDITOR.instances['CreateSummary'].getData();
                                         var encoded = encodeURIComponent(htmlout);
                                     }
                                     else {
                                         var htmlout = "";
                                     }
                                 }

                                 // see which tab if selected

                                 if ($("#div1").hasClass("vis") == true) {
                                     var scriptType = "New"


                                 }

                                 if ($("#div2").hasClass("vis") == true) {
                                     var scriptType = "Existing"

                                 }

                                 if ($("#div3").hasClass("vis") == true) {
                                     var scriptType = "Template"

                                 }

                                 if ($("#div4").hasClass("vis") == true) {
                                     var scriptType = "ExistingVideo"

                                 }

                                 if ($("#divNew").hasClass("vis") == true) {
                                     var SummaryType = "New"


                                 }

                                 if ($("#divExisting").hasClass("vis") == true) {
                                     var SummaryType = "Existing"

                                 }




                                 //var htmlencode = jQuery('<div />').text(htmlout).html();

                                 // do server side validation
                                 $.ajax(
                                 {
                                     url: '@Url.Action("tabval", "create")',
                                     type: "POST",
                                     data: "Current= " + selected + "&Selected= " + ui.index + "&Col= " + $('form').serialize() + "&editor= " + encoded + "&Scripttype=" + scriptType + "&ScriptId= " + $('#ScriptId option:selected').val() + "&SummaryId= " + $('#ScriptId option:selected').val() + "&SummaryType=" + SummaryType,
                                     //data: "Current= " + selected + "&Selected= " + ui.index,
                                     success: function (data) {
                                         if (data == "True") {
                                             //alert('success')
                                             requestcomplete = true
                                             //$('#tabs a[href="#tabs-2"]').trigger('click');
                                             
                                             
                                             
                                             
                                             
                                             //if (ui.index == 0) {
                                             //    $('#tabs a[href="#tabs-1"]').trigger('click');
                                             //}
                                             //else if(ui.index == 1)
                                             //{
                                             //    $('#tabs a[href="#tabs-2"]').trigger('click');
                                             //}
                                             //else if (ui.index == 2)
                                             //{
                                             //    $('#tabs a[href="#tabs-3"]').trigger('click');
                                             //}

                                             if (ui.index == 0) {
                                                 $('#tabs a[href="#tabs-1"]').trigger('click');
                                             }
                                             else if (ui.index == 1) {
                                                 $('#tabs a[href="/Create/Step2"]').trigger('click');
                                             }
                                             else if (ui.index == 2) {
                                                 $('#tabs a[href="/Create/Step3"]').trigger('click');
                                             }
                                             else if (ui.index == 3) {
                                                 $('#tabs a[href="/Create/Step4"]').trigger('click');
                                             }
                                             else if (ui.index == 4) {
                                                 
                                                
                                                 
                                             }
                                             else if (ui.index == 5) {
                                                 $('#tabs a[href="/Create/Step6"]').trigger('click');
                                             }



                             @*            $.ajax(
                                {
                                    url: '@Url.Action("tabChange", "create")',
                                    type: "POST",
                                    data: "Current= " + selected + "&Selected= " + ui.index,
                                    success: function (data) {}


                                })*@






                                         //$(ui.item).trigger('click');

                                     }
                                     else {
                                         // alert('Error');



                                     }


                                 }


                                 })

                         }






                     }




                     }
                 })

                 //look up currentstep in hidden field

                 var currentstep = parseInt($('#Step').val());
                 $('#tabs').tabs('select', currentstep);

             });
</script>




    <link href='http://fonts.googleapis.com/css?family=Oxygen' rel='stylesheet' type='text/css'>
       <link href="/Content/site.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" href="favicon.ico" />
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 80%;
        }
        
        .style3
        {
            width: 296px;
        }
        
        .style4
        {
            width: 35%;
        }
        
    </style>
</head>
<body>
    
<div>

</div>

<div class="top">
    <div class="bottom">
        <div class="MainBody">
        			<div id="header">
                    
                    <div class="fright"><section id="login">
                    <div  style="background-color: #F99C25; float:right; height: 19px;"> 
                        @Html.Partial("_LoginPartial")
                    </div>
                    </section>  @*<img src="images/phone.jpg" alt="" align="right" />*@<div style="padding-top: 80px;">   
                                   
                    <a href="@Url.Action("Index", "Home")" class="nav" id="a_home">Home</a>&nbsp;&nbsp;
                    <a href="@Url.Action("Index", "Listing")" class="nav">Job Listings</a>&nbsp;&nbsp;
                    <a href="@Url.Action("online_cv", "Home")" class="nav">Employees</a>&nbsp;&nbsp;
                    <a href="@Url.Action("online_cv", "Home")" class="nav">Employers</a>&nbsp;&nbsp;
                    <a href="@Url.Action("recruitment_software", "Home")" class="nav" id="a_Recruitment">Recruitment</a>&nbsp;&nbsp;
                    <a href="@Url.Action("video_resume", "Home")" class="nav">Contact Us</a>&nbsp;&nbsp;
                    <a href="@Url.Action("faqs_site_info", "Home")" class="nav">FAQ'S</a></div></div>
				<div class="title">
					<div class="fleft">
                        

                        <a href="@Url.Action("Index", "Home")"><img src="@Url.Content("~/Images/logo.gif")" alt="MyChatPack" border="0"  style="height: 76px; width: 216px" /></a>
                       
				    </div>
				</div>
                <br />
                <br />
                <div class="box"> 
                    <table class="style1">
                        <tr>
                            <td valign="top" style="padding: 0px;">
                            <h1 class="header">GET THAT JOB INTERVIEW with a video CV from MyVcv</h1>
                            </td>
                            <td><a id="HyperLink3" class="HeaderLink" href="Cart.aspx" style="display:inline-block;border-style:None;"><img src="../../images/buy_header.jpg" alt="Buy Now" style="border-width:0px;" /></a></td>
                        </tr>
                    </table>
                </div>


							
			</div>
       <br /><br />
            
            <div id="content">

                <div class="box2" height:350px;">
                
                
                 @RenderSection("featured", required:=false)
          
           <section class="content-wrapper main-content clear-fix">
           @If HttpContext.Current.Request.RequestContext.RouteData.Values("controller").ToString() = "Manage" Then
               @:<p>@Html.Partial("_ManagePartial")</p><br /><br />
           End If
                  
                @RenderBody()
            </section>
                 
                    
                @Scripts.Render("~/bundles/jquery")
        @RenderSection("scripts", required:=False)
                
                
                 
                    
                </div>
               

          </div>
          
                
</div>
         </div>
        </div>
        <div id="bottom" style="background-color: #DADADA" >&nbsp;<br />
            <br />
            <br />
            <div id="footer" >
                <table class="style1">
                    <tr>
                        <td valign="top" width="25%">

                    <a href="@Url.Action("Index", "Home")">Home</a><br /><br />
                    <a href="@Url.Action("online_cv", "Home")" >Employees</a><br /><br />
                    <a href="@Url.Action("online_cv", "Home")" >Employers</a><br /><br />
                    <a href="@Url.Action("recruitment_software", "Home")">Recruitment</a><br /><br />
                    <a href="@Url.Action("video_resume", "Home")">Contact Us</a><br /><br />
                    <a href="@Url.Action("faqs_site_info", "Home")">FAQ'S</a><br /><br />
                            
                            
                            </td>
                        <td valign="top" width="25%">
                         @*<a href="recruitment-software.aspx">Recruitment</a><br />                          
                            <br />
                              <a href="education.aspx">Education</a><br /><br />
                            <a href="Tips.aspx">Interview Hints</a><br />
                            <br />
                            <a href="Terms.aspx">Terms &amp; Conditions</a> </td>*@
                        <td valign="top" class="style4">
                            Call <strong>0800 MyVcv123</strong> (0800 698 28 123)
                            <br />
                            <br />
                            MyVCV © Copyright 2013
                        </td>
                        <td align="right" valign="top" width="25%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <a id="HyperLink4" href="https://www.facebook.com/MyVCV" target="_blank" style="display:inline-block;background-color:#DADADA;border-style:None;"><img src="../../images/Facebook.jpg" alt="Facebook" style="border-width:0px;" /></a>
                        </td>
                    </tr>
                </table>
        <br />
        
        </div>
   </div>

  <script type="text/javascript">

      var _gaq = _gaq || [];
      _gaq.push(['_setAccount', 'UA-6868224-44']);
      _gaq.push(['_trackPageview']);

      (function () {
          var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
          ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
          var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
      })();

</script>
    
<script language="javascript">
    document.getElementById("a_home").className = "navhighlited ";
    </script>
</body>
</html>






       
     