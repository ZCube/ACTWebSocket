using System;

namespace ACTWebSocket_Plugin
{
    public static class FFXIV_OverlayStaticAPI
    {
        public static string JSONSafeString(this string s)
        {
            return s.Replace("\\", "\\\\").Replace("\r", "\\r").Replace("\n", "\\n").Replace("\t", "\\t").Replace("'", "\\'").Replace("\"", "\\\"");
        }

        public static string ReplaceNaN(this string str, string replace = "---")
        {
            return str.Replace(double.NaN.ToString(), replace);
        }

        public static string[] SplitStr(this string str, string needle, StringSplitOptions option = StringSplitOptions.None)
        {
            if (str.Contains(needle))
                return str.Split(new string[] { str }, option);
            else
                return new string[] { str };
        }
    }
}
