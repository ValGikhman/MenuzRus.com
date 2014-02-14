using System;

namespace Extensions {

    public static class StringExtensions {

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