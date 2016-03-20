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
        private ICategoryService _categoryService;
        private ICommentService _commentService;
        private IFloorService _floorService;
        private IItemService _itemService;
        private IOrderService _orderService;

        public OrderController(ISessionData sessionData
                , IOrderService orderService
                , IItemService itemService
                , ICommentService commentService
                , IFloorService floorService
                , ICategoryService categoryService)
            : base(sessionData) {
            _orderService = orderService;
            _itemService = itemService;
            _commentService = commentService;
            _floorService = floorService;
            _categoryService = categoryService;
        }

        #region order

        [HttpPost]
        public JsonResult AddNewTableOrder(Int32 tableId) {
            Int32 newId = 0;
            try {
                newId = _orderService.AddNewTableOrder(tableId);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }

            var retVal = new { Data = newId };
            return Json(retVal);
        }

        [HttpGet]
        public String ChecksPrint(String checkIds, Int32 split, Decimal adjustment) {
            List<Int32> Ids = new JavaScriptSerializer().Deserialize<List<Int32>>(checkIds);
            String retVal = String.Empty;
            Services.Check check;

            foreach (Int32 id in Ids) {
                check = _orderService.GetCheck(id);
                retVal += PrintChecks(id, EnumHelper<Common.CheckType>.Parse(check.Type.ToString()).ToString(), EnumHelper<Common.CheckStatus>.Parse(check.Status.ToString()).ToString(), split, adjustment);
            }
            return retVal;
        }

        [HttpGet]
        public ActionResult DeleteChecks(String checksIds) {
            List<Int32> Ids;

            try {
                Ids = new JavaScriptSerializer().Deserialize<List<Int32>>(checksIds);
                foreach (Int32 id in Ids) {
                    _orderService.DeleteCheck(id);
                }
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
            return null;
        }

        [HttpGet]
        public JsonResult DeleteMenu(Int32 id, Int32 checkId) {
            try {
                _orderService.DeleteMenu(id);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
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
            if (SessionData.user == null) {
                return PartialView("_SessionEnd");
            }

            KitchenOrderModel model;

            try {
                model = GetKitchenTableModel(TableId);
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
            String retVal = String.Empty;
            Printout order2Print;

            order2Print = _orderService.GetPrintKitchenOrder(id);
            return PrintKitchenOrders(order2Print);
        }

        [HttpGet]
        public JsonResult KitchenOrders2Print() {
            List<Services.Printout> orders2Print;
            orders2Print = _orderService.GetQueued4PrintKitchenOrders();

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
            if (SessionData.user == null) {
                return PartialView("_SessionEnd");
            }

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
            if (SessionData.user == null) {
                return PartialView("_SessionEnd");
            }

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
            ChecksMenu menu = null;
            try {
                Services.Item MenuItem = _itemService.GetItem(id);

                if (MenuItem != null) {
                    menu = _orderService.SaveMenuItem(MenuItem, tableId, checkId, SessionData.user.id);
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
            try {
                _orderService.SaveItem(productId, knopaId, type);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
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
        public String ShowMenuItem(Int32 id, ChecksMenu menuItem) {
            CheckMenuItem menu = new CheckMenuItem();

            try {
                Services.Item Item = _itemService.GetItem(id);
                menu.id = menuItem.id;
                menu.ItemId = Item.id;
                menu.Name = Item.Name;
                menu.CheckId = menuItem.CheckId;
                menu.Price = (Decimal)Item.ItemPrices.OrderByDescending(m => m.DateCreated).Take(1).Select(m => m.Price).FirstOrDefault();
                menu.HasProducts = (_orderService.GetProducts(menu.id).Count() > 0);
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
            List<CheckMenuItemProduct> model = new List<CheckMenuItemProduct>();
            List<ChecksMenuProduct> products = new List<ChecksMenuProduct>();
            Services.ItemProduct itemProduct;
            Item Item;

            try {
                products = _orderService.GetProducts(menuId);
                foreach (Services.ChecksMenuProduct productItem in products) {
                    CheckMenuItemProduct product = new CheckMenuItemProduct();
                    itemProduct = _itemService.GetItemProduct(productItem.ItemId);
                    product.id = productItem.id;
                    product.ItemId = productItem.ItemId;
                    product.CheckMenuItemProductAssociations = new List<CheckMenuItemProductAssociation>();
                    product.Type = (Common.ProductType)itemProduct.Type;

                    foreach (Services.ItemProductAssociation associatedItem in itemProduct.ItemProductAssociations) {
                        CheckMenuItemProductAssociation association = new CheckMenuItemProductAssociation();
                        Item = _itemService.GetItem(associatedItem.ItemId);
                        association.id = associatedItem.id;
                        association.ItemId = associatedItem.ItemId;
                        association.Selected = productItem.ChecksMenuProductItems.Any(m => m.ItemId == associatedItem.id);
                        association.Name = Item.Name;
                        association.Price = (Decimal)Item.ItemPrices.OrderByDescending(m => m.DateCreated).Take(1).Select(m => m.Price).FirstOrDefault();
                        association.ImageUrl = Item.ImageUrl;
                        association.Customer = SessionData.customer;
                        product.CheckMenuItemProductAssociations.Add(association);
                    }
                    model.Add(product);
                }
            }
            catch (Exception ex) {
                base.Log(ex);
                return String.Format("Error");
            }
            return RenderViewToString(this.ControllerContext, "_OrderMenuProductPartial", model);
        }

        // Show Menus strips for a check
        [CheckUserSession]
        [HttpGet]
        public JsonResult ShowMenus(Int32 checkId) {
            List<CheckMenuItem> Menus = new List<CheckMenuItem>();
            CheckMenuItem menu;

            try {
                List<Services.ChecksMenu> menus = _orderService.GetMenuItems(checkId);
                foreach (Services.ChecksMenu menuItem in menus) {
                    Services.Item item = _itemService.GetItem(menuItem.MenuId);
                    menu = new CheckMenuItem();
                    menu.CheckId = checkId;
                    menu.id = menuItem.id;
                    menu.ItemId = item.id;
                    menu.Name = item.Name;
                    menu.Description = item.Description;
                    menu.Price = (Decimal)item.ItemPrices.OrderByDescending(m => m.DateCreated).Take(1).Select(m => m.Price).FirstOrDefault();
                    menu.HasProducts = (_orderService.GetProducts(menu.id).Count() > 0);
                    menu.Ordered = ((Common.MenuItemStatus)menuItem.Status == Common.MenuItemStatus.Ordered);
                    Menus.Add(menu);
                }
            }
            catch (Exception ex) {
                base.Log(ex);
                return null;
            }
            finally {
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
            CheckPrint model;
            try {
                if (status == Common.CheckStatus.Paid) {
                    model = GetCheckPrintModel(checkId, type.ToString(), status.ToString(), split, adjustment);
                    if (model != null)
                        _orderService.UpdateCheckStatusPaid(checkId, model.Summary, model.Tax, model.Adjustment);
                }
                else {
                    _orderService.UpdateCheckStatus(checkId, status);
                }
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }

            var retVal = new {
            };
            return Json(retVal);
        }

        [HttpPost]
        public JsonResult UpdateCheckType(Int32 checkId, Common.CheckType type) {
            try {
                _orderService.UpdateCheckType(checkId, type);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }

            var retVal = new {
            };
            return Json(retVal);
        }

        [HttpPost]
        public JsonResult UpdateKitchenOrderPrintStatus(Int32 id) {
            try {
                _orderService.UpdateKitchenOrderPrintStatus(id);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }

            var retVal = new {
            };
            return Json(retVal);
        }

        [HttpPost]
        public JsonResult UpdateTableStatus(Int32 tableOrderId, Common.TableOrderStatus status) {
            try {
                _orderService.UpdateTableStatus(tableOrderId, status);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }

            var retVal = new {
            };

            return Json(retVal);
        }

        #endregion order

        #region private

        private CheckPrint GetCheckPrintModel(Int32 checkId, String type, String status, Int32 split, Decimal adjustment) {
            Services.Item item, itemMenu;
            CheckPrint model;
            Decimal tax = SessionData.customer.Tax.HasValue ? (Decimal)SessionData.customer.Tax / 100 : 0;
            Decimal price = 0, menuPrice = 0;
            List<LineItem> subItems;
            try {
                model = new CheckPrint();
                model.Items = new List<LineItem>();
                model.Check = _orderService.GetCheck(checkId);
                model.CreatedDate = model.Check.DateCreated;
                model.PrinterPOSWidth = SessionData.printerPOSWidth;

                List<Services.ChecksMenu> menus = _orderService.GetMenuItems(checkId);
                List<ChecksMenuProduct> products;
                model.Summary = 0;
                model.Split = split;
                foreach (Services.ChecksMenu menuItem in menus) {
                    itemMenu = _itemService.GetItem(menuItem.MenuId);
                    menuPrice = (Decimal)itemMenu.ItemPrices.OrderByDescending(m => m.DateCreated).Take(1).Select(m => m.Price).FirstOrDefault();
                    model.Summary += menuPrice;
                    products = _orderService.GetProducts(menuItem.id);
                    subItems = new List<LineItem>();
                    foreach (Services.ChecksMenuProduct productItem in products) {
                        foreach (Services.ChecksMenuProductItem associatedItem in productItem.ChecksMenuProductItems) {
                            item = _itemService.GetItemProductAssosiationsById(associatedItem.ItemId);
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
            }
            return model;
        }

        private KitchenModel GetKitchenModel() {
            KitchenModel model = new KitchenModel();
            List<Models.TableOrder> tables = new List<Models.TableOrder>();
            Models.TableOrder order;
            List<Services.TableOrder> alltables = new List<Services.TableOrder>();
            Services.Table table;
            try {
                alltables = _orderService.GetKitchenOrders();
                foreach (Services.TableOrder tab in alltables) {
                    order = new Models.TableOrder();
                    table = _orderService.GetTable(tab.TableId);
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
            }

            return null;
        }

        private KitchenOrderPrint GetKitchenOrderPrintModel(Services.Printout order) {
            Services.Item item, itemMenu;
            KitchenOrderPrint model;
            List<LineItem> subItems;

            try {
                model = new KitchenOrderPrint();
                model.Items = new List<LineItem>();
                model.Check = _orderService.GetCheck(order.CheckId);
                model.id = order.id;
                model.CreatedDate = model.Check.DateCreated;
                model.Comments = _commentService.GetItemComment(order.CheckId, Common.CommentType.Check, SessionData.customer.id);
                model.PrinterKitchenWidth = SessionData.printerKitchenWidth;

                List<Services.ChecksMenu> menus = _orderService.GetMenuItems(order.CheckId);
                List<ChecksMenuProduct> products;
                Boolean ordered;
                foreach (Services.ChecksMenu menuItem in menus) {
                    itemMenu = _itemService.GetItem(menuItem.MenuId);
                    ordered = ((Common.MenuItemStatus)menuItem.Status == Common.MenuItemStatus.Ordered);
                    products = _orderService.GetProducts(menuItem.id);
                    subItems = new List<LineItem>();
                    foreach (Services.ChecksMenuProduct productItem in products) {
                        foreach (Services.ChecksMenuProductItem associatedItem in productItem.ChecksMenuProductItems) {
                            item = _itemService.GetItemProductAssosiationsById(associatedItem.ItemId);
                            if (item != null) {
                                subItems.Add(new LineItem() { Description = item.Name, Ordered = ordered });
                            }
                        }
                    }
                    model.Items.Add(new LineItem() { Description = itemMenu.Name, Ordered = ordered, id = itemMenu.id, Comments = _commentService.GetItemComment(menuItem.id, Common.CommentType.MenuItem, SessionData.customer.id), SubItems = subItems });
                    _orderService.UpdateMenuItemStatus(menuItem.id, Common.MenuItemStatus.Ordered);
                }
            }
            catch (Exception ex) {
                base.Log(ex);
                return null;
            }
            finally {
            }

            return model;
        }

        private KitchenOrderModel GetKitchenTableModel(Int32 tableId) {
            KitchenOrderModel model = new KitchenOrderModel();

            Services.Item item, itemMenu;
            List<LineItem> subItems;
            CheckPrint orderModel;

            try {
                model.Table = _orderService.GetTable(tableId);
                model.TableOrder = _orderService.GetTableOrder(tableId);
                model.Table.Name = model.Table.Name;

                if (model.TableOrder != null && model.TableOrder.Checks != null) {
                    model.Checks = new List<CheckPrint>();
                    IEnumerable<Services.Check> checks = model.TableOrder.Checks.Where(m => m.Status == (Int32)Common.CheckStatus.Ordered);
                    foreach (Services.Check checkItem in checks) {
                        orderModel = new CheckPrint();
                        orderModel.Check = checkItem;
                        orderModel.Items = new List<LineItem>();
                        orderModel.PrinterPOSWidth = SessionData.printerPOSWidth;

                        List<Services.ChecksMenu> menus = _orderService.GetMenuItems(checkItem.id);
                        List<ChecksMenuProduct> products;

                        foreach (Services.ChecksMenu menuItem in menus) {
                            itemMenu = _itemService.GetItem(menuItem.MenuId);
                            products = _orderService.GetProducts(menuItem.id);
                            subItems = new List<LineItem>();
                            if (products.Any()) {
                                foreach (Services.ChecksMenuProduct productItem in products) {
                                    foreach (Services.ChecksMenuProductItem associatedItem in productItem.ChecksMenuProductItems) {
                                        item = _itemService.GetItemProductAssosiationsById(associatedItem.ItemId);
                                        if (item != null) {
                                            subItems.Add(new LineItem() { Description = item.Name });
                                        }
                                    }
                                }
                            }
                            orderModel.Items.Add(new LineItem() { Description = itemMenu.Name, id = itemMenu.id, Alerted = menuItem.Alerts.Any(m => m.CheckMenuId == menuItem.id), CheckMenuId = menuItem.id, SubItems = subItems });
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
            }
            return null;
        }

        private MonitorFloorModel GetMonitorModel(Int32? id) {
            MonitorFloorModel model = new MonitorFloorModel();
            List<Models.TableOrder> tables = new List<Models.TableOrder>();
            Models.TableOrder order;
            List<Services.TableOrder> alltables = new List<Services.TableOrder>();
            Services.Floor floor;
            Services.Table table;
            try {
                id = id.HasValue ? id.Value : 0;
                model.Floors = _floorService.GetFloors(SessionData.customer.id);
                if (id == 0) {
                    floor = new Services.Floor();
                    floor.id = 0;
                    floor.Name = "All";
                }
                else {
                    floor = _floorService.GetFloor(id.Value);
                    if (floor == null && model.Floors.Count > 0) {
                        floor = model.Floors[0];
                    }
                }
                model.Floor = new Models.MonitorFloor();
                if (floor != null) {
                    SessionData.floor = floor;
                    model.Floor.id = floor.id;
                    model.Floor.Name = floor.Name;
                    alltables = _orderService.GetTableOrdersByFloorId(model.Floor.id);
                    if (alltables != null) {
                        foreach (Services.TableOrder tab in alltables) {
                            order = new Models.TableOrder();
                            table = _orderService.GetTable(tab.TableId);
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
            }

            return null;
        }

        private List<Services.TableOrder> GetMonitorTables(Int32 floorId) {
            try {
                return _orderService.GetTableOrders(floorId);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }

            return null;
        }

        private OrderModel GetTableModel(Int32 tableId) {
            OrderModel model = new OrderModel();

            try {
                model.Referer = "Tables";
                if (Request.UrlReferrer != null && Request.UrlReferrer.LocalPath.IndexOf("Order/Monitor") > -1) {
                    model.Referer = "Monitor";
                }
                model.Categories = _categoryService.GetCategories(SessionData.customer.id, Common.CategoryType.Menu);
                model.Table = _orderService.GetTable(tableId);
                model.TableOrder = _orderService.GetTableOrder(tableId);
                model.TableId = model.Table.id;
                model.Table.Name = model.Table.Name;

                if (model.TableOrder != null && model.TableOrder.Checks != null) {
                    model.Checks = new List<Models.Check>();
                    foreach (Services.Check checkItem in model.TableOrder.Checks) {
                        Models.Check check = new Models.Check();
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
            }
            return null;
        }

        private String GetTables(Int32 id) {
            try {
                List<Services.Table> tables = _floorService.GetTables(id);
                var result = (from var in tables
                              where var.FloorId == id
                              select new {
                                  var.Top,
                                  var.FloorId,
                                  var.id,
                                  var.Name,
                                  var.Left,
                                  var.Type,
                                  var.Width,
                                  var.Height,
                                  Status = _orderService.GetTableOrderStatus(var.id),
                                  Checks = _orderService.GetChecksIds(var.id, true),
                                  DateModified = _orderService.GetTableOrderDate(var.id)
                              }).ToList();
                return result.OrderByDescending(m => m.DateModified).ToJson();
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }

            return null;
        }

        private FloorModel GetTablesModel(Int32? id) {
            FloorModel model = new FloorModel();
            Services.Floor floor;
            try {
                id = id.HasValue ? id.Value : 0;
                model.Floors = _floorService.GetFloors(SessionData.customer.id);
                floor = _floorService.GetFloor(id.Value);
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
            }

            return null;
        }

        #endregion private
    }
}