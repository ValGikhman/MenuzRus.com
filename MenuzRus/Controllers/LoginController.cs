using System;
using System.Collections.Generic;
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

        public ActionResult Index() {
            try {
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
            String pathToNavigate = "~/Order/Tables";
            Tuple<Services.User, Services.Customer> data;

            try {
                data = _loginService.Login(model.Email, model.Password);
                if (data == null || data.Item1 == null) {
                    model.Success = false;
                    return View(model);
                }

                SessionData.SetSession(Constants.SESSION_USER, (Services.User)data.Item1);
                SessionData.SetSession(Constants.SESSION_CUSTOMER, (Services.Customer)data.Item2);

                IsLoggedIn = true;
                model.Success = true;

                if (Request.UrlReferrer != null && !String.IsNullOrEmpty(Request.UrlReferrer.Query)) {
                    pathToNavigate = Request.UrlReferrer.Query.Replace("?", String.Empty);
                }

                base.Log(Common.LogType.LogIn, "Logged in", "User", String.Format("{0} {1}", SessionData.user.FirstName, SessionData.user.LastName));
                return Redirect(pathToNavigate);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
            return View(model);
        }
    }
}