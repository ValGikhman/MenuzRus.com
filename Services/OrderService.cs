using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public class OrderService {

        public Int32 AddNewTableOrder(Int32 tableId) {
            Int32 retVal = 0;
            TableOrder tableOrder = new TableOrder();
            if (tableId != 0) {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    tableOrder = db.TableOrders.FirstOrDefault(m => m.TableId == tableId && m.Status != (Int32)Common.TableOrderStatus.Closed); // Нет открытых столов
                    if (tableOrder == default(TableOrder)) {
                        tableOrder = new TableOrder();
                        tableOrder.TableId = tableId;
                        db.TableOrders.InsertOnSubmit(tableOrder);
                    }
                    tableOrder.Status = (Int32)Common.TableOrderStatus.Open;
                    tableOrder.DateModified = DateTime.UtcNow;
                    db.SubmitChanges();
                    retVal = tableOrder.id;
                }
            }
            return retVal;
        }

        public Boolean DeleteCheck(Int32 id) {
            IEnumerable<ChecksMenu> checkMenus;

            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    Check orderCheck = db.Checks.FirstOrDefault(m => m.id == id);
                    if (orderCheck != default(Check)) {
                        checkMenus = db.ChecksMenus.Where(m => m.CheckId == orderCheck.id);
                        if (checkMenus.Any()) {
                            foreach (ChecksMenu menu in checkMenus) {
                                IEnumerable<ChecksMenuProduct> products = db.ChecksMenuProducts.Where(m => m.CheckMenuId == menu.id);
                                if (products.Any()) {
                                    foreach (ChecksMenuProduct product in products) {
                                        IEnumerable<ChecksMenuProductItem> items = db.ChecksMenuProductItems.Where(m => m.ProductId == product.id);
                                        if (items.Any()) {
                                            db.ChecksMenuProductItems.DeleteAllOnSubmit(items);
                                        }
                                    }
                                }
                                db.ChecksMenuProducts.DeleteAllOnSubmit(products);
                            }
                            db.ChecksMenus.DeleteAllOnSubmit(checkMenus);
                        }
                        db.Checks.DeleteOnSubmit(orderCheck);
                    }
                    db.SubmitChanges();
                }
            }
            catch (Exception ex) {
                SessionData.exeption = ex;
                return false;
            }
            return true;
        }

        public Boolean DeleteMenu(Int32 id) {
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    ChecksMenu menuItem = db.ChecksMenus.FirstOrDefault(m => m.id == id);
                    if (menuItem != default(ChecksMenu)) {
                        db.ChecksMenus.DeleteOnSubmit(menuItem);
                        IEnumerable<ChecksMenuProduct> products = db.ChecksMenuProducts.Where(m => m.CheckMenuId == id);
                        if (products.Any()) {
                            db.ChecksMenuProducts.DeleteAllOnSubmit(products);
                            foreach (ChecksMenuProduct product in products) {
                                IEnumerable<ChecksMenuProductItem> items = db.ChecksMenuProductItems.Where(m => m.ProductId == product.id);
                                db.ChecksMenuProductItems.DeleteAllOnSubmit(items);
                            }
                        }
                    }
                    db.SubmitChanges();
                }
            }
            catch (Exception ex) {
                SessionData.exeption = ex;
                return false;
            }
            return true;
        }

        public Check GetCheck(Int32 checkId) {
            if (checkId != 0) {
                menuzRusDataContext db = new menuzRusDataContext();
                Check query = db.Checks.FirstOrDefault(m => m.id == checkId);
                if (query != default(Check)) {
                    return query;
                }
            }
            return null;
        }

        public String GetChecksIds(Int32 tableId) {
            return GetChecksIds(tableId, true);
        }

        public String GetChecksIds(Int32 tableId, Boolean showPaidChecks) {
            TableOrder tableOrder = GetTableOrder(tableId);
            if (tableOrder != default(TableOrder)) {
                List<Check> checks;
                if (!showPaidChecks) {
                    checks = tableOrder.Checks.Where(m => m.Status != (Int32)Common.CheckStatus.Paid).ToList();
                }
                else {
                    checks = tableOrder.Checks.ToList();
                }
                if (checks != null) {
                    return String.Join("|", checks.Select(m => String.Format("{0}:{1}", m.id, m.Status)).ToArray());
                }
            }
            return String.Empty;
        }

        public List<TableOrder> GetKitchenOrders() {
            List<TableOrder> retVal = new List<TableOrder>();
            menuzRusDataContext db = new menuzRusDataContext();
            return db.TableOrders.Where(m => m.Status != (Int32)Common.TableOrderStatus.Closed && m.Checks.Where(c => c.Status == (Int32)Common.CheckStatus.Ordered).Any()).OrderByDescending(m => m.DateModified).ToList();
        }

        public ChecksMenu GetMenuItem(Int32 id) {
            if (id != 0) {
                menuzRusDataContext db = new menuzRusDataContext();
                return db.ChecksMenus.FirstOrDefault(m => m.id == id);
            }
            return null;
        }

        public List<ChecksMenu> GetMenuItems(Int32 checkId) {
            if (checkId != 0) {
                menuzRusDataContext db = new menuzRusDataContext();
                return db.ChecksMenus.Where(m => m.CheckId == checkId).ToList();
            }
            return null;
        }

        public Printout GetPrintKitchenOrder(Int32 id) {
            menuzRusDataContext db = new menuzRusDataContext();
            return db.Printouts.FirstOrDefault(m => m.id == id);
        }

        public List<Printout> GetPrintouts(DateTime date) {
            menuzRusDataContext db = new menuzRusDataContext();
            return db.Printouts.Where(m => m.DateCreated >= date.ToUniversalTime()).ToList();
        }

        public List<ChecksMenuProduct> GetProducts(Int32 menuId) {
            if (menuId != 0) {
                menuzRusDataContext db = new menuzRusDataContext();
                return db.ChecksMenuProducts.Where(m => m.CheckMenuId == menuId).ToList();
            }
            return null;
        }

        public List<Printout> GetQueued4PrintKitchenOrders() {
            menuzRusDataContext db = new menuzRusDataContext();
            return db.Printouts.Where(m => m.Status == (Int32)Common.PrintStatus.Queued).ToList();
        }

        public Table GetTable(Int32 tableId) {
            if (tableId != 0) {
                menuzRusDataContext db = new menuzRusDataContext();
                return db.Tables.FirstOrDefault(m => m.id == tableId);
            }
            return null;
        }

        public TableOrder GetTableOrder(Int32 tableId) {
            TableOrder tableOrder = new TableOrder();
            if (tableId != 0) {
                menuzRusDataContext db = new menuzRusDataContext();
                tableOrder = db.TableOrders.FirstOrDefault(m => m.TableId == tableId && m.Status != (Int32)Common.TableOrderStatus.Closed);
                if (tableOrder == default(TableOrder)) {
                    tableOrder = db.TableOrders.Where(m => m.TableId == tableId && m.Status == (Int32)Common.TableOrderStatus.Closed).OrderByDescending(m => m.DateModified).FirstOrDefault();
                }
                return tableOrder;
            }
            return null;
        }

        public List<TableOrder> GetTableOrders(Int32 tableId) {
            List<TableOrder> retVal = new List<TableOrder>();
            if (tableId != 0) {
                menuzRusDataContext db = new menuzRusDataContext();
                return db.TableOrders.Where(m => m.TableId == tableId && m.Status != (Int32)Common.TableOrderStatus.Closed).OrderByDescending(m => m.DateModified).ToList();
            }
            return null;
        }

        public List<TableOrder> GetTableOrdersByFloorId(Int32 floorId) {
            List<TableOrder> retVal = new List<TableOrder>();
            menuzRusDataContext db = new menuzRusDataContext();
            if (floorId == 0) {
                retVal = (from table in db.Tables
                          join order in db.TableOrders on table.id equals order.TableId
                          where table.Status == (Int32)Common.Status.Active && order.Status != (Int32)Common.TableOrderStatus.Closed
                          select order).OrderByDescending(m => m.DateModified).ToList();
            }
            else {
                retVal = (from table in db.Tables
                          join order in db.TableOrders on table.id equals order.TableId
                          where table.FloorId == floorId && table.Status == (Int32)Common.Status.Active && order.Status != (Int32)Common.TableOrderStatus.Closed
                          select order).OrderByDescending(m => m.DateModified).ToList();
            }
            return retVal;
        }

        public void SaveItem(Int32 productId, Int32 knopaId, Common.ProductType type) {
            ChecksMenuProductItem query;
            if (productId != 0 && knopaId != 0) {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    if (type == Common.ProductType.Alternatives) {
                        query = db.ChecksMenuProductItems.FirstOrDefault(m => m.ProductId == productId);
                        if (query != default(ChecksMenuProductItem)) {
                            query.ItemId = knopaId;
                        }
                    }
                    else if (type == Common.ProductType.Addons) {
                        query = db.ChecksMenuProductItems.FirstOrDefault(m => m.ProductId == productId && m.ItemId == knopaId);
                        if (query == default(ChecksMenuProductItem)) {
                            query = new ChecksMenuProductItem();
                            query.ItemId = knopaId;
                            query.ProductId = productId;
                            db.ChecksMenuProductItems.InsertOnSubmit(query);
                        }
                        else {
                            db.ChecksMenuProductItems.DeleteOnSubmit(query);
                        }
                    }
                    db.SubmitChanges();
                }
            }
        }

        public ChecksMenu SaveMenuItem(Item menuItem, Int32 tableId, Int32 orderId) {
            ChecksMenu orderCheckMenu;
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    // New order
                    TableOrder tableOrder = db.TableOrders.FirstOrDefault(m => m.TableId == tableId && m.Status != (Int32)Common.TableOrderStatus.Closed);
                    if (tableOrder == default(TableOrder)) {
                        tableOrder = new TableOrder();
                        tableOrder.TableId = tableId;
                        tableOrder.Status = (Int32)Common.TableOrderStatus.Open;
                        tableOrder.DateModified = DateTime.UtcNow; ;
                        db.TableOrders.InsertOnSubmit(tableOrder);
                        db.SubmitChanges();
                    }

                    Check orderCheck = db.Checks.FirstOrDefault(m => m.id == orderId);
                    if (orderCheck == default(Check)) {
                        orderCheck = new Check();
                        orderCheck.TableOrderId = tableOrder.id;
                        orderCheck.Type = (Int32)Common.CheckType.Guest;
                        orderCheck.Status = (Int32)Common.CheckStatus.Active;
                        orderCheck.UserId = SessionData.user.id;
                        orderCheck.DateModified = DateTime.UtcNow;
                        db.Checks.InsertOnSubmit(orderCheck);
                        db.SubmitChanges();
                    }

                    orderCheckMenu = new ChecksMenu();
                    orderCheckMenu.CheckId = orderCheck.id;
                    orderCheckMenu.MenuId = menuItem.id;
                    orderCheckMenu.Status = (Int32)Common.MenuItemStatus.Active;
                    db.ChecksMenus.InsertOnSubmit(orderCheckMenu);
                    db.SubmitChanges();

                    foreach (ItemProduct itemProduct in menuItem.ItemProducts) {
                        ChecksMenuProduct product = new ChecksMenuProduct();
                        product.CheckMenuId = orderCheckMenu.id;
                        product.ItemId = itemProduct.id;
                        db.ChecksMenuProducts.InsertOnSubmit(product);
                        db.SubmitChanges();
                        if (itemProduct.ItemProductAssociations.Count() > 0) {
                            ChecksMenuProductItem item = new ChecksMenuProductItem();
                            item.ProductId = product.id;
                            item.ItemId = itemProduct.ItemProductAssociations[0].id;
                            db.ChecksMenuProductItems.InsertOnSubmit(item);
                            db.SubmitChanges();
                        }
                    }
                }
            }
            catch (Exception ex) {
                SessionData.exeption = ex;
                return null;
            }
            return orderCheckMenu;
        }

        public Boolean UpdateCheckStatus(Int32 checkId, Common.CheckStatus status) {
            Check query = new Check();
            Printout kitchenOrder;
            if (checkId != 0) {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    query = db.Checks.FirstOrDefault(m => m.id == checkId);
                    if (query != default(Check)) {
                        query.Status = (Int32)status;
                        query.DateModified = DateTime.UtcNow;
                        if (status == Common.CheckStatus.Ordered) {
                            kitchenOrder = db.Printouts.FirstOrDefault(m => m.CheckId == checkId);
                            if (kitchenOrder == default(Printout)) {
                                kitchenOrder = new Printout();
                            }
                            kitchenOrder.Status = (Int32)Common.PrintStatus.Queued;
                            kitchenOrder.Type = (Int32)Common.PrintType.KitchenOrder;
                            kitchenOrder.CheckId = checkId;
                            kitchenOrder.DateModified = DateTime.UtcNow;
                            if (kitchenOrder.id == 0) {
                                db.Printouts.InsertOnSubmit(kitchenOrder);
                            }
                        }
                        db.SubmitChanges();
                        return true;
                    }
                }
            }
            return false;
        }

        public Boolean UpdateCheckStatusPaid(Int32 checkId, Decimal price, Decimal tax, Decimal adjustment) {
            Check query = new Check();
            if (checkId != 0) {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    query = db.Checks.FirstOrDefault(m => m.id == checkId);
                    if (query != default(Check)) {
                        query.Status = (Int32)Common.CheckStatus.Paid;
                        query.Adjustment = adjustment;
                        query.Price = price;
                        query.Tax = tax;
                        db.SubmitChanges();
                        return true;
                    }
                }
            }
            return false;
        }

        public Boolean UpdateCheckType(Int32 checkId, Common.CheckType type) {
            Check query = new Check();
            if (checkId != 0) {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    query = db.Checks.FirstOrDefault(m => m.id == checkId);
                    if (query != default(Check)) {
                        query.Type = (Int32)type;
                        db.SubmitChanges();
                        return true;
                    }
                }
            }
            return false;
        }

        public Boolean UpdateKitchenOrderPrintStatus(Int32 id) {
            Printout query = new Printout();
            if (id != 0) {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    query = db.Printouts.FirstOrDefault(m => m.id == id);
                    if (query != default(Printout)) {
                        query.Status = (Int32)Common.PrintStatus.Printed;
                        query.DateModified = DateTime.UtcNow;
                        db.SubmitChanges();
                        return true;
                    }
                }
            }
            return false;
        }

        public void UpdateMenuItemStatus(Int32 id, Common.MenuItemStatus status) {
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    ChecksMenu menuItem = db.ChecksMenus.FirstOrDefault(m => m.id == id);
                    if (menuItem != default(ChecksMenu)) {
                        menuItem.Status = (Int32)status;
                        db.SubmitChanges();
                    }
                }
            }
            catch (Exception ex) {
            }
            finally {
            }
        }

        public Boolean UpdateTableStatus(Int32 tableOrderId, Common.TableOrderStatus status) {
            TableOrder query = new TableOrder();
            if (tableOrderId != 0) {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    query = db.TableOrders.FirstOrDefault(m => m.id == tableOrderId);
                    if (query != default(TableOrder)) {
                        query.Status = (Int32)status;
                        query.DateModified = DateTime.UtcNow;
                        db.SubmitChanges();
                        return true;
                    }
                }
            }

            return false;
        }
    }
}