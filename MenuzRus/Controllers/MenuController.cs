using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using MenuzRus.Models;

namespace MenuzRus.Controllers {

    public class MenuController : BaseController {

        [CheckUserSession]
        public ActionResult Index(Int32? id) {
            return View(GetModel(id));
        }

        #region private

        private MenuDesignerModel GetModel(Int32? id) {
            String wallDir = Server.MapPath("~/Images/Backgrounds/Wall/thumbnails");
            String pagesDir = Server.MapPath("~/Images/Backgrounds/Pages/thumbnails");

            CategoryService categoryService = new CategoryService();
            SettingsService settingsService = new SettingsService();
            MenuDesignerModel model = new MenuDesignerModel();
            List<Services.MenuItem> categories = new List<Services.MenuItem>();
            try {
                if (model.Menu == null)
                    model.Menu = new Menu();
                model.Menu.id = id.HasValue ? id.Value : 1;
                model.MenuCategories = model.ConvertToCategory(categoryService.GetMenuCategories(SessionData.customer.id, Common.CategoryType.Menu));

                model.Settings = settingsService.GetSettings(model.MyCompany.id);
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
                return model;
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                categoryService = null;
                settingsService = null;
            }
            return null;
        }

        #endregion private
    }
}