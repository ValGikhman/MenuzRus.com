using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public interface ISettingsService {

        #region Public Methods

        Dictionary<String, String> GetSettings(Int32 id);

        String GetSettings(Int32 id, CommonUnit.Settings type);

        Boolean SaveOrder(String ids, String type);

        Boolean SaveSetting(Setting setting, Int32 customerId);

        #endregion Public Methods
    }
}