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
    }
}