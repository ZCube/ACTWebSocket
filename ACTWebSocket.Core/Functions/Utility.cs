using ACTWebSocket_Plugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ACTWebSocket.Core
{
    class Utility
    {
        public static long ToUnixTimestamp(DateTime value)
        {
            return (long)Math.Truncate((value.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
        }

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

        // Enable/disable useUnsafeHeaderParsing.
        // See http://o2platform.wordpress.com/2010/10/20/dealing-with-the-server-committed-a-protocol-violation-sectionresponsestatusline/
        public static bool ToggleAllowUnsafeHeaderParsing(bool enable)
        {
            //Get the assembly that contains the internal class
            Assembly assembly = Assembly.GetAssembly(typeof(SettingsSection));
            if (assembly != null)
            {
                //Use the assembly in order to get the internal type for the internal class
                Type settingsSectionType = assembly.GetType("System.Net.Configuration.SettingsSectionInternal");
                if (settingsSectionType != null)
                {
                    //Use the internal static property to get an instance of the internal settings class.
                    //If the static instance isn't created already invoking the property will create it for us.
                    object anInstance = settingsSectionType.InvokeMember("Section",
                    BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.NonPublic, null, null, new object[] { });
                    if (anInstance != null)
                    {
                        //Locate the private bool field that tells the framework if unsafe header parsing is allowed
                        FieldInfo aUseUnsafeHeaderParsing = settingsSectionType.GetField("useUnsafeHeaderParsing", BindingFlags.NonPublic | BindingFlags.Instance);
                        if (aUseUnsafeHeaderParsing != null)
                        {
                            aUseUnsafeHeaderParsing.SetValue(anInstance, enable);
                            return true;
                        }

                    }
                }
            }
            return false;
        }

        public static List<T> Distinct<T>(IEnumerable<T> source)
        {
            List<T> uniques = new List<T>();
            foreach (T item in source)
            {
                if (!uniques.Contains(item)) uniques.Add(item);
            }
            return uniques;
        }

        public static string Str2Hex(string strData)
        {
            string resultHex = string.Empty;
            byte[] arr_byteStr = Encoding.Default.GetBytes(strData);

            foreach (byte byteStr in arr_byteStr)
                resultHex += string.Format("{0:X2}", byteStr);

            return resultHex;
        }
        public static String GetExternalIp()
        {
            string whatIsMyIp = "https://api.ipify.org";
            ToggleAllowUnsafeHeaderParsing(true);
            WebClient wc = new WebClient();
            wc.Headers["User-Agent"] = "ACTWebSocket (" + ACTWebSocketCore.currentVersionString + ")";

            UTF8Encoding utf8 = new UTF8Encoding();
            string ipAddress = "";
            try
            {
                ipAddress = utf8.GetString(wc.DownloadData(whatIsMyIp)).Trim();
            }
            catch (WebException we)
            {
                Console.Write(we.ToString());
            }
            return ipAddress;
        }
        public static String ReleaseTag(string releaseURL = "https://github.com/ZCube/ACTWebSocket/releases")
        {
            ToggleAllowUnsafeHeaderParsing(true);
            WebClient wc = new WebClient();
            wc.Headers["User-Agent"] = "ACTWebSocket (" + ACTWebSocketCore.currentVersionString + ")";
            UTF8Encoding utf8 = new UTF8Encoding();
            ///ZCube/ACTWebSocket/tree/
            string releaseTag = null;
            try
            {
                releaseTag = utf8.GetString(wc.DownloadData(releaseURL));
                //<a href="/ZCube/ACTWebSocket/tree/1.1.3" class="css-truncate">
                var zz = Regex.Match(releaseTag, "/tree/(?<tag>[^\"]*)", RegexOptions.IgnoreCase);
                releaseTag = zz.Groups["tag"].Value;

                int dotCount = releaseTag.Count(x => x == '.');
                if (dotCount == 0)
                {
                    releaseTag = "0.0.0.0";
                }
                else if (dotCount < 3 && dotCount > 0)
                {
                    releaseTag += string.Concat(Enumerable.Repeat(".0", 3 - dotCount));
                }
            }
            catch (WebException we)
            {
                Console.Write(we.ToString());
            }
            return releaseTag;
        }
        public static String DevelVersion(string versionURL = "https://www.dropbox.com/s/eivc89zuj1lclhd/ACTWebSocket_version?dl=1")
        {
            ToggleAllowUnsafeHeaderParsing(true);
            WebClient wc = new WebClient();
            wc.Headers["User-Agent"] = "ACTWebSocket (" + ACTWebSocketCore.currentVersionString + ")";
            UTF8Encoding utf8 = new UTF8Encoding();
            ///ZCube/ACTWebSocket/tree/
            string version = null;
            try
            {
                version = utf8.GetString(wc.DownloadData(versionURL)).Trim();
            }
            catch (WebException we)
            {
                Console.Write(we.ToString());
            }
            return version;
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
