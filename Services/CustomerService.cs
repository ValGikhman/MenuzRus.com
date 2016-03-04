﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public class CustomerService : ICustomerService {

        #region customer

        public Customer GetCustomer(Int32 id) {
            menuzRusDataContext db = new menuzRusDataContext();
            return db.Customers.Where(m => m.id == id).FirstOrDefault();
        }

        public Int32 SaveCustomer(Customer customer) {
            Customer query = new Customer();
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    if (customer.id != 0)
                        query = db.Customers.Where(m => m.id == customer.id).FirstOrDefault();
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
                        db.Customers.InsertOnSubmit(customer);
                    }
                    db.SubmitChanges();
                    // Update ImageName for new category
                    if (customer.id == 0 && query.ImageUrl != null) {
                        query.ImageUrl = String.Format("{0}{1}", query.id, customer.ImageUrl);
                        db.SubmitChanges();
                    }

                    // Create infostructure
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
                return 0;
            }
            return query.id;
        }

        #endregion customer
    }
}