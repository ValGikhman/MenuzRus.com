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

        [CheckUserSession]
        public ActionResult Index(Int32 id) {
            UserService service = new UserService();
            UsersModel model = new UsersModel();
            model.Users = service.GetUsers(id);
            return View(model);
        }
    }
}