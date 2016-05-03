using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Linq;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public interface ICategoryService {

        Boolean DeleteCategory(Int32? id);

        List<Category> GetCategories(Int32 customerId, Common.CategoryType type);

        Category GetCategory(Int32 id);

        List<Category> GetMenuCategories(Int32 customerId, Common.CategoryType type, Int32 menuId);

        Int32 SaveCategory(Category category);
    }
}