using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Services;
using StringExtensions;

namespace MenuzRus {

    public class OrderService {

        public Boolean DeleteCheck(Int32 id) {
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    OrderCheck orderCheck = db.OrderChecks.FirstOrDefault(m => m.id == id);
                    if (orderCheck != default(OrderCheck)) {
                        IEnumerable<OrderChecksMenu> checkMenus = db.OrderChecksMenus.Where(m => m.CheckId == orderCheck.id);
                        if (checkMenus.Any()) {
                            foreach (OrderChecksMenu menu in checkMenus) {
                                IEnumerable<OrderChecksMenuProduct> products = db.OrderChecksMenuProducts.Where(m => m.CheckMenuId == menu.id);
                                if (products.Any()) {
                                    foreach (OrderChecksMenuProduct product in products) {
                                        IEnumerable<OrderChecksMenuProductItem> items = db.OrderChecksMenuProductItems.Where(m => m.ProductId == product.id);
                                        if (items.Any()) {
                                            db.OrderChecksMenuProductItems.DeleteAllOnSubmit(items);
                                        }
                                    }
                                }
                                db.OrderChecksMenuProducts.DeleteAllOnSubmit(products);
                            }
                            db.OrderChecksMenus.DeleteAllOnSubmit(checkMenus);
                        }
                        db.OrderChecks.DeleteOnSubmit(orderCheck);
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
                    OrderChecksMenu menuItem = db.OrderChecksMenus.FirstOrDefault(m => m.id == id);
                    if (menuItem != default(OrderChecksMenu)) {
                        db.OrderChecksMenus.DeleteOnSubmit(menuItem);
                        IEnumerable<OrderChecksMenuProduct> products = db.OrderChecksMenuProducts.Where(m => m.CheckMenuId == id);
                        if (products.Any()) {
                            db.OrderChecksMenuProducts.DeleteAllOnSubmit(products);
                            foreach (OrderChecksMenuProduct product in products) {
                                IEnumerable<OrderChecksMenuProductItem> items = db.OrderChecksMenuProductItems.Where(m => m.ProductId == product.id);
                                db.OrderChecksMenuProductItems.DeleteAllOnSubmit(items);
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

        public List<OrderCheck> GetChecks(Int32 tableId) {
            if (tableId != 0) {
                menuzRusDataContext db = new menuzRusDataContext();
                TableOrder query = db.TableOrders.FirstOrDefault(m => m.TableId == tableId);
                if (query != default(TableOrder)) {
                    return query.OrderChecks.ToList();
                }
            }
            return null;
        }

        public OrderChecksMenu GetMenuItem(Int32 id) {
            if (id != 0) {
                menuzRusDataContext db = new menuzRusDataContext();
                return db.OrderChecksMenus.FirstOrDefault(m => m.id == id);
            }
            return null;
        }

        public List<OrderChecksMenu> GetMenuItems(Int32 checkId) {
            if (checkId != 0) {
                menuzRusDataContext db = new menuzRusDataContext();
                return db.OrderChecksMenus.Where(m => m.CheckId == checkId).ToList(); ;
            }
            return null;
        }

        public List<OrderChecksMenuProduct> GetProducts(Int32 menuId) {
            if (menuId != 0) {
                menuzRusDataContext db = new menuzRusDataContext();
                return db.OrderChecksMenuProducts.Where(m => m.CheckMenuId == menuId).ToList();
            }
            return null;
        }

        public TableOrder GetTableOrder(Int32 tableId) {
            if (tableId != 0) {
                menuzRusDataContext db = new menuzRusDataContext();
                return db.TableOrders.FirstOrDefault(m => m.TableId == tableId && m.Status != (Int32)Common.TableOrderStatus.Closed);
            }
            return null;
        }

        public Int32 SaveMenuItem(Item menuItem, Int32 tableId, Int32 orderId) {
            Int32 retVal = 0;
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    // New order
                    TableOrder tableOrder = db.TableOrders.FirstOrDefault(m => m.TableId == tableId && m.Status != (Int32)Common.TableOrderStatus.Closed);
                    if (tableOrder == default(TableOrder)) {
                        tableOrder = new TableOrder();
                        tableOrder.TableId = tableId;
                        tableOrder.Status = (Int32)Common.TableOrderStatus.Open;
                        db.TableOrders.InsertOnSubmit(tableOrder);
                        db.SubmitChanges();
                    }

                    OrderCheck orderCheck = db.OrderChecks.FirstOrDefault(m => m.id == orderId);
                    if (orderCheck == default(OrderCheck)) {
                        orderCheck = new OrderCheck();
                        orderCheck.TableOrderId = tableOrder.id;
                        orderCheck.Type = (Int32)Common.OrderType.Active;
                        db.OrderChecks.InsertOnSubmit(orderCheck);
                        db.SubmitChanges();
                    }

                    OrderChecksMenu orderCheckMenu = new OrderChecksMenu();
                    orderCheckMenu.CheckId = orderCheck.id;
                    orderCheckMenu.MenuId = menuItem.id;
                    db.OrderChecksMenus.InsertOnSubmit(orderCheckMenu);
                    db.SubmitChanges();
                    retVal = orderCheckMenu.id;

                    foreach (ItemProduct itemProduct in menuItem.ItemProducts) {
                        OrderChecksMenuProduct product = new OrderChecksMenuProduct();
                        product.CheckMenuId = orderCheckMenu.id;
                        product.ItemId = itemProduct.id;
                        db.OrderChecksMenuProducts.InsertOnSubmit(product);
                        db.SubmitChanges();
                        if (itemProduct.ItemProductAssociations.Count() > 0) {
                            OrderChecksMenuProductItem item = new OrderChecksMenuProductItem();
                            item.ProductId = product.id;
                            item.ItemId = itemProduct.ItemProductAssociations[0].id;
                            db.OrderChecksMenuProductItems.InsertOnSubmit(item);
                            db.SubmitChanges();
                        }
                    }
                }
            }
            catch (Exception ex) {
                SessionData.exeption = ex;
                return 0;
            }
            return retVal;
        }
    }
}