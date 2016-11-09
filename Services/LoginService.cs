using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public class LoginService : BaseService, ILoginService {

        #region Public Methods

        public Boolean GetProduction() {
            return (connectionString.IndexOf(devComputer) > -1);
        }

        public Tuple<User, Customer, List<String>> Login(String email, String password) {
            Tuple<User, Customer, List<String>> retValue;
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);

            User user;
            Customer customer;
            List<String> modules = new List<String>();

            user = db.Users.Where(m => m.Email == email && m.Password == password && m.Active && m.EmailConfirmed).FirstOrDefault();
            if (user != null) {
                customer = db.Customers.Where(m => m.id == user.CustomerId).FirstOrDefault();
                var mods = customer.CustomerModules.Select(m => m.ModulePrices.Select(t => t.Module)).ToList().Select(r => r.Select(d => d.Name)).ToList();
                foreach (var mod in mods) {
                    modules.Add(mod.ToArray()[0]);
                }

                //menu = db.Menus.Where(m => m.CustomerId == user.CustomerId).FirstOrDefault();

                retValue = new Tuple<User, Customer, List<String>>(user, customer, modules);

                return retValue;
            }

            return null;
        }

        #endregion Public Methods
    }
}