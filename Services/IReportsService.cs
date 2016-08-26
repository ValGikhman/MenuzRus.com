using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace Services {

    public interface IReportsService {

        #region Sales

        List<Services.ReportsService.SalesRecord> SalesDataSet(DateTime dateFrom, DateTime dateTo);

        #endregion Sales
    }
}