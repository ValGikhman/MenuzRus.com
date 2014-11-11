using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using Extensions;
using MenuzRus.Models;
using Newtonsoft.Json;
using Services;

namespace MenuzRus.Controllers {

    public class OrderController : BaseController {

        #region order

        [HttpPost]
        public JsonResult AddNewTableOrder(Int32 tableId) {
            OrderService orderService = new OrderService();
            Int32 newId = 0;
            try {
                newId = orderService.AddNewTableOrder(tableId);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                orderService = null;
            }

            var retVal = new { Data = newId };
            return Json(retVal);
        }

        [HttpGet]
        public String ChecksPrint(String checksIds, Int32 split, Decimal adjustment) {
            List<Int32> Ids = new JavaScriptSerializer().Deserialize<List<Int32>>(checksIds);
            OrderService service = new OrderService();
            String retVal = String.Empty;
            Services.OrderCheck check;
            foreach (Int32 id in Ids) {
                check = service.GetCheck(id);
                retVal += PrintChecks(id, EnumHelper<Common.CheckType>.Parse(check.Type.ToString()).ToString(), EnumHelper<Common.CheckStatus>.Parse(check.Status.ToString()).ToString(), split, adjustment);
            }
            return retVal;
        }

        [HttpGet]
        public ActionResult DeleteChecks(String checksIds) {
            OrderService orderService;
            List<Int32> Ids;
            try {
                Ids = new JavaScriptSerializer().Deserialize<List<Int32>>(checksIds);
                orderService = new OrderService();
                foreach (Int32 id in Ids) {
                    orderService.DeleteCheck(id);
                }
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                orderService = null;
            }
            return null;
        }

