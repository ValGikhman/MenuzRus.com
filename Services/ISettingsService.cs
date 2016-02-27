using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public interface ISettingsService {

        Dictionary<String, String> GetSettings(Int32 id);

        String GetSettings(Int32 id, Common.Settings type);

        Boolean SaveOrder(String ids, String type);

        Boolean SaveSetting(Setting setting, Int32 customerId);
    }
}