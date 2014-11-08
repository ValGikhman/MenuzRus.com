using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using MenuzRus.Models;
using Newtonsoft.Json;
using Services;

namespace MenuzRus.Controllers {

    public class KitchenOrderController : BaseController {

        [CheckUserSession]
        public ActionResult Index() {
            return View("Index", GetModel(DateTime.Now));
        }

        [HttpGet]
        public JsonResult LoadData(DateTime? date) {
            try {
                if (!date.HasValue) {
                    date = DateTime.Now;
                }

                KitchenOrderPrintModel model = GetModel(date.Value);
                var jsonData = new {
                    total = (Int32)Math.Ceiling((float)model.Printouts.Count),
                    records = model.Printouts.Count,
                    rows = (
                         from order in model.Printouts
                         select new {
                             id = order.id,
                             CheckId = order.CheckId,
                             DateCreated = order.DateCreated.ToLocalTime().ToString(),
                             Status = ((Common.PrintStatus)order.Status).ToString()
                         }
                    ).ToArray()
                };
                return new JsonResult() { Data = jsonData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }

            return new JsonResult() { Data = String.Empty, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        #region private

        private KitchenOrderPrintModel GetModel(DateTime date) {
            OrderService service = new OrderService();
            KitchenOrderPrintModel model = null;
            try {
                model = new KitchenOrderPrintModel();
                model.Printouts = service.GetPrintouts(date);
                return model;
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                service = null;
            }
            return null;
        }

        #endregion private
    }
}