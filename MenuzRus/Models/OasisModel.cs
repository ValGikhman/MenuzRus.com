using System;
using System.Collections.Generic;

namespace MenuzRus.Models {

    public class OasisMenu {
        public OasisSettings Settings;
        public List<OasisCategory> MenuCategories;
    }

    public class OasisCategory {

        public String Id { get; set; }

        public String ImageUrl { get; set; }

        public Boolean ImageOnly { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public List<OasisItem> MenuItems;

        public String Side { get; set; }

        public Boolean Show { get; set; }
    }

    public class OasisItem {

        public String Id { get; set; }

        public String ImageUrl { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public String Price { get; set; }

        public Boolean Show { get; set; }
    }

    public class OasisSettings {

        public Int32 ResolutionWidth { get; set; }

        public Int32 ResolutionHeight { get; set; }

        public Int32 OffsetLeft { get; set; }

        public Int32 OffsetTop { get; set; }
    }
}