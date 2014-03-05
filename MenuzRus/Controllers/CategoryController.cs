﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using MenuzRus.Controllers;
using MenuzRus.Models;
using Services;

namespace MenuzRus {

    public class CategoryController : BaseController {

        [HttpPost]
        public ActionResult DeleteCategory(Int32? id) {
            Services service = new Services();
            if (!service.DeleteCategory(id))
                return RedirectToAction("Index", "Error");

            return Json("OK");
        }

        [HttpGet]
        public String EditCategory(Int32? id) {
            CategoryModel model = new CategoryModel();
            model = GetModel(model, id);
            ViewData.Model = model;

            using (StringWriter sw = new StringWriter()) {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, "_CategoryPartial");
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }

        public ActionResult Index(Int32? id) {
            try {
                CategoryModel model = new CategoryModel();
                model = GetModel(model, id);

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
            category.MenuId = model.MenuId;
            category.Name = model.Name;
            category.Description = model.Description;
            category.Side = model.Side.ToString();
            category.Active = true;
            category.ImageUrl = model.ImageUrl;
            if (model.Image != null) {
                if (category.id == 0)
                    category.ImageUrl = Path.GetExtension(model.Image.FileName);
                else
                    category.ImageUrl = String.Format("{0}{1}", model.id, Path.GetExtension(model.Image.FileName));
            }

            Int32 result = service.SaveCategory(category);
            if (result == 0)
                return RedirectToAction("Index", "Error");

            String fileName = (model.Image == null ? model.ImageUrl : model.Image.FileName);
            String path = Path.Combine(Server.MapPath("~/Images/Menus/"), SessionData.customer.id.ToString(), "Categories", String.Format("{0}{1}", result, Path.GetExtension(fileName)));
            if (model.Image == null && model.ImageUrl == null) {
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }
            else if (model.Image != null)
                model.Image.SaveAs(path);

            return RedirectToAction("Index", "YourMenu", new { id = model.MenuId });
        }

        #region private

        private CategoryModel GetModel(CategoryModel model, Int32? id) {
            //set for new or existing category
            model.id = id.HasValue ? id.Value : 0;
            Category category;
            Services service = new Services();
            model.Menus = service.GetMenus(SessionData.customer.id);
            model.MenuId = SessionData.menu.id;
            model.Active = Common.Status.Active;
            if (id.HasValue) {
                category = service.GetCategory((Int32)id.Value);
                if (category != null) {
                    model.id = category.id;
                    model.Name = category.Name;
                    model.Description = category.Description;
                    model.ImageUrl = category.ImageUrl;
                    model.MenuId = category.MenuId;
                    model.Side = Utility.GetEnumItem<Common.Side>(category.Side);
                }
            }
            return model;
        }

        #endregion private
    }
}