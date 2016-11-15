using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public class CustomerService : BaseService, ICustomerService {

        #region customer

        public Customer GetCustomer(Int32 id) {
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            return db.Customers.Where(m => m.id == id).FirstOrDefault();
        }

        public List<Module> GetModulesAll() {
            try {
                menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
                return db.Modules.ToList();
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public List<Module> GetModulesByCustomer(Int32 id) {
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            return (from cm in db.CustomerModules
                    join mp in db.ModulePrices on cm.ModulePriceId equals mp.id
                    join m in db.Modules on mp.ModuleId equals m.id
                    where cm.CustomerId == id && cm.EndDate == null
                    select m).ToList();
        }

        public Int32 SaveCustomer(Customer customer) {
            Customer query = new Customer();
            try {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    if (customer.id != 0) {
                        query = db.Customers.Where(m => m.id == customer.id).FirstOrDefault();
                    }

                    if (query != default(Customer)) {
                        query.Name = customer.Name;
                        query.Address = customer.Address;
                        query.Address2 = customer.Address2;
                        query.City = customer.City;
                        query.State = customer.State;
                        query.Phone = customer.Phone.CleanPhone();
                        query.Zip = customer.Zip;
                        query.Tax = customer.Tax;
                        query.ImageUrl = customer.ImageUrl;
                    }

                    if (customer.id == 0) {
                        db.Customers.InsertOnSubmit(query);
                    }
                    db.SubmitChanges();
                    // Update ImageName for a new category
                    if (customer.id == 0 && query.ImageUrl != null) {
                        query.ImageUrl = String.Format("{0}{1}", query.id, customer.ImageUrl);
                        db.SubmitChanges();
                    }

                    // Create infrastructure
                    String path = String.Format("{0}//Images/Menus/{1}", AppDomain.CurrentDomain.BaseDirectory, customer.id);
                    if (!Directory.Exists(path)) {
                        DirectoryInfo di = Directory.CreateDirectory(path);
                        String subpath = String.Format("{0}/Customers", path);
                        if (!Directory.Exists(subpath)) {
                            di = Directory.CreateDirectory(subpath);
                        }
                        subpath = String.Format("{0}/Users", path);
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
                throw ex;
            }
            return query.id;
        }

        public void SaveModulesByCustomer(Int32 id, Int32[] modulesIds) {
            try {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    CustomerModule customerModule = new CustomerModule();
                    customerModule.CustomerId = id;

                    foreach (Int32 priceId in modulesIds) {
                        customerModule.ModulePriceId = priceId;
                        db.CustomerModules.InsertOnSubmit(customerModule);
                    }
                    db.SubmitChanges();
                }
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public void UpdateModules(Int32 id, Int32 moduleId) {
            CustomerModule query;
            try {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    // Negative - stops service by ending it as of today
                    if (moduleId < 0) {
                        query = db.CustomerModules.Where(m => m.CustomerId == id && m.ModulePriceId == Math.Abs(moduleId) && m.EndDate == null).FirstOrDefault();
                        if (query != default(CustomerModule)) {
                            query.EndDate = DateTime.Now.Date;
                        }
                    }
                    // Positive - inserts
                    else if (moduleId > 0) {
                        query = new CustomerModule();
                        query.CustomerId = id;
                        query.ModulePriceId = moduleId;
                        db.CustomerModules.InsertOnSubmit(query);
                    }
                    else if (moduleId == 0) { }
                    db.SubmitChanges();
                }
            }
            catch (Exception ex) {
                throw ex;
            }

            return;
        }

        #endregion customer
    }
}