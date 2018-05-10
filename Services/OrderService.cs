using System;
using System.Collections.Generic;
using System.Linq;
using MenuzRus;
using Services;

namespace MenuzRus {

    public class OrderService : BaseService, IOrderService {

        #region Public Methods

        public Int32 AddNewTableOrder(Int32 tableId) {
            Int32 retVal = 0;
            TableOrder tableOrder = new TableOrder();
            if (tableId != 0) {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    tableOrder = db.TableOrders.FirstOrDefault(m => m.TableId == tableId && m.Status != (Int32)CommonUnit.TableOrderStatus.Closed); // Нет открытых столов
                    if (tableOrder == default(TableOrder)) {
                        tableOrder = new TableOrder();
                        tableOrder.TableId = tableId;
                        db.TableOrders.InsertOnSubmit(tableOrder);
                    }
                    tableOrder.Status = (Int32)CommonUnit.TableOrderStatus.Open;
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
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
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
                return false;
            }
            return true;
        }

        public Boolean DeleteMenu(Int32 id) {
            ItemService _itemService = new ItemService();
            InventoryService _inventoryService = new InventoryService();

            try {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
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
                return false;
            }
            return true;
        }

        public Boolean DeletePayment(Int32 id) {
            PaymentCC cards = null;
            Payment payments = null;
            try {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    cards = db.PaymentCCs.Where(m => m.PaymentId == id).FirstOrDefault();
                    if (cards != null) {
                        db.PaymentCCs.DeleteOnSubmit(cards);
                    }

                    payments = db.Payments.Where(m => m.id == id).FirstOrDefault();
                    if (payments != null) {
                        db.Payments.DeleteOnSubmit(payments);
                    }

                    db.SubmitChanges();
                }
            }
            catch (Exception ex) {
                return false;
            }
            return true;
        }

        public Check GetCheck(Int32 checkId) {
            if (checkId != 0) {
                menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
                Check query = db.Checks.FirstOrDefault(m => m.id == checkId);
                if (query != default(Check)) {
                    return query;
                }
            }
            return null;
        }

        public List<TableOrder> GetKitchenOrders() {
            List<TableOrder> retVal = new List<TableOrder>();
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            return db.TableOrders.Where(m => m.Status != (Int32)CommonUnit.TableOrderStatus.Closed && m.Checks.Where(c => c.Status == (Int32)CommonUnit.CheckStatus.Ordered).Any()).OrderByDescending(m => m.DateModified).ToList();
        }

        public ChecksMenu GetMenuItem(Int32 id) {
            if (id != 0) {
                menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
                return db.ChecksMenus.FirstOrDefault(m => m.id == id);
            }
            return null;
        }

        public List<ChecksMenu> GetMenuItems(Int32 checkId) {
            if (checkId != 0) {
                menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
                return db.ChecksMenus.Where(m => m.CheckId == checkId).ToList();
            }
            return null;
        }

        public List<Payment> GetPayments(Int32 checkId) {
            if (checkId != 0) {
                menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
                List<Payment> query = db.Payments.Where(m => m.CheckId == checkId).OrderByDescending(m => m.DateCreated).ToList();

                if (query != default(List<Payment>)) {
                    return query;
                }
            }
            return null;
        }

        public Printout GetPrintKitchenOrder(Int32 id) {
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            return db.Printouts.FirstOrDefault(m => m.id == id);
        }

        public List<Printout> GetPrintouts(DateTime dateFrom, DateTime dateTo) {
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            return db.Printouts.Where(m => m.DateCreated >= dateFrom.ToUniversalTime() && m.DateCreated <= dateTo.ToUniversalTime()).ToList();
        }

        public List<ChecksMenuProduct> GetProducts(Int32 menuId) {
            if (menuId != 0) {
                menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
                return db.ChecksMenuProducts.Where(m => m.CheckMenuId == menuId).ToList();
            }
            return null;
        }

