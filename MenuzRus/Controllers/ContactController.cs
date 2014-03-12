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
            ContactService contactService = new ContactService();
            CommonService commonService = new CommonService();
            try {
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
                if (model.Image != null) {
                    if (contact.id == 0)
                        contact.ImageUrl = Path.GetExtension(model.Image.FileName);
                    else
                        contact.ImageUrl = String.Format("{0}{1}", model.id, Path.GetExtension(model.Image.FileName));
                }

                Int32 result = contactService.SaveContact(contact);
                if (result == 0)
                    return RedirectToAction("Index", "Error");

                String fileName = (model.Image == null ? model.ImageUrl : model.Image.FileName);
                String path = Path.Combine(Server.MapPath("~/Images/Menus/"), SessionData.customer.id.ToString(), "Contacts", String.Format("{0}{1}", result, Path.GetExtension(fileName)));
                if (model.Image == null && model.ImageUrl == null) {
                    if (System.IO.File.Exists(path)) {
                        System.IO.File.Delete(path);
                    }
                }
                else {
                    model.Image.SaveAs(path);
                }

                commonService.SendEmailConfirmation(contact);
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex) {
            }
            finally {
                contactService = null;
                commonService = null;
            }
            return null;
        }
    }
}