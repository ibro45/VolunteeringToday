using CodeChange.Common;
using CodeChange.Models;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OAuth;
using System.Net;
using System.IO;
using ConsoleApp5;
using System.Threading;
using System.Threading.Tasks;

namespace CodeChange.Controllers
{
    public class StartController : Controller
    {
        FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyBkGaZF1oIvUmldy_zdYrK9avVGQPmKAts")); // Key!
        // GET: Start
        public ActionResult Index()
        {
            return View();
        }

        #region Login
        [Route("Login")]
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        [Route("Login")]
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Login(string token, string username)
        {
            try
            {
                using (ChangeCodeEntities db = new ChangeCodeEntities())
                {
                    Users korisnik = db.Users.Where(l => l.Email == username).FirstOrDefault();

                    if (korisnik != null)
                    {
                        ViewBag.Success = true;

                        HttpContext ctx = System.Web.HttpContext.Current;

                        HttpCookie cookie = new HttpCookie("Sessionid");
                        cookie.HttpOnly = true;
                        string tok = token;
                        cookie.Value = token;
                        cookie.Expires = DateTime.Now.AddDays(1);

                        ctx.Response.SetCookie(cookie);

                        korisnik.Token = tok;
                        db.Entry(korisnik).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        korisnik = new Users();
                        korisnik.ID = Guid.NewGuid();
                        korisnik.Email = username;
                        korisnik.UserKey = token;

                        string tok = Hash.HashSHA512(token + korisnik.Email);

                        db.Users.Add(korisnik);
                        db.SaveChanges();

                        return RedirectToAction("Registracija/" + korisnik.ID, "Start");
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Detalji 
        [Route("Actions/{id}")]
        public ActionResult Detalji(Guid id)
        {
            using (ChangeCodeEntities db = new ChangeCodeEntities())
            {
                ViewData["TrenutniKorisnik"] = CurrentUser.DohvatiTrenutnog();

                Actions a = db.Actions.Where(l => l.ID == id).FirstOrDefault();
                string tweetID = a.ActionURL.Split('/').LastOrDefault();

                ProcessTools pt = new ProcessTools();
                pt.pokreniProces("python", "c:/Micro/TwitterAnalytics.py " + tweetID);
                Thread.Sleep(5000);
                if (a != null)
                {
                    ViewData["Akcija"] = a;
                    ViewData["Stat"] = db.ActionStats.Where(l => l.IDAction == a.ID).OrderBy(l => l.Date).ToList().LastOrDefault();

                    System.Web.HttpRequest context = System.Web.HttpContext.Current.Request;
                    HttpCookie kuki = context.Cookies["Sessionid"];
                    string token = "";
                    if (kuki != null)
                    {
                        token = kuki.Value;
                    }
                    List<UserAction> listaKorisnika = db.UserAction.Where(l => l.IDAction == a.ID && l.Dosao == true).ToList();

                    List<Users> dosliKorisnici = new List<Users>();

                    foreach(UserAction ua in listaKorisnika)
                    {
                        Users temp = db.Users.Where(l => l.ID == ua.IDUser).FirstOrDefault();
                        if (temp != null)
                        {
                            dosliKorisnici.Add(temp);
                        }
                    }

                    ViewData["DosliKorisnici"] = dosliKorisnici;


                    Users trenutniKorisnik = db.Users.Where(l => l.UserKey.Contains(token)).FirstOrDefault();

                    if (trenutniKorisnik != null)
                    {
                        string userName = a.UserURL.Split('/').LastOrDefault();
                        if (trenutniKorisnik.Email == userName)
                        {
                            ViewBag.UserAdmin = true;
                        }
                        ViewBag.Korisnik = db.UserAction.Where(l => l.IDAction == a.ID && l.IDUser == trenutniKorisnik.ID).FirstOrDefault();
                        ViewBag.KorisnikID = trenutniKorisnik.ID;
                        ViewBag.ActionID = a.ID;
                    }
                    else
                    {
                        ViewBag.Korisnik = null;
                    }
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound, "");
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Detalji(string userid, string actionid)
        {
            Guid userGUID = new Guid(userid);
            Guid actionGUID = new Guid(actionid);
            ViewBag.UserAdmin = null;
            using (ChangeCodeEntities db = new ChangeCodeEntities())
            {
                
                ViewData["TrenutniKorisnik"] = CurrentUser.DohvatiTrenutnog();

                UserAction ua = db.UserAction.Where(l => l.IDAction == actionGUID && l.IDUser == userGUID).FirstOrDefault();
                if (ua == null)
                {
                    ua = new UserAction();
                    ua.ID = Guid.NewGuid();
                    ua.IDAction = actionGUID;
                    ua.IDUser = userGUID;

                    db.UserAction.Add(ua);
                    db.SaveChanges();
                }

                Actions a = db.Actions.Where(l => l.ID == actionGUID).FirstOrDefault();
                string tweetID = a.ActionURL.Split('/').LastOrDefault();

                ProcessTools pt = new ProcessTools();
                pt.pokreniProces("python", "c:/Micro/TwitterAnalytics.py " + tweetID);

                if (a != null)
                {
                    ViewData["Akcija"] = a;
                    ViewData["Stat"] = db.ActionStats.Where(l => l.IDAction == a.ID).OrderBy(l => l.Date).ToList().LastOrDefault();

                    System.Web.HttpRequest context = System.Web.HttpContext.Current.Request;
                    HttpCookie kuki = context.Cookies["Sessionid"];
                    string token = "";
                    if (kuki != null)
                    {
                        token = kuki.Value;
                    }
                    List<UserAction> listaKorisnika = db.UserAction.Where(l => l.IDAction == a.ID && l.Dosao == true).ToList();

                    List<Users> dosliKorisnici = new List<Users>();

                    foreach (UserAction uac in listaKorisnika)
                    {
                        Users temp = db.Users.Where(l => l.ID == uac.IDUser).FirstOrDefault();
                        if (temp != null)
                        {
                            dosliKorisnici.Add(temp);
                        }
                    }

                    ViewData["DosliKorisnici"] = dosliKorisnici;


                    Users trenutniKorisnik = db.Users.Where(l => l.UserKey.Contains(token)).FirstOrDefault();

                    if (trenutniKorisnik != null)
                    {
                        string userName = a.UserURL.Split('/').LastOrDefault();
                        if (trenutniKorisnik.Email == userName)
                        {
                            ViewBag.UserAdmin = true;
                        }
                        ViewBag.Korisnik = db.UserAction.Where(l => l.IDAction == a.ID && l.IDUser == trenutniKorisnik.ID).FirstOrDefault();
                        ViewBag.KorisnikID = trenutniKorisnik.ID;
                        ViewBag.ActionID = a.ID;
                    }
                    else
                    {
                        ViewBag.Korisnik = null;
                    }
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound, "");
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> DodajSliku(HttpPostedFileBase file, string id)//
        {
            if (file != null && file.ContentLength > 0) {

                string temp = Guid.NewGuid().ToString();
                string path = Path.Combine(Server.MapPath("~/Content/"),
               (temp + "." + file.ContentType.Split('/').Last()));
                file.SaveAs(path);

                byte[] fileR = System.IO.File.ReadAllBytes(path);

                List<string> guidi = await Task.Run(() => IdentifyFace.identifyFace(Convert.ToBase64String(fileR)));
                Thread.Sleep(5000);
                foreach (string t in guidi)
                {
                    Guid uID = new Guid(t);
                    Guid aID = new Guid(id);

                    using (ChangeCodeEntities db = new ChangeCodeEntities())
                    {
                        UserAction ua = db.UserAction.Where(l => l.IDAction == aID && l.IDUser == uID).FirstOrDefault();
                        if (ua == null)
                        {
                            ua = new UserAction();
                            ua.ID = Guid.NewGuid();
                            ua.IDAction = aID;
                            ua.IDUser = uID;
                            ua.Dosao = true;
                            db.UserAction.Add(ua);
                            db.SaveChanges();
                        }
                        else
                        {
                            ua.Dosao = true;
                            db.Entry(ua).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }
                System.IO.File.Delete(path);
                return RedirectToAction("Index", "Home");
                    }
            else
            {
                ViewBag.Message = "Please select file";
            }
            ViewBag.ID = id;
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Registracija
        public ActionResult Registracija(string id)
        {
            ChangeCodeEntities db = new ChangeCodeEntities();
            System.Web.HttpRequest context = System.Web.HttpContext.Current.Request;
            HttpCookie kuki = context.Cookies["Sessionid"];
            string token = "";
            if (kuki != null)
            {
                token = kuki.Value;
            }
            Users trenutniKorisnik = db.Users.Where(l => l.UserKey.Contains(token)).FirstOrDefault();
            ViewData["TrenutniKorisnik"] = trenutniKorisnik;

            ViewBag.ID = id;

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Registracija(HttpPostedFileBase file, string id)//
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/Content/Users/"),
                   (id + "." + file.ContentType.Split('/').Last()));
                    file.SaveAs(path);

                    byte[] fileR = System.IO.File.ReadAllBytes(path);

                    PersonGroup.dodajOsobuUGrupu(Convert.ToBase64String(fileR), id);
                    await Task.Run(() => RequestTraining.makeTrainRequest());
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "Please select file";
            }
            ViewBag.ID = id;
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}
