using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using MenuzRus;
using Services;

namespace MenuzRus {

    public interface IInventoryService {

        Boolean AddInventoryRegestryCheckMenu(InventoryRegistry registry, ChecksMenu checkMenu);

        Boolean AddInventoryRegistry(ItemInventoryAssociation association, ChecksMenu checkMenu, Item itemAssociated);

        Boolean DeleteInventoryAssociation(Int32 id);

        Boolean DeleteInventoryRegistry(ItemInventoryAssociation association, ChecksMenu checkMenu, Item itemAssociated);

        Boolean SaveInventoryAssociation(ItemInventoryAssociation item);
    }
}