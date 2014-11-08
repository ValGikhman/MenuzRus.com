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
        public HttpPostedFileBase Image { get; set; }

        [DisplayName("Image")]
        public String ImageUrl { get; set; }

        [DisplayName("Business Name")]
        [Required]
        public String Name { get; set; }

        [DisplayName("Phone#")]
        public String Phone { get; set; }

        [DisplayName("Kitchen Printer")]
        [Required]
        public String PrinterKitchen { get; set; }

        [DisplayName("Kitchen Printer Width")]
        [Required]
        public String PrinterKitchenWidth { get; set; }

        [DisplayName("POS Printer")]
        [Required]
        public String PrinterPOS { get; set; }

        [DisplayName("POS Printer Width")]
        [Required]
        public String PrinterPOSWidth { get; set; }

        public IEnumerable<SelectListItem> Printers { get; set; }

        public IEnumerable<SelectListItem> PrinterWidth { get; set; }

        [DisplayName("State")]
        [Required]
        public String State { get; set; }

        public IEnumerable<SelectListItem> States { get; set; }

        [DisplayName("Tax")]
        [Required]
        public Decimal Tax { get; set; }

        [DisplayName("Zip")]
        [Required]
        public String Zip { get; set; }
    }
}