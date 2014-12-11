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

    public class CheckPrint {

        public Decimal Adjustment { get; set; }

        public Decimal AdjustmentPercent { get; set; }

        public Services.Check Check { get; set; }

        public DateTime CreatedDate { get; set; }

        public List<LineItem> Items { get; set; }

        public Int32 Split { get; set; }

        public IEnumerable<Decimal> SplitValues { get; set; }

        public Decimal Subtotal { get; set; }

        public Decimal Summary { get; set; }

        public Decimal Tax { get; set; }

        public Decimal TaxPercent { get; set; }

        public Decimal Total { get; set; }

        public User User { get; set; }
    }

    public class KitchenOrderPrint {

        public Services.Check Check { get; set; }

        public String Comments { get; set; }

        public DateTime CreatedDate { get; set; }

        public Int32 id { get; set; }

        public List<LineItem> Items { get; set; }

        public User User { get; set; }
    }

    public class LineItem {

        public Boolean Alerted { get; set; }

        public Int32 CheckMenuId { get; set; }

        public String Comments { get; set; }

        public String Description { get; set; }

        public Int32 id { get; set; }

        public Boolean Ordered { get; set; }

        public Decimal Price { get; set; }

        public List<LineItem> SubItems { get; set; }
    }
}