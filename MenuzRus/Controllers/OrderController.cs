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

namespace MenuzRus.Controllers {

    public class OrderController : BaseController {

        #region order

        [HttpPost]
        public ActionResult DeleteOrder(Int32? id) {
            return null;
        }

        [HttpPost]
        public ActionResult SaveOrder(Services.Floor model) {
            return null;
        }

        [HttpGet]
        public ActionResult ShowOrder(Int32 id) {
            return PartialView("_CheckPartial");
        }

        [HttpGet]
        public ActionResult Table(Int32? id) {
            OrderModel model = GetTableModel(id);
            return View(model);
        }

        [HttpGet]
        public ActionResult Tables(Int32? id) {
            return View(GetTablesModel(id));
        }

        #endregion order

        #region private

        private OrderModel GetTableModel(Int32? id) {
            CategoryService categoryService = new CategoryService();
            MenuService menuService = new MenuService();
            OrderModel model = new OrderModel();
            model.Order = new MenuDesignerModel();
            try {
                model.Order.Categories = categoryService.GetAllCategories(Common.CategoryType.Menu);
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