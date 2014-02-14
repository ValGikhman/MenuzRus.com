using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MenuzRus.Models;
using Services;

namespace MenuzRus.Controllers {

    public class ItemController : BaseController {

        #region item

        public ActionResult Edit(Int32? id) {
            try {
                ItemModel model = new ItemModel();
                Item item;
                Services service = new Services();
                if (id.HasValue) {
                    item = service.GetItem(id.Value);
                    if (item != null) {
                        model.Active = item.Active ? Common.Status.Active : Common.Status.NotActive;
                        model.id = item.id;
                        model.Name = item.Name;
                        model.CategoryId = item.CategoryId;
                        model.Description = item.Description;
                        model.ShowAsPrice = item.ShowAsPrice;
                        model.ImageUrl = String.Format("{0}?{1}", item.ImageUrl, Guid.NewGuid().ToString("N"));
                    }
                }

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
            item.ImageUrl = model.Image != null ? String.Format("{0}{1}", model.id, Path.GetExtension(model.Image.FileName)) : null;
            item.Active = (model.Active == Common.Status.Active);
            item.ShowAsPrice = model.ShowAsPrice;
            String fileName = (model.Image == null ? model.ImageUrl : model.Image.FileName);
            String path = Path.Combine(Server.MapPath("~/Images/Menus/"), SessionData.customer.id.ToString(), "Items", String.Format("{0}{1}", model.id, Path.GetExtension(fileName)));
            if (model.Image != null) {
                model.Image.SaveAs(path);
            }
            else {
                String imageUrl = service.GetItem(item.id).ImageUrl;
                path = Path.Combine(Server.MapPath("~/Images/Menus/"), SessionData.customer.id.ToString(), "Items", imageUrl);
                if (System.IO.File.Exists(path)) {
                    System.IO.File.Delete(path);
                }
            } if (!service.SaveItem(item))
                return RedirectToAction("Index", "Error");

            return RedirectToAction("Style", "YourMenu");
        }

        #endregion item
    }
}