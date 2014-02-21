using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MenuzRus.Models {

    public class CategoryModel : BaseModel {

        [DisplayName("Show")]
        [Required]
        public Common.Status Active { get; set; }

        [DisplayName("Description")]
        public String Description { get; set; }

        public Int32 id { get; set; }

        [DisplayName("Image")]
        public HttpPostedFileBase Image { get; set; }

        [DisplayName("Image Only")]
        public Boolean ImageOnly { get; set; }

        [DisplayName("Image")]
        public String ImageUrl { get; set; }

        public Int32 MenuId { get; set; }

        [DisplayName("Monitor")]
        [Required]
        public Common.Monitor Monitor { get; set; }

        [DisplayName("Category Name")]
        public String Name { get; set; }

        [DisplayName("Side")]
        [Required]
        public Common.Side Side { get; set; }
    }
}