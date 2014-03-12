using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Services;
using StringExtensions;

namespace MenuzRus {

    public class MenuService {

        public Boolean DeleteMenu(Int32? id) {
            Menus query = new Menus();
            id = id.HasValue ? id : 0;
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    query = db.Menus.Where(m => m.id == id).FirstOrDefault();
                    if (query != default(Menus)) {
                        IEnumerable<Category> categories = db.Categories.Where(m => m.MenuId == id);
                        if (categories != null) {
                            foreach (Category category in categories) {
                                IEnumerable<Item> items = db.Items.Where(m => m.CategoryId == category.id);
                                db.Items.DeleteAllOnSubmit(items);
                            }
                            db.Categories.DeleteAllOnSubmit(categories);
                        }
                        db.Menus.DeleteOnSubmit(query);
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

        public List<Menus> GetMenus(Int32 id) {
            menuzRusDataContext db = new menuzRusDataContext();
            return db.Menus.Where(m => m.CustomerId == id).ToList();
        }

        public Int32 SaveMenu(Menus menu) {
            Menus query = new Menus();
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    if (menu.id != 0)
                        query = db.Menus.Where(m => m.id == menu.id).FirstOrDefault();
                    if (query != default(Menus)) {
                        query.CustomerId = menu.CustomerId;
                        query.Name = menu.Name;
                        query.Description = menu.Description;
                    }
                    if (menu.id == 0) {
                        db.Menus.InsertOnSubmit(query);
                    }
                    db.SubmitChanges();
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