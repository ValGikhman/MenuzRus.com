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

    public class ItemProduct {

        #region Public Properties

        public Int32 id { set; get; }

        public Int32 ItemId { set; get; }

        public List<ItemProductAssociation> ItemProductAssociation { set; get; }

        public CommonUnit.ProductType Type { set; get; }

        #endregion Public Properties
    }

    public class ItemProductModel : BaseModel {

        #region Public Properties

        public List<ItemProduct> ItemsProduct { set; get; }

        #endregion Public Properties
    }
}