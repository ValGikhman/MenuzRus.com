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

        public Double Adjustment { get; set; }

        public Double AdjustmentPercent { get; set; }

        public OrderCheck Check { get; set; }

        public List<LineItem> Items { get; set; }

        public Int32 Split { get; set; }

        public IEnumerable<Double> SplitValues { get; set; }

        public Double Subtotal { get; set; }

        public Double Summary { get; set; }

        public Double Tax { get; set; }

        public Double TaxPercent { get; set; }

        public Double Total { get; set; }
    }

    public class LineItem {

        public String Description { get; set; }

        public Double Price { get; set; }

        public List<LineItem> SubItems { get; set; }
    }
}