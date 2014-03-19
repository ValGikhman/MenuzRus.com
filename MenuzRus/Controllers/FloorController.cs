using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using MenuzRus.Models;
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
            }
            finally {
            }
            return null;
        }

        public ActionResult Index(Int32? id) {
            return View(GetModel(id));
        }

        [HttpPost]
        public ActionResult SaveFloor(Floor model) {
            FloorService service = new FloorService();
            Floors floor = new Floors();
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
            }
            finally {
                service = null;
            }
            return null;
        }

        #endregion floor

        #region private

        private FloorModel GetModel(Int32? id) {
            FloorService menuService = new FloorService();
            FloorModel model = new FloorModel();
            try {
                model.Floors = menuService.GetFloors(SessionData.customer.id);
                if (model.Floor == null) {
                    model.Floor = new Floor();
                }
                model.Floor.id = id.HasValue ? id.Value : 0;
                if (model.Floor.id == 0 && model.Floors.Count() > 0) {
                    model.Floor.id = model.Floors[0].id;
                    model.Floor.Name = model.Floors[0].Name;
                    SessionData.menu.Name = model.Floors[0].Name;
                }
                else if (model.Floor.id != 0)
                    model.Floor.Name = model.Floors.Where(m => m.id == model.Floor.id).FirstOrDefault().Name;

                return model;
            }
            catch (Exception ex) {
            }
            finally {
            }
            return null;
        }

        #endregion private
    }
}