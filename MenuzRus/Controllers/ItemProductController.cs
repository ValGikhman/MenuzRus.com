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

        #region item

        [HttpPost]
        public ActionResult DeleteItemProduct(Int32? id) {
            ItemProductService service = new ItemProductService();
            try {
                if (!service.DeleteItemProduct(id))
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
        public ActionResult ItemProductAssociate(Int32 id) {
            CategoryService categoryService = new CategoryService();
            ItemService itemService = new ItemService();
            DesignerModel model = new DesignerModel();
            try {
                SessionData.item = itemService.GetItem(id);
                model.Categories = categoryService.GetCategories(SessionData.customer.id, Common.CategoryType.Product);
                return PartialView("_ItemProductAssociatePartial", model);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                categoryService = null;
                itemService = null;
            }
            return null;
        }

        [HttpPost]
        public String SaveAssociatedItems(String model) {
            ItemProductService service = new ItemProductService();
            ItemProductModel model2Save;
            Services.ItemProduct itemProduct;
            Services.ItemProductAssociation itemProductAssociation;
            try {
                model2Save = SetModel(model);
                if (model2Save != null) {
                    foreach (Models.ItemProduct item in model2Save.ItemsProduct) {
                        itemProduct = new Services.ItemProduct();
                        itemProduct.id = item.id;
                        itemProduct.ItemId = item.ItemId;
                        itemProduct.Type = (Int32)item.Type;
                        foreach (ItemProductAssociation itemAssosiation in item.ItemProductAssociation) {
                            itemProductAssociation = new Services.ItemProductAssociation();
                            itemProductAssociation.ItemId = itemAssosiation.ItemId;
                            itemProductAssociation.ItemProductId = itemAssosiation.ItemProductId;
                            itemProduct.ItemProductAssociations.Add(itemProductAssociation);
                        }
                        service.SaveItemProduct(itemProduct);
                    }
                }
            }
            catch (Exception ex) {
                base.Log(ex);
                return ex.Message;
            }
            finally {
                service = null;
                SessionData.item = null;
            }
            return "OK";
        }

        #endregion item

        #region private

        private ItemProductModel SetModel(String model) {
            JavaScriptSerializer objJavascript = new JavaScriptSerializer();
            Char[] commaDelimiter = new char[] { ',' };
            Char[] colonDelimiter = new char[] { ':' };
            if (SessionData.item != null) {
                ItemProductModel productModel = new ItemProductModel();

                Array assosiations = (Array)objJavascript.DeserializeObject(model);
                productModel.ItemsProduct = new List<Models.ItemProduct>();
                foreach (String assosiation in assosiations) {
                    Models.ItemProduct itemProduct = new Models.ItemProduct();
                    itemProduct.ItemId = SessionData.item.id;
                    Array values = assosiation.Split(commaDelimiter, StringSplitOptions.RemoveEmptyEntries);

                    itemProduct.ItemProductAssociation = new List<ItemProductAssociation>();
                    foreach (String value in values) {
                        ItemProductAssociation itemProductAssociation = new ItemProductAssociation();

                        Array vars = value.Split(colonDelimiter, StringSplitOptions.RemoveEmptyEntries);

                        itemProduct.id = Int32.Parse(vars.GetValue(0).ToString());
                        itemProduct.Type = EnumHelper<Common.ProductType>.Parse(vars.GetValue(2).ToString());
                        itemProductAssociation.ItemProductId = Int32.Parse(vars.GetValue(0).ToString());
                        itemProductAssociation.ItemId = Int32.Parse(vars.GetValue(1).ToString());

                        itemProduct.ItemProductAssociation.Add(itemProductAssociation);
                    }
                    productModel.ItemsProduct.Add(itemProduct);
                }
                return productModel;
            }
            return null;
        }

        #endregion private
    }
}