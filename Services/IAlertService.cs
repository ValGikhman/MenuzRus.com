using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using MenuzRus;

namespace Services {

    public interface IAlertService {

        #region public

        Alert GetAlert(Int32 id);

        Check GetAlertCheck(Int32 id);

        Item GetAlertItem(Int32 id);

        List<Alert> GetAlerts(Int32 userId);

        Int32 GetAlertsCount(Int32 userId);

        Int32 SaveAlert(Alert alert);

        #endregion public
    }
}