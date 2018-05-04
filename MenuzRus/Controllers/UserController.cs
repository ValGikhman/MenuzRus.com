using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using Extensions;
using MenuzRus.Controllers;
using MenuzRus.Models;
using Services;

namespace MenuzRus {

    public class UserController : BaseController {

        #region Private Fields

        private IUserService _userService;

        #endregion Private Fields

        #region Public Constructors

        public UserController(ISessionData sessionData, IUserService userService)
            : base(sessionData) {
            _userService = userService;
        }

        #endregion Public Constructors

        #region Public Methods

        [HttpPost]
        public ActionResult DeleteUser(Int32? id) {
            try {
                if (!_userService.DeleteUser(id))
                    return RedirectToAction("Index", "Error");

                return Json("OK");
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }

            return null;
        }

        [HttpGet]
        public ActionResult Edituser(Int32? id) {
            try {
                return PartialView("_UserPartial", GetModel(id));
            }
            catch (Exception ex) {
            }
            finally {
            }
            return null;
        }

        [CheckUserSession]
        public ActionResult Index(Int32? id) {
            return View(GetModel(id));
        }

        [HttpPost]
        public ActionResult SaveUser(UserModel model) {
            try {
                User user = new User();
                user.id = model.id;
                user.CustomerId = SessionData.customer.id;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.WorkPhone = model.WorkPhone.FormatPhone();
                user.MobilePhone = model.MobilePhone.FormatPhone();
                user.Password = model.Password;
                user.Hash = model.LastName.ToHashMD5();
                user.Active = (model.Active == CommonUnit.Status.Active);
                user.Type = (Int32)model.Type;
                user.Email = model.Email;
                user.ImageUrl = model.ImageUrl;
                if (model.Image != null) {
                    if (user.id == 0)
                        user.ImageUrl = Path.GetExtension(model.Image.FileName);
                    else
                        user.ImageUrl = String.Format("{0}{1}", model.id, Path.GetExtension(model.Image.FileName));
                }
                if (user.id == 0) {
                    EmailHelper.SendEmailConfirmation(this.ControllerContext, user);
                }

                Int32 result = _userService.SaveUser(user);
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

                if (model.Referer == "Form") {
                    return RedirectToAction("Index", "Login");
                }
                else {
                    return RedirectToAction("Index", "Users", new { id = user.CustomerId });
                }
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }

            return null;
        }

        #endregion Public Methods

        #region private

        private UserModel GetModel(Int32? id) {
            UserModel model;
            User user;

            try {
                //set for new or existing user
                model = new UserModel();
                model.id = id.HasValue ? id.Value : 0;

                model.Active = CommonUnit.Status.Active;
                if (id.HasValue) {
                    user = _userService.GetUser((Int32)id.Value);
                    if (user != null) {
                        model.id = user.id;
                        model.CustomerId = user.CustomerId;
                        model.FirstName = user.FirstName;
                        model.LastName = user.LastName;
                        model.WorkPhone = user.WorkPhone;
                        model.MobilePhone = user.MobilePhone;
                        model.Email = user.Email;
                        model.Password = user.Password;
                        model.Active = user.Active ? CommonUnit.Status.Active : CommonUnit.Status.NotActive;
                        model.Type = (CommonUnit.UserType)user.Type;
                        model.Hash = user.Hash;
                        model.ImageUrl = user.ImageUrl;
                    }
                }
                return model;
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }

            return null;
        }

        #endregion private
    }
}