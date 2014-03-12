using System;
using System.Web.Mvc;
using MenuzRus.Models;

namespace MenuzRus.Controllers {

    public class MenuController : Controller {

        public ActionResult Index(Int32? id) {
            return View(GetModel(id));
        }

        #region private

        private YourMenuModel GetModel(Int32? id) {
            CategoryService categoryService = new CategoryService();
            SettingsService settingsService = new SettingsService();
            YourMenuModel model = new YourMenuModel();
            try {
                if (model.Menu == null)
                    model.Menu = new Menu();
                model.Menu.id = id.HasValue ? id.Value : 1;
                model.Categories = categoryService.GetCategories(model.Menu.id);
                model.Settings = settingsService.GetSettings(model.MyCompany.id);
                return model;
            }
            catch (Exception ex) {
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