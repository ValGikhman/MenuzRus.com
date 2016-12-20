using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Services;

namespace MenuzRus.Models {

    public class PaymentModel : BaseModel {

        #region Public Properties

        public List<Payment> Payments { set; get; }

        public Decimal Total { set; get; }

        #endregion Public Properties
    }
}