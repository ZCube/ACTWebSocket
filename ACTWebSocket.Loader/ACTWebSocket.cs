using Advanced_Combat_Tracker;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ACTWebSocket_Plugin
{
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using ACTWebSocket_Plugin.Classes;
    public interface PluginDirectory
    {
        void SetPluginDirectory(string path);
        string GetPluginDirectory();
    }

    public class ACTWebSocket : IActPluginV1
    {
        public ACTWebSocket()
        {

        }
        ~ACTWebSocket()
        {
        }
        IActPluginV1 main = null;
        AssemblyResolver asmResolver = null;

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            string pluginDirectory = GetPluginDirectory();

            var directories = new List<string>();
            directories.Add(pluginDirectory);
            asmResolver = new AssemblyResolver(directories);

            Initialize(pluginScreenSpace, pluginStatusText);
        }

        private void Initialize(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            pluginScreenSpace.Text = "오버레이 & 웹소켓 ";
            asmResolver.ExceptionOccured += (o, e) =>
            {
                //logger.Log(LogLevel.Error, "AssemblyResolver: Error: {0}", e.Exception);
            };
            asmResolver.AssemblyLoaded += (o, e) =>
            {
                //logger.Log(LogLevel.Debug, "AssemblyResolver: Loaded: {0}", e.LoadedAssembly.FullName);
            };

            AppDomain.CurrentDomain.AssemblyResolve += delegate (object sender, ResolveEventArgs args)
            {
                string asmFile = (args.Name.Contains(",") ? args.Name.Substring(0, args.Name.IndexOf(",")) : args.Name);

                if (!Settings.ASMCHK.Contains(asmFile))
                {
                    return null;
                }

                try
                {
                    if(Environment.Is64BitOperatingSystem)
                        return Assembly.LoadFile(Path.Combine(Settings.CEFDIR64, asmFile + ".dll"));
                    else
                        return Assembly.LoadFile(Path.Combine(Settings.CEFDIR, asmFile + ".dll"));
                }
                catch
                {
                    return null;
                }
            };

            var m = new ACTWebSocketMain();
            string pluginDirectory = GetPluginDirectory();
            m.SetSkinDir(Path.Combine(Environment.CurrentDirectory, "OverlaySkin"));
            m.SetPluginDirectory(pluginDirectory);
            m.InitPlugin(pluginScreenSpace, pluginStatusText);
            main = m;
        }

        public void DeInitPlugin()
        {
            main.DeInitPlugin();
            asmResolver.Dispose();
        }

        private string GetPluginDirectory()
        {
            var plugin = ActGlobals.oFormActMain.ActPlugins.Where(x => x.pluginObj == this).FirstOrDefault();
            if (plugin != null)
            {
                return Path.GetDirectoryName(plugin.pluginFile.FullName);
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
