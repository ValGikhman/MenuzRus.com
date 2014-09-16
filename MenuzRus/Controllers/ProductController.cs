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

        [CheckUserSession]
        public ActionResult Index(Int32? id) {
            return View(GetModel(id));
        }

        #region private

        private MenuDesignerModel GetModel(Int32? id) {
            CategoryService categoryService = new CategoryService();
            MenuService menuService = new MenuService();
            MenuDesignerModel model = new MenuDesignerModel();
            try {
                model.Menus = menuService.GetMenus(SessionData.customer.id);
                if (model.Menu == null) {
                    model.Menu = new MenuzRus.Models.Menu();
                }
                model.Menu.id = id.HasValue ? id.Value : 0;
                model.Menu.Name = String.Empty;

                if (model.Menu.id == 0 && model.Menus.Count() > 0) {
                    model.Menu.id = model.Menus[0].id;
                    model.Menu.Name = model.Menus[0].Name;
                    SessionData.menu.Name = model.Menus[0].Name;
                }
                else if (model.Menu.id != 0) {
                    model.Menu.Name = model.Menus.FirstOrDefault(m => m.id == model.Menu.id).Name;
                }

                SessionData.menu.id = model.Menu.id;
                SessionData.menu.Name = model.Menu.Name;
                model.Categories = categoryService.GetCategories(model.Menu.id, Common.CategoryType.Product);
                return model;
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                categoryService = null;
                menuService = null;
            }
            return null;
        }

        #endregion private
    }
}