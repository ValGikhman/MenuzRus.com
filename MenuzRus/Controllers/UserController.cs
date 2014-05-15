﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using MenuzRus.Controllers;
using MenuzRus.Models;
using Services;

namespace MenuzRus {

    public class UserController : BaseController {

        [HttpPost]
        public ActionResult DeleteUser(Int32? id) {
            UserService service = new UserService();
            try {
                if (!service.DeleteUser(id))
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
        public String Edituser(Int32? id) {
            try {
                ViewData.Model = GetModel(id);

                using (StringWriter sw = new StringWriter()) {
                    ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, "_UserPartial");
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
            return View(GetModel(id));
        }

        [HttpPost]
        public ActionResult SaveUser(UserModel model) {
            UserService userService = new UserService();
            CommonService commonService = new CommonService();
            try {
                User user = new User();
                user.id = model.id;
                user.CustomerId = model.CustomerId;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.WorkPhone = model.WorkPhone;
                user.MobilePhone = model.MobilePhone;
                user.Password = model.Password;
                user.Hash = String.Empty;
                user.Active = (model.Active == Common.Status.Active);
                user.Type = model.Type.ToString();
                user.Email = model.Email;
                user.ImageUrl = model.ImageUrl;
                if (model.Image != null) {
                    if (user.id == 0)
                        user.ImageUrl = Path.GetExtension(model.Image.FileName);
                    else
                        user.ImageUrl = String.Format("{0}{1}", model.id, Path.GetExtension(model.Image.FileName));
                }

                Int32 result = userService.SaveUser(user);
                if (result == 0)
                    return RedirectToAction("Index", "Error");

                String fileName = (model.Image == null ? model.ImageUrl : model.Image.FileName);
                String path = Path.Combine(Server.MapPath("~/Images/Menus/"), SessionData.customer.id.ToString(), "users", String.Format("{0}{1}", result, Path.GetExtension(fileName)));
                if (model.Image == null && model.ImageUrl == null) {
                    if (System.IO.File.Exists(path)) {
                        System.IO.File.Delete(path);
                    }
                }
                else if (model.Image != null) {
                    model.Image.SaveAs(path);
                }

                commonService.SendEmailConfirmation(user);
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex) {
            }
            finally {
                userService = null;
                commonService = null;
            }
            return null;
        }

        #region private

        private UserModel GetModel(Int32? id) {
            UserModel model = new UserModel();
            try {
                //set for new or existing user
                model.id = id.HasValue ? id.Value : 0;
                User user;
                UserService service = new UserService();
                model.Active = Common.Status.Active;
                if (id.HasValue) {
                    user = service.GetUser((Int32)id.Value);
                    if (user != null) {
                        model.id = user.id;
                        model.CustomerId = user.CustomerId;
                        model.FirstName = user.FirstName;
                        model.LastName = user.LastName;
                        model.WorkPhone = user.WorkPhone;
                        model.MobilePhone = user.MobilePhone;
                        model.Email = user.Email;
                        model.Password = user.Password;
                        model.Active = Common.Status.Active;
                        model.Type = Utility.GetEnumItem<Common.UserType>(user.Type);
                        model.Hash = user.Hash;
                        model.ImageUrl = user.ImageUrl;
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