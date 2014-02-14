using System.Web.Mvc;

namespace MenuzRus {

    public class HomeController : Controller {

        public ActionResult Index() {
            return RedirectToAction("Index", "Login");
        }
    }
}