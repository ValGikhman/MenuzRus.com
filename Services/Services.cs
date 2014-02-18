using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Services;
using StringExtensions;

namespace MenuzRus {

    public static class SessionData {

        public static Contact contact { get; set; }

        public static Customer customer { get; set; }

        public static Exception exeption { get; set; }
    }

    public class Services {

        public Contact Login(String email, String password) {
            menuzRusDataContext db = new menuzRusDataContext();
            SessionData.contact = db.Contacts.Where(m => m.Email == email && m.Password == password).FirstOrDefault();
            if (SessionData.contact != null) {
                SessionData.customer = db.Customers.Where(m => m.id == SessionData.contact.CustomerId).FirstOrDefault();
            }
            return SessionData.contact;
        }

        #region categories

        public Boolean DeleteCategory(Category category) {
            Boolean retVal = true;
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    db.Categories.DeleteOnSubmit(category);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex) {
                SessionData.exeption = ex;
                retVal = false;
            }
            return retVal;
        }

        public List<Category> GetCategories(Int32 id, Common.Monitor monitor) {
            menuzRusDataContext db = new menuzRusDataContext();
            return db.Categories.Where(m => m.CustomerId == id && m.Monitor == monitor.ToString() && m.Active).OrderBy(m => m.SortOrder).ToList();
        }

        public Category GetCategory(Int32 id) {
            menuzRusDataContext db = new menuzRusDataContext();
            return db.Categories.Where(m => m.id == id && m.Active).FirstOrDefault();
        }

        public Boolean SaveCategory(Category category) {
            Boolean retVal = true;
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    Category query = db.Categories.Where(m => m.id == category.id && m.Active).FirstOrDefault();
                    if (query != default(Category)) {
                        query.Active = category.Active;
                        query.Name = category.Name;
                        query.Description = category.Description;
                        query.ImageUrl = category.ImageUrl;
                        query.Monitor = category.Monitor;
                        query.Side = category.Side;
                    }
                    if (category.id == 0) {
                        db.Categories.InsertOnSubmit(query);
                    }
                    db.SubmitChanges();
                }
            }
            catch (Exception ex) {
                SessionData.exeption = ex;
                retVal = false;
            }
            return retVal;
        }

        #endregion categories

        #region items

        public Boolean DeleteItem(Item item) {
            Boolean retVal = true;
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    db.Items.DeleteOnSubmit(item);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex) {
                SessionData.exeption = ex;
                retVal = false;
            }
            return retVal;
        }

        public Item GetItem(Int32 id) {
            menuzRusDataContext db = new menuzRusDataContext();
            Item item = db.Items.Where(m => m.id == id && m.Active).FirstOrDefault();
            return item;
        }

        public List<Item> GetItems(Int32 id) {
            List<Item> items;
            menuzRusDataContext db = new menuzRusDataContext();
            items = db.Items.Where(m => m.CategoryId == id && m.Active).OrderBy(m => m.SortOrder).ToList();
            return items;
        }

        public Boolean SaveItem(Item item) {
            Boolean retVal = true;
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    Item query = db.Items.Where(m => m.id == item.id).FirstOrDefault();
                    if (query != default(Item)) {
                        query.Active = item.Active;
                        query.Name = item.Name;
                        query.Description = item.Description;
                        query.ImageUrl = item.ImageUrl;
                        query.ShowAsPrice = item.ShowAsPrice;
                    }
                    if (item.id == 0) {
                        db.Items.InsertOnSubmit(query);
                    }
                    db.SubmitChanges();
                }
            }
            catch (Exception ex) {
                SessionData.exeption = ex;
                retVal = false;
            }
            return retVal;
        }

        #endregion items

        #region customer

        public Customer GetCustomer(Int32 id) {
            menuzRusDataContext db = new menuzRusDataContext();
            return db.Customers.Where(m => m.id == id).FirstOrDefault();
        }

        public Boolean SaveCustomer(Customer customer) {
            Boolean retVal = true;
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    Customer query = db.Customers.Where(m => m.id == customer.id).FirstOrDefault();
                    if (query != default(Customer)) {
                        query.Name = customer.Name;
                        query.Address = customer.Address;
                        query.Address2 = customer.Address2;
                        query.City = customer.City;
                        query.State = customer.State;
                        query.Phone = customer.Phone.CleanPhone();
                        query.Zip = customer.Zip;
                        query.ImageUrl = customer.ImageUrl;
                    }

                    if (customer.id == 0) {
                        db.Customers.InsertOnSubmit(customer);
                    }
                    db.SubmitChanges();

                    // Create infostructure
                    String path = String.Format("{0}//Images/Menus/{1}", AppDomain.CurrentDomain.BaseDirectory, customer.id);
                    if (!Directory.Exists(path)) {
                        DirectoryInfo di = Directory.CreateDirectory(path);
                        String subpath = String.Format("{0}/Customer", path);
                        if (!Directory.Exists(subpath)) {
                            di = Directory.CreateDirectory(subpath);
                        }
                        subpath = String.Format("{0}/Contacts", path);
                        if (!Directory.Exists(subpath)) {
                            di = Directory.CreateDirectory(subpath);
                        }
                        subpath = String.Format("{0}/Categories", path);
                        if (!Directory.Exists(subpath)) {
                            di = Directory.CreateDirectory(subpath);
                        }
                        subpath = String.Format("{0}/Items", path);
                        if (!Directory.Exists(subpath)) {
                            di = Directory.CreateDirectory(subpath);
                        }
                    }
                }
            }
            catch (Exception ex) {
                SessionData.exeption = ex;
                retVal = false;
            }
            return retVal;
        }

        #endregion customer

        #region contact

        public Contact GetContact(Int32 id) {
            menuzRusDataContext db = new menuzRusDataContext();
            return db.Contacts.Where(m => m.id == id && m.Active).FirstOrDefault();
        }

        public Boolean SaveContact(Contact contact) {
            Boolean retVal = true;
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    db.Contacts.InsertOnSubmit(contact);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex) {
                SessionData.exeption = ex;
                retVal = false;
            }
            return retVal;
        }

        #endregion contact

        #region settings

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

        #endregion settings

        public Boolean SendEmailConfirmation(Contact contact) {
            Boolean retVal = true;
            String html = System.IO.File.ReadAllText(String.Format("{0}/App_Data/emailTemplates/EmailConfirmation.html", AppDomain.CurrentDomain.BaseDirectory));
            html = html.Replace("<<FirstName>>", contact.FirstName).Replace("<<LastName>>", contact.LastName);
            retVal = Common.SendEmail(contact.Email, "MenuzRus email confirmation", html);
            if (!retVal) {
                SessionData.exeption = new Exception("Cannot send Email");
            }

            return retVal;
        }
    }
}