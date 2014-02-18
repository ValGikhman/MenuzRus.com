using System;
using System.Text.RegularExpressions;

namespace StringExtensions {

    public static class StringExtensions {

        public static String CleanPhone(this String phone) {
            Regex digitsOnly = new Regex(@"[^\d]");
            return digitsOnly.Replace(phone, "");
        }

        public static String Ellipsis(this String text, Int32 length) {
            Int32 pos;

            if (String.IsNullOrEmpty(text))
                return String.Empty;

            if (text.Length <= length)
                return text;

            pos = text.IndexOf(" ", length);

            if (pos >= 0)
                return text.Substring(0, pos) + " ...";

            return text;
        }
    }
}