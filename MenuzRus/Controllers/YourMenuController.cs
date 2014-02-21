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

        public ActionResult Index(Int32? id) {
            return View(GetModel(id));
        }

        [HttpPost]
        public ActionResult SaveOrder(String ids, String type) {
            Services service = new Services();
            if (!service.SaveOrder(ids, type))
                return RedirectToAction("Index", "Error");

            return View("Index", GetModel(0));
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

            return View(GetModel(0));
        }

        #region private

        private YourMenuModel GetModel(Int32? id) {
            String wallDir = Server.MapPath("~/Images/Backgrounds/Wall/thumbnails");
            String pagesDir = Server.MapPath("~/Images/Backgrounds/Pages/thumbnails");
            Services service = new Services();
            YourMenuModel model = new YourMenuModel();
            model.Categories = service.GetCategories(id.HasValue ? id.Value : 0);
            model.Menus = service.GetMenus(SessionData.customer.id);
            model.MenuId = id.HasValue ? id.Value : 0;
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