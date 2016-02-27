﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MenuzRus {

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CheckUserSessionAttribute : ActionFilterAttribute {
        private SessionData _sessionData;

        private SessionData SessionData {
            set {
                _sessionData = value;
            }
            get {
                return _sessionData;
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            HttpSessionStateBase session = filterContext.HttpContext.Session;

            Services.User user = (Services.User)session[Constants.SESSION_USER];
            if (user != null) {
                return;
            }

            String urlFrom = String.Empty;
            UrlHelper url;

            //send them off to the login page
            url = new UrlHelper(filterContext.RequestContext);
            urlFrom = filterContext.Controller.ControllerContext.RequestContext.HttpContext.Request.RawUrl;
            if (!String.IsNullOrEmpty(urlFrom)) {
                urlFrom = String.Format("?{0}", urlFrom);
            }
            var loginUrl = url.Content(String.Format("~/LogIn{0}", urlFrom));
            session.RemoveAll();
            session.Clear();
            session.Abandon();

            filterContext.HttpContext.Response.StatusCode = 403;
            filterContext.HttpContext.Response.Redirect(loginUrl, false);
            filterContext.Result = new EmptyResult();
        }
    }
}