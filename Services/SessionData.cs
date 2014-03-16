using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Services;
using StringExtensions;

namespace MenuzRus {

    public static class SessionData {

        public static Contact contact { get; set; }

        public static Customer customer { get; set; }

        public static Exception exeption { get; set; }

        public static Menus menu { get; set; }
    }
}