using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Services;

namespace MenuzRus.Models {

    public class KitchenOrderPrintModel : BaseModel {

        public List<KitchenOrder> KitchenOrders { get; set; }
    }
}