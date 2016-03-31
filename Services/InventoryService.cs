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

        public Boolean AddInventoryRegestryCheckMenu(InventoryRegistry registry, ChecksMenu checkMenu) {
            InventoryRegestryCheckMenu item;
            try {
                item = new InventoryRegestryCheckMenu();
                item.ChecksMenuId = checkMenu.id;
                item.InventoryRegistryId = registry.id;

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

        public Boolean AddInventoryRegistry(ItemInventoryAssociation association, ChecksMenu checkMenu, Item itemAssociated) {
            InventoryRegistry item;
            try {
                item = new InventoryRegistry();
                item.Comment = String.Format("Taken [{0}] of [{1}] from: [{2}]. Check# {3}", association.Quantity, itemAssociated.Name, association.Item.Name, checkMenu.CheckId);
                item.Quantity = Math.Abs(association.Quantity) * -1;
                item.AssociatedtemId = association.AssociatedItemId;
                item.Type = (Int32)Common.InventoryType.Out;

                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    db.InventoryRegistries.InsertOnSubmit(item);
                    db.SubmitChanges();
                }

                AddInventoryRegestryCheckMenu(item, checkMenu);
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

        public Boolean DeleteInventoryRegistry(ItemInventoryAssociation association, ChecksMenu checkMenu, Item itemAssociated) {
            InventoryRegistry item;

            try {
                item = new InventoryRegistry();
                item.Comment = String.Format("Added [{0}] of [{1}] for: [{2}]. Check# {3}", association.Quantity, itemAssociated.Name, association.Item.Name, checkMenu.CheckId);
                item.Quantity = Math.Abs(association.Quantity);
                item.AssociatedtemId = association.AssociatedItemId;
                item.Type = (Int32)Common.InventoryType.In;

                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    db.InventoryRegistries.InsertOnSubmit(item);
                    db.SubmitChanges();
                }

                AddInventoryRegestryCheckMenu(item, checkMenu);
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