var vcvid
var googleaccessToken
var fileid
var filename
var summary



    //$("input[name='PostType']").change(function (e) {
    //    if ($(this).val() == 'New') {
    //        $("#name").addClass("vis");
    //        $("#name").removeClass("inv");
    //        $("#txtname").addClass("vis");
    //        $("#txtname").removeClass("inv");

    //        $("#Existing").addClass("inv");
    //        $("#Existing").removeClass("vis");
    //        $("#ddlPostId").addClass("inv");
    //        $("#ddlPostId").removeClass("vis");

    //    } else {
    //        $("#Existing").addClass("vis");
    //        $("#Existing").removeClass("inv");
    //        $("#ddlPostId").addClass("vis");
    //        $("#ddlPostId").removeClass("inv");

    //        $("#name").addClass("inv");
    //        $("#name").removeClass("vis");
    //        $("#txtname").addClass("inv");
    //        $("#txtname").removeClass("vis");
    //    }

    //});


    function doEmailShare(id) {
        vcvid = id;
        $("#dialogEmail-form").dialog("open");
    }


function doLogin(id) {
    // alert(id);
    $("#name").addClass("vis");
    $("#name").removeClass("inv");
    FB.init({ appId: '359520694171070', status: true, cookie: true, xfbml: true });
    FB.getLoginStatus(

        function (response) {

            //alert(response.status);
            if (response.status === 'connected') {

                // the user is logged in and has authenticated your
                // app, and response.authResponse supplies
                // the user's ID, a valid access token, a signed
                // request, and the time the access token 
                // and signed request each expire
                var uid = response.authResponse.userID;
                var accessToken = response.authResponse.accessToken;
                //alert(uid);
                showprogressbar()
                //do ajax post with  userid, accesstoken and vcvid
                $.ajax(
{
    //url: '@Url.Action("FacebookShare", "Home")',
    url: '/Home/FacebookShare',
    type: "POST",
    data: "FacebookId= " + uid + "&Token= " + accessToken + "&id= " + id,
    success: function () {
        //change tab
        stopProgressbar()
        alert("Successfully Posted to facebook");


    }


})






                //         FB.api('/me', function (response) {
                //             alert('User: ' + response.name);
                //             alert('Full details: ' + JSON.stringify(response));
                //         }
                //);



            }


            else {

                //login
                //alert("Logging in");
                FB.login(function (response) {

                    if (response.status === 'connected') {
                        // the user is logged in and has authenticated your
                        // app, and response.authResponse supplies
                        // the user's ID, a valid access token, a signed
                        // request, and the time the access token 
                        // and signed request each expire
                        var uid = response.authResponse.userID;
                        var accessToken = response.authResponse.accessToken;
                        // TODO: Handle the access token
                        //do ajax post with  userid, accesstoken and vcvid
                        showprogressbar()
                        $.ajax(
        {
            url: '@Url.Action("FacebookShare", "Home")',
            type: "POST",
            data: "FacebookId= " + uid + "&Token= " + accessToken + "&id= " + id,
            success: function () {
                //change tab
                stopProgressbar()
                alert("Successfully Posted to facebook");



            }


        })

                    } else if (response.status === 'not_authorized') {
                        // the user is logged in to Facebook, 
                        // but has not authenticated your app

                    } else {
                        // the user isn't logged in to Facebook.
                    }
                }, { scope: 'publish_actions' });





            }





        }

        );




}


function showprogressbar() {
    document.getElementById('divAnim').style.display = "block";
}
function stopProgressbar() {
    document.getElementById('divAnim').style.display = "none";
}

function doLoginyoutube(id) {
    vcvid = id;
    var clientId = '614827406472-vf02gggv105tspj1pepat7smj2l6aqk6.apps.googleusercontent.com';
    var apiKey = 'AIzaSyC_hvdBmUc - UxRlc0vGsJhIxrX7KC819Ww';
    var scopes = 'https://www.googleapis.com/auth/youtube';
    //alert("In youtube login");
    gapi.client.setApiKey(apiKey);
    gapi.auth.authorize({ client_id: clientId, scope: scopes, immediate: false }, handleAuthResult);


}






function handleAuthResult(authResult) {
    //alert("In handle");
    //alert(vcvid);
    accessToken = gapi.auth.getToken().access_token;
    //alert(accessToken);
    //var authorizeButton = document.getElementById('authorize-button');
    if (authResult && !authResult.error) {
        //authorizeButton.style.visibility = 'hidden';
        //alert(authResult.access_token);
        $.ajax(
            {
                //url: '@Url.Action("blogger", "Home")',
                url: '/Home/blogger',
                type: "POST",
                data: "id=" + vcvid,
                dataType: "json",
                success: function (data) {
                    $('#txtDescription').val(data.SummaryStrip);


                }
            });
        
        $("#dialogtube-form").dialog("open");


    } else {
        alert("Authentication error");
        //authorizeButton.style.visibility = '';
        //authorizeButton.onclick = handleAuthClick;
    }
}

//function handleAuthClick(event) {
//    gapi.auth.authorize({ client_id: clientId, scope: scopes, immediate: false }, handleAuthResult);
//    return false;
//}



