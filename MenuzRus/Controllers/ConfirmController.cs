using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MenuzRus.Controllers;
using MenuzRus.Models;
using Services;

namespace MenuzRus {

    public class ConfirmController : BaseController {

        public ActionResult ByHash(String id) {
            User model;
            ConfirmationService service;
            try {
                service = new ConfirmationService();
                model = service.Confirm(id);
                if (model != null) {
                    base.Log(Common.LogType.Activity, "Email confirmation successful", "User", String.Format("{0} {1}, phone#{2}, mobile#{3}", model.FirstName, model.LastName, model.WorkPhone, model.MobilePhone));
                }
                else {
                    base.Log(Common.LogType.Activity, "Email confirmation failed", "Hash", id);
                }
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            return RedirectToAction("EmailConfirmation", "Confirmations", new { hash = id });
        }
    }
}