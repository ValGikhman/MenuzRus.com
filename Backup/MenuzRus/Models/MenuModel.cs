using System;
using System.Collections.Generic;

namespace MenuzRus.Models {
    public class Menu {
        public Settings Settings;
        public List<MenuCategory> MenuCategories;
    }

    public class MenuCategory {
        public String Id { get; set; }
        public String ImageUrl { get; set; }
        public Boolean ImageOnly { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public List<MenuItem> MenuItems;
        public String Monitor { get; set; }
        public String Side { get; set; }
        public Boolean Show { get; set; }
    }

    public class MenuItem {
        public String Id { get; set; }
        public String ImageUrl { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String Price { get; set; }
        public Boolean Show { get; set; }
    }

    public class Settings {
        public Int32 ResolutionWidth { get; set; }
        public Int32 ResolutionHeight { get; set; }
        public Int32 OffsetLeft { get; set; }
        public Int32 OffsetTop { get; set; }
    }

}