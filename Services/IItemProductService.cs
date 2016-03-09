using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public interface IItemProductService {

        Boolean DeleteItemProduct(Int32? id);

        Boolean DeleteItemProductAssociations(Int32 id);

        Boolean SaveItemProduct(ItemProduct item);
    }
}