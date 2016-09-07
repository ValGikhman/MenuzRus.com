using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public class ItemService : BaseService, IItemService {

        public Boolean AddItemPrice(Int32 id, Decimal price) {
            ItemPrice query = new ItemPrice();
            try {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    query.Price = price;
                    query.ItemId = id;
                    db.ItemPrices.InsertOnSubmit(query);
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

        public Boolean DeleteItem(Int32? id) {
            Item query;
            id = id.HasValue ? id : 0;
            try {
                query = new Item();
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    query = db.Items.FirstOrDefault(m => m.id == id);
                    if (query != default(Item)) {
                        query.Status = (Int32)Common.Status.NotActive;
                        db.SubmitChanges();
                    }
                }
            }
            catch (Exception ex) {
                return false;
            }
            return true;
        }

        public Boolean DeleteMenuItem(Int32 id) {
            //MenuDesign query;
            //try {
            //    query = new MenuDesign();
            //    using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
            //        query = db.MenuDesigns.FirstOrDefault(m => m.ItemId == id);
            //        if (query != default(MenuDesign)) {
            //            db.MenuDesigns.DeleteOnSubmit(query);
            //            db.SubmitChanges();
            //        }
            //    }
            //}
            //catch (Exception ex) {
            //    return false;
            //}
            return true;
        }

        public Item GetItem(Int32 id) {
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            Item item = db.Items.FirstOrDefault(m => m.id == id);
            return item;
        }

        public List<ItemPrice> GetItemPrices(Int32 id) {
            List<ItemPrice> items;
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            // 20 last price changes
            items = db.ItemPrices.Where(m => m.ItemId == id).OrderByDescending(m => m.DateCreated).Take(20).ToList();
            return items;
        }

        public ItemProduct GetItemProduct(Int32 id) {
            ItemProduct item;
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            item = db.ItemProducts.FirstOrDefault(m => m.id == id);
            return item;
        }

        public List<Item> GetItemProductAssosiations(Int32 productId) {
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            List<Item> item = (from var in db.ItemProductAssociations
                               join it in db.Items on var.ItemId equals it.id
                               where var.ItemProductId == productId
                               select it).ToList();
            return item;
        }

        public Item GetItemProductAssosiationsById(Int32 associationId) {
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            Item item = (from var in db.ItemProductAssociations
                         join it in db.Items on var.ItemId equals it.id
                         where var.id == associationId
                         select it).FirstOrDefault();
            return item;
        }

        public List<Item> GetItems(Int32 id) {
            List<Item> items;
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            items = db.Items.Where(m => m.CategoryId == id).OrderBy(m => m.Name).ToList();
            return items;
        }

        public ItemPrice GetLastItemPrice(Int32 id) {
            ItemPrice item;
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            item = db.ItemPrices.Where(m => m.ItemId == id).OrderByDescending(m => m.DateCreated).Take(1).FirstOrDefault();
            return item;
        }

        public Int32 SaveItem(Item item) {
            Item query = new Item();
            try {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    if (item.id != 0) {
                        query = db.Items.Where(m => m.id == item.id).FirstOrDefault();
                    }

                    if (query != default(Item)) {
                        query.CategoryId = item.CategoryId;
                        query.Status = item.Status;
                        query.Name = item.Name;
                        query.Description = item.Description;
                        query.ImageUrl = item.ImageUrl;
                        query.UOM = item.UOM;
                    }

                    if (item.id == 0) {
                        db.Items.InsertOnSubmit(query);
                    }

                    db.SubmitChanges();

                    // Update ImageName for new category
                    if (item.id == 0 && query.ImageUrl != null) {
                        query.ImageUrl = String.Format("{0}{1}", query.id, item.ImageUrl);
                        db.SubmitChanges();
                    }
                }
            }
            catch (Exception ex) {
                return 0;
            }
            return query.id;
        }

        //public Int32 SaveMenuItem(Int32 id) {
        //    MenuDesign query = new MenuDesign();
        //    try {
        //        using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
        //            if (id != 0)
        //                query = db.MenuDesigns.Where(m => m.ItemId == id).FirstOrDefault();
        //            if (query == default(MenuDesign)) {
        //                query = new MenuDesign();
        //                query.ItemId = id;
        //            }
        //            db.MenuDesigns.InsertOnSubmit(query);
        //            db.SubmitChanges();
        //        }
        //    }
        //    catch (Exception ex) {
        //        return 0;
        //    }
        //    return query.id;
        //}
    }
}