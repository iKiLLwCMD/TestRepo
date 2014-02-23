<!DOCTYPE html>
<html lang="en">
    <head>
        <meta charset="utf-8" />
        <title>@ViewData("Title") - My ASP.NET MVC Application</title>
        <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
        <meta name="viewport" content="width=device-width" />
        @Styles.Render("~/Content/themes/base/css", "~/Content/css")
        @Scripts.Render("~/bundles/modernizr")

<link rel="stylesheet" href="http://code.jquery.com/ui/1.10.2/themes/smoothness/jquery-ui.css" />
<script src="http://code.jquery.com/jquery-1.9.1.js"></script>
<script src="http://code.jquery.com/ui/1.9.2/jquery-ui.js"></script>
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

                         if (selected == 1) {
                             var instance = CKEDITOR.instances['Create'];

                             if (instance) {
                                 CKEDITOR.remove(instance); //if existed then remove it
                             }
                             CKEDITOR.replace('Create'); // Create instance for 'editor'

                         }

                         if (selected == 3) {

                             var instanceSummary = CKEDITOR.instances['CreateSummary'];
                             if (instanceSummary) {
                                 CKEDITOR.remove(instanceSummary); //if existed then remove it
                             }
                             CKEDITOR.replace('CreateSummary'); // Create instance for 'editor' 
                         }
                          
                        
                     },
                     select: function (event, ui) {
                         //alert('validating tab ');

                         
                                           
                         var valid = true;

                        
                         var $tabs = $('#tabs').tabs();
                         var selected = $tabs.tabs('option', 'selected');
                         
                         // do your validating here...
                         // determine validity

                         if ($("#div1").hasClass("vis") == true) {
                             if (selected == 1) {
                                 //check textbox has text
                                 var scriptname = $('#ScriptName').val();
                                 if (scriptname == "") {
                                     alert("Please give your script a name");
                                     var valid = false;
                                 }

                             }

                         }
                                              

                        
                         if (!valid) {
                            
                             event.preventDefault();
                         }
                     
                         else 
                     
                         {
                             
                             //alert('valid');

                             if (requestcomplete == true) {
                                 requestcomplete = false
                             }

                             else {
                                 event.preventDefault();

                                                              


                                 
                                 


                                  //var url = ui.tab.href
                                  //var url2 = url.replace("http://localhost:56176/", "");

                                

                                 if (selected == 1)
                                 {
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

                                 if ($("#div1").hasClass("vis") == true)

                                 {
                                     var scriptType = "New"
                                     

                                 }

                                 if ($("#div2").hasClass("vis") == true) {
                                     var scriptType = "Existing"

                                 }

                                 if ($("#div3").hasClass("vis") == true) {
                                     var scriptType = "Template"

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
                                             $('#tabs a[href="/Create/Step5"]').trigger('click');
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



@*<script>
    $(function () {
        $("#tabs").tabs({
            select: function (event, ui) {
                alert('validating tab ' + ui.index);

                var valid = false;

                // do your validating here...
                // determine validity

                if (!valid) {
                    alert('not valid');
                    event.preventDefault();
                }
                else {
                    alert('valid');

                      if (res == true)
                           {

                           // do server side validation
                               $.ajax(
                               {
                                   url: "http://localhost:56176/HomeController/TabVal/" ,
                                   type: "post",
                                   data: "&materialKey=" + $("#MaterialKey option:selected").val(),
                                   success: function (response)
                                   {







                }
            }
        });
    //});
    
</script>*@
    </head>
    <body>
 <div id="fb-root"></div>
@*<script>
    window.fbAsyncInit = function () {
        FB.init({
            appId: '359520694171070', // App ID
            status: true, // check login status
            cookie: true, // enable cookies to allow the server to access the session
            xfbml: true  // parse XFBML
        });

        // Additional initialization code here
        FB.Event.subscribe('auth.authResponseChange', function (response) {
            if (response.status === 'connected') {
                // the user is logged in and has authenticated your
                // app, and response.authResponse supplies
                // the user's ID, a valid access token, a signed
                // request, and the time the access token 
                // and signed request each expire
                var uid = response.authResponse.userID;
                var accessToken = response.authResponse.accessToken;

                // TODO: Handle the access token

            } else if (response.status === 'not_authorized') {
                // the user is logged in to Facebook, 
                // but has not authenticated your app
            } else {
                // the user isn't logged in to Facebook.
            }
        });

    };

    // Load the SDK Asynchronously
    (function (d) {
        var js, id = 'facebook-jssdk', ref = d.getElementsByTagName('script')[0];
        if (d.getElementById(id)) { return; }
        js = d.createElement('script'); js.id = id; js.async = true;
        js.src = "//connect.facebook.net/en_US/all.js";
        ref.parentNode.insertBefore(js, ref);
    }(document));
</script>*@
        <header>
            <div class="content-wrapper">
                <div class="float-left">
                    <p class="site-title">@Html.ActionLink("Your logo here", "Index", "Home")</p>
                </div>
                <div class="float-right">
                    <section id="login">
                        @Html.Partial("_LoginPartial")
                    </section>
                    <nav>
                        <ul id="menu">
                            <li>@Html.ActionLink("Home", "Index", "Home")</li>
                            <li>@Html.ActionLink("About", "About", "Home")</li>
                            <li>@Html.ActionLink("Contact", "Contact", "Home")</li>
                        </ul>
                    </nav>
                </div>
            </div>
        </header>
        <div id="body">
            @RenderSection("featured", required:=false)
            <section class="content-wrapper main-content clear-fix">
                @RenderBody()
            </section>
        </div>
        <footer>
            <div class="content-wrapper">
                <div class="float-left">
                    <p>&copy; @DateTime.Now.Year - My ASP.NET MVC Application</p>
                </div>
                <div class="float-right">
                    <ul id="social">
                        <li><a href="http://facebook.com" class="facebook">Facebook</a></li>
                        <li><a href="http://twitter.com" class="twitter">Twitter</a></li>
                    </ul>
                </div>
            </div>
        </footer>



        @Scripts.Render("~/bundles/jquery")
        @RenderSection("scripts", required:=False)


    </body>
</html>
