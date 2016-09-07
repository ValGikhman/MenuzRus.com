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

        #region Inventory

        List<Services.ReportsService.InventoryRecord> InventoryDataSet(DateTime dateFrom, DateTime dateTo);

        #endregion Inventory
    }
}