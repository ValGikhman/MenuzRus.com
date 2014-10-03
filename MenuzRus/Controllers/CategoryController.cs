using System;
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
            CategoryService service = new CategoryService();
            try {
                if (!service.DeleteCategory(id))
                    return RedirectToAction("Index", "Error");
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }

            return Json("OK");
        }

        [HttpGet]
        public String EditCategory(Int32 id, Common.CategoryType type) {
            try {
                return RenderViewToString(this.ControllerContext, "_CategoryPartial", GetModel(id, type));
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
            return null;
        }

        [HttpPost]
        public ActionResult Save(CategoryModel model) {
            CategoryService service = new CategoryService();
            Services.Category category = new Services.Category();
            try {
                category.id = model.id;
                category.Name = model.Name;
                category.Description = model.Description;
                category.Status = (Int32)Common.Status.Active;
                category.Type = (Int32)model.Type;
                category.ImageUrl = model.ImageUrl;
                category.CustomerId = SessionData.customer.id;
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
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                service = null;
            }
            // Default menuDesigner
            return RedirectToAction("Index", "Designer", new { id = SessionData.menu.id });
        }

        #region private

        private CategoryModel GetModel(Int32 id, Common.CategoryType type) {
            CategoryModel model = new CategoryModel();
            try {
                //set for new or existing category
                model.id = id;
                Services.Category category;
                CategoryService categoryService = new CategoryService();
                category = categoryService.GetCategory(id);
                model.Type = type;
                if (category != null) {
                    model.id = category.id;
                    model.Name = category.Name;
                    model.Description = category.Description;
                    model.Status = (Common.Status)category.Status;
                    model.Type = (Common.CategoryType)category.Type;
                    model.ImageUrl = category.ImageUrl;
                }
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
            return model;
        }

        #endregion private
    }
}