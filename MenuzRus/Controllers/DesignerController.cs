﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using MenuzRus.Models;
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
            Int32 categoryType = Int32.Parse(type);
            try {
                model = GetModel((Common.CategoryType)categoryType);
                List<Tuple<Int32, String, Int32, String, String, Decimal, Boolean>> gridData = new List<Tuple<Int32, String, Int32, String, String, Decimal, Boolean>>();
                Tuple<Int32, String, Int32, String, String, Decimal, Boolean> gridRow;
                foreach (Services.Category category in model.Categories) {
                    if (category.Items.Count > 0) {
                        foreach (Item item in category.Items) {
                            gridRow = new Tuple<Int32, String, Int32, String, String, Decimal, Boolean>(
                                    category.id
                                    , category.Name
                                    , item.id
                                    , item.Name
                                    , item.Description
                                    , item.ItemPrices.OrderByDescending(m => m.DateCreated).Take(1).Select(m => m.Price).FirstOrDefault()
                                    , (item.Status == (Int32)Common.Status.Active)
                            );
                            gridData.Add(gridRow);
                        }
                    }
                    else {
                        gridRow = new Tuple<Int32, String, Int32, String, String, Decimal, Boolean>(
                                category.id
                                , category.Name
                                , 0
                                , String.Empty
                                , String.Empty
                                , 0
                                , false
                        );
                        gridData.Add(gridRow);
                    }
                }

                var jsonData = new {
                    total = (Int32)Math.Ceiling((float)gridData.Count),
                    records = gridData.Count,
                    rows = (
                         from data in gridData
                         select new {
                             categoryId = data.Item1,
                             category = data.Item2,
                             itemId = data.Item3,
                             action = String.Empty,
                             name = data.Item4,
                             description = data.Item5,
                             price = data.Item6,
                             active = data.Item7
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