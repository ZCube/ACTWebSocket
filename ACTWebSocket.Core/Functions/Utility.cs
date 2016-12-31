using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ACTWebSocket.Core
{
    class Utility
    {
        public static string GetRelativePath(string filespec, string folder)
        {
            Uri pathUri = new Uri(filespec);
            // Folders must end in a slash
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                folder += Path.DirectorySeparatorChar;
            }
            Uri folderUri = new Uri(folder);
            return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        public static IPAddress GetExternalIp()
        {
            string whatIsMyIp = ""; // TODO: where?;
            string getIpRegex = @"(?<=<TITLE>.*)\d*\.\d*\.\d*\.\d*(?=</TITLE>)";
            WebClient wc = new WebClient();
            UTF8Encoding utf8 = new UTF8Encoding();
            string requestHtml = "";
            try
            {
                requestHtml = utf8.GetString(wc.DownloadData(whatIsMyIp));
            }
            catch (WebException we)
            {
                // do something with exception
                Console.Write(we.ToString());
            }
            Regex r = new Regex(getIpRegex);
            Match m = r.Match(requestHtml);
            IPAddress externalIp = null;
            if (m.Success)
            {
                externalIp = IPAddress.Parse(m.Value);
            }
            return externalIp;
        }

        public static string Base64Encoding(string EncodingText, Encoding oEncoding = null)
        {
            if (oEncoding == null)
                oEncoding = Encoding.UTF8;

            byte[] arr = oEncoding.GetBytes(EncodingText);
            return Convert.ToBase64String(arr);
        }

        public static string Base64Decoding(string DecodingText, Encoding oEncoding = null)
        {
            if (oEncoding == null)
                oEncoding = Encoding.UTF8;

            byte[] arr = Convert.FromBase64String(DecodingText);
            return oEncoding.GetString(arr);
        }
    }

}
