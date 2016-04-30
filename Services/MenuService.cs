using System;
using System.Collections.Generic;
using System.Linq;
using Services;

namespace MenuzRus {

    public class MenuService : BaseService, IMenuService {

        public Boolean DeleteMenu(Int32? id) {
            Menus query = new Menus();
            id = id.HasValue ? id : 0;
            try {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    query = db.Menus.Where(m => m.id == id).FirstOrDefault();
                    if (query != default(Menus)) {
                        IEnumerable<MenuItem> menuItems = db.MenuItems.Where(m => m.MenuId == id);
                        if (menuItems != null) {
                            db.MenuItems.DeleteAllOnSubmit(menuItems);
                        }
                        db.Menus.DeleteOnSubmit(query);
                        db.SubmitChanges();
                    }
                }
            }
            catch (Exception ex) {
                return false;
            }
            return true;
        }

        public List<Menus> GetMenus(Int32 id) {
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            return db.Menus.Where(m => m.CustomerId == id).ToList();
        }

        public Int32 SaveMenu(Menus menu) {
            Menus query = new Menus();
            try {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    if (menu.id != 0) {
                        query = db.Menus.Where(m => m.id == menu.id).FirstOrDefault();
                    }

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
                return 0;
            }
            return query.id;
        }

        public Boolean SaveMenuItems(List<MenuItem> model) {
            MenuItem query = new MenuItem();
            MenuItem toDelete;
            IEnumerable<Int32> itemsToDelete;

            if (model == null) {
                return false;
            }
            try {
                query = model[0];

                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    // Deleting
                    itemsToDelete = db.MenuItems.Where(m => m.MenuId == query.MenuId).Select(m => m.ItemId).Where(m => !model.Select(x => x.ItemId).Contains(m));
                    if (itemsToDelete.Any()) {
                        foreach (Int32 item in itemsToDelete) {
                            toDelete = db.MenuItems.Where(m => m.MenuId == query.MenuId && m.ItemId == item).FirstOrDefault();
                            db.MenuItems.DeleteOnSubmit(toDelete);
                        }
                        db.SubmitChanges();
                    }

                    //Saving
                    foreach (MenuItem item in model) {
                        query = db.MenuItems.Where(m => m.MenuId == item.MenuId && m.ItemId == item.ItemId).FirstOrDefault();
                        if (query == default(MenuItem)) {
                            query = new MenuItem();
                            query.MenuId = item.MenuId;
                            query.ItemId = item.ItemId;
                            db.MenuItems.InsertOnSubmit(query);
                        }
                    }
                    db.SubmitChanges();
                }
            }
            catch (Exception ex) {
                return false;
            }
            return true;
        }
    }
}