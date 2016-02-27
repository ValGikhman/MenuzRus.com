using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MenuzRus.Controllers;
using MenuzRus.Models;
using Services;

namespace MenuzRus {

    public class ConfirmationsController : BaseController {
        private IUserService _userService;

        public ConfirmationsController(ISessionData sessionData, IUserService userService)
            : base(sessionData) {
            _userService = userService;
        }

        public ActionResult EmailConfirmation(String hash) {
            User model;
            try {
                model = new Services.User();
                model = _userService.GetUserByHash(hash);
                return View("EmailConfirmation", model);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            return null;
        }
    }
}