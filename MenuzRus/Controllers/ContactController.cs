using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using MenuzRus.Controllers;
using MenuzRus.Models;
using Services;

namespace MenuzRus {

    public class ContactController : BaseController {

        [HttpPost]
        public ActionResult DeleteContact(Int32? id) {
            ContactService service = new ContactService();
            try {
                if (!service.DeleteContact(id))
                    return RedirectToAction("Index", "Error");

                return Json("OK");
            }
            catch (Exception ex) {
            }

            finally {
                service = null;
            }
            return null;
        }

        [HttpGet]
        public String EditContact(Int32? id) {
            ContactModel model = new ContactModel();
            try {
                model = GetModel(model, id);
                ViewData.Model = model;

                using (StringWriter sw = new StringWriter()) {
                    ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, "_ContactPartial");
                    ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                    viewResult.View.Render(viewContext, sw);
                    return sw.GetStringBuilder().ToString();
                }
            }
            catch (Exception ex) {
            }

            finally {
            }
            return null;
        }

        public ActionResult Index(Int32? id) {
            ContactModel model = new ContactModel();
            model = GetModel(model, id);
            return View(model);
        }

        [HttpPost]
        public ActionResult SaveContact(ContactModel model) {
            ContactService contactService = new ContactService();
            CommonService commonService = new CommonService();
            try {
                Contact contact = new Contact();
                contact.id = model.id;
                contact.CustomerId = model.CustomerId;
                contact.FirstName = model.FirstName;
                contact.LastName = model.LastName;
                contact.WorkPhone = model.WorkPhone;
                contact.MobilePhone = model.MobilePhone;
                contact.Password = model.Password;
                contact.Hash = "";
                contact.Active = (model.Active == Common.Status.Active);
                contact.Type = model.Type.ToString();
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

        #region private

        private ContactModel GetModel(ContactModel model, Int32? id) {
            try {
                //set for new or existing contact
                model.id = id.HasValue ? id.Value : 0;
                Contact contact;
                ContactService service = new ContactService();
                model.Active = Common.Status.Active;
                if (id.HasValue) {
                    contact = service.GetContact((Int32)id.Value);
                    if (contact != null) {
                        model.id = contact.id;
                        model.CustomerId = contact.CustomerId;
                        model.FirstName = contact.FirstName;
                        model.LastName = contact.LastName;
                        model.WorkPhone = contact.WorkPhone;
                        model.MobilePhone = contact.MobilePhone;
                        model.Email = contact.Email;
                        model.Password = contact.Password;
                        model.Active = contact.Active ? Common.Status.Active : Common.Status.NotActive;
                        model.Type = Utility.GetEnumItem<Common.ContactType>(contact.Type);
                        model.Hash = contact.Hash;
                        model.ImageUrl = contact.ImageUrl;
                    }
                }
            }
            catch (Exception ex) {
            }
            finally {
            }
            return model;
        }

        #endregion private
    }
}