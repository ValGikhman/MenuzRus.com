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
                return View();
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
            try {
                User user = service.Login(model.Email, model.Password);
                if (user == null) {
                    return View();
                }
                SessionData.sessionId = Session.SessionID;
                base.Log(Common.LogType.LogIn, "Logged in", "User", String.Format("{0} {1}", SessionData.user.FirstName, SessionData.user.LastName));
                return RedirectToAction("Tables", "Order");
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