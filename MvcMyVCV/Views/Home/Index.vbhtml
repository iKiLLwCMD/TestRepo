@*@ModelType PagedList.IPagedList(Of MvcMyVCV.VCV)*@

@Code
    ViewData("Title") = "eLearning NZ | New Zealand"
    ViewBag.Description = "Get the ultimate eLearning NZ classroom experience with MyChatPacks eLearning software, easy and fun to use tool for students, teachers and parents."
    ViewBag.Keywords = "e Learning NZ, eLearning NZ, eLearning software, eLearning tool,  New Zealand"
End Code

@If Request.IsAuthenticated = True Then
   

    @Code
        Dim user As New MvcMyVCV.User
        Dim db As New MvcMyVCV.VCVOnlineEntities
        user = db.Users.Find(Context.User.Identity.Name)
        If user.UserType = "Demo" Or user.UserType = "Employee" Then
            
             @:<p align="center"><a class="CreateButton" href="/Create">Create New MyVCV</a></p>
    @:<br />
            @Html.Partial("_PartialVCVS", ViewData.Model)
    Else
        @Html.Partial("_PartialListings", ViewData.Model)
    End If
    End Code

    
Else
    @Html.Partial("_PartialInfo")
End If