using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Services;

namespace MenuzRus.Models {

    public class ItemModel : BaseModel {

        #region Private Fields

        private Decimal _Price;

        #endregion Private Fields

        #region Public Properties

        [DisplayName("Category")]
        public List<Services.Category> Categories { get; set; }

        public Int32 CategoryId { get; set; }

        [DisplayName("Category Type")]
        public CommonUnit.CategoryType CategoryType { get; set; }

        [DisplayName("Description")]
        public String Description { get; set; }

        public Int32 id { get; set; }

        [DisplayName("Image")]
        public HttpPostedFileBase Image { get; set; }

        [DisplayName("Image")]
        public String ImageUrl { get; set; }

        [DisplayName("Comment")]
        public String InventoryComment { get; set; }

        [DisplayName("InventoryRegistries")]
        public List<InventoryRegistry> InventoryRegistries { get; set; }

        [DisplayName("Type")]
        [Required]
        public CommonUnit.InventoryType InventoryType { get; set; }

        [DisplayName("ItemInventoryAssociations")]
        public List<ItemInventoryAssociation> ItemInventoryAssociations { get; set; }

        [DisplayName("ItemPrices")]
        public List<ItemPrice> ItemPrices { get; set; }

        [DisplayName("Name")]
        public String Name { get; set; }

        [DisplayName("Price")]
        public Decimal Price {
            get {
                if (_Price == 0 && ItemPrices != null) {
                    _Price = (Decimal)ItemPrices.OrderByDescending(m => m.DateCreated).Take(1).Select(m => m.Price).FirstOrDefault();
                }
                return _Price;
            }
            set {
                _Price = value;
            }
        }

        [DisplayName("Price to add")]
        public Decimal Price2Add { get; set; }

        [DisplayName("Quantity")]
        public Decimal Quantity { get; set; }

        [DisplayName("Status")]
        [Required]
        public CommonUnit.Status Status { get; set; }

        [DisplayName("UOM")]
        [Required]
        public CommonUnit.UOM UOM { get; set; }

        #endregion Public Properties
    }
}