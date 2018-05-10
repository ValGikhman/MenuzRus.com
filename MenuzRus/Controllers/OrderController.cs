using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MenuzRus.Models;
using Services;

namespace MenuzRus.Controllers {

    public class OrderController : BaseController {

        #region Private Fields

        private ICategoryService _categoryService;
        private ICommentService _commentService;
        private IFloorService _floorService;
        private IItemService _itemService;
        private IMenuService _menuService;
        private IOrderService _orderService;

        #endregion Private Fields

        #region Public Constructors

        public OrderController(ISessionData sessionData
                , IOrderService orderService
                , IItemService itemService
                , ICommentService commentService
                , IFloorService floorService
                , ICategoryService categoryService
                , IMenuService menuService
            )
            : base(sessionData) {
            _orderService = orderService;
            _itemService = itemService;
            _commentService = commentService;
            _floorService = floorService;
            _categoryService = categoryService;
            _menuService = menuService;
        }

        #endregion Public Constructors

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

        [HttpPost]
        public Int32 AddPayment(Int32 checkId, String type, String account, String firstName, String lastName, Int32 expiredMonth, Int32 expiredYear, Decimal amount) {
            Payment payment;
            PaymentCC paymentCC = null;
            Int32 id;

            try {
                CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                TextInfo textInfo = cultureInfo.TextInfo;

                payment = new Payment();

                payment.CheckId = checkId;
                payment.Type = ((Int32)(CommonUnit.Payments)Enum.Parse(typeof(CommonUnit.Payments), textInfo.ToTitleCase(type)));
                payment.Amount = amount;

                if ((CommonUnit.Payments)payment.Type != CommonUnit.Payments.Cash) {
                    paymentCC = new PaymentCC();
                    paymentCC.FirstName = firstName;
                    paymentCC.LastName = lastName;
                    paymentCC.Number = account;
                    paymentCC.ExpiredMonth = expiredMonth;
                    paymentCC.ExpiredYear = expiredYear;
                }

                id = _orderService.SavePayment(payment, paymentCC, SessionData.user.id);
            }
            catch (Exception ex) {
                base.Log(ex);
                return 0;
            }
            finally {
            }

            return id;
        }

