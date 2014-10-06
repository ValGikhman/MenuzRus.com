using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MenuzRus {
    // Note: For instructions on enabling IIS6 or IIS7 classic mode,
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication {

        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.MapRoute(
            //name: "MenuDesigner",
            //url: "MenuDesigner/Index/{id}",
            //defaults: new {
            //    controller = "MenuDesigner",
            //    action = "Index",
            //    monitor = UrlParameter.Optional
            //});

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Session_End(Object sender, EventArgs e) {
            SessionData.user = null;
            SessionData.floor = null;
            SessionData.item = null;
            SessionData.customer = null;
            SessionData.menu = null;
            SessionData.sessionId = null;
        }
    }
}