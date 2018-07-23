using AutoMapper;
using CodeChange.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CodeChange.Controllers.API
{
    public class StatisticsController : ApiController
    {
        public HttpResponseMessage Post([FromBody]APIModels.ActionStat actions)
        {
            try
            {
                
                    using (ChangeCodeEntities db = new ChangeCodeEntities())
                    {
                        actions.IDAction = db.Actions.Where(l => l.ActionURL.Contains(actions.IDStatus)).FirstOrDefault().ID;
                        ActionStats akcija = Mapper.Map<ActionStats>(actions);
                        akcija.ID = Guid.NewGuid();
                        akcija.Date = DateTime.Now;

                        db.ActionStats.Add(akcija);
                        db.SaveChanges();
                    }

               

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
