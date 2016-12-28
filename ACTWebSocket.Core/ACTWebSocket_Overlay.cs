using ACTWebSocket.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace ACTWebSocket_Plugin
{
    public partial class ACTWebSocketCore
    {
        static public string overlayWindowPrefix = "_private_overlay_";
        static public string overlayFullscreenName = "FullScreen_Overlay";


        String [] savedvar = {
          "url",
          "opacity",
          "zoom",
          "fps",
          "clickthru",
          "nonfocus",
          "dragging",
          "dragndrop",
          "hide",
          "resize",
          "x",
          "y",
          "width",
          "height"
        };
        String [] nativevar = {
          "url",
          "opacity",
          "zoom",
          "fps",
          "Transparent",
          "NoActivate",
          "useDragFilter",
          "useDragMove",
          "hide",
          "useResizeGrip",
          "x",
          "y",
          "width",
          "height"
        };

        public JObject APIOverlayWindowClose(JObject o)
        {
            //  in
            //    "title",
            //  out
            //    "title",
            //    "error",
            JObject ret = new JObject();
            String title = o.Value<String>("title");
            ret["title"] = title;
            IntPtr hwnd = Native.FindWindow(null, overlayWindowPrefix + title);
            if (hwnd == null || hwnd.ToInt64() == 0)
            {
                //ret["error"] = "Not Exist";
            }
            else
            {
                Native.SendMessage(hwnd, 0x0400 + 1, new IntPtr(0x08), new IntPtr(0x08));
                Native.CloseWindow(hwnd);
            }
            return ret;
        }

        public async Task<JObject> APIOverlayWindow_UpdatePosition(JObject o)
        {
            //  in
            //    "title",
            //    "x",
            //    "y",
            //    "width",
            //    "height"
            //  out
            //    "title",
            //    "x",
            //    "y",
            //    "width",
            //    "height"
            //    "error",
            JObject ret = new JObject();
            String title = o.Value<String>("title");
            IntPtr hwnd = Native.FindWindow(null, overlayWindowPrefix + title);
            ret["title"] = title;
            if ((hwnd == null || hwnd.ToInt64() == 0) && title != overlayFullscreenName)
            {
                ret["error"] = "NoWindow";
            }
            else
            {
                String json = o.ToString();
                Native.SendMessageToWindow(hwnd, 1, json);
            }
            return ret;
        }

        public async Task<JObject> APIOverlayWindow_GetPosition(JObject o)
        {
            //  in
            //    "title",
            //  out  
            //    "title",
            //    "x",
            //    "y",
            //    "width",
            //    "height"
            //    "error",
            JObject ret = new JObject();
            String title = o.Value<String>("title");
            IntPtr hwnd = Native.FindWindow(null, overlayWindowPrefix + title);
            ret["title"] = title;
            if ((hwnd == null || hwnd.ToInt64() == 0) && title != overlayFullscreenName)
            {
                ret["error"] = "NoWindow";
            }
            else
            {
                Native.RECT rect = new Native.RECT();
                Native.GetWindowRect(hwnd, out rect);

                ret["x"] = rect.Left;
                ret["y"] = rect.Top;
                ret["width"] = (rect.Right - rect.Left);
                ret["height"] = (rect.Bottom - rect.Top);
            }
            return ret;
        }

        public JObject APIOverlayWindow_UpdatePreference(JObject o)
        {
            //  in
            //    "title",
            //    "url",
            //    "opacity",
            //    "zoom",
            //    "fps",
            //    "clickthru",
            //    "nonfocus",
            //    "dragging",
            //    "dragndrop",
            //    "hide",
            //    "resize",
            //  out  
            //    "title",
            //    "url",
            //    "opacity",
            //    "zoom",
            //    "fps",
            //    "clickthru",
            //    "nonfocus",
            //    "dragging",
            //    "dragndrop",
            //    "hide",
            //    "resize",
            //    "error",
            JObject ret = new JObject();
            String title = o.Value<String>("title");
            IntPtr hwnd = Native.FindWindow(null, overlayWindowPrefix + title);
            ret["title"] = title;
            if ((hwnd == null || hwnd.ToInt64() == 0) && title != overlayFullscreenName)
            {
                ret["error"] = "NoWindow";
            }
            else
            {
                JObject src = (JObject)o.DeepClone();
                src["title"] = overlayWindowPrefix + title;
                ret = o;
                Native.SendMessageToWindow(hwnd, 1, src.ToString());
            }
            return ret;
        }

        public async Task<JObject> APIOverlayWindow_GetPreference(JObject o)
        {
            //  in
            //    "title",
            //  out  
            //    "title",
            //    "url",
            //    "opacity",
            //    "zoom",
            //    "fps",
            //    "clickthru",
            //    "nonfocus",
            //    "dragging",
            //    "dragndrop",
            //    "hide",
            //    "resize",
            //    "error",
            JObject ret = new JObject();
            String title = o.Value<String>("title");
            IntPtr hwnd = Native.FindWindow(null, overlayWindowPrefix + title);
            ret["title"] = title;
            if ((hwnd == null || hwnd.ToInt64() == 0) && title != overlayFullscreenName)
            {
                ret["error"] = "NoWindow";
            }
            else
            {
                JObject src = (JObject)o.DeepClone();
                src["title"] = overlayWindowPrefix + title;
                // request
                ret["hwnd"] = hwnd.ToInt64().ToString();
                Native.SendMessageToWindow(hwnd, 2, src.ToString());
            }
            return ret;
        }

        public JObject APIOverlayWindow_New(JObject o)
        {
            //  in
            //    "title",
            //    "url",
            //    "opacity",
            //    "zoom",
            //    "fps",
            //    "clickthru",
            //    "nonfocus",
            //    "dragging",
            //    "dragndrop",
            //    "hide",
            //    "resize",
            //    "x",
            //    "y",
            //    "width",
            //    "height"
            //  out  
            //    "title",
            //    "url",
            //    "opacity",
            //    "zoom",
            //    "fps",
            //    "clickthru",
            //    "nonfocus",
            //    "dragging",
            //    "dragndrop",
            //    "hide",
            //    "resize",
            //    "error",
            //o["Transparent"] = false;
            //o["NoActivate"] = false;
            //o["hide"] = false;
            //o["useDragFilter"] = true;
            //o["useDragMove"] = true;
            //o["useResizeGrip"] = true;
            //o["opacity"] = 1.0;
            //o["zoom"] = 1.0;
            //o["url"] = url;
            //o["title"] = title;
            //o["fps"] = 30.0;
            //o["x"] = Convert.ToInt32(x.Text);
            //o["y"] = Convert.ToInt32(y.Text);
            //o["width"] = Convert.ToInt32(width.Text) <= 0 ? 1 : Convert.ToInt32(width.Text);
            //o["height"] = Convert.ToInt32(height.Text) <= 0 ? 1 : Convert.ToInt32(height.Text);
            //o["width"] = 100;
            //o["height"] = 100;

            if(_NewOverlayWindow(o))
            {
                return o;
            }
            o["error"] = "NoWindow";
            return o;
        }

        private bool _NewOverlayWindow(JObject obj)
        {
            string overlayPath = pluginDirectory + "/overlay/overlay_proc.exe";
            if (File.Exists(overlayPath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = overlayPath;
                JObject o = (JObject)obj.DeepClone();
                string title = o["title"].Value<string>();
                o["title"] = overlayWindowPrefix + o["title"];

                string json = json = o.ToString();
                startInfo.Arguments = Utility.Base64Encoding(json);
                Process p = Process.Start(startInfo);
                return true;
            }
            return false;
        }
        public JObject APIOverlayWindow_GetSkinList(JObject o)
        {
            //  in
            //  out
            //    "skins"
            //    "error",
            try
            {
                JObject ret = new JObject();
                JArray skins = new JArray();
                foreach (string file in Directory.EnumerateFiles(overlaySkinDirectory, "*.html", SearchOption.AllDirectories))
                {
                    skins.Add(Utility.GetRelativePath(file, overlaySkinDirectory));
                }
                ret["skins"] = skins;
                return ret;
            }
            catch (Exception e)
            {
                JObject ret = new JObject();
                ret["error"] = e.Message;
                return ret;
            }
        }

        public JObject APISaveSettings(JObject o)
        {
            //  in
            //    "overlaywindows"
            //  out
            //    "error",
            //
            try
            {
                File.WriteAllText(pluginDirectory + "\\overlaywindows.json", o.ToString());
                return new JObject();
            }
            catch (Exception e)
            {
                JObject ret = new JObject();
                ret["error"] = e.Message;
                return ret;
            }
        }

        public JObject APILoadSettings(JObject o)
        {
            try
            {
                JObject ret = JObject.Parse(File.ReadAllText(pluginDirectory + "\\overlaywindows.json"));
                return ret;
            }
            catch(Exception e)
            {
                JObject ret = new JObject();
                ret["error"] = e.Message;
                return ret;
            }
        }

    }
}
