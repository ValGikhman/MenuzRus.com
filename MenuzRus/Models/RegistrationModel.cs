using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MenuzRus.Models {

    public class RegistrationModel : BaseModel {

        #region Public Properties

        public CustomerModel Customer { get; set; }

        public String Modules { get; set; }

        public UserModel User { get; set; }

        #endregion Public Properties
    }
}