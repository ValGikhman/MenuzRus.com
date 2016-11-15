using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using MenuzRus.Controllers;
using MenuzRus.Models;
using Services;

namespace MenuzRus {

    public class LoginController : BaseController {

        #region Constructors

        private ILoginService _loginService;
        private ISettingsService _settingsService;

        public LoginController(ISessionData sessionData, ILoginService loginService, ISettingsService settingsService)
            : base(sessionData) {
            _loginService = loginService;
            _settingsService = settingsService;
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
            String pathToNavigate = "~/Order/Monitor";
            Tuple<Services.User, Services.Customer, List<String>> data;

            try {
                data = _loginService.Login(model.Email, model.Password);
                if (data == null || data.Item1 == null) {
                    model.Success = false;
                    return View(model);
                }
                SessionData.SetSession(Constants.SESSION_USER, (Services.User)data.Item1);
                SessionData.SetSession(Constants.SESSION_CUSTOMER, (Services.Customer)data.Item2);

                SessionData.SetSession(Constants.SESSION_MODULE_INVENTORY, (Boolean)data.Item3.Contains(Common.Modules.Inventory.ToString()));
                SessionData.SetSession(Constants.SESSION_MODULE_PRINT, (Boolean)data.Item3.Contains(Common.Modules.Print.ToString()) && SessionData.printers != null);
                SessionData.SetSession(Constants.SESSION_MODULE_REPORTS, (Boolean)data.Item3.Contains(Common.Modules.Reports.ToString()));

                SessionData.SetSession(Constants.SESSION_LANGUAGE, _settingsService.GetSettings(SessionData.customer.id, Common.Settings.Language));
                // Cook it as well. Will need for login view
                HttpCookie cook = new HttpCookie("language");

                cook.Value = EnumHelper<Common.Languages>.GetDisplayValue((Common.Languages)Convert.ToInt32(SessionData.GetSession<String>(Constants.SESSION_LANGUAGE)));
                cook.Expires = DateTime.MaxValue;
                Response.Cookies.Add(cook);

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