using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using MenuzRus.Controllers;
using MenuzRus.Models;
using Services;

namespace MenuzRus {

    public class RegisterController : BaseController {

        #region Private Fields

        private IOrderService _orderService;

        #endregion Private Fields

        #region Public Constructors

        public RegisterController(ISessionData sessionData, IOrderService orderService)
            : base(sessionData) {
            _orderService = orderService;
        }

        #endregion Public Constructors

        #region Public Methods

        [HttpGet]
        public String Register(Int32 checkId, Decimal total) {
            PaymentModel model;
            try {
                model = new PaymentModel();
                model.Payments = _orderService.GetPayments(checkId);
                model.Total = total;
                return RenderViewToString(this.ControllerContext, "_RegisterPartial", model);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
            return null;
        }

        #endregion Public Methods

        #region private

        private PaymentModel GetPaymentModel(Int32 checkId) {
            PaymentModel model = new PaymentModel();
            model.Payments = _orderService.GetPayments(checkId);
            return model;
        }

        #endregion private
    }
}