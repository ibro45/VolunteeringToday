using CodeChange.Common;
using CodeChange.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeChange.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (ChangeCodeEntities db = new ChangeCodeEntities())
            {

                ViewData["TrenutniKorisnik"] = CurrentUser.DohvatiTrenutnog();

                List<Actions> akcije = db.Actions.OrderByDescending(l => l.DateBegin).ToList();
                ViewData["Akcije"] = akcije;
            }
            return View();
        }
    }
}