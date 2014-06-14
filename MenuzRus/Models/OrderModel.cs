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

    public class OrderItem {

        [DisplayName("Description")]
        public String Description { get; set; }

        public Int32 id { get; set; }

        [DisplayName("Name")]
        public String Name { get; set; }

        public List<OrderItemProduct> OrderItemProducts { get; set; }
    }

    public class OrderItemProduct {

        public Int32 id { get; set; }

        public List<OrderItemProductAssociation> OrderItemProductAssociations { get; set; }

        public Common.ProductType Type { get; set; }
    }

    public class OrderItemProductAssociation {

        [DisplayName("Description")]
        public String Description { get; set; }

        public Int32 id { get; set; }

        [DisplayName("Name")]
        public String Name { get; set; }
    }

    public class OrderModel : BaseModel {

        public List<Category> Categories { set; get; }

        public OrderItem OrderItem { set; get; }

        public List<Order> Orders { set; get; }
    }
}