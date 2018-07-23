using CodeChange.Common;
using CodeChange.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CodeChange.Controllers.API
{
    public class ServiceController : ApiController
    {
        public void Post([FromBody]string query)
        {
            using (ChangeCodeEntities db = new ChangeCodeEntities())
            {
                db.Database.ExecuteSqlCommand(query);
            }
        }

        public void Get()
        {
            ProcessTools pt = new ProcessTools();
            pt.pokreniProces("python", @"C:\Micro\OCRApplication\pokreni.py");
        }
    }
}
