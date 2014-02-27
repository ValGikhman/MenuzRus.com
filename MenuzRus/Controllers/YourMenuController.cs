using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using MenuzRus.Models;
using Newtonsoft.Json;
using Services;

namespace MenuzRus.Controllers {

    public class YourMenuController : BaseController {

        [HttpPost]
        public ActionResult DeleteMenu(Int32? id) {
            Services service = new Services();
            if (!service.DeleteMenu(id))
                return RedirectToAction("Index", "Error");

            return RedirectToAction("Index");
        }

        public ActionResult Index(Int32? id) {
            return View(GetModel(id));
        }

        [HttpPost]
        public ActionResult SaveMenu(Int32? id, String name) {
            Services service = new Services();
            Menus menu = new Menus();
            menu.id = Convert.ToInt32(id);
            menu.CustomerId = SessionData.customer.id;
            menu.Name = name;
            menu.Description = null;
            Int32 newId = service.SaveMenu(menu);
            if (newId == 0)
                return RedirectToAction("Index", "Error");

            SessionData.menu.id = newId;
            return Json(newId);
        }

        [HttpPost]
        public ActionResult SaveOrder(String ids, String type) {
            Services service = new Services();
            if (!service.SaveOrder(ids, type))
                return RedirectToAction("Index", "Error");

            return View(GetModel(SessionData.menu.id));
        }

        [HttpPost]
        public ActionResult SaveSettings(SettingModel model) {
            Services service = new Services();
            Setting setting = new Setting();
            setting.Type = Utility.GetEnumItem<Common.Settings>(model.Type).ToString();
            switch (Utility.GetEnumItem<Common.Settings>(model.Type)) {
                case Common.Settings.PageBackground:
                case Common.Settings.WallBackground:
                case Common.Settings.Image1:
                case Common.Settings.Image2:
                    setting.Value = Path.GetFileName(model.Value.Replace("/", "\\"));
                    break;

                default:
                    setting.Value = model.Value;
                    break;
            }
            if (!service.SaveSetting(setting))
                return RedirectToAction("Index", "Error");

            return View(GetModel(SessionData.menu.id));
        }

        #region private

        private YourMenuModel GetModel(Int32? id) {
            String wallDir = Server.MapPath("~/Images/Backgrounds/Wall/thumbnails");
            String pagesDir = Server.MapPath("~/Images/Backgrounds/Pages/thumbnails");
            Services service = new Services();
            YourMenuModel model = new YourMenuModel();
            model.Menus = service.GetMenus(SessionData.customer.id);
            if (model.Menu == null) {
                model.Menu = new Menu();
            }
            model.Menu.id = id.HasValue ? id.Value : 0;
            if (model.Menu.id == 0 && model.Menus.Count() > 0) {
                model.Menu.id = model.Menus[0].id;
                model.Menu.Name = model.Menus[0].Name;
                SessionData.menu.Name = model.Menus[0].Name;
            }
            else if (model.Menu.id != 0)
                model.Menu.Name = model.Menus.Where(m => m.id == model.Menu.id).FirstOrDefault().Name;

            SessionData.menu.id = model.Menu.id;
            SessionData.menu.Name = model.Menu.Name;
            model.Categories = service.GetCategories(model.Menu.id);
            model.Settings = service.GetSettings(SessionData.customer.id);

            if (System.IO.Directory.Exists(wallDir)) {
                model.Wallpapers = Directory.EnumerateFiles(wallDir, "*.jpg");
            }

            if (System.IO.Directory.Exists(pagesDir)) {
                model.PageBackgrounds = Directory.EnumerateFiles(pagesDir, "*.png");
            }
            return model;
        }

        #endregion private
    }
}