        [HttpGet]
        public String ChecksPrint(String checksIds, Int32 split, Decimal adjustment) {
            List<Int32> Ids = new JavaScriptSerializer().Deserialize<List<Int32>>(checksIds);
            String retVal = String.Empty;
            Services.Check check;

            foreach (Int32 id in Ids) {
                check = _orderService.GetCheck(id);
                retVal += PrintChecks(id, EnumHelper<CommonUnit.CheckType>.Parse(check.Type.ToString()).ToString(), split, adjustment);
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
        public JsonResult DeleteMenu(Int32 id) {
            Services.Item item;
            Services.ChecksMenu checkMenu;

            try {
                checkMenu = _orderService.GetMenuItem(id);
                item = _itemService.GetItem(checkMenu.MenuId);
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

        [HttpGet]
        public Boolean DeletePayment(Int32 id) {
            try {
                _orderService.DeletePayment(id);
            }
            catch (Exception ex) {
                base.Log(ex);
                return false;
            }
            finally {
            }

            return true;
        }

        [HttpGet]
        public String GetCurrentMenu(Int32 id) {
            OrderModel model;
            try {
                model = new OrderModel();
                model.Categories = _categoryService.GetMenuCategories(SessionData.customer.id, CommonUnit.CategoryType.Menu, id);
                return RenderViewToString(this.ControllerContext, "_OrderMenuPartial", model);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
            return null;
        }

        [HttpGet]
        public JsonResult GraphRefresh() {
            try {
                var retVal = new {
                    inventory = _orderService.LatestInventory(SessionData.customer.id),
                    sales = _orderService.LatestSale(SessionData.customer.id)
                };
                return new JsonResult() { Data = retVal, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex) {
            }
            finally {
            }
            return null;
        }

        [CheckUserSession]
        [HttpGet]
        public ActionResult Kitchen() {
            return View(GetKitchenModel());
        }

        [HttpGet]
        public ActionResult KitchenDetails(Int32 TableId) {
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
            Services.Item item;

            ChecksMenu menu = null;
            try {
                item = _itemService.GetItem(id);

                if (item != null) {
                    menu = _orderService.SaveMenuItem(item, tableId, checkId, SessionData.user.id);
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
        public String PrintChecks(Int32 checkId, String type, Int32 split, Decimal adjustment) {
            return RenderViewToString(this.ControllerContext, "Printouts/_SendChecks2PrinterPartial", GetCheckPrintModel(checkId, type, split, adjustment));
        }

        [HttpGet]
        public String PrintKitchenOrders(Services.Printout order) {
            KitchenOrderPrint model = GetKitchenOrderPrintModel(order);
            return RenderViewToString(this.ControllerContext, "Printouts/_SendKitchenOrder2PrinterPartial", model);
        }

        [HttpPost]
        public JsonResult SaveItem(Int32 checkId, Int32 productId, Int32 knopaId, CommonUnit.ProductType type) {
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
        public String ShowCheck(Int32 checkId) {
            return RenderViewToString(this.ControllerContext, "_OrderCheckPrintPartial", GetCheckModel(checkId));
        }

        [HttpGet]
        public String ShowCheckPrint(Int32 checkId, String type, String status, Int32 split, Decimal adjustment) {
            return RenderViewToString(this.ControllerContext, "_OrderCheckPrintPartial", GetCheckPrintModel(checkId, type, split, adjustment));
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
                    product.Type = (CommonUnit.ProductType)itemProduct.Type;

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
                    menu.Ordered = ((CommonUnit.MenuItemStatus)menuItem.Status == CommonUnit.MenuItemStatus.Ordered);
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
        public JsonResult UpdateCheckStatus(Int32 checkId, CommonUnit.CheckType type, CommonUnit.CheckStatus status, Decimal adjustment, Int32 split) {
            CheckPrint model;
            try {
                if (status == CommonUnit.CheckStatus.Paid) {
                    model = GetCheckPrintModel(checkId, type.ToString(), split, adjustment);
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
        public JsonResult UpdateCheckType(Int32 checkId, CommonUnit.CheckType type) {
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
        public JsonResult UpdateTableStatus(Int32 tableOrderId, CommonUnit.TableOrderStatus status) {
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

        private CheckPrint GetCheckModel(Int32 checkId) {
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

                // If no printers - webSocket not running, or no connection to webSocket
                if (SessionData.printers != null) {
                    model.PrinterPOSWidth = SessionData.printerPOSWidth;
                }

                List<Services.ChecksMenu> menus = _orderService.GetMenuItems(checkId);
                List<ChecksMenuProduct> products;
                model.Summary = 0;
                model.Split = 0;
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
                if ((CommonUnit.CheckType)model.Check.Type == CommonUnit.CheckType.Guest) {
                    model.TaxPercent = tax;
                }
                model.Tax = Math.Round(model.Summary * model.TaxPercent, 2);

                model.AdjustmentPercent = model.Check.Type / 100;
                model.Adjustment = Math.Round(model.Summary * model.AdjustmentPercent, 2);
                model.Subtotal = model.Summary + model.Adjustment;
                model.Total = Math.Round(model.Summary + model.Tax + model.Adjustment, 2);
            }
            catch (Exception ex) {
                base.Log(ex);
                return null;
            }
            finally {
            }
            return model;
        }

        private CheckPrint GetCheckPrintModel(Int32 checkId, String type, Int32 split, Decimal adjustment) {
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

                // If no printers - webSocket not running, or no connection to webSocket
                if (SessionData.printers != null) {
                    model.PrinterPOSWidth = SessionData.printerPOSWidth;
                }

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
                if (EnumHelper<CommonUnit.CheckType>.Parse(type) == CommonUnit.CheckType.Guest) {
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
                model.Comments = _commentService.GetItemComment(order.CheckId, CommonUnit.CommentType.Check, SessionData.customer.id);

                // If no printers - webSocket not running, or no connection to webSocket
                if (SessionData.printers != null) {
                    model.PrinterKitchenWidth = SessionData.printerKitchenWidth;
                }

                List<Services.ChecksMenu> menus = _orderService.GetMenuItems(order.CheckId);
                List<ChecksMenuProduct> products;
                Boolean ordered;
                foreach (Services.ChecksMenu menuItem in menus) {
                    itemMenu = _itemService.GetItem(menuItem.MenuId);
                    ordered = ((CommonUnit.MenuItemStatus)menuItem.Status == CommonUnit.MenuItemStatus.Ordered);
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
                    model.Items.Add(new LineItem() { Description = itemMenu.Name, Ordered = ordered, id = itemMenu.id, Comments = _commentService.GetItemComment(menuItem.id, CommonUnit.CommentType.MenuItem, SessionData.customer.id), SubItems = subItems });
                    _orderService.UpdateMenuItemStatus(menuItem.id, CommonUnit.MenuItemStatus.Ordered);
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
                    IEnumerable<Services.Check> checks = model.TableOrder.Checks.Where(m => m.Status == (Int32)CommonUnit.CheckStatus.Ordered);
                    foreach (Services.Check checkItem in checks) {
                        orderModel = new CheckPrint();
                        orderModel.Check = checkItem;
                        orderModel.Items = new List<LineItem>();

                        // If no printers - webSocket not running, or no connection to webSocket
                        if (SessionData.printers != null) {
                            orderModel.PrinterPOSWidth = SessionData.printerPOSWidth;
                        }

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

        private Models.Menu GetMenus() {
            Services.Menus menu;
            Models.Menu model = new Models.Menu();

            model.Menus = _menuService.GetMenus(SessionData.customer.id);
            model.Menus.Insert(0, new Menus() { id = 0, CustomerId = SessionData.customer.id, Name = Resources.Resource.COMMON_ALL, Description = "" });
            menu = model.Menus.Take(1).FirstOrDefault();

            if (menu != null) {
                model.id = menu.id;
                model.Name = menu.Name;
                model.Description = menu.Description;
            }
            model.CurrentMenu = menu;
            return model;
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
                    floor.Name = Resources.Resource.COMMON_ALL;
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
                    alltables = _orderService.GetTableOrdersByFloorId(model.Floor.id, SessionData.customer.id);
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
                    model.Inventory = _orderService.LatestInventory(SessionData.customer.id);
                    model.Sales = _orderService.LatestSale(SessionData.customer.id);
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
                model.Categories = _categoryService.GetCategories(SessionData.customer.id, CommonUnit.CategoryType.Menu);
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
                        check.Status = (CommonUnit.CheckStatus)checkItem.Status;
                        check.Type = (CommonUnit.CheckType)checkItem.Type;
                        check.CheckMenuItems = new List<CheckMenuItem>();
                        model.Checks.Add(check);
                    }
                }

                model.Menu = GetMenus();

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
                              let attributes = _orderService.GetTableAttributes(var.id, true)
                              select new {
                                  var.Top,
                                  var.FloorId,
                                  var.id,
                                  var.Name,
                                  var.Left,
                                  var.Type,
                                  var.Width,
                                  var.Height,
                                  Checks = attributes.Item1,
                                  Status = attributes.Item2,
                                  DateModified = attributes.Item3
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
                    model.Floor.Width = floor.Width;
                    model.Floor.Height = floor.Height;
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