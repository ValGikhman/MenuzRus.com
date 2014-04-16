using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using MenuzRus.Models;
using Services;

namespace MenuzRus.Models {

    public class Menu {

        public String Description { set; get; }

        public Int32 id { set; get; }

        public String Name { set; get; }
    }

    public class MenuDesignerModel : BaseModel {

        public List<Category> Categories { set; get; }

        public Menu Menu { set; get; }

        public List<Services.Menu> Menus { set; get; }

        public IEnumerable<String> PageBackgrounds { set; get; }

        public Dictionary<String, String> Settings { set; get; }

        public IEnumerable<String> Wallpapers { set; get; }
    }

    public class SettingModel {

        public string Type { get; set; }

        public string Value { get; set; }
    }

    public class SortOrderModel {

        public String ids { get; set; }
    }
}