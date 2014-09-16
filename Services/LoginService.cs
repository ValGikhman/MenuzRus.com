using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public class LoginService {

        public User Login(String email, String password) {
            menuzRusDataContext db = new menuzRusDataContext();
            SessionData.user = db.Users.Where(m => m.Email == email && m.Password == password && m.Active && m.EmailConfirmed).FirstOrDefault();
            if (SessionData.user != null) {
                SessionData.customer = db.Customers.Where(m => m.id == SessionData.user.CustomerId).FirstOrDefault();
                SessionData.menu = db.Menus.Where(m => m.CustomerId == SessionData.user.CustomerId).FirstOrDefault();
            }
            return SessionData.user;
        }
    }
}