﻿using System;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MenuzRus.Models;

namespace MenuzRus.Controllers {

    public class HomeController : BaseController {

        #region Private Fields

        private ISettingsService _settingsService;

        #endregion Private Fields

        #region Public Constructors

        public HomeController(ISessionData sessionData, ISettingsService settingsService)
            : base(sessionData) {
            _settingsService = settingsService;
        }

        #endregion Public Constructors

        #region Public Methods

        public ActionResult Dashboard() {
            HomeModel model = new HomeModel();
            return View("Dashboard", model);
        }

        public ActionResult Index() {
            return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public JsonResult SendPrinters(String model) {
            try {
                if (!String.IsNullOrEmpty(model)) {
                    JavaScriptSerializer objJavascript = new JavaScriptSerializer();
                    SessionData.printers = objJavascript.Deserialize<String[]>(model);
                    SessionData.printerKitchenWidth = Convert.ToInt32(_settingsService.GetSettings(SessionData.customer.id, Common.Settings.PrinterKitchenWidth));
                    SessionData.printerPOSWidth = Convert.ToInt32(_settingsService.GetSettings(SessionData.customer.id, Common.Settings.PrinterPOSWidth));
                    var retVal = new {
                        printerPOS = _settingsService.GetSettings(SessionData.customer.id, Common.Settings.PrinterPOS),
                        printerKitchen = _settingsService.GetSettings(SessionData.customer.id, Common.Settings.PrinterKitchen),
                        printerPOSWidth = SessionData.printerPOSWidth,
                        printerKitchenWidth = SessionData.printerKitchenWidth
                    };
                    return new JsonResult() { Data = retVal, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                return new JsonResult() { Data = { }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex) {
                throw;
            }
            finally {
            }
        }

        #endregion Public Methods
    }
}