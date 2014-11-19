using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using MenuzRus;

namespace Services {

    public class AlertService {

        #region public

        public Alert GetAlert(Int32 id) {
            menuzRusDataContext db = new menuzRusDataContext();
            return db.Alerts.FirstOrDefault(m => m.id == id);
        }

        public List<Alert> GetAlerts(Int32 UserId) {
            menuzRusDataContext db = new menuzRusDataContext();
            return db.Alerts.Where(m => m.UserId == UserId && m.Status == (Int32)Common.Status.Active).ToList();
        }

        public Int32 GetAlertsCount(Int32 UserId) {
            menuzRusDataContext db = new menuzRusDataContext();
            return GetAlerts(UserId).Count();
        }

        public Int32 SaveAlert(Alert alert) {
            Alert query;
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    query = db.Alerts.FirstOrDefault(m => m.id == alert.id);
                    if (query == default(Alert)) {
                        query = new Alert();
                    }

                    query.CheckId = alert.CheckId;
                    query.ItemId = alert.ItemId;
                    query.UserId = alert.UserId;
                    query.Type = alert.Type;
                    query.Status = alert.Status;
                    query.DateModified = DateTime.UtcNow;

                    if (alert.id == 0) {
                        db.Alerts.InsertOnSubmit(query);
                    }

                    db.SubmitChanges();
                }
            }
            catch (Exception ex) {
                SessionData.exeption = ex;
                return 0;
            }
            return query.id;
        }

        #endregion public
    }
}