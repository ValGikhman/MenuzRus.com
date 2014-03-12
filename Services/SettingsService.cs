using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Services;
using StringExtensions;

namespace MenuzRus {

    public class SettingsService {

        public Dictionary<String, String> GetSettings(Int32 id) {
            menuzRusDataContext db = new menuzRusDataContext();
            return db.Settings.Where(m => m.CustomerId == id).ToDictionary(m => m.Type, m => m.Value);
        }

        public Boolean SaveOrder(String ids, String type) {
            Boolean retVal = true;
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    Int32 i = 0;
                    String[] order = ids.Split(',');
                    switch (type) {
                        case "Category":
                            Category category;
                            foreach (String o in order) {
                                category = db.Categories.Where(m => m.id == Convert.ToInt32(o)).FirstOrDefault();
                                if (category != default(Category)) {
                                    category.SortOrder = i;
                                    db.SubmitChanges();
                                    i++;
                                }
                            }
                            break;

                        case "Items":
                            Item item;
                            foreach (String o in order) {
                                item = db.Items.Where(m => m.id == Convert.ToInt32(o)).FirstOrDefault();
                                if (item != default(Item)) {
                                    item.SortOrder = i;
                                    db.SubmitChanges();
                                    i++;
                                }
                            }
                            break;
                    }
                }
            }

            catch (Exception ex) {
                SessionData.exeption = ex;
                retVal = false;
            }
            return retVal;
        }

        public Boolean SaveSetting(Setting setting) {
            Boolean retVal = true;
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    Setting query = db.Settings.Where(m => m.CustomerId == SessionData.customer.id && m.Type == setting.Type).FirstOrDefault();
                    if (query == default(Setting)) {
                        query = new Setting();
                        db.Settings.InsertOnSubmit(query);
                    }

                    query.Type = setting.Type;
                    query.Value = setting.Value;
                    query.CustomerId = SessionData.customer.id;
                    db.SubmitChanges();
                }
            }
            catch (Exception ex) {
                SessionData.exeption = ex;
                retVal = false;
            }
            return retVal;
        }
    }
}