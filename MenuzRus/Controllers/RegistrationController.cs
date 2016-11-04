using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Extensions;
using MenuzRus.Controllers;
using MenuzRus.Models;
using Services;

namespace MenuzRus {

    public class RegistrationController : BaseController {

        #region Private Fields

        private ICustomerService _customerService;
        private IUserService _userService;

        #endregion Private Fields

        #region Public Constructors

        public RegistrationController(ISessionData sessionData, ICustomerService customerService, IUserService userService)
            : base(sessionData) {
            _customerService = customerService;
            _userService = userService;
        }

        #endregion Public Constructors

        #region Public Methods

        public ActionResult Index(Int32? id) {
            RegistrationModel model = new RegistrationModel();
            model.Customer = new CustomerModel();
            model.Customer.States = Utility.States.ToSelectListItems();
            model.Customer.State = "OH";
            model.Customer.Modules = _customerService.GetModulesAll();
            model.User = new UserModel();
            model.Modules = "N/A";
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(RegistrationModel model) {
            // Save Customer info
            Customer customer;
            User user;

            try {
                customer = new Customer();
                user = new User();

                customer.id = 0;
                customer.Name = model.Customer.Name;
                customer.Address = model.Customer.Address;
                customer.Address2 = model.Customer.Address2;
                customer.City = model.Customer.City;
                customer.State = model.Customer.State;
                customer.Zip = model.Customer.Zip;
                customer.Phone = model.Customer.Phone;
                customer.Tax = model.Customer.Tax;
                customer.ImageUrl = model.Customer.ImageUrl;

                Int32 result = _customerService.SaveCustomer(customer);
                // Save registered modules
                _customerService.SaveModulesByCustomer(result, model.Modules.Split(',').Select(Int32.Parse).ToArray());

                SessionData.customer = customer;

                // Save user personal info
                user.id = 0;
                user.CustomerId = SessionData.customer.id;
                user.FirstName = model.User.FirstName;
                user.LastName = model.User.LastName;
                user.WorkPhone = model.User.WorkPhone.CleanPhone();
                user.MobilePhone = model.User.MobilePhone.CleanPhone();
                user.Password = model.User.Password;
                user.Email = model.User.Email;
                user.Active = false;
                user.Hash = Utility.GetNewConfirmationNumber();
                user.Type = (Int32)Common.UserType.Administrator;

                result = _userService.SaveUser(user);
                SessionData.user = user;
                base.Log(Common.LogType.Activity, "Registering", String.Format("{0} {1}, phone#{2}, mobile#{3}", model.User.FirstName, model.User.LastName, model.User.WorkPhone, model.User.MobilePhone));

                EmailHelper.SendEmailConfirmation(this.ControllerContext, user);
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }

            return null;
        }

        #endregion Public Methods
    }
}