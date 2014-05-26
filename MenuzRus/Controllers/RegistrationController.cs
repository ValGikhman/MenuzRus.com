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
                ViewData.Model = model;

                using (StringWriter sw = new StringWriter()) {
                    ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, "_RegistrationPartial");
                    ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                    viewResult.View.Render(viewContext, sw);
                    return sw.GetStringBuilder().ToString();
                }
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
            model.User = new UserModel();
            return View();
        }

        [HttpPost]
        public ActionResult Index(RegistrationModel model) {
            // Save Customer info
            CustomerService customerService = new CustomerService();
            UserService userService = new UserService();
            CommonService commonService = new CommonService();
            Customer customer = new Customer();
            try {
                customer.id = 0;
                customer.Name = model.Customer.Name;
                customer.Address = model.Customer.Address;
                customer.Address2 = model.Customer.Address2;
                customer.City = model.Customer.City;
                customer.State = model.Customer.State;
                customer.Zip = model.Customer.Zip;
                customer.Phone = model.Customer.Phone;
                customer.ImageUrl = model.Customer.ImageUrl;
                if (model.Customer.Image != null) {
                    if (customer.id == 0)
                        customer.ImageUrl = Path.GetExtension(model.Customer.Image.FileName);
                    else
                        customer.ImageUrl = String.Format("{0}{1}", model.Customer.id, Path.GetExtension(model.Customer.Image.FileName));
                }

                Int32 result = customerService.SaveCustomer(customer);
                if (result == 0)
                    return RedirectToAction("Index", "Error");
                SessionData.customer = customer;

                String fileName = (model.Customer.Image == null ? model.Customer.ImageUrl : model.Customer.Image.FileName);
                String path = Path.Combine(Server.MapPath("~/Images/Menus/"), SessionData.customer.id.ToString(), "Customer", String.Format("{0}{1}", result, Path.GetExtension(fileName)));
                if (model.Customer.Image == null && model.Customer.ImageUrl == null) {
                    if (System.IO.File.Exists(path)) {
                        System.IO.File.Delete(path);
                    }
                }
                else {
                    model.Customer.Image.SaveAs(path);
                }

                // Save user personal info
                User user = new User();
                user.id = 0;
                user.CustomerId = SessionData.customer.id;
                user.FirstName = model.User.FirstName;
                user.LastName = model.User.LastName;
                user.WorkPhone = model.User.WorkPhone;
                user.MobilePhone = model.User.MobilePhone;
                user.Password = model.User.Password;
                user.Email = model.User.Email;

                result = userService.SaveUser(user);
                if (result == 0)
                    return RedirectToAction("Index", "Error");

                commonService.SendEmailConfirmation(user);
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                customerService = null;
                userService = null;
                commonService = null;
            }
            return null;
        }
    }
}