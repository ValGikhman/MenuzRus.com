using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MenuzRus.Controllers;
using MenuzRus.Models;
using Services;

namespace MenuzRus {

    public class InventoryController : BaseController {
        private ICategoryService _categoryService;
        private IInventoryService _inventoryService;
        private IItemService _itemService;

        public InventoryController(ISessionData sessionData, IItemService itemService, ICategoryService categoryService, IInventoryService inventoryService)
            : base(sessionData) {
            _itemService = itemService;
            _categoryService = categoryService;
            _inventoryService = inventoryService;
        }

        [HttpGet]
        public ActionResult AssociateInventory(Int32 id) {
            if (SessionData.user == null) {
                return PartialView("_SessionEnd");
            }

            InventoryAssosiationModel model = new InventoryAssosiationModel();
            try {
                SessionData.item = _itemService.GetItem(id);
                model.Categories = _categoryService.GetCategories(SessionData.customer.id, Common.CategoryType.Inventory);
                model.ItemInventoryAssociation = SessionData.item.ItemInventoryAssociations;

                return PartialView("_InventoryAssociatePartial", model);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
            return null;
        }

        [HttpPost]
        public ActionResult DeleteInventoryAssociation(Int32 id) {
            try {
                if (!_inventoryService.DeleteInventoryAssociation(id))
                    return RedirectToAction("Index", "Error");

                return Json("OK");
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
            return null;
        }

        [HttpPost]
        public String SaveInventoryAssociation(String model) {
            InventoryModel model2Save;
            try {
                model2Save = SetModel(model);
                if (model2Save != null) {
                    foreach (Models.Inventory item in model2Save.Inventory) {
                        Services.ItemInventoryAssociation association = new ItemInventoryAssociation();
                        association.ItemInventoryId = item.ItemId;
                        association.AssociatedItemId = item.id;
                        _inventoryService.SaveInventoryAssociation(association);
                    }
                }
            }
            catch (Exception ex) {
                base.Log(ex);
                return ex.Message;
            }
            finally {
            }
            return "OK";
        }

        #region private

        private InventoryModel SetModel(String model) {
            JavaScriptSerializer objJavascript = new JavaScriptSerializer();
            Char[] commaDelimiter = new char[] { ',' };
            Char[] colonDelimiter = new char[] { ':' };
            if (SessionData.item != null) {
                InventoryModel inventoryModel = new InventoryModel();

                Array associations = (Array)objJavascript.DeserializeObject(model);
                inventoryModel.Inventory = new List<Models.Inventory>();
                foreach (String assosiation in associations) {
                    Models.Inventory inventory = new Models.Inventory();
                    inventory.ItemId = SessionData.item.id;
                    Array values = assosiation.Split(commaDelimiter, StringSplitOptions.RemoveEmptyEntries);

                    inventory.ItemInventoryAssociation = new List<ItemInventoryAssociation>();
                    foreach (String value in values) {
                        ItemInventoryAssociation itemInventoryAssociation = new ItemInventoryAssociation();

                        Array vars = value.Split(colonDelimiter, StringSplitOptions.RemoveEmptyEntries);

                        inventory.id = Int32.Parse(vars.GetValue(0).ToString());
                        itemInventoryAssociation.AssociatedItemId = Int32.Parse(vars.GetValue(0).ToString());
                        itemInventoryAssociation.ItemInventoryId = Int32.Parse(vars.GetValue(1).ToString());

                        inventory.ItemInventoryAssociation.Add(itemInventoryAssociation);
                    }
                    inventoryModel.Inventory.Add(inventory);
                }
                return inventoryModel;
            }
            return null;
        }

        #endregion private
    }
}