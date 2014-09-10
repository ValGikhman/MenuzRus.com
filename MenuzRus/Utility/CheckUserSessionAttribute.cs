using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MenuzRus {

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CheckUserSessionAttribute : ActionFilterAttribute {
        //public static CheckSessionDelegate CheckSessionAlive;

        //public delegate bool CheckSessionDelegate(HttpSessionStateBase session);

        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            //if ((CheckSessionAlive == null) || (CheckSessionAlive(session)))
            if (SessionData.user != null)
                return;

            //send them off to the login page
            var url = new UrlHelper(filterContext.RequestContext);
            var loginUrl = url.Content("~/LogIn");
            session.RemoveAll();
            session.Clear();
            session.Abandon();

            filterContext.HttpContext.Response.StatusCode = 403;
            filterContext.HttpContext.Response.Redirect(loginUrl, false);
            filterContext.Result = new EmptyResult();
        }
    }
}