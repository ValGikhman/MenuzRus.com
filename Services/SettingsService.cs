using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public class SettingsService : BaseService, ISettingsService {

        public Dictionary<String, String> GetSettings(Int32 id) {
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            return db.Settings.Where(m => m.CustomerId == id).ToDictionary(m => m.Type, m => m.Value);
        }

        public String GetSettings(Int32 id, Common.Settings type) {
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            return db.Settings.Where(m => m.CustomerId == id && m.Type == type.ToString()).Select(m => m.Value).FirstOrDefault();
        }

        public Boolean SaveOrder(String ids, String type) {
            Boolean retVal = true;
            try {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
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
                retVal = false;
            }
            return retVal;
        }

        public Boolean SaveSetting(Setting setting, Int32 customerId) {
            Boolean retVal = true;
            try {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    Setting query = db.Settings.Where(m => m.CustomerId == customerId && m.Type == setting.Type).FirstOrDefault();
                    if (query == default(Setting)) {
                        query = new Setting();
                        db.Settings.InsertOnSubmit(query);
                    }

                    query.Type = setting.Type;
                    query.Value = setting.Value;
                    query.CustomerId = customerId;
                    db.SubmitChanges();
                }
            }
            catch (Exception ex) {
                retVal = false;
            }
            return retVal;
        }
    }
}