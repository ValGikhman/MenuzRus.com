using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using MenuzRus.Models;
using Services;

namespace MenuzRus.Controllers {

    public class ItemProductController : BaseController {
        private ICategoryService _categoryService;
        private IItemProductService _itemProductService;
        private IItemService _itemService;

        public ItemProductController(ISessionData sessionData, IItemProductService itemProductService, ICategoryService categoryService, IItemService itemService)
            : base(sessionData) {
            _itemProductService = itemProductService;
            _categoryService = categoryService;
            _itemService = itemService;
        }

        #region item

        [HttpPost]
        public ActionResult DeleteItemAssociation(Int32 id) {
            try {
                if (!_itemProductService.DeleteItemAssociation(id))
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

        [HttpGet]
        public ActionResult ItemAssociate(Int32 id) {
            DesignerModel model = new DesignerModel();
            try {
                SessionData.item = _itemService.GetItem(id);
                model.Categories = _categoryService.GetCategories(SessionData.customer.id, Common.CategoryType.Product);
                model.ItemAssociation = SessionData.item.ItemAssociations;

                return PartialView("_ItemAssociatePartial", model);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
            return null;
        }

        [HttpPost]
        public String SaveAssociatedItems(String model) {
            ItemAssociationModel model2Save;
            Services.ItemAssociation itemAssociation;
            try {
                model2Save = SetModel(model);
                if (model2Save != null) {
                    foreach (Models.ItemAssociation item in model2Save.ItemAssociations) {
                        itemAssociation = new Services.ItemAssociation();
                        itemAssociation.id = item.id;
                        itemAssociation.ItemId = item.ItemId;
                        itemAssociation.ItemReferenceId = item.ItemReferenceId;
                        itemAssociation.Type = (Int32)item.Type;
                        _itemProductService.SaveItemAssociation(itemAssociation);
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

        #endregion item

        #region private

        private ItemAssociationModel SetModel(String model) {
            JavaScriptSerializer objJavascript = new JavaScriptSerializer();
            Char[] commaDelimiter = new char[] { ',' };
            Char[] colonDelimiter = new char[] { ':' };
            if (SessionData.item != null) {
                ItemAssociationModel itemAssociationModel = new ItemAssociationModel();

                Array assosiations = (Array)objJavascript.DeserializeObject(model);
                itemAssociationModel.ItemAssociations = new List<Models.ItemAssociation>();
                Models.ItemAssociation itemAssociation;
                foreach (String assosiation in assosiations) {
                    Array values = assosiation.Split(commaDelimiter, StringSplitOptions.RemoveEmptyEntries);
                    foreach (String value in values) {
                        itemAssociation = new Models.ItemAssociation();
                        itemAssociation.ItemId = SessionData.item.id;

                        Array vars = value.Split(colonDelimiter, StringSplitOptions.RemoveEmptyEntries);
                        itemAssociation.Type = EnumHelper<Common.ProductType>.Parse(vars.GetValue(2).ToString());
                        itemAssociation.ItemReferenceId = Int32.Parse(vars.GetValue(1).ToString());
                        itemAssociationModel.ItemAssociations.Add(itemAssociation);
                    }
                }
                return itemAssociationModel;
            }
            return null;
        }

        #endregion private
    }
}