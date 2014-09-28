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
        public ActionResult Index(Int32? id) {
            if (!id.HasValue) {
                id = (Int32)Common.CategoryType.Menu;
            }

            return View("Index", GetModel((Common.CategoryType)id));
        }

        [HttpGet]
        public JsonResult LoadData(String type) {
            DesignerModel model;
            List<Item> items;
            Int32 categoryType = Int32.Parse(type);
            try {
                items = new List<Item>();

                model = GetModel((Common.CategoryType)categoryType);
                foreach (Services.Category category in model.Categories) {
                    foreach (Item item in category.Items) {
                        items.Add(item);
                    }
                }

                var jsonData = new {
                    total = (Int32)Math.Ceiling((float)items.Count),
                    records = items.Count,
                    rows = (
                         from item in items
                         select new {
                             categoryId = item.Category.id,
                             category = item.Category.Name,
                             itemId = item.id,
                             action = String.Empty,
                             name = item.Name,
                             description = item.Description,
                             price = item.ItemPrices.OrderByDescending(m => m.DateCreated).Take(1).Select(m => m.Price).FirstOrDefault(),
                             active = (item.Status == (Int32)Common.Status.Active)
                         }
                    ).ToArray()
                };
                return new JsonResult() { Data = jsonData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }

            return new JsonResult() { Data = "", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

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