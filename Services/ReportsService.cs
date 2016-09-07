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
                      where
                        tor.Status == 3 &&
                        tor.DateModified.Date >= dateFrom.Date && tor.DateModified.Date <= dateTo.Date
                      orderby
                        tor.DateModified descending
                      select new SalesRecord() {
                          Check = String.Format("{0}{1}", "#", c.id),
                          Price = c.Price,
                          Tax = c.Tax,
                          Adjustment = c.Adjustment,
                          Total = c.Price + c.Tax + c.Adjustment,
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
            public Decimal? Adjustment { get; set; }
            public String Check { get; set; }
            public String DateModified { get; set; }
            public Decimal? Price { get; set; }
            public Decimal? Tax { get; set; }
            public Decimal? Total { get; set; }
        }

        #endregion Sales Report

        #region Inventory Report

        public List<Services.ReportsService.InventoryRecord> InventoryDataSet(DateTime dateFrom, DateTime dateTo) {
            try {
                String aaaa = EnumHelper<Common.UOM>.Parse(2.ToString()).ToString();

                menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
                List<Services.ReportsService.InventoryRecord> retVal = null;
                retVal = (from ib in db.InventoryBalances
                          where ib.DateCreated.Date >= dateFrom.Date && ib.DateCreated.Date <= dateTo.Date
                          let item = ib.Items.Where(m => m.id == ib.AssociatedItemId).FirstOrDefault()
                          let price = (Decimal)item.ItemPrices.OrderByDescending(m => m.DateCreated).Take(1).Select(m => m.Price).FirstOrDefault()
                          select new InventoryRecord {
                              Date = ib.DateCreated.ToShortDateString(),
                              Item = item.Name,
                              Price = price,
                              UOM = item.UOM.ToString(),
                              Start = ib.Start,
                              StartTotal = price * ib.Start,
                              In = ib.In,
                              InTotal = price * ib.In,
                              Out = ib.Out,
                              OutTotal = price * ib.Out,
                              Total = ib.Saldo,
                              TotalTotal = price * ib.Saldo
                          }).ToList();

                // LINQ does not understand EnumHelper<Common.UOM>.Parse - need to do it manually
                foreach (Services.ReportsService.InventoryRecord var in retVal) {
                    var.Item = String.Format("{0} ({1})", var.Item, EnumHelper<Common.UOM>.Parse(var.UOM).ToString());
                }

                return retVal;
            }
            catch (Exception ex) {
                return null;
            }
        }

        public class InventoryRecord {
            public String Date { get; set; }
            public String Item { get; set; }
            public String UOM { get; set; }
            public Decimal Price { get; set; }
            public Decimal Start { get; set; }
            public Decimal StartTotal { get; set; }
            public Decimal In { get; set; }
            public Decimal InTotal { get; set; }
            public Decimal Out { get; set; }
            public Decimal OutTotal { get; set; }
            public Decimal? Total { get; set; }
            public Decimal? TotalTotal { get; set; }
        }

        #endregion Inventory Report
    }
}