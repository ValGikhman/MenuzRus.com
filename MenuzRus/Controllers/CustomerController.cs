using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using MenuzRus.Controllers;
using MenuzRus.Models;
using Services;

namespace MenuzRus {

    public class CustomerController : BaseController {

        public ActionResult Index(Int32? id) {
            CustomerModel model = new CustomerModel();
            return View(GetModel(id, model));
        }

        [HttpPost]
        public ActionResult Index(CustomerModel model) {
            Services service = new Services();
            Customer customer = new Customer();
            customer.id = model.id;
            customer.Name = model.Name;
            customer.Address = model.Address;
            customer.Address2 = model.Address2;
            customer.City = model.City;
            customer.State = model.State;
            customer.Zip = model.Zip;
            customer.ImageUrl1 = model.Image1 != null ? String.Format("{0}{1}", model.id, Path.GetExtension(model.Image1.FileName)) : null;
            customer.ImageUrl2 = model.Image2 != null ? String.Format("{0}{1}", model.id, Path.GetExtension(model.Image2.FileName)) : null;
            SessionData.customer = customer;

            String fileName1 = (model.Image1 == null ? model.ImageUrl1 : model.Image1.FileName);
            String path1 = Path.Combine(Server.MapPath("~/Images/Menus/"), SessionData.customer.id.ToString(), "Customer", String.Format("{0}{1}", model.id, Path.GetExtension(fileName1)));
            String fileName2 = (model.Image1 == null ? model.ImageUrl1 : model.Image1.FileName);
            String path2 = Path.Combine(Server.MapPath("~/Images/Menus/"), SessionData.customer.id.ToString(), "Customer", String.Format("{0}{1}", model.id, Path.GetExtension(fileName2)));
            if (model.Image1 != null) {
                model.Image1.SaveAs(path1);
                model.Image1.SaveAs(path2);
            }
            else {
                String imageUrl = service.GetCustomer(customer.id).ImageUrl1;
                String path = Path.Combine(Server.MapPath("~/Images/Menus/"), SessionData.customer.id.ToString(), "Customer", imageUrl);

                if (System.IO.File.Exists(path)) {
                    System.IO.File.Delete(path);
                }

                imageUrl = service.GetCustomer(customer.id).ImageUrl2;
                path = Path.Combine(Server.MapPath("~/Images/Menus/"), SessionData.customer.id.ToString(), "Customer", imageUrl);
                if (System.IO.File.Exists(path)) {
                    System.IO.File.Delete(path);
                }
            }

            if (!service.SaveCustomer(customer))
                return RedirectToAction("Index", "Error");

            return RedirectToAction("Index", "Contact");
        }

        private CustomerModel GetModel(Int32? id, CustomerModel model) {
            Services service = new Services();
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
            }
            return model;
        }
    }
}