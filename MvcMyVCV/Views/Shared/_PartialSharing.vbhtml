<script src="http://connect.facebook.net/en_US/all.js"></script>
<style type="text/css">
DIV#divAnim{position:fixed;z-index:101;top:0px;left:5px;height:100%;width:100%;
background:url(/images/spinning-progress2.gif) no-repeat center;  /* change gif img */
filter:alpha(opacity=60);display:none;background-color: rgba(54, 25, 25, .5); }
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

<script src="https://apis.google.com/js/client.js"></script>

<div id="divAnim">  </div>

<div id="dialog-form" title="" >
<p class="validateTips">All form fields are required.</p>
<form>
<fieldset>
<label  for="blog">Blog</label>
<select name="ddlBlogId" id="ddlBlogId"></select><br />

<input type="radio" name="PostType" value="New" checked="checked">New Post<br>
<input type="radio" name="PostType" value="Existing">Existing<br />

<label class="inv" id="Existing" for="post">Existing Post</label>
<select class="inv" name="ddlPostId" id="ddlPostId"></select>

<label id="name" for="name">Post Name</label>
<input type="text" name="name" id="txtname" class="text ui-widget-content ui-corner-all" />


</fieldset>
</form>
</div>

<div id="dialogtube-form" title="" >
<p class="validateTips">All form fields are required.</p>
<form>
<fieldset>
<label  for="name">Title</label>
<input type="text" name="name" id="txtyoutubename" class="text ui-widget-content ui-corner-all"  />

<label id="Description" for="Description">Description</label>
<textarea  id="txtDescription" ></textarea>
<label>Type</label>
<select name="ddlType" id="ddlType">
<option value="1">Public</option>
<option value="2">Private</option>



</select><br />

<label>Category</label>
<select name="ddlCategory" id="ddlCategory">
 <option value="Comedy" selected="">Comedy</option>
    <option value="Education">Education</option>
    <option value="Entertainment">Entertainment</option>
    <option value="Film &amp; Animation">Film &amp; Animation</option>
    <option value="Gaming">Gaming</option>
    <option value="Howto &amp; Style">Howto &amp; Style</option>
    <option value="Music">Music</option>
    <option value="News &amp; Politics">News &amp; Politics</option>
    <option value="Nonprofits &amp; Activism">Nonprofits &amp; Activism</option>
    <option value="People &amp; Blogs">People &amp; Blogs</option>
    <option value="Pets &amp; Animals">Pets &amp; Animals</option>
    <option value="Science &amp; Technology">Science &amp; Technology</option>
    <option value="Sports">Sports</option>
    <option value="Travel &amp; Events">Travel &amp; Events</option>


</select>




</fieldset>
</form>
</div>

<div id="dialogEmail-form" title="" >
<p class="validateTips">All form fields are required.</p>
<form>
<fieldset>

<table>
<tr ><td>@Html.Label("Destination Email Address")</td><td>@Html.TextBox("txtEmail")</td></tr>
<tr "><td style="background:#FFFFFF;">@Html.Label("From Email Address")</td><td style="background:#FFFFFF;">@Html.TextBox("txtFromEmail")</td></tr>


</table>
   

</fieldset>
</form>
</div>
