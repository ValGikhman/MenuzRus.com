using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MenuzRus.Models;
using Services;

namespace MenuzRus.Controllers {

    public class DesignerController : BaseController {

        #region Public Fields

        public ICategoryService _categoryService;
        public IMenuService _menuService;

        #endregion Public Fields

        #region Public Constructors

        public DesignerController(ISessionData sessionData, ICategoryService categoryService, IMenuService menuService)
            : base(sessionData) {
            _categoryService = categoryService;
            _menuService = menuService;
        }

        #endregion Public Constructors

        #region Public Methods

        [HttpPost]
        public ActionResult DeleteMenu(Int32 id) {
            try {
                if (!_menuService.DeleteMenu(id)) {
                    return RedirectToAction("Menu", "Error");
                }
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
            return null;
        }

        [CheckUserSession]
        public ActionResult Index(String id) {
            DesignerModel model;
            CommonUnit.CategoryType categoryType;

            if (String.IsNullOrEmpty(id)) {
                categoryType = CommonUnit.CategoryType.Menu;
            }
            else {
                categoryType = EnumHelper<CommonUnit.CategoryType>.Parse(id);
            }
            model = GetModel(categoryType, null);

            return View("Index", model);
        }

        [HttpGet]
        public JsonResult LoadData(String type, String search) {
            DesignerModel model;
            Int32 categoryType = Int32.Parse(type);
            List<Tuple<Int32, String, Int32, String, String, Decimal, Boolean>> gridData;

            try {
                gridData = new List<Tuple<Int32, String, Int32, String, String, Decimal, Boolean>>();
                model = GetModel((CommonUnit.CategoryType)categoryType, search);

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
                                    , (item.Status == (Int32)CommonUnit.Status.Active)
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

        [CheckUserSession]
        public ActionResult Menu(Int32? id) {
            Menus menu;
            Models.Menu model = new Menu();

            model.Menus = _menuService.GetMenus(SessionData.customer.id);
            if (id.HasValue) {
                menu = model.Menus.Where(m => m.id == id).FirstOrDefault();
            }
            else {
                menu = model.Menus.Take(1).FirstOrDefault();
            }

            if (menu != null) {
                model.id = menu.id;
                model.Name = menu.Name;
                model.Description = menu.Description;
            }

            model.CurrentMenu = menu;
            model.Categories = _categoryService.GetCategories(SessionData.customer.id, CommonUnit.CategoryType.Menu);
            return View("Menu", model);
        }

        [HttpPost]
        public ActionResult SaveMenu(Models.Menu model) {
            Services.Menus menu = new Services.Menus();
            try {
                menu.id = model.id;
                menu.CustomerId = SessionData.customer.id;
                menu.Name = model.Name;
                menu.Description = model.Description;
                Int32 newId = _menuService.SaveMenu(menu);
                if (newId == 0) {
                    return RedirectToAction("Index", "Error");
                }
                model.Menus = _menuService.GetMenus(SessionData.customer.id);

                return View("Menu", model);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
            return null;
        }

        [HttpPost]
        public ActionResult SaveMenuItems(Models.MenuItems model) {
            JavaScriptSerializer js;
            List<Int32> menuItems;
            List<Services.MenuItem> itemsModel;
            Services.MenuItem itemModel;

            try {
                js = new JavaScriptSerializer();
                menuItems = js.Deserialize<List<Int32>>(model.Items);
                itemsModel = new List<Services.MenuItem>();
                foreach (Int32 item in menuItems) {
                    itemModel = new Services.MenuItem();
                    itemModel.ItemId = item;
                    itemModel.MenuId = model.id;
                    itemsModel.Add(itemModel);
                }
                _menuService.SaveMenuItems(itemsModel);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
            return null;
        }

        #endregion Public Methods

        #region private

        private DesignerModel GetModel(CommonUnit.CategoryType type, String search) {
            DesignerModel model = new DesignerModel();

            try {
                search = String.IsNullOrEmpty(search) ? search : search.ToUpper();

                model.CategoryType = type;
                model.Categories = _categoryService.GetCategories(SessionData.customer.id, type, search);
                model.ItemProducts = null;
                if (SessionData.item != null) {
                    model.ItemProducts = SessionData.item.ItemProducts;
                }
                return model;
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
            return null;
        }

        #endregion private
    }
}