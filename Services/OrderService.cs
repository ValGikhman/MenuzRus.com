﻿using System;
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
                        tableOrder.Status = (Int32)Common.TableOrderStatus.Open;
                        db.TableOrders.InsertOnSubmit(tableOrder);
                    }
                    else {
                        tableOrder.Status = (Int32)Common.TableOrderStatus.Open;
                        tableOrder.DateModified = DateTime.UtcNow;
                    }
                    db.SubmitChanges();
                    retVal = tableOrder.id;
                }
            }
            return retVal;
        }

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

        public OrderCheck GetCheck(Int32 checkId) {
            if (checkId != 0) {
                menuzRusDataContext db = new menuzRusDataContext();
                OrderCheck query = db.OrderChecks.FirstOrDefault(m => m.id == checkId);
                if (query != default(OrderCheck)) {
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
                List<OrderCheck> checks;
                if (!showPaidChecks) {
                    checks = tableOrder.OrderChecks.Where(m => m.Status != (Int32)Common.CheckStatus.Paid).ToList();
                }
                else {
                    checks = tableOrder.OrderChecks.ToList();
                }
                if (checks != null) {
                    return String.Join("|", checks.Select(m => m.id).ToArray());
                }
            }
            return String.Empty;
        }

        public List<TableOrder> GetKitchenOrders() {
            List<TableOrder> retVal = new List<TableOrder>();
            menuzRusDataContext db = new menuzRusDataContext();
            return db.TableOrders.Where(m => m.Status != (Int32)Common.TableOrderStatus.Closed && m.OrderChecks.Where(c => c.Status == (Int32)Common.CheckStatus.Ordered).Any()).OrderByDescending(m => m.DateModified).ToList();
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
            OrderChecksMenuProductItem query;
            if (productId != 0 && knopaId != 0) {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    if (type == Common.ProductType.Alternatives) {
                        query = db.OrderChecksMenuProductItems.FirstOrDefault(m => m.ProductId == productId);
                        if (query != default(OrderChecksMenuProductItem)) {
                            query.ItemId = knopaId;
                        }
                    }
                    else if (type == Common.ProductType.Addons) {
                        query = db.OrderChecksMenuProductItems.FirstOrDefault(m => m.ProductId == productId && m.ItemId == knopaId);
                        if (query == default(OrderChecksMenuProductItem)) {
                            query = new OrderChecksMenuProductItem();
                            query.ItemId = knopaId;
                            query.ProductId = productId;
                            db.OrderChecksMenuProductItems.InsertOnSubmit(query);
                        }
                        else {
                            db.OrderChecksMenuProductItems.DeleteOnSubmit(query);
                        }
                    }
                    db.SubmitChanges();
                }
            }
        }

        public OrderChecksMenu SaveMenuItem(Item menuItem, Int32 tableId, Int32 orderId) {
            OrderChecksMenu orderCheckMenu;
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
                        orderCheck.Type = (Int32)Common.CheckType.Guest;
                        orderCheck.Status = (Int32)Common.CheckStatus.Active;
                        db.OrderChecks.InsertOnSubmit(orderCheck);
                        db.SubmitChanges();
                    }

                    orderCheckMenu = new OrderChecksMenu();
                    orderCheckMenu.CheckId = orderCheck.id;
                    orderCheckMenu.MenuId = menuItem.id;
                    db.OrderChecksMenus.InsertOnSubmit(orderCheckMenu);
                    db.SubmitChanges();

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
                return null;
            }
            return orderCheckMenu;
        }

        public Boolean UpdateCheckStatus(Int32 checkId, Common.CheckStatus status) {
            OrderCheck query = new OrderCheck();
            if (checkId != 0) {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    query = db.OrderChecks.FirstOrDefault(m => m.id == checkId);
                    if (query != default(OrderCheck)) {
                        query.Status = (Int32)status;
                        db.SubmitChanges();
                        return true;
                    }
                }
            }
            return false;
        }

        public Boolean UpdateCheckType(Int32 checkId, Common.CheckType type) {
            OrderCheck query = new OrderCheck();
            if (checkId != 0) {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    query = db.OrderChecks.FirstOrDefault(m => m.id == checkId);
                    if (query != default(OrderCheck)) {
                        query.Type = (Int32)type;
                        db.SubmitChanges();
                        return true;
                    }
                }
            }
            return false;
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