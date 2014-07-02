using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Services;
using StringExtensions;

namespace MenuzRus {

    public class OrderService {

        public Boolean DeleteCheck(Int32 id) {
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    IEnumerable<ItemProductAssociation> query = db.ItemProductAssociations.Where(m => m.ItemProductId == id);
                    if (query != default(ItemProductAssociation)) {
                        db.ItemProductAssociations.DeleteAllOnSubmit(query);
                    }
                    db.SubmitChanges();
                }
            }
            catch (Exception ex) {
                SessionData.exeption = ex;
                return false;
            }
            return true;
        }

        public Boolean DeleteMenu(Int32? id) {
            ItemProduct itemProduct = new ItemProduct();
            IEnumerable<ItemProductAssociation> itemProductAssocoation; ;
            id = id.HasValue ? id : 0;
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    itemProductAssocoation = db.ItemProductAssociations.Where(m => m.ItemProductId == id);
                    if (itemProductAssocoation != default(ItemProductAssociation)) {
                        db.ItemProductAssociations.DeleteAllOnSubmit(itemProductAssocoation);
                    }

                    itemProduct = db.ItemProducts.Where(m => m.id == id).FirstOrDefault();
                    if (itemProduct != default(ItemProduct)) {
                        db.ItemProducts.DeleteOnSubmit(itemProduct);
                    }

                    db.SubmitChanges();
                }
            }
            catch (Exception ex) {
                SessionData.exeption = ex;
                return false;
            }
            return true;
        }

        public Boolean SaveMenu(dynamic order) {
            TableOrder orderQuery = new TableOrder();
            return true;
        }

        public Boolean SaveOrder(dynamic order) {
            TableOrder orderQuery = new TableOrder();
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    Int32 tableId = Int32.Parse(order["tableId"]);
                    Object[] Checks = order["Checks"];
                    foreach (IDictionary<String, object> Check in Checks) {
                        Int32 checkId = Int32.Parse(Check["id"].ToString());
                        dynamic Menus = Check["Menus"];
                        foreach (IDictionary<String, object> Menu in Menus) {
                            Int32 menuId = Int32.Parse(Menu["id"].ToString());
                            dynamic Products = Menu["Products"];
                            foreach (IDictionary<String, object> Product in Products) {
                                Int32 productId = Int32.Parse(Product["id"].ToString());
                                dynamic Items = Product["Items"];
                                foreach (IDictionary<String, object> Item in Items) {
                                    Int32 itemId = Int32.Parse(Product["id"].ToString());
                                    Common.ProductType type = (Common.ProductType)Item["type"];
                                }
                            }
                        }
                    }

                    if (order.id == 0) {
                        db.TableOrders.InsertOnSubmit(orderQuery);
                    }
                    db.SubmitChanges();
                }
            }
            catch (Exception ex) {
                SessionData.exeption = ex;
                return false;
            }
            return true;
        }

        public Boolean SaveProduct(dynamic order) {
            TableOrder orderQuery = new TableOrder();
            return true;
        }
    }
}