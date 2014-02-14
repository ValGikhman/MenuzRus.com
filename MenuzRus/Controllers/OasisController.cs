using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml;
using MenuzRus.Models;

namespace MenuzRus {

    public class OasisController : Controller {

        public ActionResult Index() {
            String file = "1";
            if (Request.QueryString.Count > 0 && Request.QueryString[0] != String.Empty)
                file = Request.QueryString[0].ToString();

            String fileName = System.IO.Path.Combine(Request.PhysicalApplicationPath, String.Format("App_Data/{0}.xml", file));
            if (!System.IO.File.Exists(fileName)) {
                Response.Write("xml does not exist.");
                Response.End();
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);

            OasisMenu model = new OasisMenu();
            OasisSettings Settings = new OasisSettings();

            foreach (XmlNode setting in doc.SelectNodes("/Menu/Settings")) {
                Settings.ResolutionWidth = setting.Attributes["ResolutionWidth"] == null && setting.Attributes["ResolutionWidth"].Value != String.Empty ? 0 : Int32.Parse(setting.Attributes["ResolutionWidth"].Value);
                Settings.ResolutionHeight = setting.Attributes["ResolutionHeight"] == null && setting.Attributes["ResolutionHeight"].Value != String.Empty ? 0 : Int32.Parse(setting.Attributes["ResolutionHeight"].Value);
                Settings.OffsetLeft = setting.Attributes["OffsetLeft"] == null && setting.Attributes["OffsetLeft"].Value != String.Empty ? 0 : Int32.Parse(setting.Attributes["OffsetLeft"].Value);
                Settings.OffsetTop = setting.Attributes["OffsetTop"] == null && setting.Attributes["OffsetTop"].Value != String.Empty ? 0 : Int32.Parse(setting.Attributes["OffsetTop"].Value);
            }
            model.Settings = Settings;

            model.MenuCategories = new List<OasisCategory>();
            XmlNodeList menuCategories = doc.SelectNodes("/Menu/Categories/Category");
            // Category In Categories
            foreach (XmlNode menuCategory in menuCategories) {
                OasisCategory category = new OasisCategory();
                category.Id = menuCategory.Attributes["Id"].Value;
                category.Name = menuCategory.Attributes["Name"].Value;

                if (menuCategory.Attributes["Description"] != null)
                    category.Description = menuCategory.Attributes["Description"].Value;

                category.ImageUrl = menuCategory.Attributes["ImageUrl"].Value;
                category.ImageOnly = (menuCategory.Attributes["ImageOnly"] != null);
                category.Side = menuCategory.Attributes["Side"].Value;
                // Items
                foreach (XmlNode menuCategoryItems in menuCategory.ChildNodes) {
                    category.MenuItems = new List<OasisItem>();
                    // Item in Items
                    foreach (XmlNode menuCategoryItem in menuCategoryItems.ChildNodes) {
                        OasisItem item = new OasisItem();
                        item.Id = menuCategoryItem["Id"].InnerText;
                        item.Name = menuCategoryItem["Name"].InnerText;
                        item.Description = menuCategoryItem["Description"].InnerText;
                        item.Price = menuCategoryItem["Price"].InnerText;
                        item.ImageUrl = menuCategoryItem["ImageUrl"].InnerText;
                        item.Name = menuCategoryItem["Name"].InnerText;
                        category.MenuItems.Add(item);
                    }
                }
                model.MenuCategories.Add(category);
            }

            return View(model);
        }
    }
}