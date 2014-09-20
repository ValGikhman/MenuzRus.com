using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using MenuzRus.Controllers;
using MenuzRus.Models;
using Services;

namespace MenuzRus {

    public class RegistrationController : BaseController {

        [HttpGet]
        public String GetRegistrationForm(Int32? id) {
            RegistrationModel model = new RegistrationModel();
            try {
                model.Customer = new CustomerModel();
                model.User = new UserModel();
                model.Customer.States = Utility.States.ToSelectListItems();
                model.Customer.State = "OH";
                return RenderViewToString(this.ControllerContext, "_RegistrationPartial", model);
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
            return null;
        }

        public ActionResult Index(Int32? id) {
            RegistrationModel model = new RegistrationModel();
            model.Customer = new CustomerModel();
            model.Customer.States = Utility.States.ToSelectListItems();
            model.Customer.State = "OH";
            model.User = new UserModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(RegistrationModel model) {
            // Save Customer info
            CustomerService customerService = new CustomerService();
            UserService userService = new UserService();
            Customer customer = new Customer();
            User user = new User();
            try {
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

                Int32 result = customerService.SaveCustomer(customer);
                SessionData.customer = customer;

                // Save user personal info
                user.id = 0;
                user.CustomerId = SessionData.customer.id;
                user.FirstName = model.User.FirstName;
                user.LastName = model.User.LastName;
                user.WorkPhone = model.User.WorkPhone;
                user.MobilePhone = model.User.MobilePhone;
                user.Password = model.User.Password;
                user.Email = model.User.Email;
                user.Active = false;
                user.Hash = Utility.GetNewConfirmationNumber(); ;
                user.Type = (Int32)Common.UserType.Administrator;

                result = userService.SaveUser(user);
                SessionData.user = user;
                base.Log(Common.LogType.Activity, "Registering", "User", String.Format("{0} {1}, phone#{2}, mobile#{3}", model.User.FirstName, model.User.LastName, model.User.WorkPhone, model.User.MobilePhone));

                EmailHelper.SendEmailConfirmation(this.ControllerContext, user);
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                customerService = null;
                userService = null;
            }
            return null;
        }
    }
}