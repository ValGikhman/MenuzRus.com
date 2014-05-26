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

    public class FloorController : BaseController {

        #region floor

        [HttpPost]
        public ActionResult DeleteFloor(Int32? id) {
            FloorService service = new FloorService();
            try {
                if (!service.DeleteFloor(id))
                    return RedirectToAction("Index", "Error");

                return RedirectToAction("Index");
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
            return null;
        }

        public ActionResult Index(Int32? id) {
            return View(GetModel(id));
        }

        [HttpPost]
        public ActionResult SaveFloor(Services.Floor model) {
            FloorService service = new FloorService();
            Services.Floor floor = new Services.Floor();
            try {
                floor.id = model.id;
                floor.CustomerId = SessionData.customer.id;
                floor.Name = model.Name;
                floor.Description = null;
                Int32 newId = service.SaveFloor(floor);
                if (newId == 0)
                    return RedirectToAction("Index", "Error");

                SessionData.menu.id = newId;
                return Json(newId);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                service = null;
            }
            return null;
        }

        [HttpPost]
        public ActionResult SaveTables(String tables) {
            FloorService service = new FloorService();
            try {
                service.SaveTables(new JavaScriptSerializer().Deserialize<List<Services.Table>>(tables));
            }
            catch (Exception ex) {
            }
            finally {
            }

            return Json("OK");
        }

        #endregion floor

        #region private

        public String GetTables(Int32 id) {
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

        private FloorModel GetModel(Int32? id) {
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