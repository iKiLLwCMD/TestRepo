@ModelType MvcMyVCV.VCVShared
@Code
    ViewData("Title") = "ViewVCV"
End Code
<div class="box" style="width:500px;">
    <h3 style="text-align:center;">Profile of @Model.User.UserName</h3>
</div>

 <table style="background-image: url('metal_bg.gif');width:100%;">
        <tr>
            <td style="padding: 10px 15px;vertical-align:top;">
                <table style="margin-left:auto;margin-right:auto;width:800px;">
                    <tr>
                        <td style="padding-left:35px;background-color:#ffffff">
                            <table style="width:445px;border-radius:30px;margin-left:auto;margin-right:auto;background-color:#6A8088;">
                                <tr>
                                    <td style="padding-left:30px;height:364px" >
                                        <a href="/Streams/@(Model.VideoName).flv" class="myPlayer" id="@Model.VCVSharedId"
                                        style="display:block;width:400px;height:320px;margin:10px auto"></a>
                                            
                                    </td>
                                </tr>
                            </table>
                            <table style="margin-left:auto;margin-right:auto;width:500px;">
                                <tr>
                                    <td style="text-align:center;width:33%;"><a target="_blank" href="~/Attachments/@Model.Attachment1Id">@Model.Attachment1Name</a></td>
                                    @If Model.Attachment2Name <> "" And Model.Attachment3Name <> "" Then
                                        @<td style="text-align:center;width:33%;"><a target="_blank" href="~/Attachments/@Model.Attachment2Id">@Model.Attachment2Name</a></td>
                                        @<td style="text-align:center;width:33%;"><a target="_blank" href="~/Attachments/@Model.Attachment3Id">@Model.Attachment3Name</a></td>
                                    ElseIf Model.Attachment2Name <> "" And Model.Attachment3Name = "" Then
                                        @<td style="text-align:center;width:33%;"></td>
                                        @<td style="text-align:center;width:33%;"><a target="_blank" href="~/Attachments/@Model.Attachment2Id">@Model.Attachment2Name</a></td>
                                    ElseIf Model.Attachment2Name = "" And Model.Attachment3Name <> "" Then
                                        @<td style="text-align:center;width:33%;"></td>
                                        @<td style="text-align:center;width:33%;"><a target="_blank" href="~/Attachments/@Model.Attachment3Id">@Model.Attachment3Name</a></td>
                                    Else
                                        @<td style="text-align:center;width:33%;"></td>
                                        @<td style="text-align:center;width:33%;"></td>
                                    End If
                                    
                                </tr>
                            </table>
                            <br />
                            
                            <table style="background-color:#E2F1F6;width:90%;padding-left:50px">
                                <tr>
                                    <td style="padding: 3px;background-color:#E2F1F6;margin-left:auto;margin-right:auto;">
                                            <p style="text-align:center;">@Model.SummaryContent</p>
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

<script>
    flowplayer("p.myPlayer", { src: "http://releases.flowplayer.org/swf/flowplayer-3.2.16.swf", wmode: "opaque" }, {

        clip: {
            autoPlay: false,
            bufferLength: 5,
            autoBuffering: true
        }
    });
</script>