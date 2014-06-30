using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Services;
using StringExtensions;

namespace MenuzRus {

    public class OrderService {

        public Boolean DeleteOrder(Int32 id) {
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

        public Boolean DeleteOrderItem(Int32? id) {
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

        public Boolean SaveOrder(TableOrder order) {
            TableOrder orderQuery = new TableOrder();
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    if (order.id != 0)
                        orderQuery = db.TableOrders.FirstOrDefault(m => m.id == order.id);

                    if (orderQuery != default(TableOrder)) {
                        orderQuery.id = order.id;
                        orderQuery.Status = order.Status;
                        orderQuery.TableId = order.TableId;
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
    }
}