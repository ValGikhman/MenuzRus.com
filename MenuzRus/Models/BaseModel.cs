using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Services;

namespace MenuzRus.Models {

    public class BaseModel {

        #region Public Constructors

        public BaseModel() {
            Me = (Services.User)HttpContext.Current.Session[Constants.SESSION_USER];
            MyCompany = (Services.Customer)HttpContext.Current.Session[Constants.SESSION_CUSTOMER];

            if (MyCompany != null && Me != null) {
                if (!String.IsNullOrEmpty(MyCompany.ImageUrl)) {
                    String img = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/Menus/"), Me.id.ToString(), "Customers", MyCompany.ImageUrl);
                    byte[] buffer = Utility.ReadImageFile(img);
                    CompanyImageBase64 = Convert.ToBase64String(buffer);
                }
            }

            if (HttpContext.Current.Session[Constants.SESSION_MODULE_INVENTORY] != null) {
                isModuleInventory = (Boolean)HttpContext.Current.Session[Constants.SESSION_MODULE_INVENTORY];
            }
            if (HttpContext.Current.Session[Constants.SESSION_MODULE_PRINT] != null) {
                isModulePrint = (Boolean)HttpContext.Current.Session[Constants.SESSION_MODULE_PRINT];
            }
            if (HttpContext.Current.Session[Constants.SESSION_MODULE_REPORTS] != null) {
                isModuleReports = (Boolean)HttpContext.Current.Session[Constants.SESSION_MODULE_REPORTS];
            }
        }

        #endregion Public Constructors

        #region Public Properties

        public String CompanyImageBase64 { get; set; }

        public Boolean isModuleInventory { get; set; }

        public Boolean isModulePrint { get; set; }

        public Boolean isModuleReports { get; set; }

        public User Me { get; set; }

        public Customer MyCompany { get; set; }

        public Boolean Production { get; set; }

        #endregion Public Properties
    }
}