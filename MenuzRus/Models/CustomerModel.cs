using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MenuzRus.Models {

    public class CustomerModel : BaseModel {

        [DisplayName("Address")]
        [Required]
        public String Address { get; set; }

        [DisplayName("Address 2")]
        public String Address2 { get; set; }

        [DisplayName("City")]
        [Required]
        public String City { get; set; }

        public Int32 id { get; set; }

        [DisplayName("Image")]
        public HttpPostedFileBase Image1 { get; set; }

        [DisplayName("Image")]
        public HttpPostedFileBase Image2 { get; set; }

        [DisplayName("Image")]
        public String ImageUrl1 { get; set; }

        [DisplayName("Image")]
        public String ImageUrl2 { get; set; }

        [DisplayName("Business Name")]
        [Required]
        public String Name { get; set; }

        [DisplayName("State")]
        [Required]
        public String State { get; set; }

        public IEnumerable<SelectListItem> States { get; set; }

        [DisplayName("Zip")]
        [Required]
        public String Zip { get; set; }
    }
}