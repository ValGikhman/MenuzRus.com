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

    public class InventoryModel : BaseModel {
        public List<ItemInventoryAssociation> ItemInventoryAssociations { set; get; }
    }
}