using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public interface IUserService {

        #region user

        Boolean DeleteUser(Int32? id);

        User GetUser(Int32 id);

        User GetUserByHash(String hash);

        List<User> GetUsers(Int32 id);

        Int32 SaveUser(User user);

        #endregion user
    }
}