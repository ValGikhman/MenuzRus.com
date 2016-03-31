using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public interface ILoginService {

        Tuple<User, Customer> Login(String email, String password);
    }
}