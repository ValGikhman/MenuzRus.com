using System;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MenuzRus {

    public class HomeController : Controller {

        [HttpGet]
        public JsonResult GetPrinters() {
            SettingsService service = new SettingsService();
            try {
                var retVal = new {
                    printerPOS = service.GetSettings(SessionData.customer.id, Common.Settings.PrinterPOS),
                    printerKitchen = service.GetSettings(SessionData.customer.id, Common.Settings.PrinterKitchen),
                    printerPOSWidth = service.GetSettings(SessionData.customer.id, Common.Settings.PrinterPOSWidth),
                    printerKitchenWidth = service.GetSettings(SessionData.customer.id, Common.Settings.PrinterKitchenWidth)
                };
                return new JsonResult() { Data = retVal, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex) {
                throw;
            }
            finally {
                service = null;
            }
        }

        public ActionResult Index() {
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public void SendPrinters(String model) {
            SettingsService service = new SettingsService();
            try {
                if (!String.IsNullOrEmpty(model)) {
                    JavaScriptSerializer objJavascript = new JavaScriptSerializer();
                    SessionData.printers = objJavascript.Deserialize<String[]>(model);
                    SessionData.printerKitchenWidth = (2 * Convert.ToInt32(service.GetSettings(SessionData.customer.id, Common.Settings.PrinterKitchenWidth)) - 5);
                    SessionData.printerPOSWidth = (2 * Convert.ToInt32(service.GetSettings(SessionData.customer.id, Common.Settings.PrinterPOSWidth)) - 5);
                }
            }
            catch (Exception ex) {
                throw;
            }
            finally {
                service = null;
            }
        }
    }
}