using Advanced_Combat_Tracker;
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
        public ACTWebSocketCore(){}
        
        public Dictionary<String, Boolean> Filters = new Dictionary<string, bool>();
        HttpServer httpServer = null;
        Timer updateTimer = null;
        Timer pingTimer = null;
        public String randomDir = "Test";

        class EchoSocketBehavior : WebSocketBehavior
        {
            public EchoSocketBehavior(){}
            protected async override void OnOpen(){base.OnOpen();}
            protected override void OnClose(CloseEventArgs e){base.OnClose(e);}
            protected override async void OnMessage(MessageEventArgs e){
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


        internal void StartServer(String address, int port, String domain = null)
        {
            StopServer();
            httpServer = new HttpServer(System.Net.IPAddress.Parse(address), port);

            // TODO : SSL
            //wssv = new WebSocketServer(System.Net.IPAddress.Parse(address), port, true);
            //wssv.SslConfiguration.ServerCertificate =
            //  new X509Certificate2("/path/to/cert.pfx", "password for cert.pfx");

            String parent_path = "";
            if(randomDir != null)
            {
                parent_path = "/" + randomDir;
            }
            httpServer.AddWebSocketService<EchoSocketBehavior>(parent_path + "/MiniParse");
            httpServer.AddWebSocketService<EchoSocketBehavior>(parent_path + "/BeforeLogLineRead");
            httpServer.AddWebSocketService<EchoSocketBehavior>(parent_path + "/OnLogLineRead");


            httpServer.RootPath = pluginDirectory;
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

                path = System.Uri.UnescapeDataString(path);

                var content = httpServer.GetFile(path);
                if (content == null)
                {
                    res.StatusCode = (int)HttpStatusCode.NotFound;
                    return;
                }
                String extension = System.IO.Path.GetExtension(path);
                extension = extension.ToLower();
                res.ContentType = MimeTypes.MimeTypeMap.GetMimeType(System.IO.Path.GetExtension(path));
                if (extension == ".html" || extension == ".js")
                {
                    res.ContentType = "text/html";
                    res.ContentEncoding = Encoding.UTF8;
                    String host = "";
                    if (address == "127.0.0.1" || address == "localhost")
                    {
                        host = "localhost";
                    }
                    else  if (domain != null && domain.Length >0)
                    {
                        host = domain;
                    }

                    String host_port = host + ":" + port.ToString();
                    if (context.User != null)
                    {
                        String username = context.User.Identity.Name;
                        NetworkCredential cred = httpServer.UserCredentialsFinder(context.User.Identity);
                        String password = cred.Password;
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

            pingTimer = new System.Timers.Timer();
            pingTimer.Interval = 2000;
            pingTimer.Elapsed += async (o, e) =>
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

            updateTimer = new System.Timers.Timer();
            updateTimer.Interval = 1000;
            updateTimer.Elapsed += async (o, e) =>
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
                String parent_path = "";
                if (randomDir != null)
                {
                    parent_path = "/" + randomDir;
                }
                foreach (var s in httpServer.WebSocketServices.Hosts)
                {
                    String x = s.Path;
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
