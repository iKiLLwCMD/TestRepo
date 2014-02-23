@ModelType MvcMyVCV.User
@Code
    ViewData("Title") = "Renew"
End Code

<h2>Account Expired</h2>



@Using Html.BeginForm()
    @<fieldset>

Your Account Expired @Html.DisplayFor(Function(model) model.NextBillingDate)

  <table style="width: 80%;">
    <tr>
        <td><img src="" id="ProductLogo" /></td><td style="text-align:left;"><label id="lblProduct">@(ViewBag.Product)</label><br />@(ViewBag.Startdate) - @(ViewBag.Enddate)</td><td style="text-align:right;"><label id="lblPrice">$@(ViewBag.Total)</label>
                 </td>
       

    </tr>
    <tr>
    @If (ViewBag.Recurring = True) Then
            @:<td colspan="2">@Html.CheckBox("Recurring", True)I authorise MyChatPack to charge my Credit Card for recurring payments.<br></td>
Else
        @:<td colspan="2"></td>
    End If

        
        <td style="text-align:right;">
            Sub Total: <label id="lblSubtotal" >$@(ViewBag.Subtotal)</label><br />
            Estimated Tax: <label id="lblTax" >$@(ViewBag.Tax)</label><br />
            Total: <label id="lblTotal" >$@(ViewBag.Total)</label><br />
        </td>
    </tr>
    <tr>
        <td colspan="3"><input type="submit" value="Place Order" /></td>

    </tr>

</table>
    </fieldset>
End Using