using System;
using System.Web.Mvc;
using MenuzRus.Models;

namespace MenuzRus.Controllers {

    public class MenuController : Controller {

        public ActionResult Index(Int32? id) {
            return View(GetModel(id.HasValue ? id.Value : 0));
        }

        #region private

        private YourMenuModel GetModel(Int32? id) {
            Services service = new Services();
            YourMenuModel model = new YourMenuModel();
            if (model.Menu == null)
                model.Menu = new Menu();
            model.Menu.id = id.HasValue ? id.Value : 1;
            model.Categories = service.GetCategories(model.Menu.id);
            model.Settings = service.GetSettings(model.MyCompany.id);
            return model;
        }

        #endregion private
    }
}