function doLoginDrive(id) {
    vcvid = id;
    var clientId = '614827406472-vf02gggv105tspj1pepat7smj2l6aqk6.apps.googleusercontent.com';
    var apiKey = 'AIzaSyC_hvdBmUc-UxRlc0vGsJhIxrX7KC819Ww';

    var scopes = 'https://www.googleapis.com/auth/drive https://www.googleapis.com/auth/blogger';
    //alert("In drive login");
    gapi.client.setApiKey(apiKey);
    gapi.auth.authorize({ client_id: clientId, scope: scopes, immediate: false }, handleAuthResultDrive);


}

function handleAuthResultDrive(authResult) {
    //alert("In handle drive");
    //alert(vcvid);
    googleaccessToken = gapi.auth.getToken().access_token;
    //alert(googleaccessToken);
    //var authorizeButton = document.getElementById('authorize-button');
    if (authResult && !authResult.error) {
        //authorizeButton.style.visibility = 'hidden';
        //alert(authResult.access_token);
        showprogressbar()

        $.ajax(
            {
                //url: '@Url.Action("blogger", "Home")',
                url: '/Home/blogger',
                type: "POST",
                data: "id=" + vcvid,
                dataType: "json",
                success: function (data) {
                    filename = data.VideoName
                    summary = data.Summary
                    var filedata = getBinary(window.location.protocol + "//" + window.location.host + '/Streams/' + filename + ".flv")

                    insertFile(filedata);
                }
            });


    } else {
        alert("auth error");
        //authorizeButton.style.visibility = '';
        //authorizeButton.onclick = handleAuthClick;
    }
}

function GetBlogs() {



    $.getJSON('https://www.googleapis.com/blogger/v3/users/self/blogs?access_token=' + googleaccessToken, function (data) {
        var listItems = "";
        for (var i = 0; i < data.items.length; i++) {
            listItems += "<option value='" + data.items[i].id + "'>" + data.items[i].name + "</option>";
        }
        $("#ddlBlogId").html(listItems);
        $("#dialog-form").dialog("open");
        $("#ddlBlogId").change();
    });
}


//#ddlBlogId select - look up posts 

$('#ddlBlogId').change(function () {
    //alert('Handler for .change() called.');
    var blogId = $('#ddlBlogId option:selected').val()
    $.getJSON('https://www.googleapis.com/blogger/v3/blogs/' + blogId + '/posts?access_token=' + googleaccessToken, function (data) {
        var listItems = "";
        for (var i = 0; i < data.items.length; i++) {
            listItems += "<option value='" + data.items[i].id + "'>" + data.items[i].title + "</option>";
        }
        $("#ddlPostId").html(listItems);

    });

});




$("#dialog-form").dialog({
    autoOpen: false,
    height: 500,
    width: 500,
    modal: true,
    buttons: {
        "Share to blogger": function () {
            var type = $("input[name='PostType']:checked").val()

            if (type == 'New') {
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    url: 'https://www.googleapis.com/blogger/v3/blogs/' + $('#ddlBlogId option:selected').val() + '/posts/?access_token=' + googleaccessToken,
                    data: JSON.stringify({
                        "kind": "blogger#post",
                        "blog": {
                            "id": $('#ddlBlogId option:selected').val()
                        },
                        "title": $("#txtname").val(),
                        "content": summary + "<iframe src='https://docs.google.com/file/d/" + fileid + "/preview' width='640' height='385'></iframe>"
                    }),
                    success: function () {

                        alert("Successfully Posted to Blogger");
                        $(this).dialog("close");
                    },
                    contentType: "application/json; charset=utf-8"

                });







            } else {

                // update post
                // get post details so the the video can be added to front of post.
                var currentcontent = "";
                var title = "";
                $.getJSON('https://www.googleapis.com/blogger/v3/blogs/' + $('#ddlBlogId option:selected').val() + '/posts/' + $('#ddlPostId option:selected').val() + '/?access_token=' + googleaccessToken, function (data) {
                    currentcontent = data.content;
                    title = data.title;
                    $.ajax({
                        type: "PUT",
                        dataType: 'json',
                        url: 'https://www.googleapis.com/blogger/v3/blogs/' + $('#ddlBlogId option:selected').val() + '/posts/' + $('#ddlPostId option:selected').val() + '?access_token=' + googleaccessToken,
                        data: JSON.stringify({
                            "kind": "blogger#post",
                            "blog": {
                                "id": $('#ddlBlogId option:selected').val()
                            },
                            "id": $('#ddlPostId option:selected').val(),
                            "title": title,
                            "content": summary + "<iframe src='https://docs.google.com/file/d/" + fileid + "/preview' width='640' height='385'></iframe>" + currentcontent
                        }),
                        success: function () {

                            alert("Successfully Posted to Blogger");

                        },
                        contentType: "application/json; charset=utf-8"

                    });



                });



            }




            $(this).dialog("close");

        },
        Cancel: function () {
            $(this).dialog("close");
        }
    },
    close: function () {
        //allFields.val( "" ).removeClass( "ui-state-error" );
    }
});


