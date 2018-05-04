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

        #region Private Fields

        private ICategoryService _categoryService;
        private IInventoryService _inventoryService;
        private IItemService _itemService;

        #endregion Private Fields

        #region Public Constructors

        public InventoryController(ISessionData sessionData, IItemService itemService, ICategoryService categoryService, IInventoryService inventoryService)
            : base(sessionData) {
            _itemService = itemService;
            _categoryService = categoryService;
            _inventoryService = inventoryService;
        }

        #endregion Public Constructors

        #region Public Methods

        [HttpGet]
        public ActionResult AssociateInventory(Int32 id) {
            InventoryAssosiationModel model = new InventoryAssosiationModel();
            try {
                SessionData.item = _itemService.GetItem(id);
                model.Categories = _categoryService.GetCategories(SessionData.customer.id, CommonUnit.CategoryType.Inventory);
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
                    foreach (Services.ItemInventoryAssociation itemInventoryAssociation in model2Save.ItemInventoryAssociations) {
                        _inventoryService.SaveInventoryAssociation(itemInventoryAssociation);
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

        #endregion Public Methods

        #region private

        private InventoryModel SetModel(String model) {
            JavaScriptSerializer objJavascript = new JavaScriptSerializer();
            Char[] commaDelimiter = new char[] { ',' };
            Char[] colonDelimiter = new char[] { ':' };
            if (SessionData.item != null) {
                InventoryModel inventoryModel = new InventoryModel();

                Array associations = (Array)objJavascript.DeserializeObject(model);
                inventoryModel.ItemInventoryAssociations = new List<ItemInventoryAssociation>();

                foreach (String association in associations) {
                    Array values = association.Split(commaDelimiter, StringSplitOptions.RemoveEmptyEntries);

                    ItemInventoryAssociation itemInventoryAssociation = new ItemInventoryAssociation();
                    foreach (String value in values) {
                        Array vars = value.Split(colonDelimiter, StringSplitOptions.RemoveEmptyEntries);
                        itemInventoryAssociation.id = Int32.Parse(vars.GetValue(0).ToString()); ;
                        itemInventoryAssociation.AssociatedItemId = Int32.Parse(vars.GetValue(1).ToString());
                        itemInventoryAssociation.Quantity = Decimal.Parse(vars.GetValue(2).ToString());
                        itemInventoryAssociation.ItemInventoryId = SessionData.item.id;
                    }
                    inventoryModel.ItemInventoryAssociations.Add(itemInventoryAssociation);
                }
                return inventoryModel;
            }
            return null;
        }

        #endregion private
    }
}