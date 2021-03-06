﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public class UserService : BaseService, IUserService {

        #region user

        public Boolean DeleteUser(Int32? id) {
            User query = new User();
            id = id.HasValue ? id : 0;
            try {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    query = db.Users.Where(m => m.id == id).FirstOrDefault();
                    if (query != default(User)) {
                        db.Users.DeleteOnSubmit(query);
                    }
                    db.SubmitChanges();
                }
            }
            catch (Exception ex) {
                return false;
            }
            return true;
        }

        public User GetUser(Int32 id) {
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            return db.Users.Where(m => m.id == id && m.Active).FirstOrDefault();
        }

        public User GetUserByHash(String hash) {
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            return db.Users.Where(m => m.Hash == hash).FirstOrDefault();
        }

        public List<User> GetUsers(Int32 id) {
            List<User> users;
            menuzRusDataContext db = new menuzRusDataContext(base.connectionString);
            users = db.Users.Where(m => m.CustomerId == id).OrderBy(m => m.LastName).OrderBy(m => m.FirstName).ToList();
            return users;
        }

        public Int32 SaveUser(User user) {
            User query = new User();
            try {
                using (menuzRusDataContext db = new menuzRusDataContext(base.connectionString)) {
                    if (user.id != 0) {
                        query = db.Users.Where(m => m.id == user.id).FirstOrDefault();
                    }

                    if (query != default(User)) {
                        query.CustomerId = user.CustomerId;
                        query.Active = user.Active;
                        query.FirstName = user.FirstName;
                        query.LastName = user.LastName;
                        query.MobilePhone = user.MobilePhone;
                        query.WorkPhone = user.WorkPhone;
                        query.Email = user.Email;
                        query.Password = user.Password;
                        query.ImageUrl = user.ImageUrl;
                        query.Type = user.Type;
                        query.Hash = user.Hash;
                    }
                    if (user.id == 0) {
                        db.Users.InsertOnSubmit(query);
                    }
                    db.SubmitChanges();
                    // Update ImageName for new user
                    if (user.id == 0 && query.ImageUrl != null) {
                        query.ImageUrl = String.Format("{0}{1}", query.id, user.ImageUrl);
                        db.SubmitChanges();
                    }
                }
            }
            catch (Exception ex) {
                return 0;
            }
            return query.id;
        }

        #endregion user
    }
}