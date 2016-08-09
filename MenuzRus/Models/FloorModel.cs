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

    public class Floor {
        public String Description { set; get; }

        public Int32 Height { set; get; }

        public Int32 id { set; get; }

        public String Layout { set; get; }

        public String Name { set; get; }

        public Int32 Width { set; get; }
    }

    public class FloorModel : BaseModel {
        public Floor Floor { set; get; }

        public List<Services.Floor> Floors { set; get; }
    }

    public class KitchenModel : BaseModel {
        public List<TableOrder> Tables { set; get; }
    }

    public class MonitorFloor {
        public String Description { set; get; }

        public Int32 id { set; get; }

        public String Name { set; get; }

        public List<TableOrder> Tables { set; get; }
    }

    public class MonitorFloorModel : BaseModel {
        public MonitorFloor Floor { set; get; }

        public List<Services.Floor> Floors { set; get; }

        public String Referer { set; get; }
    }

    public class TableOrder {
        public Services.TableOrder Order { set; get; }

        public String TableName { set; get; }
    }
}