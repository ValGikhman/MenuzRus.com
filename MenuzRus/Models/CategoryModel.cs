using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Services;

namespace MenuzRus.Models {

    public class CategoryModel : BaseModel {

        [DisplayName("Show")]
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

        [DisplayName("Menu name")]
        public Int32 MenuId { get; set; }

        [DisplayName("Menu")]
        [Required]
        public List<Services.Menu> Menus { set; get; }

        [DisplayName("Category Name")]
        public String Name { get; set; }

        [DisplayName("Side")]
        [Required]
        public Common.Side Side { get; set; }
    }
}