using System;
using System.Linq;
using Services;

namespace MenuzRus {

    public class ConfirmationService : BaseService, IConfirmationService {

        #region Private Fields

        private User user;

        #endregion Private Fields

        #region Public Methods

        public User Confirm(String hash) {
            try {
                menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
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

        #endregion Public Methods
    }
}