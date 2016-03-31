using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI.WebControls;
using MenuzRus.Models;
using Services;

namespace MenuzRus.Controllers {

    public class ItemController : BaseController {
        private ICategoryService _categoryService;
        private IItemService _itemService;

        public ItemController(ISessionData sessionData, IItemService itemService, ICategoryService categoryService)
            : base(sessionData) {
            _itemService = itemService;
            _categoryService = categoryService;
        }

        #region item

        [HttpPost]
        public ActionResult DeleteItem(Int32? id) {
            try {
                if (!_itemService.DeleteItem(id)) {
                    return RedirectToAction("Index", "Error");
                }

                return Json("OK");
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
            return null;
        }

        [HttpGet]
        public ActionResult EditItem(Int32? id, Common.CategoryType type) {
            if (SessionData.user == null) {
                return PartialView("_SessionEnd");
            }

            try {
                return PartialView("_ItemPartial", GetModel(id, type));
            }
            catch (Exception ex) {
            }
            finally {
            }
            return null;
        }

        [HttpPost]
        public ActionResult SaveItem(ItemModel model) {
            Item item = new Item();

            try {
                item.id = model.id;
                item.CategoryId = model.CategoryId;
                item.Name = model.Name;
                item.Description = model.Description;
                item.ImageUrl = model.ImageUrl;
                item.Status = (Int32)Common.Status.Active;
                item.UOM = (Int32)model.UOM;

                if (model.Image != null) {
                    if (item.id == 0) {
                        item.ImageUrl = Path.GetExtension(model.Image.FileName);
                    }
                    else {
                        item.ImageUrl = String.Format("{0}{1}", model.id, Path.GetExtension(model.Image.FileName));
                    }
                }

                Int32 result = _itemService.SaveItem(item);
                if (result == 0)
                    return RedirectToAction("Index", "Error");

                String fileName = (model.Image == null ? model.ImageUrl : model.Image.FileName);
                String path = Path.Combine(Server.MapPath("~/Images/Menus/"), SessionData.user.id.ToString(), "Items", String.Format("{0}{1}", result, Path.GetExtension(fileName)));
                if (model.Image == null && model.ImageUrl == null) {
                    if (System.IO.File.Exists(path)) {
                        System.IO.File.Delete(path);
                    }
                }
                else if (model.Image != null) {
                    model.Image.SaveAs(path);
                }

                if (model.Price2Add > 0) {
                    AddItemPrice(result, model.Price2Add);
                }

                if (model.Quantity > 0) {
                    if (model.InventoryType == Common.InventoryType.Out) {
                        // Out is always negative
                        model.Quantity = Math.Abs(model.Quantity) * -1;
                    }

                    AddItemRegistry(result, model.Quantity, model.InventoryType, model.InventoryComment);
                }

                // Default menuDesigner
                return RedirectToAction("Index", "Designer", new { id = (Common.CategoryType)model.CategoryType });
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
            return null;
        }

        #endregion item

        #region private

        private void AddItemPrice(Int32 id, Decimal price) {
            try {
                _itemService.AddItemPrice(id, price);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
        }

        private void AddItemRegistry(Int32 id, Decimal qty, Common.InventoryType type, String comment) {
            try {
                _itemService.AddItemRegistry(id, qty, type, comment);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
        }

        private ItemModel GetModel(Int32? id, Common.CategoryType type) {
            ItemModel model = new ItemModel();
            Item item;
            try {
                model.Categories = _categoryService.GetCategories(SessionData.customer.id, type);
                if (model.Categories.Any()) {
                    model.CategoryId = model.Categories[0].id;
                }
                model.Status = Common.Status.Active;
                model.CategoryType = type;
                if (id.HasValue) {
                    item = _itemService.GetItem((Int32)id.Value);
                    if (item != null) {
                        model.Status = (Common.Status)item.Status;
                        model.id = item.id;
                        model.CategoryId = item.CategoryId;
                        model.Name = item.Name;
                        model.Description = item.Description;
                        model.ImageUrl = item.ImageUrl;
                        model.ItemPrices = _itemService.GetItemPrices(model.id);
                        model.InventoryRegistries = item.InventoryRegistries.Where(m => m.DateCreated > DateTime.Now.AddDays(-7)).ToList();
                        //model.ItemInventoryAssociations = item.ItemInventoryAssociations.ToList();
                        model.UOM = (Common.UOM)item.UOM;
                    }
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