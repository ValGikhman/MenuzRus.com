using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MenuzRus.Models {

    public class ContactMeModel {

        [DisplayName("Name")]
        public String Name { get; set; }

        [DisplayName("Email Address")]
        [DataType(DataType.EmailAddress)]
        [Required]
        public String Email { get; set; }

        [DisplayName("Message")]
        [DataType(DataType.MultilineText)]
        public String Message { get; set; }
    }
}