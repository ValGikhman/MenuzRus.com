using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MenuzRus.Models {

    public class LoginModel : BaseModel {

        [DisplayName("Email Address")]
        [DataType(DataType.EmailAddress)]
        [Required]
        public String Email { get; set; }

        [DisplayName("Password")]
        [DataType(DataType.Password)]
        [Required]
        public String Password { get; set; }

        public Boolean Success { get; set; }
    }
}