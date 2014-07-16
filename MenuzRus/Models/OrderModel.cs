using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Services;

namespace MenuzRus.Models {

    public class Check {

        public List<CheckMenuItem> CheckMenuItems { get; set; }

        public Int32 id { get; set; }

        public Decimal Price { get; set; }

        public Common.OrderType Type { get; set; }
    }

    public class CheckMenuItem {

        public Int32 CheckId { get; set; }

        public List<CheckMenuItemProduct> CheckMenuItemProducts { get; set; }

        public Int32 id { get; set; }

        public Int32 ItemId { get; set; }

        public String Name { get; set; }

        public Decimal Price { get; set; }
    }

    public class CheckMenuItemProduct {

        public List<CheckMenuItemProductAssociation> CheckMenuItemProductAssociations { get; set; }

        public Int32 id { get; set; }

        public Int32 ItemId { get; set; }

        public Common.ProductType Type { get; set; }
    }

    public class CheckMenuItemProductAssociation {

        public Int32 id { get; set; }

        public Int32 ItemId { get; set; }

        public String Name { get; set; }

        public Decimal Price { get; set; }

        public Boolean Selected { get; set; }

        public String ShortName { get; set; }
    }

    public class OrderModel : BaseModel {

        public List<Category> Categories { set; get; }

        public List<Check> Checks { set; get; }

        public Int32 TableId { set; get; }

        public Int32 uid { set; get; }
    }
}