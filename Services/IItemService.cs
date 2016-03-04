﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public interface IItemService {

        Boolean AddItemPrice(Int32 id, Decimal price);

        Boolean DeleteItem(Int32? id);

        Boolean DeleteMenuItem(Int32 id);

        Item GetItem(Int32 id);

        ItemAssociation GetItemAssociations(Int32 id);

        List<ItemPrice> GetItemPrices(Int32 id);

        List<Item> GetItems(Int32 id);

        Int32 SaveItem(Item item);

        Int32 SaveMenuItem(Int32 id);
    }
}