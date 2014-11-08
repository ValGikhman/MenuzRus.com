using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Extensions;
using Newtonsoft.Json;
using Services;

namespace MenuzRus {

    public static class SessionData {

        public static Customer customer { get; set; }

        public static Exception exeption { get; set; }

        public static Services.Floor floor { get; set; }

        public static Services.Item item { get; set; }

        public static Services.Menu menu { get; set; }

        public static String[] printers { get; set; }

        public static String route { get; set; }

        public static String sessionId { get; set; }

        public static User user { get; set; }
    }
}