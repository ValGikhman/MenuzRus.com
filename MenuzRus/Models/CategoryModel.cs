using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace MenuzRus.Models {

    public class CategoryModel : BaseModel {

        #region Public Properties

        [Display(Name = "CATEGORY_DESCRIPTION", ResourceType = typeof(Resources.Resource))]
        public String Description { get; set; }

        public Int32 id { get; set; }

        [DisplayName("Image")]
        public HttpPostedFileBase Image { get; set; }

        [DisplayName("Image Only")]
        public Boolean ImageOnly { get; set; }

        [DisplayName("Image")]
        public String ImageUrl { get; set; }

        [Display(Name = "CATEGORY_NAME", ResourceType = typeof(Resources.Resource))]
        public String Name { get; set; }

        [Display(Name = "CATEGORY_STATUS", ResourceType = typeof(Resources.Resource))]
        public CommonUnit.Status Status { get; set; }

        [Display(Name = "CATEGORY_TYPE", ResourceType = typeof(Resources.Resource))]
        public CommonUnit.CategoryType Type { get; set; }

        #endregion Public Properties
    }
}