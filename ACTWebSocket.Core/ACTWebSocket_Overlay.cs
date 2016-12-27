using ACTWebSocket.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace ACTWebSocket_Plugin
{
    public partial class ACTWebSocketCore
    {
        static public string overlayWindowPrefix = "overlay_";
        static public string overlayFullscreenName = "FullScreen_Overlay";

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
            throw new NotImplementedException();
        }

        public JObject APIOverlayWindow_GetPreference(JObject o)
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
                JObject skins = new JObject();
                foreach (string file in Directory.EnumerateFiles(pluginDirectory, "*.html", SearchOption.AllDirectories))
                {
                    skins.Add(Utility.GetRelativePath(file, pluginDirectory));
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
