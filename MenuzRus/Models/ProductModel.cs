using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using MenuzRus.Models;
using Services;

namespace MenuzRus.Models {

    public class ProductModel : BaseModel {

        public List<Category> Categories { get; set; }
    }
}