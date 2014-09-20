using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Services;

namespace MenuzRus.Models {

    public class CategoryModel : BaseModel {

        [DisplayName("Description")]
        public String Description { get; set; }

        public Int32 id { get; set; }

        [DisplayName("Image")]
        public HttpPostedFileBase Image { get; set; }

        [DisplayName("Image Only")]
        public Boolean ImageOnly { get; set; }

        [DisplayName("Image")]
        public String ImageUrl { get; set; }

        [DisplayName("Category Name")]
        public String Name { get; set; }

        [DisplayName("Status")]
        public Common.Status Status { get; set; }

        [DisplayName("Type")]
        public Common.CategoryType Type { get; set; }
    }
}