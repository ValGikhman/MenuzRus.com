using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MenuzRus.Models {

    public class RegistrationModel : BaseModel {

        public CustomerModel Customer { get; set; }

        public UserModel User { get; set; }
    }
}