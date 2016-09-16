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

        Boolean AddInventoryRegestryCheckMenu(Int32 registryId, Int32 checkMenuId);

        Boolean AddInventoryRegistry(ItemInventoryAssociation association, ChecksMenu checkMenu, String name);

        Boolean AddItemRegistry(Int32 id, Decimal qty, Common.InventoryType type, String comment);

        Boolean DeleteInventoryAssociation(Int32 id);

        Boolean DeleteInventoryRegistry(ItemInventoryAssociation association, ChecksMenu checkMenu, String name);

        Boolean SaveInventoryAssociation(ItemInventoryAssociation item);
    }
}