        public List<Printout> GetQueued4PrintKitchenOrders() {
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            return db.Printouts.Where(m => m.Status == (Int32)CommonUnit.PrintStatus.Queued).ToList();
        }

        public Table GetTable(Int32 tableId) {
            if (tableId != 0) {
                menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
                return db.Tables.FirstOrDefault(m => m.id == tableId);
            }
            return null;
        }

        public Tuple<String, Int32, String> GetTableAttributes(Int32 tableId, Boolean showPaidChecks) {
            TableOrder tableOrder = GetTableOrder(tableId);

            String checksIds = String.Empty;
            Int32 orderStatus = (Int32)CommonUnit.TableOrderStatus.Closed;
            String orderDate = String.Format("{0:M/d/yyyy HH:mm:ss}", DateTime.Now);

            // Grabbing checks
            if (tableOrder != default(TableOrder)) {
                List<Check> checks;
                if (!showPaidChecks) {
                    checks = tableOrder.Checks.Where(m => m.Status != (Int32)CommonUnit.CheckStatus.Paid).ToList();
                }
                else {
                    checks = tableOrder.Checks.ToList();
                }
                if (checks != null) {
                    checksIds = String.Join("|", checks.Select(m => String.Format("{0}:{1}", m.id, m.Status)).ToArray());
                }

                // Grab Status
                orderStatus = tableOrder.Status;
                // Grab Date
                orderDate = String.Format("{0:M/d/yyyy HH:mm:ss}", tableOrder.DateModified);
            }

            return new Tuple<String, Int32, String>(checksIds, orderStatus, orderDate);
        }

        public TableOrder GetTableOrder(Int32 tableId) {
            TableOrder tableOrder = new TableOrder();
            if (tableId != 0) {
                menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
                tableOrder = db.TableOrders.FirstOrDefault(m => m.TableId == tableId && m.Status != (Int32)CommonUnit.TableOrderStatus.Closed);
                if (tableOrder == default(TableOrder)) {
                    tableOrder = db.TableOrders.Where(m => m.TableId == tableId && m.Status == (Int32)CommonUnit.TableOrderStatus.Closed).OrderByDescending(m => m.DateModified).FirstOrDefault();
                }
                return tableOrder;
            }
            return null;
        }

        public List<TableOrder> GetTableOrders(Int32 tableId) {
            List<TableOrder> retVal = new List<TableOrder>();
            if (tableId != 0) {
                menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
                return db.TableOrders.Where(m => m.TableId == tableId && m.Status != (Int32)CommonUnit.TableOrderStatus.Closed).OrderByDescending(m => m.DateModified).ToList();
            }
            return null;
        }

        public List<TableOrder> GetTableOrdersByFloorId(Int32 floorId, Int32 customerId) {
            List<TableOrder> retVal = new List<TableOrder>();
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            if (floorId == 0) {
                retVal = (from table in db.Tables
                          join floor in db.Floors on table.FloorId equals floor.id
                          join order in db.TableOrders on table.id equals order.TableId
                          where floor.CustomerId == customerId && table.Status == (Int32)CommonUnit.Status.Active && order.Status != (Int32)CommonUnit.TableOrderStatus.Closed
                          select order).OrderByDescending(m => m.DateModified).ToList();
            }
            else {
                retVal = (from table in db.Tables
                          join order in db.TableOrders on table.id equals order.TableId
                          where table.FloorId == floorId && table.Status == (Int32)CommonUnit.Status.Active && order.Status != (Int32)CommonUnit.TableOrderStatus.Closed
                          select order).OrderByDescending(m => m.DateModified).ToList();
            }
            return retVal;
        }

