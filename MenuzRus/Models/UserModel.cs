using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MenuzRus.Models {

    public class UserModel : BaseModel {

        [DisplayName("Active")]
        public Common.Status Active { get; set; }

        [DisplayName("ConfirmPassword")]
        [DataType(DataType.Password)]
        [Required]
        public String ConfirmPassword { get; set; }

        public Int32 CustomerId { get; set; }

        [DisplayName("Email Address")]
        [DataType(DataType.EmailAddress)]
        [Required]
        public String Email { get; set; }

        [DisplayName("First Name")]
        public String FirstName { get; set; }

        [DisplayName("Hash")]
        public String Hash { get; set; }

        public Int32 id { get; set; }

        [DisplayName("Image")]
        public HttpPostedFileBase Image { get; set; }

        [DisplayName("Image")]
        public String ImageUrl { get; set; }

        [DisplayName("Last Name")]
        public String LastName { get; set; }

        [DisplayName("Mobile Phone#")]
        [DataType(DataType.PhoneNumber)]
        [Required]
        public String MobilePhone { get; set; }

        [DisplayName("Password")]
        [DataType(DataType.Password)]
        [Required]
        public String Password { get; set; }

        public String Referer { get; set; }

        [DisplayName("Type")]
        public Common.UserType Type { get; set; }

        [DisplayName("Work Phone#")]
        [DataType(DataType.PhoneNumber)]
        [Required]
        public String WorkPhone { get; set; }
    }
}