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

        [HttpPost]
        public ActionResult DeleteItem(Int32? id) {
            ItemService service = new ItemService();
            try {
                if (!service.DeleteItem(id))
                    return RedirectToAction("Index", "Error");

                return Json("OK");
            }
            catch (Exception ex) {
            }

            finally {
                service = null;
            }
            return null;
        }

        [HttpGet]
        public String EditItem(Int32? id) {
            ItemModel model = new ItemModel();
            try {
                model = GetModel(model, id);
                ViewData.Model = model;

                using (StringWriter sw = new StringWriter()) {
                    ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, "_ItemPartial");
                    ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                    viewResult.View.Render(viewContext, sw);
                    return sw.GetStringBuilder().ToString();
                }
            }
            catch (Exception ex) {
            }

            finally {
            }
            return null;
        }

        public ActionResult Index(Int32? id) {
            ItemModel model = new ItemModel();
            try {
                model = GetModel(model, id);
                return View(model);
            }
            catch (Exception ex) {
            }

            finally {
            }
            return null;
        }

        [HttpPost]
        public ActionResult Save(ItemModel model) {
            ItemService service = new ItemService();
            Item item = new Item();
            try {
                item.id = model.id;
                item.CategoryId = model.CategoryId;
                item.Name = model.Name;
                item.Description = model.Description;
                item.ImageUrl = model.ImageUrl;
                item.Active = (model.Active == Common.Status.Active);
                item.ShowAsPrice = model.ShowAsPrice;
                if (model.Image != null) {
                    if (item.id == 0)
                        item.ImageUrl = Path.GetExtension(model.Image.FileName);
                    else
                        item.ImageUrl = String.Format("{0}{1}", model.id, Path.GetExtension(model.Image.FileName));
                }

                Int32 result = service.SaveItem(item);
                if (result == 0)
                    return RedirectToAction("Index", "Error");

                String fileName = (model.Image == null ? model.ImageUrl : model.Image.FileName);
                String path = Path.Combine(Server.MapPath("~/Images/Menus/"), SessionData.contact.id.ToString(), "Items", String.Format("{0}{1}", result, Path.GetExtension(fileName)));
                if (model.Image == null && model.ImageUrl == null) {
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                }
                else if (model.Image != null)
                    model.Image.SaveAs(path);

                return RedirectToAction("Index", "YourMenu", new { id = SessionData.menu.id });
            }
            catch (Exception ex) {
            }
            finally {
                service = null;
            }
            return null;
        }

        #endregion item

        #region private

        private ItemModel GetModel(ItemModel model, Int32? id) {
            //set for new or existing category
            Item item;
            ItemService itemService = new ItemService();
            CategoryService categoryService = new CategoryService();
            try {
                model.Categories = categoryService.GetCategories(SessionData.menu.id);
                model.Active = Common.Status.Active;
                if (id.HasValue) {
                    item = itemService.GetItem((Int32)id.Value);
                    if (item != null) {
                        model.Active = item.Active ? Common.Status.Active : Common.Status.NotActive;
                        model.id = item.id;
                        model.CategoryId = item.CategoryId;
                        model.Name = item.Name;
                        model.Description = item.Description;
                        model.ShowAsPrice = item.ShowAsPrice;
                        model.ImageUrl = item.ImageUrl;
                    }
                }
                return model;
            }
            catch (Exception ex) {
            }
            finally {
                itemService = null;
                categoryService = null;
            }
            return null;
        }

        #endregion private
    }
}