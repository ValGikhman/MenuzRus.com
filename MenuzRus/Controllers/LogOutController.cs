using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MenuzRus.Controllers;
using MenuzRus.Models;
using Services;

namespace MenuzRus {

    public class LogoutController : BaseController {

        #region Public Constructors

        public LogoutController(ISessionData sessionData)
            : base(sessionData) {
        }

        #endregion Public Constructors

        #region Public Methods

        [CheckUserSession]
        public ActionResult Index() {
            base.Log(CommonUnit.LogType.LogOut);
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }

        #endregion Public Methods
    }
}