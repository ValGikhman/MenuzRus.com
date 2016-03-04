﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public interface IMenuService {

        Boolean DeleteMenu(Int32? id);

        List<Menu> GetMenus(Int32 id);

        Int32 SaveMenu(Menu menu);

        Boolean SaveMenuItems(List<MenuItem> model);
    }
}