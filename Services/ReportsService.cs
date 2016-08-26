using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace Services {

    public class ReportsService : BaseService, IReportsService {

        #region Sales Report

        public List<SalesRecord> SalesDataSet(DateTime dateFrom, DateTime dateTo) {
            try {
                menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
                var retVal = (
                      from tor in db.TableOrders
                      join c in db.Checks on tor.id equals c.TableOrderId
                      join u in db.Users on c.UserId equals u.id
                      let tax = c.Tax.HasValue ? c.Tax.Value : 0
                      let price = c.Price.HasValue ? c.Price.Value : 0
                      let adjustment = c.Adjustment.HasValue ? c.Adjustment.Value : 0
                      where
                        tor.Status == 3 &&
                        tor.DateModified >= dateFrom && tor.DateModified <= dateTo
                      orderby
                        tor.DateModified descending
                      select new SalesRecord() {
                          Check = String.Format("{0}{1}", "#", c.id),
                          Price = price,
                          Tax = tax,
                          Adjustment = c.Adjustment.HasValue ? c.Adjustment.Value : 0,
                          Total = price + tax + adjustment,
                          DateModified = tor.DateModified.ToShortDateString()
                      }
                      ).ToList();
                return retVal;
            }
            catch (Exception ex) {
                return null;
            }
        }

        public class SalesRecord {
            public Decimal Adjustment { get; set; }
            public String Check { get; set; }
            public String DateModified { get; set; }
            public Decimal Price { get; set; }
            public Decimal Tax { get; set; }
            public Decimal Total { get; set; }
        }

        #endregion Sales Report
    }
}