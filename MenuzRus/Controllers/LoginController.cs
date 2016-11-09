using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web.Mvc;
using MenuzRus.Controllers;
using MenuzRus.Models;
using Services;

namespace MenuzRus {

    public class LoginController : BaseController {

        #region Constructors

        private ILoginService _loginService;

        public LoginController(ISessionData sessionData, ILoginService loginService)
            : base(sessionData) {
            _loginService = loginService;
        }

        #endregion Constructors

        #region Public Methods

        public ActionResult Index() {
            try {
                SessionData.SetSession(Constants.SESSION_PRODUCTION, _loginService.GetProduction());
                LoginModel model = new LoginModel();
                model.Success = true;
                return View(model);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
            return null;
        }

        [HttpPost]
        public ActionResult Index(LoginModel model) {
            //String pathToNavigate = "~/Order/Tables";
            String pathToNavigate = "~/Home/Dashboard";
            Tuple<Services.User, Services.Customer, List<String>> data;

            try {
                data = _loginService.Login(model.Email, model.Password);
                if (data == null || data.Item1 == null) {
                    model.Success = false;
                    return View(model);
                }

                // Check if printers defined
                // If printers not defined or equals NONE by any reason, set up this way or WSPrint is not running - no needs to show buttons
                Boolean posPrinter = (data.Item2.Settings.Where(m => m.Type == Common.Settings.PrinterPOS.ToString()).FirstOrDefault().Value != "None");
                Boolean kitchenPrinter = (data.Item2.Settings.Where(m => m.Type == Common.Settings.PrinterKitchen.ToString()).FirstOrDefault().Value != "None");

                SessionData.SetSession(Constants.SESSION_USER, (Services.User)data.Item1);
                SessionData.SetSession(Constants.SESSION_CUSTOMER, (Services.Customer)data.Item2);

                SessionData.SetSession(Constants.SESSION_MODULE_INVENTORY, (Boolean)data.Item3.Contains(Common.Modules.Inventory.ToString()));
                SessionData.SetSession(Constants.SESSION_MODULE_PRINT, (Boolean)data.Item3.Contains(Common.Modules.Print.ToString()) && posPrinter && kitchenPrinter);
                SessionData.SetSession(Constants.SESSION_MODULE_REPORTS, (Boolean)data.Item3.Contains(Common.Modules.Reports.ToString()));

                IsLoggedIn = true;
                model.Success = true;

                if (Request.UrlReferrer != null && !String.IsNullOrEmpty(Request.UrlReferrer.Query)) {
                    pathToNavigate = Request.UrlReferrer.Query.Replace("?", String.Empty);
                }

                base.Log(Common.LogType.LogIn);
                return Redirect(pathToNavigate);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
            return View(model);
        }

        #endregion Public Methods
    }
}