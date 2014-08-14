using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public class ItemService {

        public Boolean AddItemPrice(Int32 id, Decimal price) {
            ItemPrice query = new ItemPrice();
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    query.Price = price;
                    query.ItemId = id;
                    db.ItemPrices.InsertOnSubmit(query);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex) {
                SessionData.exeption = ex;
                return false;
            }
            return true;
        }

        public Boolean DeleteItem(Int32? id) {
            Item query = new Item();
            id = id.HasValue ? id : 0;
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    query = db.Items.FirstOrDefault(m => m.id == id);
                    if (query != default(Item)) {
                        query.Active = false;
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

        public Item GetItem(Int32 id) {
            menuzRusDataContext db = new menuzRusDataContext();
            Item item = db.Items.FirstOrDefault(m => m.id == id);
            return item;
        }

        public List<ItemPrice> GetItemPrices(Int32 id) {
            List<ItemPrice> items;
            menuzRusDataContext db = new menuzRusDataContext();
            items = db.ItemPrices.Where(m => m.ItemId == id).OrderByDescending(m => m.DateCreated).ToList();
            return items;
        }

        public ItemProduct GetItemProduct(Int32 id) {
            ItemProduct item;
            menuzRusDataContext db = new menuzRusDataContext();
            item = db.ItemProducts.FirstOrDefault(m => m.id == id);
            return item;
        }

        public List<Item> GetItemProductAssosiations(Int32 productId) {
            menuzRusDataContext db = new menuzRusDataContext();
            List<Item> item = (from var in db.ItemProductAssociations
                               join it in db.Items on var.ItemId equals it.id
                               where var.ItemProductId == productId
                               select it).ToList();
            return item;
        }

        public Item GetItemProductAssosiationsById(Int32 associationId) {
            menuzRusDataContext db = new menuzRusDataContext();
            Item item = (from var in db.ItemProductAssociations
                         join it in db.Items on var.ItemId equals it.id
                         where var.id == associationId
                         select it).FirstOrDefault();
            return item;
        }

        public List<Item> GetItems(Int32 id) {
            List<Item> items;
            menuzRusDataContext db = new menuzRusDataContext();
            items = db.Items.Where(m => m.CategoryId == id).OrderBy(m => m.SortOrder).ToList();
            return items;
        }

        public Int32 SaveItem(Item item) {
            Item query = new Item();
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    if (item.id != 0)
                        query = db.Items.Where(m => m.id == item.id).FirstOrDefault();
                    if (query != default(Item)) {
                        query.CategoryId = item.CategoryId;
                        query.Active = item.Active;
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
                SessionData.exeption = ex;
                return 0;
            }
            return query.id;
        }
    }
}