using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public class MenuService {

        public Boolean DeleteMenu(Int32? id) {
            Menu query = new Menu();
            id = id.HasValue ? id : 0;
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    query = db.Menus.Where(m => m.id == id).FirstOrDefault();
                    if (query != default(Menu)) {
                        IEnumerable<MenuCategory> menuCategories = db.MenuCategories.Where(m => m.MenuId == id);
                        if (menuCategories != null) {
                            db.MenuCategories.DeleteAllOnSubmit(menuCategories);
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

        public List<Menu> GetMenus(Int32 id) {
            menuzRusDataContext db = new menuzRusDataContext();
            return db.Menus.Where(m => m.CustomerId == id).ToList();
        }

        public Int32 SaveMenu(Menu menu) {
            Menu query = new Menu();
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    if (menu.id != 0)
                        query = db.Menus.Where(m => m.id == menu.id).FirstOrDefault();
                    if (query != default(Menu)) {
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