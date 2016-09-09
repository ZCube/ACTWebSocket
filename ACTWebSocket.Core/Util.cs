using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACTWebSocket_Plugin
{
    internal static class Util
    {
        public static string CreateJsonSafeString(string str)
        {
            return str
                .Replace("\"", "\\\"")
                .Replace("'", "\\'")
                .Replace("\r", "\\r")
                .Replace("\n", "\\n")
                .Replace("\t", "\\t");
        }

        public static string ReplaceNaNString(string str, string replace)
        {
            return str.Replace(double.NaN.ToString(), replace);
        }
    }
}
