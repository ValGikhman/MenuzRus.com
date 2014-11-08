﻿using System;
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

        [HttpGet]
        public JsonResult GetAlerts() {
            AlertService service = new AlertService();
            List<Alert> items = new List<Alert>();
            try {
                items = service.GetAlerts(SessionData.user.id);
                var retVal = new {
                    alerts = from var in items
                             let item = var.Items.FirstOrDefault(m => m.id == var.ItemId)
                             select new { id = var.id, Userid = var.UserId, CheckId = var.CheckId, ItemId = var.ItemId, Item = item.Name, Url = item.ImageUrl }
                };
                return new JsonResult() { Data = retVal, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            return null;
        }

        [HttpGet]
        public Int32 GetAlertsCount() {
            AlertService service = new AlertService();
            Int32 retVal = 0;
            try {
                retVal = service.GetAlertsCount(SessionData.user.id);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            return retVal;
        }

        [HttpGet]
        public JsonResult ReadAlert(Int32 id) {
            AlertService service = new AlertService();
            Alert alert;
            Int32 alertsCount = 0;
            try {
                alert = service.GetAlert(id);
                if (alert != null) {
                    alert.Status = (Int32)Common.Status.NotActive;
                    service.SaveAlert(alert);
                }
                alertsCount = service.GetAlertsCount(SessionData.user.id);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            return new JsonResult() { Data = alertsCount, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public void SaveAlert(Int32 userId, Int32 checkId, Int32 itemId, Boolean state) {
            Alert alert = new Alert();
            AlertService service = new AlertService();
            try {
                alert.UserId = userId;
                alert.CheckId = checkId;
                alert.ItemId = itemId;
                alert.Status = (Int32)Common.Status.Active;
                if (!state) {
                    alert.Status = (Int32)Common.Status.NotActive;
                }
                alert.Type = (Int32)Common.Alert.DishIsReady;
                service.SaveAlert(alert);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
        }
    }
}