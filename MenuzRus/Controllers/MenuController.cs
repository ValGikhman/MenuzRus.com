using System;
using System.Web.Mvc;
using MenuzRus.Models;

namespace MenuzRus.Controllers {

    public class MenuController : BaseController {

        public ActionResult Index(String monitor) {
            return View(GetModel(!String.IsNullOrEmpty(monitor) ? Utility.GetEnumItem<Common.Monitor>(monitor) : Common.Monitor.First));
        }

        #region private

        private YourMenuModel GetModel(Common.Monitor monitor) {
            Services service = new Services();
            YourMenuModel model = new YourMenuModel();
            model.Categories = service.GetCategories(SessionData.customer.id, monitor);
            model.Settings = service.GetSettings(SessionData.customer.id);
            model.monitor = monitor;
            return model;
        }

        #endregion private
    }
}