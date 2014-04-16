using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using MenuzRus.Models;
using Newtonsoft.Json;
using Services;

namespace MenuzRus.Controllers {

    public class ProductController : BaseController {

        public ActionResult Index(Int32? id) {
            return View(GetModel(id));
        }

        #region private

        private ProductModel GetModel(Int32? id) {
            CategoryService categoryService = new CategoryService();
            ProductModel model = new ProductModel();
            try {
                if (model.Categories == null)
                    model.Categories = new List<Category>();
                model.Categories = categoryService.GetCategories(model.Menu.id, Common.CategoryType.Inventory);
                return model;
            }
            catch (Exception ex) {
            }
            finally {
                categoryService = null;
            }
            return null;
        }

        #endregion private
    }
}