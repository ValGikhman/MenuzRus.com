using System;
using System.Web.Mvc;
using MenuzRus.Models;

namespace MenuzRus.Controllers {

    public class MenuController : BaseController {

        public ActionResult Index(Int32? id) {
            return View(GetModel(id));
        }

        #region private

        private MenuDesignerModel GetModel(Int32? id) {
            CategoryService categoryService = new CategoryService();
            SettingsService settingsService = new SettingsService();
            MenuDesignerModel model = new MenuDesignerModel();
            try {
                if (model.Menu == null)
                    model.Menu = new Menu();
                model.Menu.id = id.HasValue ? id.Value : 1;
                model.Categories = categoryService.GetCategories(model.Menu.id, Common.CategoryType.Menu);
                model.Settings = settingsService.GetSettings(model.MyCompany.id);
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