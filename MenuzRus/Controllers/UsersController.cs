using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using MenuzRus.Models;
using Services;

namespace MenuzRus.Controllers {

    public class UsersController : BaseController {

        public ActionResult Index() {
            return View();
        }

        [HttpPost]
        public JsonResult LoadUsersData(Int32 id) {
            UserService service = new UserService();
            var result = new {
                rows = (from user in service.GetUsers(id)
                        select new {
                            id = user.id,
                            firstname = user.FirstName,
                            lastname = user.LastName
                        }).ToArray()
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}