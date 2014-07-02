using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using MenuzRus.Models;
using Services;

namespace MenuzRus.Controllers {

    public class ItemController : BaseController {

        #region item

        [HttpPost]
        public ActionResult DeleteItem(Int32? id) {
            ItemService service = new ItemService();
            try {
                if (!service.DeleteItem(id))
                    return RedirectToAction("Index", "Error");

                return Json("OK");
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                service = null;
            }
            return null;
        }

        [HttpGet]
        public ActionResult EditItem(Int32? id) {
            try {
                return PartialView("_ItemPartial", GetModel(id));
            }
            catch (Exception ex) {
            }
            finally {
            }
            return null;
        }

        public ActionResult Index(Int32? id) {
            try {
                return View(GetModel(id));
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
            return null;
        }

        [HttpPost]
        public ActionResult SaveItem(ItemModel model) {
            // Converts /MenuDesigner to Menu, /Product to Product etc
            base.Referer = "Product";
            if (Request.UrlReferrer.LocalPath.IndexOf("MenuDesigner") > -1)
                base.Referer = "Menu";

            ItemService service = new ItemService();
            Item item = new Item();
            try {
                item.id = model.id;
                item.CategoryId = model.CategoryId;
                item.Name = model.Name;
                item.Description = model.Description;
                item.ImageUrl = model.ImageUrl;
                item.Active = (model.Active == Common.Status.Active);
                item.AdditionalInfo = model.AdditionalInfo;
                if (model.Image != null) {
                    if (item.id == 0)
                        item.ImageUrl = Path.GetExtension(model.Image.FileName);
                    else
                        item.ImageUrl = String.Format("{0}{1}", model.id, Path.GetExtension(model.Image.FileName));
                }

                Int32 result = service.SaveItem(item);
                if (result == 0)
                    return RedirectToAction("Index", "Error");

                String fileName = (model.Image == null ? model.ImageUrl : model.Image.FileName);
                String path = Path.Combine(Server.MapPath("~/Images/Menus/"), SessionData.user.id.ToString(), "Items", String.Format("{0}{1}", result, Path.GetExtension(fileName)));
                if (model.Image == null && model.ImageUrl == null) {
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                }
                else if (model.Image != null)
                    model.Image.SaveAs(path);

                AddItemPrice(model.id, model.Price2Add);
                return RedirectToAction("Index", base.Referer, new { id = SessionData.menu.id });
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                service = null;
            }
            return null;
        }

        #endregion item

        #region private

        private void AddItemPrice(Int32 id, Decimal price) {
            ItemService service = new ItemService();
            try {
                service.AddItemPrice(id, price);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                service = null;
            }
        }

        private ItemModel GetModel(Int32? id) {
            ItemModel model = new ItemModel();
            // Converts /MenuDesigner to Menu, /Product to Product etc
            base.Referer = "Product";
            if (Request.UrlReferrer.LocalPath.IndexOf("MenuDesigner") > -1)
                base.Referer = "Menu";

            Item item;
            ItemService itemService = new ItemService();
            CategoryService categoryService = new CategoryService();
            try {
                model.Categories = categoryService.GetCategories(SessionData.menu.id, EnumHelper<Common.CategoryType>.Parse(base.Referer));
                model.Active = Common.Status.Active;
                if (id.HasValue) {
                    item = itemService.GetItem((Int32)id.Value);
                    if (item != null) {
                        model.Active = item.Active ? Common.Status.Active : Common.Status.NotActive;
                        model.id = item.id;
                        model.CategoryId = item.CategoryId;
                        model.Name = item.Name;
                        model.Description = item.Description;
                        model.AdditionalInfo = item.AdditionalInfo;
                        model.ImageUrl = item.ImageUrl;
                        model.ItemPrices = itemService.GetItemPrices(model.id);
                    }
                }
                return model;
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                itemService = null;
                categoryService = null;
            }
            return null;
        }

        #endregion private
    }
}