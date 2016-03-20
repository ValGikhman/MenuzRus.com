using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using MenuzRus.Models;
using Services;

namespace MenuzRus.Models {

    public class Category {
        public String Description { set; get; }

        public Int32 id { set; get; }

        public String ImageUrl { set; get; }

        public IEnumerable<Item> Items { set; get; }

        public String Name { set; get; }
    }

    public class DesignerModel : BaseModel {
        public List<Services.Category> Categories { set; get; }

        public Common.CategoryType CategoryType { set; get; }

        public EntitySet<Services.ItemProduct> ItemProducts { set; get; }

        public List<MenuItem> MenuItems { set; get; }

        public List<MenuDesign> Selected { set; get; }
    }

    public class InventoryModel : BaseModel {
        public List<Services.Category> Categories { set; get; }

        public EntitySet<Services.Iten> ItemProducts { set; get; }

        public List<MenuItem> MenuItems { set; get; }

        public List<MenuDesign> Selected { set; get; }
    }

    public class Menu {
        public String Description { set; get; }

        public Int32 id { set; get; }

        public List<MenuItem> MenuItems { set; get; }

        public String Name { set; get; }
    }

    public class MenuDesignerModel : DesignerModel {
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