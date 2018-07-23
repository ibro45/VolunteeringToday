using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CodeChange.APIModels;
using CodeChange.Models;
using AutoMapper;
using CodeChange.Common;

namespace CodeChange.Controllers.API
{
    public class ActionsController : ApiController
    {
       

        // PUT: api/Actions/5
        public HttpResponseMessage Post([FromBody]List<APIModels.Action> actions)
        {
            try
            {
                foreach (APIModels.Action a in actions)
                {
                    int broj = 0;
                    using (ChangeCodeEntities db = new ChangeCodeEntities())
                    {
                        broj = db.Actions.Where(l => l.ActionName == a.ActionName).ToList().Count();
                    }

                    if (broj == 0)
                    {
                        Actions akcija = Mapper.Map<Actions>(a);
                        akcija.ID = Guid.NewGuid();
                        if (!string.IsNullOrEmpty(akcija.ActionImage))
                        {
                            akcija.DateBegin = akcija.DateBegin == DateTime.MinValue ? null : akcija.DateBegin;
                            akcija.DateEnd = akcija.DateEnd == DateTime.MinValue ? null : akcija.DateEnd;


                            using (ChangeCodeEntities db = new ChangeCodeEntities())
                            {
                                db.Actions.Add(akcija);
                                db.SaveChanges();
                            }

                            string tweetID = akcija.ActionURL.Split('/').LastOrDefault();
                            string handle = akcija.UserURL.Split('/').LastOrDefault();
                            ProcessTools pt = new ProcessTools();
                            pt.pokreniProces("python", "c:/Micro/PostTweets.py " + tweetID + " " + akcija.ActionUserID + " " + handle);

                        }
                    }
                }

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch(Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

    }
}
