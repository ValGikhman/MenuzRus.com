using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using MenuzRus.Models;
using Services;

namespace MenuzRus.Models {

    public class SettingModel {

        public string Type { get; set; }

        public string Value { get; set; }
    }

    public class SortOrderModel {

        public String ids { get; set; }
    }

    public class YourMenuModel : BaseModel {

        public List<Category> Categories { set; get; }

        public Int32 MenuId { set; get; }

        public List<Menus> Menus { set; get; }

        public IEnumerable<String> PageBackgrounds { set; get; }

        public Dictionary<String, String> Settings { set; get; }

        public IEnumerable<String> Wallpapers { set; get; }
    }
}