$("#dialogtube-form").dialog({
    autoOpen: false,
    height: 500,
    width: 500,
    modal: true,
    buttons: {
        "Share to youtube": function () {

            showprogressbar()



            $.ajax(
{
    //url: '@Url.Action("youtubeShare", "Home")',
    url: '/Home/youtubeShare',
    type: "POST",
    data: "Token=" + accessToken + "&id=" + vcvid + "&title=" + $("#txtyoutubename").val() + "&description=" + $("#txtDescription").val() + "&category=" + $('#ddlCategory option:selected').val() + "&type=" + $('#ddlType option:selected').val(),
    success: function () {
        //change tab
        stopProgressbar()
        alert("Successfully Posted to youtube");



    }


})




            $(this).dialog("close");

        },
        Cancel: function () {
            $(this).dialog("close");
        }
    },
    close: function () {
        //allFields.val( "" ).removeClass( "ui-state-error" );
    }
});




function insertFile(fileData, callback) {
    var boundary = '-------314159265358979323846';
    var delimiter = "\r\n--" + boundary + "\r\n";
    var close_delim = "\r\n--" + boundary + "--";


    var contentType = 'video/flv'
    var metadata = {
        'title': 'MyChatPak.flv',
        'mimeType': 'video/x-flv'
    };

    var base64Data = base64Encode(fileData);
    var multipartRequestBody =
        delimiter +
        'Content-Type: application/json\r\n\r\n' +
        JSON.stringify(metadata) +
        delimiter +
        'Content-Type: ' + contentType + '\r\n' +
        'Content-Transfer-Encoding: base64\r\n' +
        '\r\n' +
        base64Data +
        close_delim;

    var request = gapi.client.request({
        'path': '/upload/drive/v2/files',
        'method': 'POST',
        'params': { 'uploadType': 'multipart' },
        'headers': {
            'Content-Type': 'multipart/mixed; boundary="' + boundary + '"'
        },
        'body': multipartRequestBody
    });
    if (!callback) {
        callback = function (file) {
            console.log(file)
            if (!file.id) {
                stopProgressbar()
                alert("Failed uploading to Google Drive");
            }

            else {
                fileid = file.id
                stopProgressbar()
                alert("Successfully Uploaded to Google Drive");
                GetBlogs()
            }


        };
    }
    request.execute(callback);

}



function getBinary(file) {
    var xhr = new XMLHttpRequest();
    xhr.open("GET", file, false);
    if (xhr.overrideMimeType) {
        xhr.overrideMimeType('text/plain; charset=x-user-defined');
        xhr.send(null);
        return xhr.responseText;
    }

    else {
        xhr.setRequestHeader('Accept-Charset', 'x-user-defined');
        xhr.send(null);
        var data = new VBArray(xhr.responseBody).toArray();
        var dataStr = ""
        for (var i = 0; i < data.length; i++) {
            dataStr += String.fromCharCode(data[i]);
        }
        return dataStr;
    }

}

//function base64encode(binary) {
//    return btoa(unescape(binary));
//}

function base64Encode(str) {
    var CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
    var out = "", i = 0, len = str.length, c1, c2, c3;
    while (i < len) {
        c1 = str.charCodeAt(i++) & 0xff;
        if (i == len) {
            out += CHARS.charAt(c1 >> 2);
            out += CHARS.charAt((c1 & 0x3) << 4);
            out += "==";
            break;
        }
        c2 = str.charCodeAt(i++);
        if (i == len) {
            out += CHARS.charAt(c1 >> 2);
            out += CHARS.charAt(((c1 & 0x3) << 4) | ((c2 & 0xF0) >> 4));
            out += CHARS.charAt((c2 & 0xF) << 2);
            out += "=";
            break;
        }
        c3 = str.charCodeAt(i++);
        out += CHARS.charAt(c1 >> 2);
        out += CHARS.charAt(((c1 & 0x3) << 4) | ((c2 & 0xF0) >> 4));
        out += CHARS.charAt(((c2 & 0xF) << 2) | ((c3 & 0xC0) >> 6));
        out += CHARS.charAt(c3 & 0x3F);
    }
    return out;
}

$("#dialogEmail-form").dialog({
    autoOpen: false,
    height: 500,
    width: 800,
    modal: true,
    buttons: {
        "Send Email": function () {

            showprogressbar()
            // do server side validation
            $.ajax(
            {
                url: '/Create/Send',
                type: "POST",
                data: "Col= " + $('form').serialize() + "&id=" + vcvid,
                success: function (data) {
                    if (data == "True") {
                        stopProgressbar()
                        alert('You have Successfully Sent your MyChatPak!')
                        requestcomplete = true


                    }
                    else {
                        stopProgressbar()
                        alert('Error Sending your MyChatPak');
                        requestcomplete = false


                    }


                }


            });



            $(this).dialog("close");

        },
        Cancel: function () {
            $(this).dialog("close");
        }
    },
    close: function () {
        //allFields.val( "" ).removeClass( "ui-state-error" );
    }
});






