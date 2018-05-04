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

        #region Public Methods

        Boolean AddInventoryRegistry(ItemInventoryAssociation association, ChecksMenu checkMenu);

        Boolean AddInventoryRegistryCheckMenu(Int32 registryId, Int32 checkMenuId);

        Boolean AddItemRegistry(Int32 id, Decimal qty, CommonUnit.InventoryType type, String comment);

        Boolean DeleteInventoryAssociation(Int32 id);

        Boolean DeleteInventoryRegistry(ItemInventoryAssociation association, ChecksMenu checkMenu, String name);

        Boolean SaveInventoryAssociation(ItemInventoryAssociation item);

        #endregion Public Methods
    }
}