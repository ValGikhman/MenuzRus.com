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

        #region Public Properties

        public String Description { set; get; }

        public Int32 id { set; get; }

        public String ImageUrl { set; get; }

        public IEnumerable<Item> Items { set; get; }

        public String Name { set; get; }

        #endregion Public Properties
    }

    public class DesignerModel : BaseModel {

        #region Public Properties

        public List<Services.Category> Categories { set; get; }

        public CommonUnit.CategoryType CategoryType { set; get; }

        public EntitySet<Services.ItemProduct> ItemProducts { set; get; }

        public String Search { set; get; }

        #endregion Public Properties

        //public List<MenuItem> MenuItems { set; get; }

        //public List<MenuDesign> Selected { set; get; }
    }

    public class InventoryAssosiationModel : BaseModel {

        #region Public Properties

        public List<Services.Category> Categories { set; get; }

        public EntitySet<Services.ItemInventoryAssociation> ItemInventoryAssociation { set; get; }

        #endregion Public Properties
    }

    public class Menu : BaseModel {

        #region Public Properties

        public List<Services.Category> Categories { set; get; }

        public Menus CurrentMenu { set; get; }

        public String Description { set; get; }

        public Int32 id { set; get; }

        public List<Menus> Menus { set; get; }

        public String Name { set; get; }

        #endregion Public Properties
    }

    public class MenuDesignerModel : DesignerModel {

        #region Public Properties

        public Menu Menu { set; get; }

        //public List<Services.Menu> Menus { set; get; }

        public IEnumerable<String> PageBackgrounds { set; get; }

        public Dictionary<String, String> Settings { set; get; }

        public IEnumerable<String> Wallpapers { set; get; }

        #endregion Public Properties
    }

    public class MenuItems : BaseModel {

        #region Public Properties

        public Int32 id { set; get; }

        public String Items { set; get; }

        #endregion Public Properties
    }

    public class SettingModel {

        #region Public Properties

        public string Type { get; set; }

        public string Value { get; set; }

        #endregion Public Properties
    }

    public class SortOrderModel {

        #region Public Properties

        public String ids { get; set; }

        #endregion Public Properties
    }
}