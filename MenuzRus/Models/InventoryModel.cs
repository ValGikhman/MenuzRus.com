﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Services;

namespace MenuzRus.Models {

    public class Inventory {
        public Int32 id { set; get; }

        public Int32 ItemId { set; get; }
        public List<ItemInventoryAssociation> ItemInventoryAssociation { set; get; }
    }

    public class InventoryModel : BaseModel {
        public List<Inventory> Inventory { set; get; }
    }
}