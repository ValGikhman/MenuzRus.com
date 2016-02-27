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
        private IUserService _userService;

        public UsersController(ISessionData sessionData, IUserService userService)
            : base(sessionData) {
            _userService = userService;
        }

        [CheckUserSession]
        public ActionResult Index(Int32 id) {
            UsersModel model = new UsersModel();
            model.Users = _userService.GetUsers(id);
            return View(model);
        }
    }
}