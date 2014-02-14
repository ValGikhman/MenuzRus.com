using System.Collections.Generic;
using System.Web.Mvc;
using MenuzRus.Models;
using Services;

namespace MenuzRus {

    public class LogoutController : Controller {

        public ActionResult Index() {
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}