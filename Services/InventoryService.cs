using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using MenuzRus;
using Services;

namespace MenuzRus {

    public class InventoryService : BaseService, IInventoryService {

        public Boolean DeleteInventoryAssociation(Int32 id) {
            ItemInventoryAssociation item = new ItemInventoryAssociation();
            try {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    item = db.ItemInventoryAssociations.Where(m => m.id == id).FirstOrDefault();
                    if (item != default(ItemInventoryAssociation)) {
                        db.ItemInventoryAssociations.DeleteOnSubmit(item);
                        db.SubmitChanges();
                    }
                }
            }
            catch (Exception ex) {
                return false;
            }
            return true;
        }

        public Boolean SaveInventoryAssociation(ItemInventoryAssociation item) {
            ItemInventoryAssociation itemQuery = new ItemInventoryAssociation();
            try {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    if (item.id != 0) {
                        itemQuery = db.ItemInventoryAssociations.Where(m => m.id == item.id).FirstOrDefault();
                    }
                    if (itemQuery != default(ItemInventoryAssociation)) {
                        itemQuery.ItemInventoryId = item.ItemInventoryId;
                        itemQuery.AssociatedItemId = item.id;
                    }
                    if (item.id == 0) {
                        db.ItemInventoryAssociations.InsertOnSubmit(itemQuery);
                    }
                    db.SubmitChanges();
                }
            }
            catch (Exception ex) {
                return false;
            }
            return true;
        }
    }
}