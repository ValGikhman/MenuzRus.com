using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Services;

namespace MenuzRus.Controllers {

    public abstract class BaseController : Controller {

        #region Properties

        public LogService _LogService { set; get; }

        public Boolean IsLoggedIn { set; get; }

        #endregion Properties

        #region Construtors

        public BaseController() {
        }

        #endregion Construtors

        #region Overrides

        protected override void Initialize(System.Web.Routing.RequestContext requestContext) {
            base.Initialize(requestContext);
            if (_LogService == null)
                _LogService = new LogService();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            MvcHandler handler;
            String route = null;

            this.IsLoggedIn = !(SessionData.user == null);

            SessionData.route = this.BuildRoute();
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

                    // These 2 controllers do not need to check if logged in or not
                    //if (route.ToUpper() != "login".ToUpper()
                    //        && route.ToUpper() != "registration".ToUpper()) {
                    //    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new {
                    //        controller = "Login",
                    //        action = "Index",
                    //    }));
                    //}
                }

                if (SessionData.user != null)
                    this.LogAcvitity(filterContext);
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

        #region public

        public void Log(Common.LogType logType, String message, params Object[] data) {
            LogData(logType, message, null, data);
        }

        public void Log(Exception exception) {
            LogData(Common.LogType.Exception, exception);
        }

        public void Log(String message, Exception exceptionToLog) {
            LogData(Common.LogType.Exception, message, exceptionToLog);
        }

        private String BuildRoute() {
            String retValue = String.Empty;
            if (RouteData.Values["controller"] != null)
                retValue = String.Format("{0}/", RouteData.Values["controller"].ToString());

            if (RouteData.Values["action"] != null)
                retValue = String.Format("{0}/{1}", retValue, RouteData.Values["action"].ToString());

            if (RouteData.Values["id"] != null)
                retValue = String.Format("{0}/{1}", retValue, RouteData.Values["id"].ToString());

            return retValue;
        }

        private void LogAcvitity(ActionExecutingContext filterContext) {
            var actionDescriptor = filterContext.ActionDescriptor;
            Log(Common.LogType.Activity, "Navigating", "User Name", String.Format("{0} {1} [2]", SessionData.user.FirstName, SessionData.user.LastName, SessionData.user.id), String.Format("Route: {0}/{1}/", actionDescriptor.ControllerDescriptor.ControllerName, actionDescriptor.ActionName));
        }

        private void LogData(Common.LogType logType, String message, params Object[] data) {
            try {
                _LogService.Log(logType, message, data);
            }
            catch {
                //Ignore
            }
        }

        private void LogData(Common.LogType logType, Exception exception, params Object[] data) {
            try {
                _LogService.Log(logType, exception.Message, exception.StackTrace, data);
            }
            catch {
                //Ignore
            }
        }

        #endregion public

        public static string RenderViewToString(ControllerContext context, string viewName) {
            return RenderViewToString(context, viewName, null);
        }

        public static string RenderViewToString(ControllerContext context, string viewName, object model) {
            if (string.IsNullOrEmpty(viewName))
                viewName = context.RouteData.GetRequiredString("action");

            ViewDataDictionary viewData = new ViewDataDictionary(model);

            using (StringWriter sw = new StringWriter()) {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                ViewContext viewContext = new ViewContext(context, viewResult.View, viewData, new TempDataDictionary(), sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}