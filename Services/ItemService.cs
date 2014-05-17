using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Services;
using StringExtensions;

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
                    query = db.Items.Where(m => m.id == id).FirstOrDefault();
                    if (query != default(Item)) {
                        db.Items.DeleteOnSubmit(query);
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
            Item item = db.Items.Where(m => m.id == id).FirstOrDefault();
            return item;
        }

        public List<ItemPrice> GetItemPrices(Int32 id) {
            List<ItemPrice> items;
            menuzRusDataContext db = new menuzRusDataContext();
            items = db.ItemPrices.Where(m => m.ItemId == id).OrderByDescending(m => m.DateModified).ToList();
            return items;
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