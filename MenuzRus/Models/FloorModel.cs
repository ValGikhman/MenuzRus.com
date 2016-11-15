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

        #region Public Properties

        public String Description { set; get; }

        public Int32 Height { set; get; }

        public Int32 id { set; get; }

        public String Layout { set; get; }

        public String Name { set; get; }

        public Int32 Width { set; get; }

        #endregion Public Properties
    }

    public class FloorModel : BaseModel {

        #region Public Properties

        public Floor Floor { set; get; }

        public List<Services.Floor> Floors { set; get; }

        #endregion Public Properties
    }

    public class KitchenModel : BaseModel {

        #region Public Properties

        public List<TableOrder> Tables { set; get; }

        #endregion Public Properties
    }

    public class MonitorFloor {

        #region Public Properties

        public String Description { set; get; }

        public Int32 id { set; get; }

        public String Name { set; get; }

        public List<TableOrder> Tables { set; get; }

        #endregion Public Properties
    }

    public class MonitorFloorModel : BaseModel {

        #region Public Properties

        public MonitorFloor Floor { set; get; }

        public List<Services.Floor> Floors { set; get; }

        public Decimal Inventory { get; set; }

        public String Referer { set; get; }

        public Decimal Sales { get; set; }

        #endregion Public Properties
    }

    public class TableOrder {

        #region Public Properties

        public Services.TableOrder Order { set; get; }

        public String TableName { set; get; }

        #endregion Public Properties
    }
}