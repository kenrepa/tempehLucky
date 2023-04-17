using Gweb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gweb.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChangeCurrentCulture(int culture)
        {
            //
            // Change the current culture for this user.
            //
            SiteSession.CurrentUICulture = culture;
            //
            // Cache the new current culture into the user HTTP session. 
            //
            Session["CurrentUICulture"] = culture;
            //
            // Redirect to the same page from where the request was made! 
            //
            return Redirect(Request.UrlReferrer.ToString());
        }

       

    }
}