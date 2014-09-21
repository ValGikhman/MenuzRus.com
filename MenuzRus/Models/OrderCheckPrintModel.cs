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

    public class CheckPrint {

        public Decimal Adjustment { get; set; }

        public Decimal AdjustmentPercent { get; set; }

        public OrderCheck Check { get; set; }

        public List<LineItem> Items { get; set; }

        public Int32 Split { get; set; }

        public IEnumerable<Decimal> SplitValues { get; set; }

        public Decimal Subtotal { get; set; }

        public Decimal Summary { get; set; }

        public Decimal Tax { get; set; }

        public Decimal TaxPercent { get; set; }

        public Decimal Total { get; set; }
    }

    public class LineItem {

        public String Description { get; set; }

        public Decimal Price { get; set; }

        public List<LineItem> SubItems { get; set; }
    }
}