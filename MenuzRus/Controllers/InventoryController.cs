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
        private IItemService _itemService;

        public InventoryController(ISessionData sessionData, IItemService itemService, ICategoryService categoryService)
            : base(sessionData) {
            _itemService = itemService;
            _categoryService = categoryService;
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