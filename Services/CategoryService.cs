using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Services;
using StringExtensions;

namespace MenuzRus {

    public class CategoryService {

        public Boolean DeleteCategory(Int32? id) {
            Category query = new Category();
            id = id.HasValue ? id : 0;
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    query = db.Categories.Where(m => m.id == id).FirstOrDefault();
                    if (query != default(Category)) {
                        IEnumerable<Item> items = db.Items.Where(m => m.CategoryId == id);
                        if (items != null) {
                            db.Items.DeleteAllOnSubmit(items);
                        }
                        db.Categories.DeleteOnSubmit(query);
                        db.SubmitChanges();
                    }
                }
            }
            catch (Exception ex) {
                SessionData.exeption = ex;
                return false;
            }
            return true;
        }

        public List<Category> GetAllCategories(Common.CategoryType type) {
            menuzRusDataContext db = new menuzRusDataContext();
            return db.Menus.Join(db.Categories, menus => menus.id, categories => categories.MenuId, (menu, category) => new { menu = menu, category = category })
                            .Where(m => m.menu.CustomerId == SessionData.customer.id && m.category.Active && m.category.Type == (Int32)type)
                            .Select(m => m.category).ToList();
        }

        public List<Category> GetCategories(Int32 id, Common.CategoryType type) {
            menuzRusDataContext db = new menuzRusDataContext();
            return db.Categories.Where(m => m.MenuId == id && m.Active && m.Type == (Int32)type).OrderBy(m => m.SortOrder).ToList();
        }

        public Category GetCategory(Int32 id) {
            menuzRusDataContext db = new menuzRusDataContext();
            return db.Categories.Where(m => m.id == id).FirstOrDefault();
        }

        public Int32 SaveCategory(Category category) {
            Category query = new Category();
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    if (category.id != 0)
                        query = db.Categories.Where(m => m.id == category.id && m.Active).FirstOrDefault();
                    if (query != default(Category)) {
                        query.MenuId = category.MenuId;
                        query.Active = category.Active;
                        query.Name = category.Name;
                        query.Description = category.Description;
                        query.ImageUrl = category.ImageUrl;
                        query.Side = category.Side;
                        query.Type = category.Type;
                    }
                    if (category.id == 0) {
                        db.Categories.InsertOnSubmit(query);
                    }
                    db.SubmitChanges();
                    // Update ImageName for new category
                    if (category.id == 0 && query.ImageUrl != null) {
                        query.ImageUrl = String.Format("{0}{1}", query.id, category.ImageUrl);
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