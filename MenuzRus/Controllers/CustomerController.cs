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

    public class CustomerController : BaseController {

        [CheckUserSession]
        public ActionResult Index(Int32? id) {
            CustomerModel model = new CustomerModel();
            try {
                return View(GetModel(id, model));
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
            return null;
        }

        [HttpPost]
        public ActionResult SaveCustomer(CustomerModel model) {
            CustomerService service = new CustomerService();
            Customer customer = new Customer();
            try {
                customer.id = model.id;
                customer.Name = model.Name;
                customer.Address = model.Address;
                customer.Address2 = model.Address2;
                customer.City = model.City;
                customer.State = model.State;
                customer.Zip = model.Zip;
                customer.Tax = model.Tax;
                customer.Phone = model.Phone;
                customer.ImageUrl = model.ImageUrl;
                if (model.Image != null) {
                    if (customer.id == 0)
                        customer.ImageUrl = Path.GetExtension(model.Image.FileName);
                    else
                        customer.ImageUrl = String.Format("{0}{1}", model.id, Path.GetExtension(model.Image.FileName));
                }

                Int32 result = service.SaveCustomer(customer);
                if (result == 0)
                    return RedirectToAction("Index", "Error");
                SessionData.customer = customer;

                String fileName = (model.Image == null ? model.ImageUrl : model.Image.FileName);
                String path = Path.Combine(Server.MapPath("~/Images/Menus/"), SessionData.customer.id.ToString(), "Customers", String.Format("{0}{1}", result, Path.GetExtension(fileName)));
                if (model.Image == null && model.ImageUrl == null) {
                    if (System.IO.File.Exists(path)) {
                        System.IO.File.Delete(path);
                    }
                }
                else if (model.Image != null) {
                    model.Image.SaveAs(path);
                }
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                service = null;
            }
            return null;
        }

        #region private

        private CustomerModel GetModel(Int32? id, CustomerModel model) {
            CustomerService service = new CustomerService();
            try {
                model.States = Utility.States.ToSelectListItems();
                if (id != null && id.HasValue) {
                    Customer customer = service.GetCustomer(id.Value);
                    model.id = customer.id;
                    model.Name = customer.Name;
                    model.Address = customer.Address;
                    model.Address2 = customer.Address2;
                    model.City = customer.City;
                    model.State = customer.State;
                    model.Zip = customer.Zip;
                    model.Tax = customer.Tax.HasValue ? customer.Tax.Value : 0;
                    model.Phone = customer.Phone.FormatPhone();
                    model.ImageUrl = customer.ImageUrl;
                }
                return model;
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
                service = null;
            }
            return null;
        }

        #endregion private
    }
}