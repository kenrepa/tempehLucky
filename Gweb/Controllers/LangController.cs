using Gweb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gweb.Controllers
{
    public class LangController : MyController
    {
        // GET: Lang
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangeLanguage(string lang)
        {
            new LanguageMang().SetLanguage(lang);
            return RedirectToAction("Index", "Home");
        }
    }
}