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

    public class ItemAssociation {
        public Int32 id { set; get; }

        public List<ItemAssociation> ItemAssociations { set; get; }
        public Int32 ItemId { set; get; }
        public Int32 ItemReferenceId { set; get; }
        public Common.ProductType Type { set; get; }
    }

    public class ItemAssociationModel : BaseModel {
        public List<ItemAssociation> ItemAssociations { set; get; }
    }
}