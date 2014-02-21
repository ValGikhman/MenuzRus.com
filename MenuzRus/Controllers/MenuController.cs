using System;
using System.Web.Mvc;
using MenuzRus.Models;

namespace MenuzRus.Controllers {

    public class MenuController : BaseController {

        public ActionResult Index(Int32? id) {
            return View(GetModel(id.HasValue ? id.Value : 0));
        }

        #region private

        private YourMenuModel GetModel(Int32 id) {
            Services service = new Services();
            YourMenuModel model = new YourMenuModel();
            model.Categories = service.GetCategories(id);
            model.Settings = service.GetSettings(SessionData.customer.id);
            model.MenuId = id;
            return model;
        }

        #endregion private
    }
}