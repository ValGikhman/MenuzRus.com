using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Text.RegularExpressions;

namespace Extensions {

    public static class Extensions {

        public static String CleanPhone(this String phone) {
            Regex digitsOnly = new Regex(@"[^\d]");
            return digitsOnly.Replace(phone, "");
        }

        public static String Ellipsis(this String text, Int32 length) {
            if (String.IsNullOrEmpty(text))
                return String.Empty;

            if (text.Length <= length)
                return text;

            return text.Substring(0, length - 4) + " ...";
        }

        public static String FormatPhone(this String phone) {
            return Regex.Replace(phone, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
        }

        public static EntitySet<T> ToEntitySet<T>(this IEnumerable<T> source) where T : class {
            var es = new EntitySet<T>();
            es.AddRange(source);
            return es;
        }
    }
}