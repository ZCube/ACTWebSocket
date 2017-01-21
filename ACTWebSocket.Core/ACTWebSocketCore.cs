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
        public ACTWebSocketCore()
        {
            overlayAPI = new FFXIV_OverlayAPI(this);
        }

        public Dictionary<string, bool> Filters = new Dictionary<string, bool>();
        public static FFXIV_OverlayAPI overlayAPI;
        public HttpServer httpServer = null;
        Timer updateTimer = null;
        Timer pingTimer = null;
        public string randomDir = null;
        internal IntPtr hwnd;
        public JObject skinObject = new JObject();

        void InitUpdate()
        {
            System.Threading.Thread.Sleep(1000);
            Broadcast("/MiniParse", SendMessageType.CombatData.ToString(), overlayAPI.CreateEncounterJsonData());
        }

        internal void StartServer(string address, int port, int extPort, string domain = null, bool skinOnAct = false)
        {
            StopServer();

            httpServer = new HttpServer(System.Net.IPAddress.Parse(address), port);
            httpServer.ReuseAddress = true;

            // TODO : SSL
            //wssv = new WebSocketServer(System.Net.IPAddress.Parse(address), port, true);
            //wssv.SslConfiguration.ServerCertificate =
            //  new X509Certificate2("/path/to/cert.pfx", "password for cert.pfx");

            string parent_path = "";
            if(randomDir != null)
            {
                parent_path = "/" + randomDir;
            }

            httpServer.AddWebSocketService<WebSocketCommunicateBehavior>(parent_path + "/Communicate");
            httpServer.AddWebSocketService<WebSocketCommunicateBehavior>(parent_path + "/MiniParse");
            httpServer.AddWebSocketService<WebSocketCommunicateBehavior>(parent_path + "/BeforeLogLineRead");
            httpServer.AddWebSocketService<WebSocketCommunicateBehavior>(parent_path + "/OnLogLineRead");
            
            if(skinOnAct)
            {
                httpServer.RootPath = overlaySkinDirectory;
            }
            else
            {
                httpServer.RootPath = pluginDirectory;
            }

            httpServer.OnConnect += (sender, e) =>
            {
                var req = e.Request;
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

                Uri uri = new Uri("http://localhost" + path);
                path = uri.AbsolutePath;
                path = Uri.UnescapeDataString(path);
                //uri.Query;
                //uri.AbsolutePath;
                var content = httpServer.GetFile(path);

                if (content == null)
                {
                    if (path == "/skins.json")
                    {
                        if(skinObject != null)
                        {
                            lock (skinObject)
                            {
                                res.ContentType = "text/html";
                                res.ContentEncoding = Encoding.UTF8;
                                res.WriteContent(res.ContentEncoding.GetBytes(skinObject.ToString()));
                            }
                        }
                        else
                        {
                            res.StatusCode = (int)HttpStatusCode.NotFound;
                        }
                    }
                    else
                    {
                        res.StatusCode = (int)HttpStatusCode.NotFound;
                    }
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

                    string host_port = host + ":" + extPort.ToString();
                    if (context.User != null)
                    {
                        string username = context.User.Identity.Name;
                        NetworkCredential cred = httpServer.UserCredentialsFinder(context.User.Identity);
                        string password = cred.Password;
                        host_port = username + ":" + password + "@" + host + ":" + extPort.ToString();
                    }
                    else
                    {
                        host_port = host + ":" + extPort.ToString();
                    }
                    host_port += parent_path;
                    res.SetCookie(new Cookie("HOST_PORT", host_port));

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
            // TODO : 아직 서버 실행 시 Attach 가 정상적으로 되지 않음...
            // overlayAPI.DetachACTEvent();
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

        internal void Broadcast(string v, string type, JToken message)
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
                            JObject obj = new JObject();
                            obj["type"] = "broadcast";
                            obj["msgtype"] = type;
                            obj["msg"] = message;
                            String str = obj.ToString();
                            s.Sessions.Broadcast(str);
                        }
                    }
                }
            }
        }
    }
}
