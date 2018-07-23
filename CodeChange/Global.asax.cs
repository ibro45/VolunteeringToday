using AutoMapper;
using CodeChange.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CodeChange
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //AutoMapper
            //Mapper.Initialize(cfg => cfg.CreateMap<Actions, CodeChange.APIModels.Action>());



            Mapper.Initialize(cfg => {
                cfg.CreateMap<CodeChange.APIModels.Action, Actions>();
                cfg.CreateMap<CodeChange.APIModels.ActionStat, ActionStats>();
            });
        }
    }
}
