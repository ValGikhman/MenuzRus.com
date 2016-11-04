using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public class CategoryService : BaseService, ICategoryService {

        #region Public Methods

        public Boolean DeleteCategory(Int32? id) {
            Category query = new Category();
            id = id.HasValue ? id : 0;
            try {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    query = db.Categories.Where(m => m.id == id).FirstOrDefault();
                    if (query != default(Category)) {
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

        public List<Category> GetCategories(Int32 customerId, Common.CategoryType type) {
            return GetCategories(customerId, type, null);
        }

        public List<Category> GetCategories(Int32 customerId, Common.CategoryType type, String search) {
            List<Category> retVal;
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            try {
                retVal = db.Categories.Where(m => m.CustomerId == customerId && m.Status == (Int32)Common.Status.Active && m.Type == (Int32)type).ToList();

                if (!String.IsNullOrEmpty(search)) {
                    retVal = (from c in retVal
                              where c.Name.ToUpper().Contains(search)
                                    || (c.Items.Where(m => m.Name.ToUpper().Contains(search) || m.Description.toUpper().Contains(search)).Any())
                              select c).ToList();
                }
            }
            catch (Exception ex) {
                throw ex;
            }

            return retVal;
        }

        public Category GetCategory(Int32 id) {
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            return db.Categories.Where(m => m.id == id).FirstOrDefault();
        }

        public List<Category> GetMenuCategories(Int32 customerId, Common.CategoryType type, Int32 menuId) {
            List<CategoryView> retVal;
            menuzRusDataContext db;
            try {
                if (menuId == 0) {
                    // All
                    return (List<Category>)GetCategories(customerId, type);
                }

                db = new menuzRusDataContext(base.connectionString);
                retVal = (from category in GetCategories(customerId, type)
                          select new CategoryView {
                              id = category.id,
                              Name = category.Name,
                              Description = category.Description,
                              Items = (from item in db.Items
                                       join menuItem in db.MenuItems on item.id equals menuItem.ItemId
                                       where category.id == item.CategoryId && menuItem.MenuId == menuId
                                       select item
                                       ).ToEntitySet()
                          }
                        ).ToList();

                return retVal.Cast<Category>().ToList();
            }
            catch (Exception ex) {
            }

            return null;
        }

        public Int32 SaveCategory(Category category) {
            Category query = new Category();
            try {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
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
                return 0;
            }
            return query.id;
        }

        #endregion Public Methods
    }

    internal class CategoryView : Category { };
}