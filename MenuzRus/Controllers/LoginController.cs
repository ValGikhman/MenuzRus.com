using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MenuzRus.Controllers;
using MenuzRus.Models;
using Services;

namespace MenuzRus {

    public class LoginController : BaseController {

        public ActionResult Index() {
            try {
                LoginModel model = new LoginModel();
                model.Success = true;
                return View(model);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
            return null;
        }

        [HttpPost]
        public ActionResult Index(LoginModel model) {
            LoginService service = new LoginService();
            String pathToNavigate = "~/Order/Tables";

            try {
                User user = service.Login(model.Email, model.Password);
                if (user == null) {
                    model.Success = false;
                    return View(model);
                }

                SessionData.sessionId = Session.SessionID;
                model.Success = true;

                if (Request.UrlReferrer != null && !String.IsNullOrEmpty(Request.UrlReferrer.Query)) {
                    pathToNavigate = Request.UrlReferrer.Query.Replace("?", String.Empty);
                }

                base.Log(Common.LogType.LogIn, "Logged in", "User", String.Format("{0} {1}", SessionData.user.FirstName, SessionData.user.LastName));
                return Redirect(pathToNavigate);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                service = null;
            }
            return null;
        }
    }
}