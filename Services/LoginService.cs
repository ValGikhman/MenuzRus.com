using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public class LoginService : BaseService, ILoginService {

        public Tuple<User, Customer, Menu> Login(String email, String password) {
            Tuple<User, Customer, Menu> retValue;
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);

            User user;
            Customer customer;
            Menu menu;

            user = db.Users.Where(m => m.Email == email && m.Password == password && m.Active && m.EmailConfirmed).FirstOrDefault();
            if (user != null) {
                customer = db.Customers.Where(m => m.id == user.CustomerId).FirstOrDefault();
                menu = db.Menus.Where(m => m.CustomerId == user.CustomerId).FirstOrDefault();

                retValue = new Tuple<User, Customer, Menu>(user, customer, menu);

                return retValue;
            }

            return null;
        }
    }
}