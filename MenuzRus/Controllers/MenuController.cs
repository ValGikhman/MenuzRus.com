using System;
using System.IO;
using System.Web.Mvc;
using MenuzRus.Models;

namespace MenuzRus.Controllers {

    public class MenuController : BaseController {

        #region Private Fields

        private ICategoryService _categoryService;
        private ISettingsService _settingsService;

        #endregion Private Fields

        #region Public Constructors

        public MenuController(ISessionData sessionData, ICategoryService categoryService, ISettingsService settingsService)
            : base(sessionData) {
            _categoryService = categoryService;
            _settingsService = settingsService;
        }

        #endregion Public Constructors

        #region Public Methods

        [CheckUserSession]
        public ActionResult Index(Int32? id) {
            return View(GetModel(id));
        }

        #endregion Public Methods

        #region private

        private MenuDesignerModel GetModel(Int32? id) {
            String wallDir = Server.MapPath("~/Images/Backgrounds/Wall/thumbnails");
            String pagesDir = Server.MapPath("~/Images/Backgrounds/Pages/thumbnails");

            MenuDesignerModel model = new MenuDesignerModel();
            //List<Services.MenuItem> categories = new List<Services.MenuItem>();
            try {
                if (model.Menu == null) {
                    model.Menu = new Menu();
                }

                model.Menu.id = id.HasValue ? id.Value : 1;
                //model.Categories = _categoryService.GetMenuCategories(SessionData.customer.id, CommonUnit.CategoryType.Menu);

                model.Settings = _settingsService.GetSettings(model.MyCompany.id);
                model.Settings = _settingsService.GetSettings(SessionData.customer.id);

                // Backgrounds
                if (!model.Settings.ContainsKey(CommonUnit.Settings.PageBackground.ToString())) {
                    model.Settings.Add(CommonUnit.Settings.PageBackground.ToString(), "");
                }

                if (!model.Settings.ContainsKey(CommonUnit.Settings.WallBackground.ToString())) {
                    model.Settings.Add(CommonUnit.Settings.WallBackground.ToString(), "");
                }

                //Color
                if (!model.Settings.ContainsKey(CommonUnit.Settings.CategoryColor.ToString())) {
                    model.Settings.Add(CommonUnit.Settings.CategoryColor.ToString(), "");
                }

                if (!model.Settings.ContainsKey(CommonUnit.Settings.CategoryDescriptionColor.ToString())) {
                    model.Settings.Add(CommonUnit.Settings.CategoryDescriptionColor.ToString(), "");
                }

                if (!model.Settings.ContainsKey(CommonUnit.Settings.ItemColor.ToString())) {
                    model.Settings.Add(CommonUnit.Settings.ItemColor.ToString(), "");
                }

                if (!model.Settings.ContainsKey(CommonUnit.Settings.ItemDescriptionColor.ToString())) {
                    model.Settings.Add(CommonUnit.Settings.ItemDescriptionColor.ToString(), "");
                }

                if (!model.Settings.ContainsKey(CommonUnit.Settings.PriceColor.ToString())) {
                    model.Settings.Add(CommonUnit.Settings.PriceColor.ToString(), "");
                }

                //Font Size
                if (!model.Settings.ContainsKey(CommonUnit.Settings.CategoryFontSize.ToString())) {
                    model.Settings.Add(CommonUnit.Settings.CategoryFontSize.ToString(), "");
                }

                if (!model.Settings.ContainsKey(CommonUnit.Settings.CategoryDescriptionFontSize.ToString())) {
                    model.Settings.Add(CommonUnit.Settings.CategoryDescriptionFontSize.ToString(), "");
                }

                if (!model.Settings.ContainsKey(CommonUnit.Settings.ItemFontSize.ToString())) {
                    model.Settings.Add(CommonUnit.Settings.ItemFontSize.ToString(), "");
                }

                if (!model.Settings.ContainsKey(CommonUnit.Settings.ItemDescriptionFontSize.ToString())) {
                    model.Settings.Add(CommonUnit.Settings.ItemDescriptionFontSize.ToString(), "");
                }

                if (!model.Settings.ContainsKey(CommonUnit.Settings.PriceFontSize.ToString())) {
                    model.Settings.Add(CommonUnit.Settings.PriceFontSize.ToString(), "");
                }

                // Others
                if (!model.Settings.ContainsKey(CommonUnit.Settings.ShowHiddenItems.ToString())) {
                    model.Settings.Add(CommonUnit.Settings.ShowHiddenItems.ToString(), "");
                }

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
            }
            return null;
        }

        #endregion private
    }
}