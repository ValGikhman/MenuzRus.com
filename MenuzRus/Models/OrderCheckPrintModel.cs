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

        public List<LineItem> Items { get; set; }

        public Double Summary { get; set; }

        public Double Tax { get; set; }

        public Double Total { get; set; }
    }

    public class LineItem {

        public String Description { get; set; }

        public Double Price { get; set; }

        public List<LineItem> SubItems { get; set; }
    }
}