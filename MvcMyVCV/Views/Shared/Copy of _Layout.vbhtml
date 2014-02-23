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

         <script>
             var requestcomplete = false

             $(function () {
                 $("#tabs").tabs({
                     select: function (event, ui) {
                         alert('validating tab ');

                         
                                           
                         var valid = true;

                        

                         
                         // do your validating here...
                         // determine validity
                        

                         if (!valid) {
                             alert('not valid');
                             event.preventDefault();
                         }
                     
                         else 
                     
                         {
                             
                             alert('valid');

                             if (requestcomplete == true) {
                                 requestcomplete = false
                             }

                             else {
                                 event.preventDefault();
                                 


                                  //var url = ui.tab.href
                                  //var url2 = url.replace("http://localhost:56176/", "");

                                 var $tabs = $('#tabs').tabs();
                                 var selected = $tabs.tabs('option', 'selected');


                                 // do server side validation
                                 $.ajax(
                                 {
                                     url: '@Url.Action("tabval", "create")',
                                     type: "POST",
                                     data: "Current= " + selected + "&Selected= " + ui.index + "&Col= " + $('form').serialize(),
                                 //data: "Current= " + selected + "&Selected= " + ui.index,
                                 success: function (data) {
                                     if (data == "True") {
                                         alert('success')
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
                                         alert('Error');



                                     }


                                 }


                             })

                             }
                        
                            




                         }




                     }
                 })
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
