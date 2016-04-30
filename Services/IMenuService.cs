using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public interface IMenuService {

        Boolean DeleteMenu(Int32? id);

        List<Menus> GetMenus(Int32 id);

        Int32 SaveMenu(Menus menu);

        Boolean SaveMenuItems(List<MenuItem> model);
    }
}