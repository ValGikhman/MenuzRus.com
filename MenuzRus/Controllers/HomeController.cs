using System;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MenuzRus.Controllers {

    public class HomeController : BaseController {
        private ISettingsService _settingsService;

        public HomeController(ISessionData sessionData, ISettingsService settingsService)
            : base(sessionData) {
            _settingsService = settingsService;
        }

        // TODO: Not needed
        [HttpGet]
        public JsonResult GetPrinters() {
            try {
                var retVal = new {
                    printerPOS = _settingsService.GetSettings(SessionData.customer.id, Common.Settings.PrinterPOS),
                    printerKitchen = _settingsService.GetSettings(SessionData.customer.id, Common.Settings.PrinterKitchen),
                    printerPOSWidth = _settingsService.GetSettings(SessionData.customer.id, Common.Settings.PrinterPOSWidth),
                    printerKitchenWidth = _settingsService.GetSettings(SessionData.customer.id, Common.Settings.PrinterKitchenWidth)
                };
                return new JsonResult() { Data = retVal, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex) {
                throw;
            }
            finally {
            }
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
    }
}