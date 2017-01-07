using System;
using System.IO;
using System.Windows.Forms;

namespace ACTWebSocket_Plugin
{
    public static class Settings
    {
        // FFXIV ACT INSTALLER CODE...!

        // Environments
        public static string APPDATA = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
        , EXECUTE = Application.ExecutablePath
        , EXECUTE_CONF = EXECUTE.Append(".config")
        , RESDIR = APPDATA.Append("\\Samhain")
        , RUNDAT = DateTime.Now.ToString("yyyy.MM.dd")

        // Directories
        , LOGDIR = string.Join("\\", RESDIR, "logs")
        , CEFDIR = string.Join("\\", RESDIR, "CEFSharp")
        , CEFDIR64 = string.Join("\\", RESDIR, "CEFSharp64")
        , CEFLOC = string.Join("\\", CEFDIR, "locales")
        , PLUGINDIR = string.Join("\\", RESDIR, "Plugins")
        , CEFUSR = string.Join("\\", CEFDIR, "UserData")
        , CEFBIN = string.Join("\\", CEFDIR, "Caches")
        , BINDIR = string.Join("\\", RESDIR, "bin")
        , LIBDIR = string.Join("\\", RESDIR, "lib")

        // Files
        , CEFLIB = string.Join("\\", CEFDIR, "libcef.dll")
        , CEFBRW = string.Join("\\", CEFDIR, "CefSharp.BrowserSubprocess.exe")
        , LOGFILE = string.Join("\\", RESDIR, "logs", RUNDAT.Append(".log"))
        , VC2012 = "2012_vcredist_x86.exe"
        , VC2013 = "2013_vcredist_x86.exe"
        , ICO = "actico.ico".LIB()
        , ICO32 = "actico32.ico".LIB()

        // Commands
        , VCATTR = "/q /norestart"

        // Extension
        , DLL = ".dll"

        // URLS
        , SERVER = "http://act.project.so";

        public static string[] ASMCHK = { "CefSharp", "CefSharp.Core", "CefSharp.WinForms" };

        public static string[] SETDIR =
        {
            CEFBIN,
            RESDIR,
            CEFDIR,
            CEFLOC,
            LOGDIR,
            PLUGINDIR,
            CEFUSR,
            BINDIR,
            LIBDIR
        };

        public static string BIN(this string s)
        {
            return string.Join("\\", RESDIR, "bin", s);
        }

        public static string LIB(this string s)
        {
            return string.Join("\\", RESDIR, "lib", s);
        }

        public static string Append(this string s, string append)
        {
            return s + append;
        }
    }
}
