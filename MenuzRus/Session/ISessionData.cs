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
        Customer customer { get; set; }

        Exception exception { get; set; }

        Services.Floor floor { get; set; }

        Services.Item item { get; set; }

        //Services.Menu menu { get; set; }

        Int32 printerKitchenWidth { get; set; }

        Int32 printerPOSWidth { get; set; }

        String[] printers { get; set; }

        Boolean production { get; set; }

        String route { get; set; }

        String sessionId { get; }

        User user { get; set; }

        T GetSession<T>(String key);

        T GetSession<T>(String key, T defaultValue);

        void SetSession(String key, Object data);
    }
}