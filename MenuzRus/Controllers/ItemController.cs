using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using MenuzRus.Models;
using Services;

namespace MenuzRus.Controllers {

    public class ItemController : BaseController {

        #region item

        [HttpGet]
        public String EditItem(Int32? id) {
            ItemModel model = new ItemModel();
            model = GetModel(model, id);
            ViewData.Model = model;

            using (StringWriter sw = new StringWriter()) {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, "_ItemPartial");
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }

        public ActionResult Index(Int32? id) {
            try {
                ItemModel model = new ItemModel();
                model = GetModel(model, id);

                return View(model);
            }
            catch {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Save(ItemModel model) {
            Services service = new Services();
            Item item = new Item();
            item.id = model.id;
            item.CategoryId = model.CategoryId;
            item.Name = model.Name;
            item.Description = model.Description;
            item.ImageUrl = model.ImageUrl;
            if (model.Image != null) {
                if (item.id == 0)
                    item.ImageUrl = Path.GetExtension(model.Image.FileName);
                else
                    item.ImageUrl = String.Format("{0}{1}", model.id, Path.GetExtension(model.Image.FileName));
            }
            item.Active = (model.Active == Common.Status.Active);
            item.ShowAsPrice = model.ShowAsPrice;

            Int32 result = service.SaveItem(item);
            if (result == 0)
                return RedirectToAction("Index", "Error");

            String fileName = (model.Image == null ? model.ImageUrl : model.Image.FileName);
            String path = Path.Combine(Server.MapPath("~/Images/Menus/"), SessionData.contact.id.ToString(), "Items", String.Format("{0}{1}", result, Path.GetExtension(fileName)));
            if (model.Image == null && model.ImageUrl == null) {
                if (System.IO.File.Exists(path)) {
                    System.IO.File.Delete(path);
                }
            }
            else {
                model.Image.SaveAs(path);
            }

            return RedirectToAction("Index", "YourMenu");
        }

        #endregion item

        #region private

        private ItemModel GetModel(ItemModel model, Int32? id) {
            //set for new or existing category
            Item item;
            Services service = new Services();
            model.Categories = service.GetCategories(SessionData.customer.id, Common.Monitor.First).Select(m => new ListItem(m.Name, m.id.ToString())).ToList();
            if (id.HasValue) {
                item = service.GetItem((Int32)id.Value);
                if (item != null) {
                    model.Active = item.Active ? Common.Status.Active : Common.Status.NotActive;
                    model.id = item.id;
                    model.CategoryId = item.CategoryId;
                    model.Name = item.Name;
                    model.Description = item.Description;
                    model.ShowAsPrice = item.ShowAsPrice;
                    model.ImageUrl = String.Format("{0}?{1}", item.ImageUrl, Guid.NewGuid().ToString("N"));
                }
            }
            return model;
        }

        #endregion private
    }
}