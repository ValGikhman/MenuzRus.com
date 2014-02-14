using System.Collections.Generic;
using System.Web.Mvc;
using MenuzRus.Controllers;
using MenuzRus.Models;
using Services;

namespace MenuzRus {

    public class LoginController : BaseController {

        public ActionResult Index() {
            Session["IsLoggedIn"] = false;
            LoginModel model = new LoginModel();
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginModel model) {
            Services service = new Services();
            try {
                Contact contact = service.Login(model.Email, model.Password);
                if (contact == null) {
                    return View();
                }
                Session["IsLoggedIn"] = true;
                return RedirectToAction("Style", "YourMenu");
            }
            catch {
                return View();
            }
        }
    }
}