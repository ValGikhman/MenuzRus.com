using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MenuzRus.Controllers;
using MenuzRus.Models;
using Services;

namespace MenuzRus {

    public class LogoutController : BaseController {

        [CheckUserSession]
        public ActionResult Index() {
            base.Log(Common.LogType.LogOut, "Logging out", "User", String.Format("{0} {1}", SessionData.user.FirstName, SessionData.user.LastName));
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}