        [HttpGet]
        public JsonResult DeleteMenu(Int32 id, Int32 checkId) {
            OrderService orderService = new OrderService();
            try {
                orderService.DeleteMenu(id);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                orderService = null;
            }
            var retVal = new {
            };
            return new JsonResult() { Data = retVal, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [CheckUserSession]
        [HttpGet]
        public ActionResult Kitchen() {
            return View(GetKitchenModel());
        }

        [HttpGet]
        public ActionResult KitchenDetails(Int32 TableId) {
            try {
                KitchenOrderModel model = GetKitchenTableModel(TableId);
                return PartialView("_KitchenDetailsPartial", model);
            }
            catch (Exception ex) {
            }
            finally {
            }
            return null;
        }

        [HttpGet]
        public String KitchenOrder2Print(Int32 id) {
            OrderService service = new OrderService();
            String retVal = String.Empty;
            Printout order2Print;

            order2Print = service.GetPrintKitchenOrder(id);
            return PrintKitchenOrders(order2Print);
        }

        [HttpGet]
        public JsonResult KitchenOrders2Print() {
            OrderService service = new OrderService();
            List<Services.Printout> orders2Print;
            orders2Print = service.GetQueued4PrintKitchenOrders();
            var jsonData = new {
                rows = (
                     from order in orders2Print
                     select new {
                         id = order.id
                     }
                ).ToArray()
            };
            return new JsonResult() { Data = jsonData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult KitchenRefresh() {
            try {
                return PartialView("_KitchenPartial", GetKitchenModel());
            }
            catch (Exception ex) {
            }
            finally {
            }
            return null;
        }

        [CheckUserSession]
        [HttpGet]
        public ActionResult Monitor(Int32? id) {
            return View(GetMonitorModel(id));
        }

        [HttpGet]
        public ActionResult MonitorRefresh(Int32? id) {
            try {
                return PartialView("_MonitorPartial", GetMonitorModel(id));
            }
            catch (Exception ex) {
            }
            finally {
            }
            return null;
        }

        [HttpGet]
        public JsonResult OrderMenuItem(Int32 id, Int32 checkId, Int32 tableId) {
            OrderChecksMenu menu = null;
            ItemService itemService = new ItemService();
            OrderService orderService = new OrderService();
            Services.Item MenuItem = itemService.GetItem(id);

            try {
                if (MenuItem != null) {
                    menu = orderService.SaveMenuItem(MenuItem, tableId, checkId);
                }

                if (menu != null) {
                    var retVal = new {
                        html = ShowMenuItem(id, menu),
                        checkId = menu.CheckId
                    };

                    return new JsonResult() { Data = retVal, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            catch (Exception ex) {
            }
            finally {
            }
            return null;
        }

        [HttpGet]
        public String PrintChecks(Int32 checkId, String type, String status, Int32 split, Decimal adjustment) {
            return RenderViewToString(this.ControllerContext, "Printouts/_SendChecks2PrinterPartial", GetCheckPrintModel(checkId, type, status, split, adjustment));
        }

        [HttpGet]
        public String PrintKitchenOrders(Services.Printout order) {
            KitchenOrderPrint model = GetKitchenOrderPrintModel(order);
            return RenderViewToString(this.ControllerContext, "Printouts/_SendKitchenOrder2PrinterPartial", model);
        }

        [HttpPost]
        public JsonResult SaveItem(Int32 checkId, Int32 productId, Int32 knopaId, Common.ProductType type) {
            OrderService orderService = new OrderService();
            try {
                orderService.SaveItem(productId, knopaId, type);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                orderService = null;
            }

            var retVal = new {
            };
            return Json(retVal);
        }

        [HttpGet]
        public String ShowCheckPrint(Int32 checkId, String type, String status, Int32 split, Decimal adjustment) {
            return RenderViewToString(this.ControllerContext, "_OrderCheckPrintPartial", GetCheckPrintModel(checkId, type, status, split, adjustment));
        }

        [HttpGet]
        public String ShowMenuItem(Int32 id, OrderChecksMenu menuItem) {
            ItemService itemService = new ItemService();
            OrderService orderService = new OrderService();
            CheckMenuItem menu = new CheckMenuItem();
            try {
                Services.Item Item = itemService.GetItem(id);
                menu.id = menuItem.id;
                menu.ItemId = Item.id;
                menu.Name = Item.Name;
                menu.CheckId = menuItem.CheckId;
                menu.Price = (Decimal)Item.ItemPrices.OrderByDescending(m => m.DateCreated).Take(1).Select(m => m.Price).FirstOrDefault();
            }
            catch (Exception ex) {
                base.Log(ex);
                return String.Empty;
            }

            return RenderViewToString(this.ControllerContext, "_OrderMenuItemPartial", menu);
        }

        // Show products for the menu
        [HttpGet]
        public String ShowMenuProducts(Int32 menuId) {
            ItemService itemService = new ItemService();
            OrderService orderService = new OrderService();
            List<CheckMenuItemProduct> model = new List<CheckMenuItemProduct>();
            List<OrderChecksMenuProduct> products = new List<OrderChecksMenuProduct>();
            Item Item;
            try {
                products = orderService.GetProducts(menuId);
                foreach (Services.OrderChecksMenuProduct productItem in products) {
                    CheckMenuItemProduct product = new CheckMenuItemProduct();
                    Services.ItemProduct itemProduct = itemService.GetItemProduct(productItem.ItemId);
                    product.id = productItem.id;
                    product.ItemId = productItem.ItemId;
                    product.CheckMenuItemProductAssociations = new List<CheckMenuItemProductAssociation>();
                    product.Type = (Common.ProductType)itemProduct.Type;
                    foreach (Services.ItemProductAssociation associatedItem in itemProduct.ItemProductAssociations) {
                        CheckMenuItemProductAssociation association = new CheckMenuItemProductAssociation();
                        Item = itemService.GetItem(associatedItem.ItemId);
                        association.id = associatedItem.id;
                        association.ItemId = associatedItem.ItemId;
                        association.Selected = productItem.OrderChecksMenuProductItems.Any(m => m.ItemId == associatedItem.id);
                        association.Name = Item.Name;
                        association.Price = (Decimal)Item.ItemPrices.OrderByDescending(m => m.DateCreated).Take(1).Select(m => m.Price).FirstOrDefault();
                        association.ImageUrl = Item.ImageUrl;
                        product.CheckMenuItemProductAssociations.Add(association);
                    }
                    model.Add(product);
                }
            }
            catch (Exception ex) {
                base.Log(ex);
                return "Error";
            }
            return RenderViewToString(this.ControllerContext, "_OrderMenuProductPartial", model);
        }

        // Show Menus strips for a check
        [HttpGet]
        public JsonResult ShowMenus(Int32 checkId) {
            ItemService itemService = new ItemService();
            OrderService orderService = new OrderService();
            List<CheckMenuItem> Menus = new List<CheckMenuItem>();
            CheckMenuItem menu;
            try {
                List<Services.OrderChecksMenu> menus = orderService.GetMenuItems(checkId);
                foreach (Services.OrderChecksMenu menuItem in menus) {
                    Services.Item item = itemService.GetItem(menuItem.MenuId);
                    menu = new CheckMenuItem();
                    menu.id = menuItem.id;
                    menu.ItemId = item.id;
                    menu.Name = item.Name;
                    menu.Price = (Decimal)item.ItemPrices.OrderByDescending(m => m.DateCreated).Take(1).Select(m => m.Price).FirstOrDefault();
                    menu.HasProducts = (orderService.GetProducts(menu.id).Count() > 0);

                    Menus.Add(menu);
                }
            }
            catch (Exception ex) {
                base.Log(ex);
                return null;
            }
            finally {
                itemService = null;
                orderService = null;
            }
            var retVal = new {
                html = RenderViewToString(this.ControllerContext, "_OrderMenusPartial", Menus),
                checkId = checkId
            };
            return new JsonResult() { Data = retVal, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        // Show Checks tabs
        [HttpGet]
        public String ShowOrder(Int32 tableId) {
            OrderModel model = null;
            try {
                model = GetTableModel(tableId);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
            return RenderViewToString(this.ControllerContext, "_OrderPartial", model);
        }

        [CheckUserSession]
        [HttpGet]
        public ActionResult Table(Int32 id) {
            OrderModel model = GetTableModel(id);
            return View(model);
        }

        [CheckUserSession]
        [HttpGet]
        public ActionResult Tables(Int32? id) {
            return View(GetTablesModel(id));
        }

        [HttpPost]
        public JsonResult UpdateCheckStatus(Int32 checkId, Common.CheckType type, Common.CheckStatus status, Decimal adjustment, Int32 split) {
            OrderService orderService = new OrderService();
            CheckPrint model;
            try {
                if (status == Common.CheckStatus.Paid) {
                    model = GetCheckPrintModel(checkId, type.ToString(), status.ToString(), split, adjustment);
                    if (model != null)
                        orderService.UpdateCheckStatusPaid(checkId, model.Summary, model.Tax, model.Adjustment);
                }
                else {
                    orderService.UpdateCheckStatus(checkId, status);
                }
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                orderService = null;
            }

            var retVal = new {
            };
            return Json(retVal);
        }

        [HttpPost]
        public JsonResult UpdateCheckType(Int32 checkId, Common.CheckType type) {
            OrderService orderService = new OrderService();
            try {
                orderService.UpdateCheckType(checkId, type);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                orderService = null;
            }

            var retVal = new {
            };
            return Json(retVal);
        }

        [HttpPost]
        public JsonResult UpdateKitchenOrderPrintStatus(Int32 id) {
            OrderService orderService = new OrderService();
            try {
                orderService.UpdateKitchenOrderPrintStatus(id);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                orderService = null;
            }

            var retVal = new {
            };
            return Json(retVal);
        }

        [HttpPost]
        public JsonResult UpdateTableStatus(Int32 tableOrderId, Common.TableOrderStatus status) {
            OrderService orderService = new OrderService();
            try {
                orderService.UpdateTableStatus(tableOrderId, status);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                orderService = null;
            }

            var retVal = new {
            };
            return Json(retVal);
        }

        #endregion order

        #region private

        private void AddPrintItem(Int32 printoutId, Int32 itemId) {
            OrderService service = new OrderService();
            service.AddPrintItem(printoutId, itemId);
        }

        private CheckPrint GetCheckPrintModel(Int32 checkId, String type, String status, Int32 split, Decimal adjustment) {
            ItemService itemService;
            Services.Item item, itemMenu;
            OrderService orderService;
            UserService userService;
            CheckPrint model;
            Decimal tax = SessionData.customer.Tax.HasValue ? (Decimal)SessionData.customer.Tax / 100 : 0;
            Decimal price = 0, menuPrice = 0;
            List<LineItem> subItems;
            try {
                itemService = new ItemService();
                orderService = new OrderService();
                userService = new UserService();

                model = new CheckPrint();
                model.Items = new List<LineItem>();
                model.Check = orderService.GetCheck(checkId);
                model.CreatedDate = model.Check.DateCreated;
                model.User = userService.GetUser(model.Check.UserId);

                List<Services.OrderChecksMenu> menus = orderService.GetMenuItems(checkId);
                List<OrderChecksMenuProduct> products;
                model.Summary = 0;
                model.Split = split;
                foreach (Services.OrderChecksMenu menuItem in menus) {
                    itemMenu = itemService.GetItem(menuItem.MenuId);
                    menuPrice = (Decimal)itemMenu.ItemPrices.OrderByDescending(m => m.DateCreated).Take(1).Select(m => m.Price).FirstOrDefault();
                    model.Summary += menuPrice;
                    products = orderService.GetProducts(menuItem.id);
                    subItems = new List<LineItem>();
                    foreach (Services.OrderChecksMenuProduct productItem in products) {
                        foreach (Services.OrderChecksMenuProductItem associatedItem in productItem.OrderChecksMenuProductItems) {
                            item = itemService.GetItemProductAssosiationsById(associatedItem.ItemId);
                            if (item != null) {
                                price = (Decimal)item.ItemPrices.OrderByDescending(m => m.DateCreated).Take(1).Select(m => m.Price).FirstOrDefault();
                                model.Summary += price;
                                subItems.Add(new LineItem() { Description = item.Name, Price = price, id = item.id });
                            }
                        }
                    }
                    model.Items.Add(new LineItem() { Description = itemMenu.Name, Price = menuPrice, id = itemMenu.id, SubItems = subItems });
                }

                model.TaxPercent = 0;
                if (EnumHelper<Common.CheckType>.Parse(type) == Common.CheckType.Guest) {
                    model.TaxPercent = tax;
                }
                model.Tax = Math.Round(model.Summary * model.TaxPercent, 2);

                model.AdjustmentPercent = adjustment / 100;
                model.Adjustment = Math.Round(model.Summary * model.AdjustmentPercent, 2);
                model.Subtotal = model.Summary + model.Adjustment;
                model.Total = Math.Round(model.Summary + model.Tax + model.Adjustment, 2);
                model.SplitValues = Utility.SplitAmount(model.Total, model.Split);
            }
            catch (Exception ex) {
                base.Log(ex);
                return null;
            }
            finally {
                itemService = null;
                orderService = null;
            }
            return model;
        }

        private KitchenModel GetKitchenModel() {
            OrderService service = new OrderService();
            KitchenModel model = new KitchenModel();
            List<Models.TableOrder> tables = new List<Models.TableOrder>();
            Models.TableOrder order;
            List<Services.TableOrder> alltables = new List<Services.TableOrder>();
            Services.Table table;
            try {
                alltables = service.GetKitchenOrders();
                foreach (Services.TableOrder tab in alltables) {
                    order = new Models.TableOrder();
                    table = service.GetTable(tab.TableId);
                    order.TableName = table.Name;
                    order.Order = tab;
                    tables.Add(order);
                }
                model.Tables = tables;
                return model;
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                service = null;
            }

            return null;
        }

        private KitchenOrderPrint GetKitchenOrderPrintModel(Services.Printout order) {
            ItemService itemService;
            Services.Item item, itemMenu;
            OrderService orderService;
            UserService userService;
            KitchenOrderPrint model;
            List<LineItem> subItems;
            try {
                itemService = new ItemService();
                orderService = new OrderService();
                userService = new UserService();

                model = new KitchenOrderPrint();
                model.Items = new List<LineItem>();
                model.Check = orderService.GetCheck(order.CheckId);
                model.id = order.id;
                model.CreatedDate = model.Check.DateCreated;
                model.User = userService.GetUser(model.Check.UserId);

                List<Services.OrderChecksMenu> menus = orderService.GetMenuItems(order.CheckId);
                List<OrderChecksMenuProduct> products;
                foreach (Services.OrderChecksMenu menuItem in menus) {
                    itemMenu = itemService.GetItem(menuItem.MenuId);
                    products = orderService.GetProducts(menuItem.id);
                    subItems = new List<LineItem>();
                    foreach (Services.OrderChecksMenuProduct productItem in products) {
                        foreach (Services.OrderChecksMenuProductItem associatedItem in productItem.OrderChecksMenuProductItems) {
                            item = itemService.GetItemProductAssosiationsById(associatedItem.ItemId);
                            if (item != null) {
                                subItems.Add(new LineItem() { Description = item.Name });
                                AddPrintItem(order.id, item.id);
                            }
                        }
                    }
                    model.Items.Add(new LineItem() { Description = itemMenu.Name, id = itemMenu.id, SubItems = subItems });
                    AddPrintItem(order.id, itemMenu.id);
                }
            }
            catch (Exception ex) {
                base.Log(ex);
                return null;
            }
            finally {
                itemService = null;
                orderService = null;
            }
            return model;
        }

        private KitchenOrderModel GetKitchenTableModel(Int32 tableId) {
            OrderService orderService = new OrderService();
            KitchenOrderModel model = new KitchenOrderModel();

            ItemService itemService = new ItemService();
            Services.Item item, itemMenu;
            List<LineItem> subItems;
            CheckPrint orderModel;

            try {
                model.Table = orderService.GetTable(tableId);
                model.TableOrder = orderService.GetTableOrder(tableId);
                model.Table.Name = model.Table.Name;

                if (model.TableOrder != null && model.TableOrder.OrderChecks != null) {
                    model.Checks = new List<CheckPrint>();
                    IEnumerable<OrderCheck> checks = model.TableOrder.OrderChecks.Where(m => m.Status == (Int32)Common.CheckStatus.Ordered);
                    foreach (Services.OrderCheck checkItem in checks) {
                        orderModel = new CheckPrint();
                        orderModel.Check = checkItem;
                        orderModel.Items = new List<LineItem>();
                        List<Services.OrderChecksMenu> menus = orderService.GetMenuItems(checkItem.id);
                        List<OrderChecksMenuProduct> products;
                        foreach (Services.OrderChecksMenu menuItem in menus) {
                            itemMenu = itemService.GetItem(menuItem.MenuId);
                            products = orderService.GetProducts(menuItem.id);
                            subItems = new List<LineItem>();
                            if (products.Any()) {
                                foreach (Services.OrderChecksMenuProduct productItem in products) {
                                    foreach (Services.OrderChecksMenuProductItem associatedItem in productItem.OrderChecksMenuProductItems) {
                                        item = itemService.GetItemProductAssosiationsById(associatedItem.ItemId);
                                        if (item != null) {
                                            subItems.Add(new LineItem() { Description = item.Name });
                                        }
                                    }
                                }
                            }
                            orderModel.Items.Add(new LineItem() { Description = itemMenu.Name, id = itemMenu.id, SubItems = subItems });
                        }
                        model.Checks.Add(orderModel);
                    }
                }
                return model;
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                orderService = null;
                itemService = null;
            }
            return null;
        }

        private MonitorFloorModel GetMonitorModel(Int32? id) {
            FloorService service = new FloorService();
            OrderService orderService = new OrderService();
            MonitorFloorModel model = new MonitorFloorModel();
            List<Models.TableOrder> tables = new List<Models.TableOrder>();
            Models.TableOrder order;
            List<Services.TableOrder> alltables = new List<Services.TableOrder>();
            Services.Floor floor;
            Services.Table table;
            try {
                id = id.HasValue ? id.Value : 0;
                model.Floors = service.GetFloors(SessionData.customer.id);
                if (id == 0) {
                    floor = new Services.Floor();
                    floor.id = 0;
                    floor.Name = "All";
                }
                else {
                    floor = service.GetFloor(id.Value);
                    if (floor == null && model.Floors.Count > 0) {
                        floor = model.Floors[0];
                    }
                }
                model.Floor = new Models.MonitorFloor();
                if (floor != null) {
                    SessionData.floor = floor;
                    model.Floor.id = floor.id;
                    model.Floor.Name = floor.Name;
                    alltables = orderService.GetTableOrdersByFloorId(model.Floor.id);
                    if (alltables != null) {
                        foreach (Services.TableOrder tab in alltables) {
                            order = new Models.TableOrder();
                            table = orderService.GetTable(tab.TableId);
                            order.TableName = table.Name;
                            order.Order = tab;
                            tables.Add(order);
                        }
                    }
                    model.Floor.Tables = tables;
                }
                return model;
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                service = null;
            }

            return null;
        }

        private List<Services.TableOrder> GetMonitorTables(Int32 floorId) {
            OrderService service = new OrderService();
            try {
                return service.GetTableOrders(floorId);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                service = null;
            }
            return null;
        }

        private OrderModel GetTableModel(Int32 tableId) {
            CategoryService categoryService = new CategoryService();
            OrderService orderService = new OrderService();
            OrderModel model = new OrderModel();
            try {
                model.Referer = "Tables";
                if (Request.UrlReferrer != null && Request.UrlReferrer.LocalPath.IndexOf("Order/Monitor") > -1) {
                    model.Referer = "Monitor";
                }
                model.Categories = categoryService.GetCategories(SessionData.customer.id, Common.CategoryType.Menu);
                model.Table = orderService.GetTable(tableId);
                model.TableOrder = orderService.GetTableOrder(tableId);
                model.TableId = model.Table.id;
                model.Table.Name = model.Table.Name;

                if (model.TableOrder != null && model.TableOrder.OrderChecks != null) {
                    model.Checks = new List<Check>();
                    foreach (Services.OrderCheck checkItem in model.TableOrder.OrderChecks) {
                        Check check = new Check();
                        check.id = checkItem.id;
                        check.Price = 0;
                        check.Status = (Common.CheckStatus)checkItem.Status;
                        check.Type = (Common.CheckType)checkItem.Type;
                        check.CheckMenuItems = new List<CheckMenuItem>();
                        model.Checks.Add(check);
                    }
                }
                return model;
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                categoryService = null;
            }
            return null;
        }

        private String GetTables(Int32 id) {
            FloorService service = new FloorService();
            OrderService serviceOrder = new OrderService();
            try {
                List<Services.Table> tables = service.GetTables(id);
                var result = (from var in tables
                              where var.FloorId == id
                              select new {
                                  var.Col,
                                  var.FloorId,
                                  var.id,
                                  var.Name,
                                  var.Row,
                                  var.Type,
                                  var.X,
                                  var.Y,
                                  Status = service.GetTableOrderStatus(var.id),
                                  Checks = serviceOrder.GetChecksIds(var.id, true),
                                  DateModified = service.GetTableOrderDate(var.id)
                              }).ToList();
                return result.OrderByDescending(m => m.DateModified).ToJson();
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                service = null;
                serviceOrder = null;
            }
            return null;
        }

        private FloorModel GetTablesModel(Int32? id) {
            FloorService service = new FloorService();
            FloorModel model = new FloorModel();
            Services.Floor floor;
            try {
                id = id.HasValue ? id.Value : 0;
                model.Floors = service.GetFloors(SessionData.customer.id);
                floor = service.GetFloor(id.Value);
                if (floor == null && model.Floors.Count > 0) {
                    floor = model.Floors[0];
                }
                model.Floor = new Models.Floor();
                if (floor != null) {
                    SessionData.floor = floor;
                    model.Floor.id = floor.id;
                    model.Floor.Name = floor.Name;
                    model.Floor.Layout = GetTables(model.Floor.id);
                }
                return model;
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                service = null;
            }

            return null;
        }

        #endregion private
    }
}