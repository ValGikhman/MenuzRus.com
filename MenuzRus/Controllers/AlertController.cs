using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using MenuzRus.Controllers;
using MenuzRus.Models;
using Services;

namespace MenuzRus {

    public class AlertController : BaseController {

        #region Private Fields

        private IAlertService _alertService;

        #endregion Private Fields

        #region Public Constructors

        public AlertController(ISessionData sessionData, IAlertService alertService)
            : base(sessionData) {
            _alertService = alertService;
        }

        #endregion Public Constructors

        #region Public Methods

        [HttpGet]
        public JsonResult GetAlerts() {
            List<Alert> items = new List<Alert>();
            try {
                if (SessionData.user != null) {
                    items = _alertService.GetAlerts(SessionData.user.id);
                    var retVal = new {
                        alerts = from var in items
                                 let item = _alertService.GetAlertItem(var.id)
                                 let url = Url.Content(String.Format("~/Images/Menus/{0}/Items/{1}?{2}", SessionData.customer.id.ToString(), item.ImageUrl, Guid.NewGuid().ToString("N")))
                                 let check = _alertService.GetAlertCheck(var.id)
                                 let table = _alertService.GetAlertTable(check.TableOrder.TableId)
                                 select new { id = var.id, CheckId = check.id, Item = item.Name, Url = url, Table = table.Name }
                    };
                    return new JsonResult() { Data = retVal, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            catch (Exception ex) {
                base.Log(ex);
            }

            return null;
        }

        [HttpGet]
        public Int32 GetAlertsCount() {
            Int32 retVal = 0;
            try {
                if (SessionData.user != null) {
                    retVal = _alertService.GetAlertsCount(SessionData.user.id);
                }
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            return retVal;
        }

        [HttpGet]
        public JsonResult ReadAlert(Int32 id) {
            Alert alert;
            Int32 alertsCount = 0;
            try {
                alert = _alertService.GetAlert(id);
                if (alert != null) {
                    alert.Status = (Int32)Common.Status.NotActive;
                    _alertService.SaveAlert(alert);
                }
                alertsCount = _alertService.GetAlertsCount(SessionData.user.id);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            return new JsonResult() { Data = alertsCount, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public void SaveAlert(Int32 checkMenuId, Boolean state) {
            Alert alert = new Alert();
            try {
                alert.Status = (Int32)Common.Status.Active;
                alert.CheckMenuId = checkMenuId;
                if (!state) {
                    alert.Status = (Int32)Common.Status.NotActive;
                }
                alert.Type = (Int32)Common.Alert.DishIsReady;
                _alertService.SaveAlert(alert);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
        }

        #endregion Public Methods
    }
}