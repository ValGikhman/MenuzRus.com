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
        private IFloorService _floorService;

        public FloorController(ISessionData sessionData, IFloorService floorService)
            : base(sessionData) {
            _floorService = floorService;
        }

        #region floor

        [HttpPost]
        public ActionResult DeleteFloor(Int32? id) {
            try {
                if (!_floorService.DeleteFloor(id)) {
                    return RedirectToAction("Index", "Error");
                }
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }

            return null;
        }

        [CheckUserSession]
        public ActionResult Index(Int32? id) {
            return View(GetModel(id));
        }

        [HttpPost]
        public ActionResult SaveFloor(Models.Floor model) {
            Services.Floor floor = new Services.Floor();
            try {
                floor.id = model.id;
                floor.CustomerId = SessionData.customer.id;
                floor.Name = model.Name;
                floor.Description = floor.Description;
                floor.Height = model.Height;
                floor.Width = model.Width;
                Int32 newId = _floorService.SaveFloor(floor);
                if (newId == 0)
                    return RedirectToAction("Index", "Error");

                SessionData.floor.id = newId;
                return Json(newId);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
            return null;
        }

        [HttpPost]
        public ActionResult SaveTables(String model) {
            JavaScriptSerializer js;
            List<Services.Table> tables;

            try {
                js = new JavaScriptSerializer();
                tables = js.Deserialize<List<Services.Table>>(model);

                _floorService.SaveTables(tables, SessionData.floor.id);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }

            return Json(SessionData.floor.id);
        }

        #endregion floor

        #region private

        public String GetTables(Int32 id) {
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
                              var.Height
                          }).ToList();
            return result.ToJson();
        }

        private FloorModel GetModel(Int32? id) {
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