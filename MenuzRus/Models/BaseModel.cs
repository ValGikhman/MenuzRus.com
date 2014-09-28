using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Services;

namespace MenuzRus.Models {

    public class BaseModel {

        public BaseModel() {
            Me = SessionData.user;
            MyCompany = SessionData.customer;
        }

        public User Me { get; set; }

        public Customer MyCompany { get; set; }

        public List<Category> ConvertToCategory(List<MenuItem> menuItems) {
            List<Category> retVal;
            try {
                retVal = new List<Category>();
                if (menuItems != null) {
                    foreach (Services.MenuItem menuItem in menuItems) {
                        Category cat = new Category();
                        cat.id = menuItem.Items[0].Category.id;
                        cat.Name = menuItem.Items[0].Category.Name;
                        cat.Description = menuItem.Items[0].Category.Description;
                        cat.ImageUrl = menuItem.Items[0].Category.ImageUrl;
                        cat.Items = menuItem.Items[0].Category.Items;
                        cat.Side = (Common.Side)menuItem.Side;
                        retVal.Add(cat);
                    }
                }
                return retVal;
            }
            catch (Exception ex) {
            }
            return null;
        }
    }
}