using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using MenuzRus;

namespace Services {

    public class AlertService : BaseService, IAlertService {

        #region public

        public Alert GetAlert(Int32 id) {
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            return db.Alerts.FirstOrDefault(m => m.id == id);
        }

        public Check GetAlertCheck(Int32 id) {
            Check check;
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            check = (from alert in db.Alerts
                     join checkMenu in db.ChecksMenus on alert.CheckMenuId equals checkMenu.id
                     join chk in db.Checks on checkMenu.CheckId equals chk.id
                     where alert.id == id
                     select chk).FirstOrDefault();

            return check;
        }

        public Item GetAlertItem(Int32 id) {
            Item item;
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);

            item = (from alert in db.Alerts
                    join checkMenu in db.ChecksMenus on alert.CheckMenuId equals checkMenu.id
                    join it in db.Items on checkMenu.MenuId equals it.id
                    where alert.id == id
                    select it).FirstOrDefault();
            return item;
        }

        public List<Alert> GetAlerts(Int32 userId) {
            List<Alert> alerts;
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            alerts = (from check in db.Checks
                      join checkMenu in db.ChecksMenus on check.id equals checkMenu.CheckId
                      join alert in db.Alerts on checkMenu.id equals alert.CheckMenuId
                      where check.UserId == userId
                        && alert.Status == (Int32)CommonUnit.Status.Active
                      select alert).ToList();

            return alerts;
        }

        public Int32 GetAlertsCount(Int32 userId) {
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            return GetAlerts(userId).Count();
        }

        public Table GetAlertTable(Int32 tableId) {
            Table table;
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            table = (from t in db.Tables
                     where t.id == tableId
                     select t).FirstOrDefault();

            return table;
        }

        public Int32 SaveAlert(Alert alert) {
            Alert query;
            try {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    query = db.Alerts.FirstOrDefault(m => m.id == alert.id);
                    if (query == default(Alert)) {
                        query = new Alert();
                    }
                    query.CheckMenuId = alert.CheckMenuId;
                    query.Type = alert.Type;
                    query.Status = alert.Status;

                    if (alert.id == 0) {
                        db.Alerts.InsertOnSubmit(query);
                    }

                    db.SubmitChanges();
                }
            }
            catch (Exception ex) {
                return 0;
            }
            return query.id;
        }

        #endregion public
    }
}