@ModelType List(Of MvcMyVCV.Product)
@Code
    ViewData("Title") = "Payment"
End Code

<h3>Your Shopping Cart</h3><br />
@Using Html.BeginForm()
    @<fieldset>
        <legend>Payment</legend>
        <table class="CartTable">
            <tr>
                <td>Item</td>
                <td>Price</td>
                <td>Estimated Tax</td>
            </tr>
            <tr>
                <td>
                    <label id="lblProduct">@(ViewBag.Product)</label>
                </td>
                <td>
                    <label id="lblSubtotal">$@(ViewBag.Subtotal)</label></td>
                <td>
                    <label id="lblTax">$@(ViewBag.Tax)</label></td>
            </tr>
        </table>
        <br />

        <label style="float: right; padding-right: 5px;">Total: $@ViewBag.Total</label><br />

        <br />
        @Html.Hidden("Recurring", False)
        <input class="vcvbutton" type="submit" value="Pay Now" />
    </fieldset>
    
End Using
  



