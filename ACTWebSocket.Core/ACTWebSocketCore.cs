using Advanced_Combat_Tracker;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;

namespace ACTWebSocket_Plugin
{
    public partial class ACTWebSocketCore
    {
        public ACTWebSocketCore() {
            StartUIServer();
            overlayAPI = new FFXIV_OverlayAPI(this);
        }

        public Dictionary<string, bool> Filters = new Dictionary<string, bool>();
        FFXIV_OverlayAPI overlayAPI;
        HttpServer uiServer = null;
        HttpServer httpServer = null;
        Timer updateTimer = null;
        Timer pingTimer = null;
        public string randomDir = "Test";
        internal IntPtr hwnd;

        class EchoSocketBehavior : WebSocketBehavior
        {
            public EchoSocketBehavior() { }
            protected override void OnOpen() { base.OnOpen(); }
            protected override void OnClose(CloseEventArgs e) { base.OnClose(e); }
            protected override void OnMessage(MessageEventArgs e)
            {
                switch (e.Type)
                {
                    case Opcode.Text:
                    case Opcode.Binary:
                    case Opcode.Cont:
                    case Opcode.Close:
                    case Opcode.Ping:
                    case Opcode.Pong:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        internal void StartUIServer()
        {
            StopUIServer();
            uiServer = new HttpServer(System.Net.IPAddress.Parse("127.0.0.1"), 9991);
            uiServer.OnPost += (sender, e) =>
            {
                var req = e.Request;
                var res = e.Response;
                res.AddHeader("Access-Control-Allow-Headers", "Origin,Accept,X-Requested-With,Content-Type,Access-Control-Request-Method,Access-Control-Request-Headers,Authorization");
                res.AddHeader("Access-Control-Allow-Origin", "*");

                HttpListenerContext context = (HttpListenerContext)req.GetType().GetField("_context", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(req);

                var path = req.RawUrl;
                path = Uri.UnescapeDataString(path);

                if (path.StartsWith("/api/"))
                {
                    JObject o = null;
                    if (req.ContentLength64 != -1)
                    {
                        byte[] data = new byte[req.ContentLength64];
                        req.InputStream.Read(data, 0, (int)req.ContentLength64);
                        string str = Encoding.UTF8.GetString(data, 0, (int)req.ContentLength64);
                        try
                        {
                            o = JObject.Parse(str);
                        }
                        catch (Exception e3)
                        {

                        }
                    }
                    //if (o != null)
                    try
                    {

                        if (path == "/api/loadsettings")
                        {
                            JObject ret = APILoadSettings(o);
                            res.WriteContent(Encoding.UTF8.GetBytes(ret.ToString()));
                        }
                        else if (path == "/api/savesettings")
                        {
                            JObject ret = APISaveSettings(o);
                            res.WriteContent(Encoding.UTF8.GetBytes(ret.ToString()));
                        }
                        else if (path == "/api/skin_get_list")
                        {
                            JObject ret = APIOverlayWindow_GetSkinList(o);
                            res.WriteContent(Encoding.UTF8.GetBytes(ret.ToString()));
                        }
                        else if (path == "/api/overlaywindow_new")
                        {
                            JObject ret = APIOverlayWindow_New(o);
                            res.WriteContent(Encoding.UTF8.GetBytes(ret.ToString()));
                        }
                        else if (path == "/api/overlaywindow_get_preference")
                        {
                            JObject ret = APIOverlayWindow_GetPreference(o).Result;
                            res.WriteContent(Encoding.UTF8.GetBytes(ret.ToString()));
                        }
                        else if (path == "/api/overlaywindow_update_preference")
                        {
                            JObject ret = APIOverlayWindow_UpdatePreference(o);
                            res.WriteContent(Encoding.UTF8.GetBytes(ret.ToString()));
                        }
                        else if (path == "/api/overlaywindow_get_position")
                        {
                            JObject ret = APIOverlayWindow_GetPosition(o).Result;
                            res.WriteContent(Encoding.UTF8.GetBytes(ret.ToString()));
                        }
                        else if (path == "/api/overlaywindow_update_position")
                        {
                            JObject ret = APIOverlayWindow_UpdatePosition(o).Result;
                            res.WriteContent(Encoding.UTF8.GetBytes(ret.ToString()));
                        }
                        else if (path == "/api/overlaywindow_close")
                        {
                            JObject ret = APIOverlayWindowClose(o);
                            res.WriteContent(Encoding.UTF8.GetBytes(ret.ToString()));
                        }
                    }
                    catch(Exception error)
                    {
                        System.Windows.Forms.MessageBox.Show(error.Message);
                    }
                }
            };
            uiServer.OnOptions += (sender, e) =>
            {
                var req = e.Request;
                var res = e.Response;
                res.AddHeader("Access-Control-Allow-Headers", "Origin,Accept,X-Requested-With,Content-Type,Access-Control-Request-Method,Access-Control-Request-Headers,Authorization");
                res.AddHeader("Access-Control-Allow-Origin", "*");
            };
            uiServer.Start();
        }

        internal void StopUIServer()
        {
            if (uiServer != null)
            {
                uiServer.Stop();
                uiServer = null;
            }
        }


        internal void StartServer(string address, int port, string domain = null)
        {
            StopServer();
            overlayAPI.AttachACTEvent();

            httpServer = new HttpServer(System.Net.IPAddress.Parse(address), port);

            // TODO : SSL
            //wssv = new WebSocketServer(System.Net.IPAddress.Parse(address), port, true);
            //wssv.SslConfiguration.ServerCertificate =
            //  new X509Certificate2("/path/to/cert.pfx", "password for cert.pfx");

            string parent_path = "";
            if(randomDir != null)
            {
                parent_path = "/" + randomDir;
            }
            httpServer.AddWebSocketService<EchoSocketBehavior>(parent_path + "/MiniParse");
            httpServer.AddWebSocketService<EchoSocketBehavior>(parent_path + "/BeforeLogLineRead");
            httpServer.AddWebSocketService<EchoSocketBehavior>(parent_path + "/OnLogLineRead");
            
            httpServer.RootPath = overlaySkinDirectory;
            httpServer.OnConnect += (sender, e) =>
            {
                var req = e.Request;
            };
            uiServer.OnPost += (sender, e) =>
            {
            };
            httpServer.OnGet += (sender, e) =>
            {
                var req = e.Request;
                var res = e.Response;
                HttpListenerContext context = (HttpListenerContext)req.GetType().GetField("_context", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(req);

                var path = req.RawUrl;
                if(randomDir != null)
                {
                    if (!path.StartsWith(parent_path))
                    {
                        res.StatusCode = (int)HttpStatusCode.NotFound;
                        return;
                    }
                }
                if(path.StartsWith(parent_path))
                {
                    path = path.Substring(parent_path.Length);
                }
                if (path == "/")
                    path += "index.html";

                path = Uri.UnescapeDataString(path);

                var content = httpServer.GetFile(path);
                if (content == null)
                {
                    res.StatusCode = (int)HttpStatusCode.NotFound;
                    return;
                }
                string extension = System.IO.Path.GetExtension(path);
                extension = extension.ToLower();
                res.ContentType = MimeTypes.MimeTypeMap.GetMimeType(System.IO.Path.GetExtension(path));
                if (extension == ".html" || extension == ".js")
                {
                    res.ContentType = "text/html";
                    res.ContentEncoding = Encoding.UTF8;
                    string host = "";
                    if (address == "127.0.0.1" || address == "localhost")
                    {
                        host = "localhost";
                    }
                    else  if (domain != null && domain.Length >0)
                    {
                        host = domain;
                    }

                    string host_port = host + ":" + port.ToString();
                    if (context.User != null)
                    {
                        string username = context.User.Identity.Name;
                        NetworkCredential cred = httpServer.UserCredentialsFinder(context.User.Identity);
                        string password = cred.Password;
                        host_port = username + ":" + password + "@" + host + ":" + port.ToString();
                    }
                    else
                    {
                        host_port = host + ":" + port.ToString();
                    }
                    host_port += parent_path;

                    content = res.ContentEncoding.GetBytes(res.ContentEncoding.GetString(content).Replace("@HOST_PORT@", host_port));
                }

                res.WriteContent(content);
            };

            //// TODO : Basic Auth
            //httpServer.Realm = "ACTWebSocket";
            //httpServer.AuthenticationSchemes = AuthenticationSchemes.Basic;
            //httpServer.UserCredentialsFinder = id => {
            //    var name = id.Name;

            //    // Return user name, password, and roles.
            //    return name == "nobita"
            //           ? new NetworkCredential(name, "password", "gunfighter")
            //           : null; // If the user credentials aren't found.
            //};

            httpServer.Start();

            pingTimer = new Timer();
            pingTimer.Interval = 2000;
            pingTimer.Elapsed += (o, e) =>
            {
                try
                {
                    if (httpServer != null)
                    {
                        httpServer.WebSocketServices.Broadcast(".");
                    }
                }
                catch (Exception ex)
                {
                    Log(LogLevel.Error, "Update: {0}", ex.ToString());
                }
            };
            pingTimer.Start();

            updateTimer = new Timer();
            updateTimer.Interval = 1000;
            updateTimer.Elapsed += (o, e) =>
            {
                try
                {
                    Update();
                }
                catch (Exception ex)
                {
                    Log(LogLevel.Error, "Update: {0}", ex.ToString());
                }
            };
            updateTimer.Start();
        }

        internal void StopServer()
        {
            overlayAPI.DetachACTEvent();
            if (httpServer != null)
            {
                httpServer.Stop();
                foreach(var s in httpServer.WebSocketServices.Hosts)
                {
                    httpServer.RemoveWebSocketService(s.Path);
                }

                httpServer = null;
            }
            if (updateTimer != null)
            {
                updateTimer.Stop();
                updateTimer.Close();
                updateTimer = null;
            }
            if (pingTimer != null)
            {
                pingTimer.Stop();
                pingTimer.Close();
                pingTimer = null;
            }

        }

        internal void Broadcast(string v, string text)
        {
            if (httpServer != null)
            {
                string parent_path = "";
                if (randomDir != null)
                {
                    parent_path = "/" + randomDir;
                }
                foreach (var s in httpServer.WebSocketServices.Hosts)
                {
                    string x = s.Path;
                    if(Filters.ContainsKey(v) && Filters[v])
                    {
                        if (s.Path.CompareTo(parent_path + v) == 0)
                        {
                            s.Sessions.Broadcast(text);
                        }
                    }
                }
            }
        }
    }
}
