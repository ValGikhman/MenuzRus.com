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

        #region Public Methods

        Boolean DeleteCategory(Int32? id);

        List<Category> GetCategories(Int32 customerId, CommonUnit.CategoryType type);

        List<Category> GetCategories(Int32 customerId, CommonUnit.CategoryType type, String search);

        Category GetCategory(Int32 id);

        List<Category> GetMenuCategories(Int32 customerId, CommonUnit.CategoryType type, Int32 menuId);

        Int32 SaveCategory(Category category);

        #endregion Public Methods
    }
}