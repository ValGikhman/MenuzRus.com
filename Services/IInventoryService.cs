﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using MenuzRus;
using Services;

namespace MenuzRus {

    public interface IInventoryService {

        Boolean DeleteInventoryAssociation(Int32 id);

        Boolean SaveInventoryAssociation(ItemInventoryAssociation item);
    }
}