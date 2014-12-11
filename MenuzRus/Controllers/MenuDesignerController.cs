using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MenuzRus.Models;
using Newtonsoft.Json;
using Services;

namespace MenuzRus.Controllers {

    public class MenuDesignerController : BaseController {

        [HttpPost]
        public ActionResult DeleteMenu(Int32 menuId) {
            MenuService service = new MenuService();
            try {
                if (!service.DeleteMenu(menuId))
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

        [CheckUserSession]
        public ActionResult Designer() {
            CategoryService categoryService = new CategoryService();
            ItemService itemService = new ItemService();
            DesignerModel model = new DesignerModel();
            try {
                model.Categories = categoryService.GetCategories(SessionData.customer.id, Common.CategoryType.Menu);
                model.Selected = categoryService.GetMenuDesignerItems(SessionData.customer.id);

                return View("Designer", model);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                categoryService = null;
                itemService = null;
            }
            return null;
        }

        [CheckUserSession]
        public ActionResult Index(Int32? id) {
            return View(GetModel(id));
        }

        [HttpPost]
        public ActionResult SaveMenu(Int32? id, String name) {
            MenuService service = new MenuService();
            Services.Menu menu = new Services.Menu();
            try {
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
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                service = null;
            }
            return null;
        }

        [HttpPost]
        public Boolean SaveMenuItems(Int32 menuId, String model) {
            MenuService service = new MenuService();
            List<MenuItem> model2Save;
            try {
                model2Save = SetModel(model, menuId);
                service.SaveMenuItems(model2Save);
            }
            catch (Exception ex) {
                base.Log(ex);
                return false;
            }
            finally {
                service = null;
                SessionData.item = null;
            }
            return true;
        }

        [HttpPost]
        public ActionResult SaveOrder(String ids, String type) {
            SettingsService service = new SettingsService();
            try {
                if (!service.SaveOrder(ids, type))
                    return RedirectToAction("Index", "Error");

                return Json("OK");
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
        public ActionResult SaveSettings(SettingModel model) {
            String retVal = "OK";
            SettingsService service = new SettingsService();
            Setting setting = new Setting();
            try {
                setting.Type = EnumHelper<Common.Settings>.Parse(model.Type).ToString();
                switch (EnumHelper<Common.Settings>.Parse(model.Type)) {
                    case Common.Settings.PageBackground:
                    case Common.Settings.WallBackground:
                        setting.Value = Path.GetFileName(model.Value.Replace("/", "\\"));
                        break;

                    default:
                        setting.Value = model.Value;
                        break;
                }
                if (!service.SaveSetting(setting)) {
                    base.Log(SessionData.exeption);
                    retVal = SessionData.exeption.Message;
                }

                return Json(retVal);
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
        public void ToggleMenuItem(Int32 itemId, Boolean selected) {
            try {
                if (selected) {
                    SaveMenuItem(itemId);
                }
                else {
                    DeleteMenuItem(itemId);
                }
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
        }

        private void DeleteMenuItem(Int32 id) {
            ItemService service = new ItemService();
            try {
                service.DeleteMenuItem(id);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
        }

        private void SaveMenuItem(Int32 id) {
            ItemService service = new ItemService();
            try {
                service.SaveMenuItem(id);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
        }

        private List<MenuItem> SetModel(String model, Int32 menuId) {
            JavaScriptSerializer objJavascript = new JavaScriptSerializer();
            Char[] colonDelimiter = new char[] { ':' };
            List<MenuItem> Items2Save;

            Array values = (Array)objJavascript.DeserializeObject(model);
            if (values.Length > 0) {
                Items2Save = new List<MenuItem>();
                foreach (String value in values) {
                    Array vars = value.Split(colonDelimiter, StringSplitOptions.RemoveEmptyEntries);
                    Items2Save.Add(new MenuItem() { ItemId = Int32.Parse(vars.GetValue(0).ToString()), MenuId = menuId });
                }
                return Items2Save;
            }
            return null;
        }

        #region private

        private MenuDesignerModel GetModel(Int32? id) {
            String wallDir = Server.MapPath("~/Images/Backgrounds/Wall/thumbnails");
            String pagesDir = Server.MapPath("~/Images/Backgrounds/Pages/thumbnails");
            SettingsService settingsService = new SettingsService();
            CategoryService categoryService = new CategoryService();
            MenuService menuService = new MenuService();
            MenuDesignerModel model = new MenuDesignerModel();
            List<Services.MenuDesign> menuDesign = new List<Services.MenuDesign>();

            try {
                SessionData.menu = new Services.Menu();
                model.Menus = menuService.GetMenus(SessionData.customer.id);
                if (model.Menu == null) {
                    model.Menu = new Models.Menu();
                }
                model.Menu.id = id.HasValue ? id.Value : 0;
                model.Menu.Name = String.Empty;
                if (model.Menu.id == 0 && model.Menus.Count() > 0) {
                    model.Menu.id = model.Menus[0].id;
                    model.Menu.Name = model.Menus[0].Name;
                    SessionData.menu.Name = model.Menus[0].Name;
                }
                else if (model.Menu.id != 0)
                    model.Menu.Name = model.Menus.Where(m => m.id == model.Menu.id).FirstOrDefault().Name;

                SessionData.menu.id = model.Menu.id;
                SessionData.menu.Name = model.Menu.Name;

                model.Settings = settingsService.GetSettings(SessionData.customer.id);
                // Backgrounds
                if (!model.Settings.ContainsKey(Common.Settings.PageBackground.ToString()))
                    model.Settings.Add(Common.Settings.PageBackground.ToString(), "");
                if (!model.Settings.ContainsKey(Common.Settings.WallBackground.ToString()))
                    model.Settings.Add(Common.Settings.WallBackground.ToString(), "");
                //Color
                if (!model.Settings.ContainsKey(Common.Settings.CategoryColor.ToString()))
                    model.Settings.Add(Common.Settings.CategoryColor.ToString(), "");
                if (!model.Settings.ContainsKey(Common.Settings.CategoryDescriptionColor.ToString()))
                    model.Settings.Add(Common.Settings.CategoryDescriptionColor.ToString(), "");
                if (!model.Settings.ContainsKey(Common.Settings.ItemColor.ToString()))
                    model.Settings.Add(Common.Settings.ItemColor.ToString(), "");
                if (!model.Settings.ContainsKey(Common.Settings.ItemDescriptionColor.ToString()))
                    model.Settings.Add(Common.Settings.ItemDescriptionColor.ToString(), "");
                if (!model.Settings.ContainsKey(Common.Settings.PriceColor.ToString()))
                    model.Settings.Add(Common.Settings.PriceColor.ToString(), "");
                //Font Size
                if (!model.Settings.ContainsKey(Common.Settings.CategoryFontSize.ToString()))
                    model.Settings.Add(Common.Settings.CategoryFontSize.ToString(), "");
                if (!model.Settings.ContainsKey(Common.Settings.CategoryDescriptionFontSize.ToString()))
                    model.Settings.Add(Common.Settings.CategoryDescriptionFontSize.ToString(), "");
                if (!model.Settings.ContainsKey(Common.Settings.ItemFontSize.ToString()))
                    model.Settings.Add(Common.Settings.ItemFontSize.ToString(), "");
                if (!model.Settings.ContainsKey(Common.Settings.ItemDescriptionFontSize.ToString()))
                    model.Settings.Add(Common.Settings.ItemDescriptionFontSize.ToString(), "");
                if (!model.Settings.ContainsKey(Common.Settings.PriceFontSize.ToString()))
                    model.Settings.Add(Common.Settings.PriceFontSize.ToString(), "");
                // Others
                if (!model.Settings.ContainsKey(Common.Settings.ShowHiddenItems.ToString()))
                    model.Settings.Add(Common.Settings.ShowHiddenItems.ToString(), "");

                if (System.IO.Directory.Exists(wallDir)) {
                    model.Wallpapers = Directory.EnumerateFiles(wallDir, "*.jpg");
                }

                if (System.IO.Directory.Exists(pagesDir)) {
                    model.PageBackgrounds = Directory.EnumerateFiles(pagesDir, "*.png");
                }

                model.Categories = categoryService.GetMenuDesigner(SessionData.customer.id);

                return model;
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                settingsService = null;
                categoryService = null;
                menuService = null;
            }
            return null;
        }

        #endregion private
    }
}