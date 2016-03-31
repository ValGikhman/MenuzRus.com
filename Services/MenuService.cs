using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public class MenuService : BaseService, IMenuService {
        //    public Boolean DeleteMenu(Int32? id) {
        //        Menu query = new Menu();
        //        id = id.HasValue ? id : 0;
        //        try {
        //            using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
        //                query = db.Menus.Where(m => m.id == id).FirstOrDefault();
        //                if (query != default(Menu)) {
        //                    IEnumerable<MenuItem> menuCategories = db.MenuItems.Where(m => m.MenuId == id);
        //                    if (menuCategories != null) {
        //                        db.MenuItems.DeleteAllOnSubmit(menuCategories);
        //                    }
        //                    db.Menus.DeleteOnSubmit(query);
        //                    db.SubmitChanges();
        //                }
        //            }
        //        }
        //        catch (Exception ex) {
        //            return false;
        //        }
        //        return true;
        //    }

        //    public List<Menu> GetMenus(Int32 id) {
        //        menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
        //        return db.Menus.Where(m => m.CustomerId == id).ToList();
        //    }

        //    public Int32 SaveMenu(Menu menu) {
        //        Menu query = new Menu();
        //        try {
        //            using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
        //                if (menu.id != 0) {
        //                    query = db.Menus.Where(m => m.id == menu.id).FirstOrDefault();
        //                }

        //                if (query != default(Menu)) {
        //                    query.CustomerId = menu.CustomerId;
        //                    query.Name = menu.Name;
        //                    query.Description = menu.Description;
        //                }
        //                if (menu.id == 0) {
        //                    db.Menus.InsertOnSubmit(query);
        //                }
        //                db.SubmitChanges();
        //            }
        //        }
        //        catch (Exception ex) {
        //            return 0;
        //        }
        //        return query.id;
        //    }

        //    public Boolean SaveMenuItems(List<MenuItem> model) {
        //        MenuItem query = new MenuItem();
        //        IEnumerable<MenuItem> itemsToDelete;
        //        if (model == null) {
        //            return false;
        //        }
        //        try {
        //            query = model[0];
        //            using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
        //                itemsToDelete = db.MenuItems.Where(m => m.MenuId == query.MenuId && !model.Contains(m));
        //                if (itemsToDelete.Any()) {
        //                    db.MenuItems.DeleteAllOnSubmit(itemsToDelete);
        //                    db.SubmitChanges();
        //                }
        //                foreach (MenuItem item in model) {
        //                    query = db.MenuItems.Where(m => m.MenuId == item.MenuId && m.ItemId == item.ItemId).FirstOrDefault();
        //                    if (query == default(MenuItem)) {
        //                        query = new MenuItem();
        //                        query.MenuId = item.MenuId;
        //                        query.ItemId = item.ItemId;
        //                        query.Side = item.Side;
        //                        db.MenuItems.InsertOnSubmit(query);
        //                    }
        //                }
        //                db.SubmitChanges();
        //            }
        //        }
        //        catch (Exception ex) {
        //            return false;
        //        }
        //        return true;
        //    }
    }
}