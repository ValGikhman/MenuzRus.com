using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public class SessionData : ISessionData {

        #region Private Fields

        private readonly IHttpContextProvider _httpContext;
        private Customer _customer = null;
        private Exception _exception = null;
        private Services.Floor _floor = null;
        private Services.Item _item = null;

        private Boolean _moduleInventory;

        private Boolean _modulePrint;

        private Boolean _moduleReports;

        //private Services.Menu _menu;
        private Int32 _printerKitchenWidth = 0;

        private Int32 _printerPOSWidth = 0;
        private String[] _printers = null;
        private Boolean _production = false;
        private String _route = null;
        private Services.User _user = null;

        #endregion Private Fields

        #region Public Constructors

        public SessionData(IHttpContextProvider httpContext) {
            _httpContext = httpContext;
        }

        #endregion Public Constructors

        #region Public Properties

        public Customer customer {
            get {
                return GetSession<Customer>(Constants.SESSION_CUSTOMER, _customer);
            }
            set {
                SetSession(Constants.SESSION_CUSTOMER, value);
            }
        }

        public Exception exception {
            get {
                return GetSession<Exception>(Constants.SESSION_EXCEPTION, _exception);
            }
            set {
                SetSession(Constants.SESSION_EXCEPTION, value);
            }
        }

        public Services.Floor floor {
            get {
                return GetSession<Services.Floor>(Constants.SESSION_FLOOR, _floor);
            }
            set {
                SetSession(Constants.SESSION_FLOOR, value);
            }
        }

        public Services.Item item {
            get {
                return GetSession<Services.Item>(Constants.SESSION_ITEM, _item);
            }
            set {
                SetSession(Constants.SESSION_ITEM, value);
            }
        }

        public Boolean moduleInventory {
            get {
                return GetSession<Boolean>(Constants.SESSION_MODULE_INVENTORY, _moduleInventory);
            }
            set {
                SetSession(Constants.SESSION_MODULE_INVENTORY, value);
            }
        }

        public Boolean modulePrint {
            get {
                return GetSession<Boolean>(Constants.SESSION_MODULE_PRINT, _modulePrint);
            }
            set {
                SetSession(Constants.SESSION_MODULE_PRINT, value);
            }
        }

        public Boolean moduleReports {
            get {
                return GetSession<Boolean>(Constants.SESSION_MODULE_REPORTS, _moduleReports);
            }
            set {
                SetSession(Constants.SESSION_MODULE_REPORTS, value);
            }
        }

        //public Services.Menu menu {
        //    get {
        //        return GetSession<Services.Menu>(Constants.SESSION_MENU, _menu);
        //    }
        //    set {
        //        SetSession(Constants.SESSION_MENU, value);
        //    }
        //}

        public Int32 printerKitchenWidth {
            get {
                return GetSession<Int32>(Constants.SESSION_PRINTER_KITCHEN_WIDTH, _printerKitchenWidth);
                //return (_printerKitchenWidth == 0 ? 0 : GetSession<Int32>(Constants.SESSION_PRINTER_KITCHEN_WIDTH, _printerKitchenWidth));
            }
            set {
                SetSession(Constants.SESSION_PRINTER_KITCHEN_WIDTH, value);
            }
        }

        public Int32 printerPOSWidth {
            get {
                return GetSession<Int32>(Constants.SESSION_PRINTER_POS_WIDTH, _printerPOSWidth);
                //return (_printerPOSWidth == 0 ? 0 : GetSession<Int32>(Constants.SESSION_PRINTER_POS_WIDTH, _printerPOSWidth));
            }
            set {
                SetSession(Constants.SESSION_PRINTER_POS_WIDTH, value);
            }
        }

        public String[] printers {
            get {
                return GetSession<String[]>(Constants.SESSION_PRINTERS, _printers);
            }
            set {
                SetSession(Constants.SESSION_PRINTERS, value);
            }
        }

        public Boolean production {
            get {
                return GetSession<Boolean>(Constants.SESSION_PRODUCTION, _production);
            }
            set {
                SetSession(Constants.SESSION_PRODUCTION, value);
            }
        }

        public String route {
            get {
                return GetSession<String>(Constants.SESSION_ROUTE, _route);
            }
            set {
                SetSession(Constants.SESSION_ROUTE, value);
            }
        }

        public String sessionId {
            get {
                return _httpContext.Current.Session.SessionID;
            }
        }

        public User user {
            get {
                return GetSession<Services.User>(Constants.SESSION_USER, _user);
            }
            set {
                SetSession(Constants.SESSION_USER, value);
            }
        }

        #endregion Public Properties

        #region Public Methods

        public T GetSession<T>(String key) {
            return GetSession<T>(key, default(T));
        }

        public T GetSession<T>(String key, T defaultValue) {
            T retVal;

            retVal = (T)_httpContext.Current.Session[key];
            if (retVal == null) {
                retVal = defaultValue;
            };

            return retVal;
        }

        public void SetSession(String key, Object data) {
            _httpContext.Current.Session[key] = data;
        }

        #endregion Public Methods
    }
}