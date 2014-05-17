using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Services;

namespace MenuzRus.Models {

    public class ItemModel : BaseModel {

        [DisplayName("Show")]
        [Required]
        public Common.Status Active { get; set; }

        [DisplayName("Additional Information")]
        public String AdditionalInfo { get; set; }

        [DisplayName("Category")]
        public List<Category> Categories { get; set; }

        public Int32 CategoryId { get; set; }

        [DisplayName("Description")]
        public String Description { get; set; }

        public Int32 id { get; set; }

        [DisplayName("Image")]
        public HttpPostedFileBase Image { get; set; }

        [DisplayName("Image")]
        public String ImageUrl { get; set; }

        [DisplayName("ItemPrices")]
        public List<ItemPrice> ItemPrices { get; set; }

        [DisplayName("Name")]
        public String Name { get; set; }
    }
}