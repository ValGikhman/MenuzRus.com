using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MenuzRus.Models;

namespace MenuzRus.Controllers {

    public class ErrorController : BaseController {

        public ErrorController(ISessionData sessionData)
            : base(sessionData) {
        }

        public ActionResult Index() {
            ExceptionModel model = new ExceptionModel();
            model.Exception = SessionData.exception;
            return View();
        }
    }
}