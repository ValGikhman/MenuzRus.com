using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public interface IItemService {

        Boolean AddItemPrice(Int32 id, Decimal price);

        Boolean AddItemRegistry(Int32 id, Decimal qty, Common.InventoryType type, String comment);

        Boolean DeleteItem(Int32? id);

        Boolean DeleteMenuItem(Int32 id);

        Item GetItem(Int32 id);

        List<ItemPrice> GetItemPrices(Int32 id);

        ItemProduct GetItemProduct(Int32 id);

        List<Item> GetItemProductAssosiations(Int32 productId);

        Item GetItemProductAssosiationsById(Int32 associationId);

        List<Item> GetItems(Int32 id);

        ItemPrice GetLastItemPrice(Int32 id);

        Int32 SaveItem(Item item);

        //Int32 SaveMenuItem(Int32 id);
    }
}