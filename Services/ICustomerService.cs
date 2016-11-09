using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public interface ICustomerService {

        #region Public Methods

        Boolean DeleteModulesByCustomer(Int32 id);

        Customer GetCustomer(Int32 id);

        List<Module> GetModulesAll();

        List<Module> GetModulesByCustomer(Int32 id);

        Int32 SaveCustomer(Customer customer);

        void SaveModulesByCustomer(Int32 id, Int32[] modulesIds);

        void UpdateModules(Int32 id, Int32 moduleId);

        #endregion Public Methods
    }
}