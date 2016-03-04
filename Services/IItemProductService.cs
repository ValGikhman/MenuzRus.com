using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public interface IItemProductService {

        Boolean DeleteItemAssociation(Int32 id);

        Boolean SaveItemAssociation(ItemAssociation item);
    }
}