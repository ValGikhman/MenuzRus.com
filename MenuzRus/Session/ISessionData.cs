using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public interface ISessionData {

        #region Public Properties

        Customer customer { get; set; }

        Exception exception { get; set; }

        Services.Floor floor { get; set; }

        Services.Item item { get; set; }

        Boolean moduleInventory { get; set; }

        Boolean modulePrint { get; set; }

        Boolean moduleReports { get; set; }

        //Services.Menu menu { get; set; }

        Boolean printable { get; set; }
        Int32 printerKitchenWidth { get; set; }

        Int32 printerPOSWidth { get; set; }

        String[] printers { get; set; }
        Boolean production { get; set; }

        String route { get; set; }

        String sessionId { get; }

        User user { get; set; }

        #endregion Public Properties

        #region Public Methods

        T GetSession<T>(String key);

        T GetSession<T>(String key, T defaultValue);

        void SetSession(String key, Object data);

        #endregion Public Methods
    }
}