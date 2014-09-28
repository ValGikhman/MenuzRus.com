﻿using System;
using System.Collections.Generic;
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

        public List<MenuItem> GetMenuCategories(Int32 customerId, Common.CategoryType type) {
            List<MenuItem> retVal;
            menuzRusDataContext db = new menuzRusDataContext();

            retVal = (from m in db.Menus
                      join mi in db.MenuItems on m.id equals mi.MenuId
                      join i in db.Items on mi.ItemId equals i.id
                      join c in db.Categories on i.CategoryId equals c.id
                      where m.CustomerId == customerId && c.Status != (Int32)Common.Status.NotActive && c.Type == (Int32)type
                      select mi).ToList();

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