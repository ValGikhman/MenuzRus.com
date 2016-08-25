using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Services;

namespace MenuzRus.Models {

    public class ReportModel : BaseModel {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }

    // Sales
    public class SalesReportModel : ReportModel {
        public List<ReportsService.SalesRecord> Records { get; set; }
    }
}