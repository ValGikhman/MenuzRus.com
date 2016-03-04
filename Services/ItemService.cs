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
            MenuDesign query;
            try {
                query = new MenuDesign();
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    query = db.MenuDesigns.FirstOrDefault(m => m.ItemId == id);
                    if (query != default(MenuDesign)) {
                        db.MenuDesigns.DeleteOnSubmit(query);
                        db.SubmitChanges();
                    }
                }
            }
            catch (Exception ex) {
                return false;
            }
            return true;
        }

        public Item GetItem(Int32 id) {
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            Item item = db.Items.FirstOrDefault(m => m.id == id);
            return item;
        }

        public ItemAssociation GetItemAssociations(Int32 id) {
            ItemAssociation item;
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            item = db.ItemAssociations.FirstOrDefault(m => m.id == id);
            return item;
        }

        public List<ItemPrice> GetItemPrices(Int32 id) {
            List<ItemPrice> items;
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            items = db.ItemPrices.Where(m => m.ItemId == id).OrderByDescending(m => m.DateCreated).ToList();
            return items;
        }

        public List<Item> GetItems(Int32 id) {
            List<Item> items;
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            items = db.Items.Where(m => m.CategoryId == id).OrderBy(m => m.SortOrder).ToList();
            return items;
        }

        public Int32 SaveItem(Item item) {
            Item query = new Item();
            try {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    if (item.id != 0)
                        query = db.Items.Where(m => m.id == item.id).FirstOrDefault();
                    if (query != default(Item)) {
                        query.CategoryId = item.CategoryId;
                        query.Status = item.Status;
                        query.Name = item.Name;
                        query.Description = item.Description;
                        query.ImageUrl = item.ImageUrl;
                        query.AdditionalInfo = item.AdditionalInfo;
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

        public Int32 SaveMenuItem(Int32 id) {
            MenuDesign query = new MenuDesign();
            try {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    if (id != 0)
                        query = db.MenuDesigns.Where(m => m.ItemId == id).FirstOrDefault();
                    if (query == default(MenuDesign)) {
                        query = new MenuDesign();
                        query.ItemId = id;
                    }
                    db.MenuDesigns.InsertOnSubmit(query);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex) {
                return 0;
            }
            return query.id;
        }
    }
}