using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using MenuzRus.Models;
using Newtonsoft.Json;
using Services;
using StringExtensions;

namespace MenuzRus.Controllers {

    public class OrderController : BaseController {

        #region order

        [HttpPost]
        public ActionResult DeleteCheck(Int32 checkId) {
            OrderService orderService = new OrderService();
            try {
                orderService.DeleteCheck(checkId);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                orderService = null;
            } return null;
        }

        [HttpPost]
        public ActionResult DeleteMenu(Int32 id) {
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
            return null;
        }

        [HttpPost]
        public String OrderMenuItem(Int32 id, Int32 orderId, Int32 tableId) {
            OrderChecksMenu menu = new OrderChecksMenu(); ;
            ItemService itemService = new ItemService();
            OrderService orderService = new OrderService();
            Services.Item MenuItem = itemService.GetItem(id);
            if (MenuItem != null) {
                menu = orderService.SaveMenuItem(MenuItem, tableId, orderId);
            }
            return ShowMenuItem(id, menu);
        }

        [HttpPost]
        public void SaveItem(Int32 productId, Int32 knopaId, Common.ProductType type) {
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
        }

        // Show one menu strip
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
                        association.ShortName = Item.Name.Ellipsis(35);
                        association.Price = (Decimal)Item.ItemPrices.OrderByDescending(m => m.DateCreated).Take(1).Select(m => m.Price).FirstOrDefault();
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
        public String ShowMenus(Int32 checkId) {
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
                    Menus.Add(menu);
                }
            }
            catch (Exception ex) {
                base.Log(ex);
                return String.Empty;
            }
            finally {
                itemService = null;
                orderService = null;
            }
            return RenderViewToString(this.ControllerContext, "_OrderMenusPartial", Menus);
        }

        // Show Checks tabs
        [HttpGet]
        public String ShowOrder(Int32 tableId) {
            ItemService itemService = new ItemService();
            OrderService orderService = new OrderService();
            OrderModel model = new OrderModel();
            try {
                List<Services.OrderCheck> Checks = orderService.GetChecks(tableId);
                model.Checks = new List<Check>();
                foreach (Services.OrderCheck checkItem in Checks) {
                    Check check = new Check();
                    check.id = checkItem.id;
                    check.Price = 0;
                    check.Type = (Common.OrderType)checkItem.Type;
                    check.CheckMenuItems = new List<CheckMenuItem>();
                    model.Checks.Add(check);
                }
                return RenderViewToString(this.ControllerContext, "_OrderPartial", model);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                itemService = null;
            }
            return null;
        }

        [HttpGet]
        public ActionResult Table(Int32 id) {
            OrderModel model = GetTableModel(id);
            return View(model);
        }

        [HttpGet]
        public ActionResult Tables(Int32? id) {
            return View(GetTablesModel(id));
        }

        #endregion order

        #region private

        private OrderModel GetTableModel(Int32 id) {
            CategoryService categoryService = new CategoryService();
            MenuService menuService = new MenuService();
            OrderModel model = new OrderModel();
            try {
                model.Categories = categoryService.GetAllCategories(Common.CategoryType.Menu);
                model.TableId = id;
                return model;
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                categoryService = null;
                menuService = null;
            }
            return null;
        }

        private String GetTables(Int32 id) {
            FloorService service = new FloorService();
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
                              var.Y
                          }).ToList();
            return result.ToJson();
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