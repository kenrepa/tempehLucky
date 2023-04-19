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

    }
}