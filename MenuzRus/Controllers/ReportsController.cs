using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Extensions;
using MenuzRus.Models;
using Newtonsoft.Json;
using Services;

namespace MenuzRus.Controllers {

    public class ReportsController : BaseController {
        private IReportsService _reportsService;

        public ReportsController(ISessionData sessionData, IReportsService reportsService)
            : base(sessionData) {
            _reportsService = reportsService;
        }

        [HttpGet]
        public JsonResult LoadSalesData(DateTime dateFrom, DateTime dateTo) {
            try {
                SalesReportModel model = GetSalesModel(dateFrom, dateTo);
                var jsonData = new {
                    total = (Int32)Math.Ceiling((float)model.Records.Count),
                    records = model.Records.Count,
                    rows = model.Records.ToArray(),
                    graph = (
                        from record in model.Records
                        let date = Convert.ToDateTime(record.DateModified).Date
                        let total = record.Total
                        group new { DateModified = date, Total = total } by date into g
                        select new {
                            Date = g.Key.ToShortDateString(),
                            Sale = g.Sum(p => p.Total).ToString()
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

        [HttpGet]
        public JsonResult LoadInventoryData(DateTime dateFrom, DateTime dateTo) {
            try {
                InventoryReportModel model = GetInventoryModel(dateFrom, dateTo);
                var jsonData = new {
                    total = (Int32)Math.Ceiling((float)model.Records.Count),
                    records = model.Records.Count,
                    rows = model.Records.ToArray(),
                    headers = model.Records.OrderBy(m => m.TotalTotal).Select(m => m.Item).Distinct().ToArray(),
                    graph = model.Records.GroupBy(n => n.Date).Select(g =>
                            new {
                                Date = g.Key,
                                Sales = g.Select(p => p.TotalTotal)
                            }
                ).ToList()
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

        #region Reports

        [CheckUserSession]
        public ActionResult Sales() {
            return View("Sales", GetSalesModel(DateTime.Now.AddDays(-1), DateTime.Now));
        }

        [CheckUserSession]
        public ActionResult Inventory() {
            return View("Inventory", GetInventoryModel(DateTime.Now.AddDays(-1), DateTime.Now));
        }

        #endregion Reports

        #region private

        private SalesReportModel GetSalesModel(DateTime dateFrom, DateTime dateTo) {
            SalesReportModel model = null;

            try {
                model = new SalesReportModel();
                model.From = dateFrom;
                model.To = dateTo;
                model.Records = _reportsService.SalesDataSet(dateFrom, dateTo);
                return model;
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
            return null;
        }

        private InventoryReportModel GetInventoryModel(DateTime dateFrom, DateTime dateTo) {
            InventoryReportModel model = null;

            try {
                model = new InventoryReportModel();
                model.From = dateFrom;
                model.To = dateTo;
                model.Records = _reportsService.InventoryDataSet(dateFrom, dateTo);
                return model;
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
            return null;
        }

        #endregion private
    }
}