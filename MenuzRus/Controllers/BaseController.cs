using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Services;

namespace MenuzRus.Controllers {

    public abstract class BaseController : Controller {

        #region Declarations

        #endregion Declarations

        #region Properties

        public Boolean IsLoggedIn { set; get; }

        #endregion Properties

        #region Construtors

        public BaseController() {
        }

        #endregion Construtors

        #region Public Functions

        #endregion Public Functions

        #region Private/Protected Functions

        public void LogSessionInfo() {
        }

        #endregion Private/Protected Functions

        #region Overrides

        protected override void Initialize(System.Web.Routing.RequestContext requestContext) {
            base.Initialize(requestContext);
            if (requestContext.HttpContext.Session["IsLoggedIn"] == null)
                requestContext.HttpContext.Session["IsLoggedIn"] = false;
            this.IsLoggedIn = (Boolean)requestContext.HttpContext.Session["IsLoggedIn"];
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            MvcHandler handler;
            String route = null;

            base.OnActionExecuting(filterContext);

            try {
                // Get the route
                handler = this.HttpContext.Handler as MvcHandler;
                if (handler != null && handler.RequestContext != null && handler.RequestContext.RouteData != null) {
                    // Route
                    if (handler.RequestContext.RouteData.Values != null && handler.RequestContext.RouteData.Route != null) {
                        route = handler.RequestContext.RouteData.Route.GetVirtualPath(handler.RequestContext, handler.RequestContext.RouteData.Values).VirtualPath;
                    }
                }

                filterContext.Controller.ViewData["Layout"] = "_Layout";
                if (!this.IsLoggedIn) {
                    filterContext.Controller.ViewData["Layout"] = "_Login";

                    // These 3 controllers do not need to check if logged in or not
                    if (route.ToUpper() != "login".ToUpper()
                            && route.ToUpper() != "customer".ToUpper()
                            && route.ToUpper() != "contact".ToUpper()) {
                        if (!this.IsLoggedIn)
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new {
                                controller = "Login",
                                action = "Index",
                            }));
                    }
                }
            }
            catch (Exception ex) {
                throw;
            }
        }

        protected override void OnAuthorization(AuthorizationContext filterContext) {
            try {
                base.OnAuthorization(filterContext);
            }
            catch (Exception ex) {
                throw;
            }
        }

        #endregion Overrides
    }
}