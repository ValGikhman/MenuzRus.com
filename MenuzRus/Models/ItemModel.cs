using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Services;

namespace MenuzRus.Models {

    public class ItemModel : BaseModel {

        [DisplayName("Show")]
        [Required]
        public Common.Status Active { get; set; }

        public Int32 CategoryId { get; set; }

        [DisplayName("Description")]
        public String Description { get; set; }

        public Int32 id { get; set; }

        [DisplayName("Image")]
        public HttpPostedFileBase Image { get; set; }

        [DisplayName("Image")]
        public String ImageUrl { get; set; }

        [DisplayName("Name")]
        public String Name { get; set; }

        [DisplayName("Show As Price")]
        public String ShowAsPrice { get; set; }
    }
}