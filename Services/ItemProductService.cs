using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public class ItemProductService : BaseService, IItemProductService {

        public Boolean DeleteItemAssociation(Int32 id) {
            try {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    IEnumerable<ItemAssociation> query = db.ItemAssociations.Where(m => m.ItemId == id);
                    if (query != default(ItemAssociation)) {
                        db.ItemAssociations.DeleteAllOnSubmit(query);
                    }
                    db.SubmitChanges();
                }
            }
            catch (Exception ex) {
                return false;
            }
            return true;
        }

        public Boolean SaveItemAssociation(ItemAssociation item) {
            ItemAssociation itemQuery = new ItemAssociation();
            try {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    if (item.id != 0) {
                        itemQuery = db.ItemAssociations.Where(m => m.id == item.id).FirstOrDefault();
                    }
                    if (itemQuery != default(ItemAssociation)) {
                        itemQuery.id = item.id;
                        itemQuery.Type = item.Type;
                        itemQuery.ItemId = item.ItemId;
                        itemQuery.ItemReferenceId = item.ItemReferenceId;
                    }
                    if (item.id == 0) {
                        db.ItemAssociations.InsertOnSubmit(itemQuery);
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