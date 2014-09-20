using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MenuzRus.Controllers;
using MenuzRus.Models;
using Services;

namespace MenuzRus {

    public class ConfirmationsController : BaseController {

        public ActionResult EmailConfirmation(String hash) {
            UserService service;
            User model;
            try {
                model = new Services.User();
                service = new UserService();
                model = service.GetUserByHash(hash);
                return View("EmailConfirmation", model);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            return null;
        }
    }
}