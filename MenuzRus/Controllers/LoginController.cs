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
                Session["IsLoggedIn"] = false;
                LoginModel model = new LoginModel();
                return View();
            }
            catch (Exception ex) {
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
                Session["IsLoggedIn"] = true;
                return RedirectToAction("Index", "Floor");
            }
            catch (Exception ex) {
            }
            finally {
                service = null;
            }
            return null;
        }
    }
}