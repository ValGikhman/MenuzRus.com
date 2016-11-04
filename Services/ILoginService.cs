using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public interface ILoginService {

        #region Public Methods

        Boolean GetProduction();

        Tuple<User, Customer, List<String>> Login(String email, String password);

        #endregion Public Methods
    }
}