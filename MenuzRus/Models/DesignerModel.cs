using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using MenuzRus.Models;
using Services;

namespace MenuzRus.Models {

    public class DesignerModel : BaseModel {

        public List<Category> Categories { set; get; }

        public Common.CategoryType CategoryType { set; get; }
    }
}