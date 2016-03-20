using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MenuzRus.Controllers;
using MenuzRus.Models;
using Services;

namespace MenuzRus {

    public class InventoryController : BaseController {
        private ICategoryService _categoryService;
        private IInventoryService _inventoryService;

        public InventoryController(ISessionData sessionData, IInventoryService inventoryService, ICategoryService categoryService)
            : base(sessionData) {
            _inventoryService = inventoryService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public ActionResult AssociateInventory(Int32 id) {
            if (SessionData.user == null) {
                return PartialView("_SessionEnd");
            }

            InventoryModel model = new InventoryModel();
            try {
                ///SessionData.item = _inventoryService.GetItem(id);
                model.Categories = _categoryService.GetCategories(SessionData.customer.id, Common.CategoryType.Inventory);

                return PartialView("_InventoryAssociatePartial", model);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
            return null;
        }
    }
}