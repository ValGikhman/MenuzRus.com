using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MenuzRus {

    public class MvcApplication : System.Web.HttpApplication {

        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                "Default",                                                                  // Route name
                "{controller}/{action}/{id}",                                               // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }   // Parameter defaults
            );
        }

        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Session_End(Object sender, EventArgs e) {
            //SessionData.user = null;
            //SessionData.floor = null;
            //SessionData.item = null;
            //SessionData.customer = null;
            //SessionData.menu = null;
            SessionData.sessionId = null;
        }
    }
}