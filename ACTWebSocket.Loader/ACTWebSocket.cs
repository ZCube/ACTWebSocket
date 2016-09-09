using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Advanced_Combat_Tracker;
using System.IO;
using System.Reflection;
using System.Xml;

namespace ACTWebSocket_Plugin
{
    using System.Linq;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using System.Timers;

    public interface PluginDirectory
    {
        void SetPluginDirectory(String path);
        String GetPluginDirectory();
    }

    public class ACTWebSocket : IActPluginV1
	{
        public ACTWebSocket()
        {

        }
        IActPluginV1 main = null;
        AssemblyResolver asmResolver = null;
        #region IActPluginV1 Members

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            String pluginDirectory = GetPluginDirectory();

            var directories = new List<string>();
            directories.Add(pluginDirectory);
            asmResolver = new AssemblyResolver(directories);

            Initialize(pluginScreenSpace, pluginStatusText);
        }
        
        private void Initialize(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            asmResolver.ExceptionOccured += (o, e) =>
            {
                //logger.Log(LogLevel.Error, "AssemblyResolver: Error: {0}", e.Exception);
            };
            asmResolver.AssemblyLoaded += (o, e) =>
            {
                //logger.Log(LogLevel.Debug, "AssemblyResolver: Loaded: {0}", e.LoadedAssembly.FullName);
            };

            var m = new ACTWebSocketMain();
            String pluginDirectory = GetPluginDirectory();
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
                return System.IO.Path.GetDirectoryName(plugin.pluginFile.FullName);
            }
            else
            {
                throw new Exception();
            }
        }
        #endregion
    }
}
