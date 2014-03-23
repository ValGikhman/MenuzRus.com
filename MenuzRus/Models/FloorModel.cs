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

        public Int32 id { set; get; }

        public String Layout { set; get; }

        public String Name { set; get; }
    }

    public class FloorModel : BaseModel {

        public Floor Floor { set; get; }

        public List<Services.Floor> Floors { set; get; }
    }
}