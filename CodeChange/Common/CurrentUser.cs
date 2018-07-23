using CodeChange.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeChange.Common
{
    public class CurrentUser
    {
        public static Users DohvatiTrenutnog()
        {
            HttpContext ctx = System.Web.HttpContext.Current;
            HttpCookie cookie = ctx.Request.Cookies["Sessionid"];


            using (ChangeCodeEntities db = new ChangeCodeEntities())
            {
                if (cookie != null)
                {
                    Users k = db.Users.Where(l => l.UserKey == cookie.Value).FirstOrDefault();

                    
                    return k;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}