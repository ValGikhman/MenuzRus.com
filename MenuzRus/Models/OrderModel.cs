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

        #region Public Properties

        public List<CheckMenuItem> CheckMenuItems { get; set; }

        public Int32 id { get; set; }

        public Decimal Price { get; set; }

        public CommonUnit.CheckStatus Status { get; set; }

        public CommonUnit.CheckType Type { get; set; }

        #endregion Public Properties
    }

    public class CheckMenuItem {

        #region Public Properties

        public Int32 CheckId { get; set; }

        public List<CheckMenuItemProduct> CheckMenuItemProducts { get; set; }

        public Customer Customer { get; set; }

        public String Description { get; set; }

        public Boolean HasProducts { get; set; }

        public Int32 id { get; set; }

        public Int32 ItemId { get; set; }

        public String Name { get; set; }

        public Boolean Ordered { get; set; }

        public Decimal Price { get; set; }

        #endregion Public Properties
    }

    public class CheckMenuItemProduct {

        #region Public Properties

        public List<CheckMenuItemProductAssociation> CheckMenuItemProductAssociations { get; set; }

        public Int32 id { get; set; }

        public Int32 ItemId { get; set; }

        public CommonUnit.ProductType Type { get; set; }

        #endregion Public Properties
    }

    public class CheckMenuItemProductAssociation {

        #region Public Properties

        public Customer Customer { get; set; }
        public Int32 id { get; set; }

        public String ImageUrl { get; set; }
        public Int32 ItemId { get; set; }

        public String Name { get; set; }

        public Decimal Price { get; set; }

        public Boolean Selected { get; set; }

        #endregion Public Properties
    }

    public class KitchenOrderModel : BaseModel {

        #region Public Properties

        public List<CheckPrint> Checks { set; get; }

        public Services.Table Table { set; get; }

        public Services.TableOrder TableOrder { set; get; }

        #endregion Public Properties
    }

    public class OrderModel : BaseModel {

        #region Public Properties

        public List<Services.Category> Categories { set; get; }

        public List<Check> Checks { set; get; }

        public Menu Menu { set; get; }

        public String Referer { set; get; }

        public Services.Table Table { set; get; }

        public Int32 TableId { set; get; }

        public Services.TableOrder TableOrder { set; get; }

        public Int32 uid { set; get; }

        #endregion Public Properties
    }
}