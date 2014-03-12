using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Services;
using StringExtensions;

namespace MenuzRus {

    public class ContactService {

        #region contact

        public Contact GetContact(Int32 id) {
            menuzRusDataContext db = new menuzRusDataContext();
            return db.Contacts.Where(m => m.id == id && m.Active).FirstOrDefault();
        }

        public Int32 SaveContact(Contact contact) {
            Contact query = new Contact();
            try {
                using (menuzRusDataContext db = new menuzRusDataContext()) {
                    if (contact.id != 0)
                        query = db.Contacts.Where(m => m.id == contact.id).FirstOrDefault();
                    if (query != default(Contact)) {
                        query.CustomerId = contact.CustomerId;
                        query.Active = contact.Active;
                        query.FirstName = contact.FirstName;
                        query.LastName = contact.LastName;
                        query.MobilePhone = contact.MobilePhone;
                        query.WorkPhone = contact.WorkPhone;
                        query.Email = contact.Email;
                        query.Password = contact.Password;
                        query.ImageUrl = contact.ImageUrl;
                    }
                    if (contact.id == 0) {
                        db.Contacts.InsertOnSubmit(query);
                    }
                    db.SubmitChanges();
                    // Update ImageName for new category
                    if (contact.id == 0 && query.ImageUrl != null) {
                        query.ImageUrl = String.Format("{0}{1}", query.id, contact.ImageUrl);
                        db.SubmitChanges();
                    }
                }
            }
            catch (Exception ex) {
                SessionData.exeption = ex;
                return 0;
            }
            return query.id;
        }

        #endregion contact
    }
}