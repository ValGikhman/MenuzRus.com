using System.Web.Mvc;
using MenuzRus.Models;

namespace MenuzRus {

    public class ContactMeController : Controller {

        public ActionResult Index() {
            ContactMeModel model = new ContactMeModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(ContactMeModel model) {
            if (Common.SendEmail(model.Email, "Contact From MenuzRus", model.Message))
                return View();
            else
                return View();
        }
    }
}