using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Gweb.Controllers
{
    public class ResourceController : Controller
    {
        // GET: Resource
        private Payment payment;
        public ActionResult Index(string language)
        {
            if (!string.IsNullOrEmpty(language))
            {
                Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(language);
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(language);
            }
            HttpCookie cookie = new HttpCookie("Language");
            cookie.Value = language;
            Response.Cookies.Add(cookie);
            return View();
        }

        public ActionResult PayPaymentWithPaypal(FormCollection form, string Cancel = null)
        {
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                string payerId = Request.Params["PayerID"];
                string description = form["ItemName"];
                string qty = form["Quantity"];
                string price = form["Price"];
                string shippingfee = "2";
                string taxRate = "0.1495";

                var _qty = Decimal.Parse(qty);
                var _shipfee = Decimal.Parse(shippingfee);
                var _price = Decimal.Parse(price);
                var _taxrate = Double.Parse(taxRate);

                Decimal _subtotal = (_qty * _price);
                Double _taxes = _taxrate * Convert.ToDouble(_subtotal);
                Double _txs = Math.Round(_taxes, 2);
                string subtotal = _subtotal.ToString();
                string total = (_subtotal + _shipfee + Convert.ToDecimal(_txs)).ToString();
                string taxes = _txs.ToString();

                if (string.IsNullOrEmpty(payerId))
                {
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Home/PaymentWithPayPal?";
                    var guid = Convert.ToString((new Random()).Next(100000));
                    var createdPayment = this.CreatePayment(apiContext, taxes, shippingfee, qty, subtotal, total, price, description, baseURI + "guid=" + guid);
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }
            return View("SuccessView");
        }

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string taxes, string shippingfee, string qty, string subtotal, string total, string price, string description, string redirectUrl)
        {
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc  
            itemList.items.Add(new Item()
            {
                name = description,
                currency = "CAD",
                price = price,
                quantity = qty
            });
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = taxes,
                shipping = shippingfee,
                subtotal = subtotal
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "CAD",
                total = total, // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            transactionList.Add(new Transaction()
            {
                description = "Lucky Tempeh",
                invoice_number = "LT" + DateTime.Now.ToString("yyyyMMddhhmmsszzz"), //Generate an Invoice No  
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }
    }
}