        public Decimal LatestInventory(Int32 customerId) {
            Decimal? saldo;
            DateTime maxDate;

            using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                maxDate = db.InventoryBalances.Max(m => m.Date);
                saldo = db.InventoryBalances.Where(m => m.Date == maxDate.Date).Select(m => m.Saldo).FirstOrDefault();
            }
            return saldo.HasValue ? saldo.Value : 0;
        }

        public Decimal LatestSale(Int32 customerId) {
            Decimal? price;

            using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                price = db.Checks.Where(m => m.DateCreated.Date == DateTime.Now.Date).Sum(m => m.Price + m.Tax);
            }
            return price.HasValue ? price.Value : 0;
        }

        public void SaveItem(Int32 productId, Int32 knopaId, CommonUnit.ProductType type) {
            ItemService _itemService = new ItemService();
            InventoryService _inventoryService = new InventoryService();

            ChecksMenuProductItem query = null;
            if (productId != 0 && knopaId != 0) {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    if (type == CommonUnit.ProductType.Alternatives) {
                        query = db.ChecksMenuProductItems.FirstOrDefault(m => m.ProductId == productId);
                        if (query != default(ChecksMenuProductItem)) {
                            query.ItemId = knopaId;
                        }
                    }
                    else if (type == CommonUnit.ProductType.Addons) {
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

        public ChecksMenu SaveMenuItem(Item menuItem, Int32 tableId, Int32 orderId, Int32 userId) {
            ItemService _itemService = new ItemService();
            InventoryService _inventoryService = new InventoryService();

            ChecksMenu orderCheckMenu;
            try {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    // New order
                    TableOrder tableOrder = db.TableOrders.FirstOrDefault(m => m.TableId == tableId && m.Status != (Int32)CommonUnit.TableOrderStatus.Closed);
                    if (tableOrder == default(TableOrder)) {
                        tableOrder = new TableOrder();
                        tableOrder.TableId = tableId;
                        tableOrder.Status = (Int32)CommonUnit.TableOrderStatus.Open;
                        db.TableOrders.InsertOnSubmit(tableOrder);
                        db.SubmitChanges();
                    }

                    Check orderCheck = db.Checks.FirstOrDefault(m => m.id == orderId);
                    if (orderCheck == default(Check)) {
                        orderCheck = new Check();
                        orderCheck.TableOrderId = tableOrder.id;
                        orderCheck.Type = (Int32)CommonUnit.CheckType.Guest;
                        orderCheck.Status = (Int32)CommonUnit.CheckStatus.Active;
                        orderCheck.UserId = userId;
                        db.Checks.InsertOnSubmit(orderCheck);
                        db.SubmitChanges();
                    }

                    orderCheckMenu = new ChecksMenu();
                    orderCheckMenu.CheckId = orderCheck.id;
                    orderCheckMenu.MenuId = menuItem.id;
                    orderCheckMenu.Status = (Int32)CommonUnit.MenuItemStatus.Active;
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
                return null;
            }
            return orderCheckMenu;
        }

        public Int32 SavePayment(Payment payment, PaymentCC paymentCC, Int32 UserId) {
            Payment queryPayment;
            PaymentCC queryPaymentCC;
            try {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    queryPayment = new Payment();
                    queryPayment.CheckId = payment.CheckId;
                    queryPayment.Type = payment.Type;
                    queryPayment.Amount = payment.Amount;
                    queryPayment.UserId = UserId;
                    db.Payments.InsertOnSubmit(queryPayment);
                    db.SubmitChanges();

                    if (paymentCC != null) {
                        queryPaymentCC = new PaymentCC();
                        queryPaymentCC.PaymentId = queryPayment.id;
                        queryPaymentCC.FirstName = paymentCC.FirstName;
                        queryPaymentCC.LastName = paymentCC.LastName;
                        queryPaymentCC.Number = paymentCC.Number;
                        queryPaymentCC.ExpiredMonth = paymentCC.ExpiredMonth;
                        queryPaymentCC.ExpiredYear = paymentCC.ExpiredYear;
                        db.PaymentCCs.InsertOnSubmit(queryPaymentCC);
                        db.SubmitChanges();
                    }
                }
            }
            catch (Exception ex) {
                return 0;
            }
            return queryPayment.id;
        }

        public Boolean UpdateCheckStatus(Int32 checkId, CommonUnit.CheckStatus status) {
            Check query = new Check();
            Printout kitchenOrder;
            if (checkId != 0) {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    query = db.Checks.FirstOrDefault(m => m.id == checkId);
                    if (query != default(Check)) {
                        query.Status = (Int32)status;
                        if (status == CommonUnit.CheckStatus.Ordered) {
                            // Run Inventory
                            UpdateInventory(checkId);

                            kitchenOrder = db.Printouts.FirstOrDefault(m => m.CheckId == checkId);
                            if (kitchenOrder == default(Printout)) {
                                kitchenOrder = new Printout();
                            }
                            kitchenOrder.Status = (Int32)CommonUnit.PrintStatus.Queued;
                            kitchenOrder.Type = (Int32)CommonUnit.PrintType.KitchenOrder;
                            kitchenOrder.CheckId = checkId;
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
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    query = db.Checks.FirstOrDefault(m => m.id == checkId);
                    if (query != default(Check)) {
                        query.Status = (Int32)CommonUnit.CheckStatus.Paid;
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

        public Boolean UpdateCheckType(Int32 checkId, CommonUnit.CheckType type) {
            Check query = new Check();
            if (checkId != 0) {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
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

        public Boolean UpdateInventory(Int32 checkId) {
            List<Services.ChecksMenu> menus;
            List<ChecksMenuProduct> products;
            ItemService _itemService = new ItemService();
            InventoryService _inventoryService = new InventoryService();

            try {
                menus = GetMenuItems(checkId);

                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    Check check = db.Checks.Where(m => m.id == checkId).FirstOrDefault();
                    foreach (Services.ChecksMenu menuItem in menus) {
                        /// We need to make sure that we are not double logging inventory
                        if (!db.InventoryRegistryCheckMenus.Any(m => m.ChecksMenuId == menuItem.id)) {
                            Item itemMenu = _itemService.GetItem(menuItem.MenuId);
                            foreach (ItemInventoryAssociation association in itemMenu.ItemInventoryAssociations) {
                                _inventoryService.AddInventoryRegistry(association, menuItem);
                            }

                            products = GetProducts(menuItem.id);
                            if (products.Any()) {
                                foreach (Services.ChecksMenuProduct productItem in products) {
                                    foreach (Services.ChecksMenuProductItem associatedItem in productItem.ChecksMenuProductItems) {
                                        Item prodItem = _itemService.GetItemProductAssosiationsById(associatedItem.ItemId);
                                        foreach (ItemInventoryAssociation association in prodItem.ItemInventoryAssociations) {
                                            _inventoryService.AddInventoryRegistry(association, menuItem);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex) {
                return false;
            }
        }

        public Boolean UpdateKitchenOrderPrintStatus(Int32 id) {
            Printout query = new Printout();
            if (id != 0) {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    query = db.Printouts.FirstOrDefault(m => m.id == id);
                    if (query != default(Printout)) {
                        query.Status = (Int32)CommonUnit.PrintStatus.Printed;
                        db.SubmitChanges();
                        return true;
                    }
                }
            }
            return false;
        }

        public void UpdateMenuItemStatus(Int32 id, CommonUnit.MenuItemStatus status) {
            try {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
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

        public Boolean UpdateTableStatus(Int32 tableOrderId, CommonUnit.TableOrderStatus status) {
            TableOrder query = new TableOrder();
            if (tableOrderId != 0) {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    query = db.TableOrders.FirstOrDefault(m => m.id == tableOrderId);
                    if (query != default(TableOrder)) {
                        query.Status = (Int32)status;
                        db.SubmitChanges();
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion Public Methods
    }
}