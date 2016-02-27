using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public interface ICustomerService {

        Customer GetCustomer(Int32 id);

        Int32 SaveCustomer(Customer customer);
    }
}