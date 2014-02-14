using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MenuzRus.Controllers;
using MenuzRus.Models;
using Services;

namespace MenuzRus {

    public class CategoryController : BaseController {

        public ActionResult Edit(Int32? id) {
            try {
                CategoryModel model = new CategoryModel();
                Category category;
                Services service = new Services();
                if (id.HasValue) {
                    category = service.GetCategory((Int32)id);
                    if (category != null) {
                        model.Active = category.Active ? Common.Status.Active : Common.Status.NotActive;
                        model.id = category.id;
                        model.CustomerId = category.Customer.id;
                        model.Name = category.Name;
                        model.Description = category.Description;
                        model.ImageUrl = String.Format("{0}?{1}", category.ImageUrl, Guid.NewGuid().ToString("N"));
                        model.Monitor = Utility.GetEnumItem<Common.Monitor>(category.Monitor);
                        model.Side = Utility.GetEnumItem<Common.Side>(category.Side);
                    }
                }

                return View(model);
            }
            catch {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Save(CategoryModel model) {
            Services service = new Services();
            Category category = new Category();
            category.id = model.id;
            category.CustomerId = model.CustomerId;
            category.Name = model.Name;
            category.Name = model.Name;
            category.Description = model.Description;
            category.ImageUrl = model.Image != null ? String.Format("{0}{1}", model.id, Path.GetExtension(model.Image.FileName)) : null;
            category.Monitor = model.Monitor.ToString();
            category.Side = model.Side.ToString();
            category.Active = (model.Active == Common.Status.Active);
            String fileName = (model.Image == null ? model.ImageUrl : model.Image.FileName);
            String path = Path.Combine(Server.MapPath("~/Images/Menus/"), SessionData.customer.id.ToString(), "Categories", String.Format("{0}{1}", model.id, Path.GetExtension(fileName)));
            if (model.Image != null) {
                model.Image.SaveAs(path);
            }
            else {
                String imageUrl = service.GetCategory(category.id).ImageUrl;
                path = Path.Combine(Server.MapPath("~/Images/Menus/"), SessionData.customer.id.ToString(), "Categories", imageUrl);
                if (System.IO.File.Exists(path)) {
                    System.IO.File.Delete(path);
                }
            }
            if (!service.SaveCategory(category))
                return RedirectToAction("Index", "Error");

            return RedirectToAction("Style", "YourMenu", new { monitor = category.Monitor });
        }
    }
}