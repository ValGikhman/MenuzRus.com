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
        public ActionResult DeleteOrder(Int32? id) {
            return null;
        }

        public ActionResult SaveMenu(String model) {
            OrderService orderService = new OrderService();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            dynamic model2Save = jss.Deserialize<dynamic>(model);
            orderService.SaveMenu(model2Save);
            return null;
        }

        [HttpPost]
        public ActionResult SaveOrders(String model) {
            OrderService orderService = new OrderService();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            dynamic model2Save = jss.Deserialize<dynamic>(model);
            orderService.SaveOrder(model2Save);
            return null;
        }

        [HttpGet]
        public ActionResult ShowOrder(Int32 id) {
            ItemService itemService = new ItemService();
            OrderModel model = new OrderModel();
            try {
                SessionData.item = itemService.GetItem(id);
                return PartialView("_CheckPartial");
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
        public String ShowOrderItem(Int32 id, Int32 uid, Int32 orderId, Int32 tableId) {
            ItemService itemService = new ItemService();
            OrderModel model = new OrderModel();
            try {
                Services.Item Item = itemService.GetItem(id);
                if (Item != null) {
                    model.uid = uid;
                    model.TableId = tableId;
                    model.OrderItem = new MenuzRus.Models.OrderItem();
                    model.OrderItem.id = Item.id;
                    model.OrderItem.Description = Item.Description;
                    model.OrderItem.Name = Item.Name;
                    if (Item.ItemPrices.Any())
                        model.OrderItem.Price = (Decimal)Item.ItemPrices.OrderByDescending(m => m.DateCreated).Take(1).Select(m => m.Price).FirstOrDefault();
                    IEnumerable<Services.ItemProduct> ItemProducts = Item.ItemProducts.OrderByDescending(m => (Int32)m.Type);
                    if (ItemProducts != null) {
                        model.OrderItem.OrderItemProducts = new List<OrderItemProduct>();
                        foreach (Services.ItemProduct itemProduct in ItemProducts) {
                            OrderItemProduct orderItemProduct = new OrderItemProduct();
                            Item item = itemService.GetItem(itemProduct.id);
                            orderItemProduct.id = itemProduct.id;
                            orderItemProduct.Type = EnumHelper<Common.ProductType>.Parse(itemProduct.Type.ToString());
                            orderItemProduct.OrderItemProductAssociations = new List<OrderItemProductAssociation>();
                            foreach (Services.ItemProductAssociation itemProductAssosiation in itemProduct.ItemProductAssociations) {
                                OrderItemProductAssociation orderItemProductAssosiation = new OrderItemProductAssociation();
                                item = itemService.GetItem(itemProductAssosiation.ItemId);
                                orderItemProductAssosiation.id = itemProductAssosiation.id;
                                orderItemProductAssosiation.Name = item.Name;
                                orderItemProductAssosiation.ShortName = item.Name.Ellipsis(35);
                                orderItemProductAssosiation.Description = item.Description;
                                if (item.ItemPrices.Any())
                                    orderItemProductAssosiation.Price = (Decimal)item.ItemPrices.OrderByDescending(m => m.DateCreated).Take(1).Select(m => m.Price).FirstOrDefault();
                                orderItemProduct.OrderItemProductAssociations.Add(orderItemProductAssosiation);
                            }
                            model.OrderItem.OrderItemProducts.Add(orderItemProduct);
                        }
                    }
                }

                var partial = RenderViewToString(this.ControllerContext, "_CheckItemPartial", model);
                return partial;
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