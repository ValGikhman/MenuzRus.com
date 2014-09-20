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

    public class DesignerController : BaseController {

        [CheckUserSession]
        public ActionResult Menu() {
            return View("MenuProduct", GetModel(Common.CategoryType.Menu));
        }

        [CheckUserSession]
        public ActionResult Product() {
            return View("MenuProduct", GetModel(Common.CategoryType.Product));
        }

        #region private

        private DesignerModel GetModel(Common.CategoryType type) {
            CategoryService categoryService = new CategoryService();
            DesignerModel model = new DesignerModel();
            try {
                model.CategoryType = type;
                model.Categories = categoryService.GetCategories(SessionData.customer.id, type);
                return model;
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                categoryService = null;
            }
            return null;
        }

        #endregion private
    }
}