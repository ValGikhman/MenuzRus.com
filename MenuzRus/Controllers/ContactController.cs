using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using MenuzRus.Controllers;
using MenuzRus.Models;
using Services;

namespace MenuzRus {

    public class ContactController : BaseController {

        public ActionResult Index(Int32? id) {
            ContactModel model = new ContactModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(ContactModel model) {
            Services service = new Services();
            Contact contact = new Contact();
            contact.id = model.id;
            contact.CustomerId = SessionData.customer.id;
            contact.FirstName = model.FirstName;
            contact.LastName = model.LastName;
            contact.WorkPhone = model.WorkPhone;
            contact.MobilePhone = model.MobilePhone;
            contact.Password = model.Password;
            contact.Email = model.Email;
            contact.ImageUrl = model.ImageUrl;
            if (model.Image != null)
                contact.ImageUrl = String.Format("{0}{1}", model.id, Path.GetExtension(model.Image.FileName));

            String fileName = (model.Image == null ? model.ImageUrl : model.Image.FileName);
            String path = Path.Combine(Server.MapPath("~/Images/Menus/"), SessionData.customer.id.ToString(), "Contacts", String.Format("{0}{1}", model.id, Path.GetExtension(fileName)));
            if (model.Image == null && model.ImageUrl == null) {
                String imageUrl = service.GetCustomer(contact.id).ImageUrl;
                if (System.IO.File.Exists(path)) {
                    System.IO.File.Delete(path);
                }
            }
            else {
                model.Image.SaveAs(path);
            }
            if (!(service.SendEmailConfirmation(contact))
                    && service.SaveContact(contact))
                return RedirectToAction("Index", "Error");

            return RedirectToAction("Index", "Login");
        }
    }
}