using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public class ConfirmationService : IConfirmationService {
        private User user;

        public User Confirm(String hash) {
            try {
                menuzRusDataContext db = new menuzRusDataContext();
                user = db.Users.Where(m => m.Hash == hash).FirstOrDefault();
                if (user != default(User)) {
                    user.Active = true;
                    user.EmailConfirmed = true;
                    db.SubmitChanges();
                }
            }
            catch (Exception ex) {
                return null;
            }
            return user;
        }
    }
}