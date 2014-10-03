using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public class CategoryService {

        public Boolean DeleteCategory(Int32? id) {
            Category query = new Category();
            id = id.HasValue ? id : 0;
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    query = db.Categories.Where(m => m.id == id).FirstOrDefault();
                    if (query != default(Category)) {
                        query.Status = (Int32)Common.Status.NotActive;
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

        public List<Category> GetCategories(Int32 customerId, Common.CategoryType type) {
            menuzRusDataContext db = new menuzRusDataContext();
            return db.Categories.Where(m => m.CustomerId == customerId && m.Status != (Int32)Common.Status.NotActive && m.Type == (Int32)type).ToList();
        }

        public Category GetCategory(Int32 id) {
            menuzRusDataContext db = new menuzRusDataContext();
            return db.Categories.Where(m => m.id == id).FirstOrDefault();
        }

        public List<Category> GetMenuCategories(Int32 customerId, Common.CategoryType type) {
            List<Category> retVal = null;
            menuzRusDataContext db = new menuzRusDataContext();

            var a = GetCategories(customerId, type).Where(m => m.Items.Any()).Distinct().OrderBy(m => m.Name).Select(c => new {
                id = c.id,
                CustomerId = c.CustomerId,
                Items = c.Items.Where(i => i.MenuItems.Any())
            }).ToList();
            return retVal;
        }

        public Int32 SaveCategory(Category category) {
            Category query = new Category();
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    if (category.id != 0)
                        query = db.Categories.Where(m => m.id == category.id && m.Status != (Int32)Common.Status.NotActive).FirstOrDefault();
                    if (query != default(Category)) {
                        query.Status = category.Status;
                        query.CustomerId = category.CustomerId;
                        query.Name = category.Name;
                        query.Description = category.Description;
                        query.ImageUrl = category.ImageUrl;
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