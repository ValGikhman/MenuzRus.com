using System;
using System.Linq;
using Services;

namespace MenuzRus {

    public class InventoryService : BaseService, IInventoryService {

        public Boolean AddInventoryRegestryCheckMenu(Int32 registryId, Int32 checkMenuId) {
            InventoryRegestryCheckMenu item;
            try {
                item = new InventoryRegestryCheckMenu();
                item.ChecksMenuId = checkMenuId;
                item.InventoryRegistryId = registryId;

                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    db.InventoryRegestryCheckMenus.InsertOnSubmit(item);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex) {
                return false;
            }
            return true;
        }

        public Boolean AddItemRegistry(Int32 id, Decimal qty, Common.InventoryType type, String comment) {
            InventoryRegistry query = new InventoryRegistry();
            try {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    query.Quantity = qty;
                    query.Type = (Int32)type;
                    query.AssociatedItemId = id;
                    query.Comment = comment;
                    db.InventoryRegistries.InsertOnSubmit(query);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex) {
                return false;
            }
            return true;
        }

        public Boolean AddInventoryRegistry(ItemInventoryAssociation association, ChecksMenu checkMenu, String name) {
            String comment;
            Int32 associatedItemId;
            Common.InventoryType type;
            Decimal quantity;
            try {
                comment = String.Format("Taken [{0}] of [{1}] from: [{2}]. Check# {3}", association.Quantity, name, association.Item.Name, checkMenu.CheckId);
                associatedItemId = association.AssociatedItemId;
                type = Common.InventoryType.Out; ;
                quantity = Math.Abs(association.Quantity) * -1;

                AddItemRegistry(associatedItemId, quantity, type, comment);
                AddInventoryRegestryCheckMenu(associatedItemId, checkMenu.id);
            }
            catch (Exception ex) {
                return false;
            }
            return true;
        }

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

        public Boolean DeleteInventoryRegistry(ItemInventoryAssociation association, ChecksMenu checkMenu, String name) {
            String comment;
            Int32 associatedItemId;
            Common.InventoryType type;
            Decimal quantity;

            try {
                comment = String.Format("Added [{0}] of [{1}] for: [{2}]. Check# {3}", association.Quantity, name, association.Item.Name, checkMenu.CheckId);
                associatedItemId = association.AssociatedItemId;
                type = Common.InventoryType.In; ;
                quantity = Math.Abs(association.Quantity);

                AddItemRegistry(associatedItemId, quantity, type, comment);
                AddInventoryRegestryCheckMenu(associatedItemId, checkMenu.id);
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
                        itemQuery.AssociatedItemId = item.AssociatedItemId;
                        itemQuery.Quantity = item.Quantity;
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