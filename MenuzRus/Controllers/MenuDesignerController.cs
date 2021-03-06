﻿using System;
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

        #region Private Fields

        private ICategoryService _categoryService;
        private IItemService _itemService;
        private IMenuService _menuService;
        private ISettingsService _settingsService;

        #endregion Private Fields

        #region Public Constructors

        public MenuDesignerController(ISessionData sessionData, IMenuService menuService, ISettingsService settingsService, ICategoryService categoryService, IItemService itemService)
            : base(sessionData) {
            _menuService = menuService;
            _settingsService = settingsService;
            _categoryService = categoryService;
            _itemService = itemService;
        }

        #endregion Public Constructors

        //[HttpPost]
        //public ActionResult DeleteMenu(Int32 menuId) {
        //    MenuService service = new MenuService();
        //    try {
        //        if (!service.DeleteMenu(menuId))
        //            return RedirectToAction("Index", "Error");

        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex) {
        //        base.Log(ex);
        //    }
        //    finally {
        //    }
        //    return null;
        //}

        #region Public Methods

        [CheckUserSession]
        public ActionResult Designer() {
            DesignerModel model;

            try {
                model = new DesignerModel();

                model.Categories = _categoryService.GetCategories(SessionData.customer.id, CommonUnit.CategoryType.Menu);
                //model.Selected = _categoryService.GetMenuDesignerItems(SessionData.customer.id);
                model.ItemProducts = SessionData.item.ItemProducts;

                return View("Designer", model);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
            return null;
        }

        //[CheckUserSession]
        //public ActionResult Index(Int32? id) {
        //    return View(GetModel(id));
        //}

        //[HttpPost]
        //public ActionResult SaveMenu(Int32? id, String name) {
        //    Services.Menu menu = new Services.Menu();
        //    try {
        //        menu.id = Convert.ToInt32(id);
        //        menu.CustomerId = SessionData.customer.id;
        //        menu.Name = name;
        //        menu.Description = null;
        //        Int32 newId = _menuService.SaveMenu(menu);
        //        if (newId == 0) {
        //            return RedirectToAction("Index", "Error");
        //        }

        //        SessionData.menu.id = newId;
        //        return Json(newId);
        //    }
        //    catch (Exception ex) {
        //        base.Log(ex);
        //    }
        //    finally {
        //    }

        //    return null;
        //}

        //[HttpPost]
        //public Boolean SaveMenuItems(Int32 menuId, String model) {
        //    List<MenuItem> model2Save;

        //    try {
        //        model2Save = SetModel(model, menuId);
        //        _menuService.SaveMenuItems(model2Save);
        //    }
        //    catch (Exception ex) {
        //        base.Log(ex);
        //        return false;
        //    }
        //    finally {
        //        SessionData.item = null;
        //    }
        //    return true;
        //}

        [HttpPost]
        public ActionResult SaveOrder(String ids, String type) {
            try {
                if (!_settingsService.SaveOrder(ids, type))
                    return RedirectToAction("Index", "Error");

                return Json("OK");
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }

            return null;
        }

        [HttpPost]
        public ActionResult SaveSettings(SettingModel model) {
            String retVal = "OK";
            Setting setting;

            try {
                setting = new Setting();

                setting.Type = EnumHelper<CommonUnit.Settings>.Parse(model.Type).ToString();
                switch (EnumHelper<CommonUnit.Settings>.Parse(model.Type)) {
                    case CommonUnit.Settings.PageBackground:
                    case CommonUnit.Settings.WallBackground:
                        setting.Value = Path.GetFileName(model.Value.Replace("/", "\\"));
                        break;

                    default:
                        setting.Value = model.Value;
                        break;
                }
                if (!_settingsService.SaveSetting(setting, SessionData.customer.id)) {
                    base.Log(SessionData.exception);
                    retVal = SessionData.exception.Message;
                }

                return Json(retVal);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }

            return null;
        }

        [HttpPost]
        public void ToggleMenuItem(Int32 itemId, Boolean selected) {
            try {
                if (selected) {
                    //SaveMenuItem(itemId);
                }
                else {
                    //DeleteMenuItem(itemId);
                }
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void DeleteMenuItem(Int32 id) {
            try {
                _itemService.DeleteMenuItem(id);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
        }

        #endregion Private Methods

        //private void SaveMenuItem(Int32 id) {
        //    try {
        //        _itemService.SaveMenuItem(id);
        //    }
        //    catch (Exception ex) {
        //        base.Log(ex);
        //    }
        //    finally {
        //    }
        //}

        //private List<MenuItem> SetModel(String model, Int32 menuId) {
        //    JavaScriptSerializer objJavascript = new JavaScriptSerializer();
        //    Char[] colonDelimiter = new char[] { ':' };
        //    List<MenuItem> Items2Save;

        //    Array values = (Array)objJavascript.DeserializeObject(model);
        //    if (values.Length > 0) {
        //        Items2Save = new List<MenuItem>();
        //        foreach (String value in values) {
        //            Array vars = value.Split(colonDelimiter, StringSplitOptions.RemoveEmptyEntries);
        //            Items2Save.Add(new MenuItem() { ItemId = Int32.Parse(vars.GetValue(0).ToString()), MenuId = menuId });
        //        }
        //        return Items2Save;
        //    }
        //    return null;
        //}

        #region private

        //private MenuDesignerModel GetModel(Int32? id) {
        //    String wallDir = Server.MapPath("~/Images/Backgrounds/Wall/thumbnails");
        //    String pagesDir = Server.MapPath("~/Images/Backgrounds/Pages/thumbnails");

        //    MenuDesignerModel model;
        //    List<Services.MenuDesign> menuDesign = new List<Services.MenuDesign>();

        //    try {
        //        model = new MenuDesignerModel();

        //        SessionData.menu = new Services.Menu();
        //        model.Menus = _menuService.GetMenus(SessionData.customer.id);
        //        if (model.Menu == null) {
        //            model.Menu = new Models.Menu();
        //        }
        //        model.Menu.id = id.HasValue ? id.Value : 0;
        //        model.Menu.Name = String.Empty;
        //        if (model.Menu.id == 0 && model.Menus.Count() > 0) {
        //            model.Menu.id = model.Menus[0].id;
        //            model.Menu.Name = model.Menus[0].Name;
        //            SessionData.menu.Name = model.Menus[0].Name;
        //        }
        //        else if (model.Menu.id != 0)
        //            model.Menu.Name = model.Menus.Where(m => m.id == model.Menu.id).FirstOrDefault().Name;

        //        SessionData.menu.id = model.Menu.id;
        //        SessionData.menu.Name = model.Menu.Name;

        //        model.Settings = _settingsService.GetSettings(SessionData.customer.id);
        //        // Backgrounds
        //        if (!model.Settings.ContainsKey(CommonUnit.Settings.PageBackground.ToString())) {
        //            model.Settings.Add(CommonUnit.Settings.PageBackground.ToString(), "");
        //        }

        //        if (!model.Settings.ContainsKey(CommonUnit.Settings.WallBackground.ToString())) {
        //            model.Settings.Add(CommonUnit.Settings.WallBackground.ToString(), "");
        //        }
        //        //Color
        //        if (!model.Settings.ContainsKey(CommonUnit.Settings.CategoryColor.ToString())) {
        //            model.Settings.Add(CommonUnit.Settings.CategoryColor.ToString(), "");
        //        }

        //        if (!model.Settings.ContainsKey(CommonUnit.Settings.CategoryDescriptionColor.ToString())) {
        //            model.Settings.Add(CommonUnit.Settings.CategoryDescriptionColor.ToString(), "");
        //        }

        //        if (!model.Settings.ContainsKey(CommonUnit.Settings.ItemColor.ToString())) {
        //            model.Settings.Add(CommonUnit.Settings.ItemColor.ToString(), "");
        //        }

        //        if (!model.Settings.ContainsKey(CommonUnit.Settings.ItemDescriptionColor.ToString())) {
        //            model.Settings.Add(CommonUnit.Settings.ItemDescriptionColor.ToString(), "");
        //        }

        //        if (!model.Settings.ContainsKey(CommonUnit.Settings.PriceColor.ToString())) {
        //            model.Settings.Add(CommonUnit.Settings.PriceColor.ToString(), "");
        //        }

        //        //Font Size
        //        if (!model.Settings.ContainsKey(CommonUnit.Settings.CategoryFontSize.ToString())) {
        //            model.Settings.Add(CommonUnit.Settings.CategoryFontSize.ToString(), "");
        //        }

        //        if (!model.Settings.ContainsKey(CommonUnit.Settings.CategoryDescriptionFontSize.ToString())) {
        //            model.Settings.Add(CommonUnit.Settings.CategoryDescriptionFontSize.ToString(), "");
        //        }

        //        if (!model.Settings.ContainsKey(CommonUnit.Settings.ItemFontSize.ToString())) {
        //            model.Settings.Add(CommonUnit.Settings.ItemFontSize.ToString(), "");
        //        }

        //        if (!model.Settings.ContainsKey(CommonUnit.Settings.ItemDescriptionFontSize.ToString())) {
        //            model.Settings.Add(CommonUnit.Settings.ItemDescriptionFontSize.ToString(), "");
        //        }

        //        if (!model.Settings.ContainsKey(CommonUnit.Settings.PriceFontSize.ToString())) {
        //            model.Settings.Add(CommonUnit.Settings.PriceFontSize.ToString(), "");
        //        }

        //        // Others
        //        if (!model.Settings.ContainsKey(CommonUnit.Settings.ShowHiddenItems.ToString())) {
        //            model.Settings.Add(CommonUnit.Settings.ShowHiddenItems.ToString(), "");
        //        }

        //        if (System.IO.Directory.Exists(wallDir)) {
        //            model.Wallpapers = Directory.EnumerateFiles(wallDir, "*.jpg");
        //        }

        //        if (System.IO.Directory.Exists(pagesDir)) {
        //            model.PageBackgrounds = Directory.EnumerateFiles(pagesDir, "*.png");
        //        }

        //        model.Categories = _categoryService.GetMenuDesigner(SessionData.customer.id);

        //        return model;
        //    }
        //    catch (Exception ex) {
        //        base.Log(ex);
        //    }
        //    finally {
        //    }
        //    return null;
        //}

        #endregion private
    }
}