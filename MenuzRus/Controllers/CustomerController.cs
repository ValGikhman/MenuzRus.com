using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Extensions;
using MenuzRus.Controllers;
using MenuzRus.Models;
using Services;

namespace MenuzRus {

    public class CustomerController : BaseController {
        private ICustomerService _customerService;
        private ISettingsService _settingsService;

        public CustomerController(ISessionData sessionData, ICustomerService customerService, ISettingsService settingsService)
            : base(sessionData) {
            _customerService = customerService;
            _settingsService = settingsService;
        }

        [CheckUserSession]
        public ActionResult Index(Int32? id) {
            CustomerModel model;
            String pathToNavigate = "~/Order/Tables";
            try {
                model = GetModel(id);
                if (model == null) {
                    return Redirect(pathToNavigate);
                }
                return View(model);
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
            Customer customer
                ;
            try {
                customer = new Customer();
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

                Int32 result = _customerService.SaveCustomer(customer);
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
                SaveSetting(Common.Settings.PrinterPOS, model.PrinterPOS);
                SaveSetting(Common.Settings.PrinterKitchen, model.PrinterKitchen);
                SaveSetting(Common.Settings.PrinterPOSWidth, ((Int32)(Common.PrinterWidth)Enum.Parse(typeof(Common.PrinterWidth), model.PrinterPOSWidth)).ToString());
                SaveSetting(Common.Settings.PrinterKitchenWidth, ((Int32)(Common.PrinterWidth)Enum.Parse(typeof(Common.PrinterWidth), model.PrinterKitchenWidth)).ToString());
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex) {
                base.Log(ex);
            }
            finally {
            }
            return null;
        }

        #region private

        private CustomerModel GetModel(Int32? id) {
            CustomerModel model;

            try {
                model = new CustomerModel();
                model.States = Utility.States.ToSelectListItems();
                if (SessionData.printers == null) {
                    return null;
                }

                model.Printers = SessionData.printers.Select(r => new SelectListItem { Text = r, Value = r });
                model.PrinterWidth = Enum.GetValues(typeof(Common.PrinterWidth)).Cast<Common.PrinterWidth>().Select(r => new SelectListItem { Text = EnumHelper<Common.PrinterWidth>.GetDisplayValue(r), Value = r.ToString() });
                if (id != null && id.HasValue) {
                    Customer customer = _customerService.GetCustomer(id.Value);
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
                    model.PrinterKitchen = _settingsService.GetSettings(SessionData.customer.id, Common.Settings.PrinterKitchen);
                    model.PrinterPOS = _settingsService.GetSettings(SessionData.customer.id, Common.Settings.PrinterPOS);
                    model.PrinterKitchenWidth = ((Common.PrinterWidth)Convert.ToInt32(_settingsService.GetSettings(SessionData.customer.id, Common.Settings.PrinterKitchenWidth))).ToString();
                    model.PrinterPOSWidth = ((Common.PrinterWidth)Convert.ToInt32(_settingsService.GetSettings(SessionData.customer.id, Common.Settings.PrinterPOSWidth))).ToString();
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

        private Boolean SaveSetting(Common.Settings type, String value) {
            Setting setting;

            try {
                setting = new Setting();

                setting.Type = type.ToString();
                setting.Value = value;
                if (!_settingsService.SaveSetting(setting, SessionData.customer.id)) {
                    base.Log(SessionData.exception);
                    return false;
                }
            }
            catch (Exception ex) {
                base.Log(ex);
                return false;
            }
            finally {
            }
            return true;
        }

        #endregion private
    }
}