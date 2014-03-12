using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Services;
using StringExtensions;

namespace MenuzRus {

    public class LoginService {

        public Contact Login(String email, String password) {
            menuzRusDataContext db = new menuzRusDataContext();
            SessionData.contact = db.Contacts.Where(m => m.Email == email && m.Password == password).FirstOrDefault();
            if (SessionData.contact != null) {
                SessionData.customer = db.Customers.Where(m => m.id == SessionData.contact.CustomerId).FirstOrDefault();
                SessionData.menu = db.Menus.Where(m => m.CustomerId == SessionData.contact.CustomerId).FirstOrDefault();
            }
            return SessionData.contact;
        }
    }
}