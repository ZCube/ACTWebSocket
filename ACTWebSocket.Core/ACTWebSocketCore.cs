using Advanced_Combat_Tracker;
using System;
using System.Collections.Generic;
using System.Linq;
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

            httpServer.AddWebSocketService<EchoSocketBehavior>("/MiniParse");
            httpServer.AddWebSocketService<EchoSocketBehavior>("/BeforeLogLineRead");


            httpServer.RootPath = pluginDirectory;

            httpServer.OnGet += (sender, e) =>
            {
                var req = e.Request;
                var res = e.Response;

                var path = req.RawUrl;
                if (path == "/")
                    path += "index.html";

                var content = httpServer.GetFile(path);
                if (content == null)
                {
                    res.StatusCode = (int)HttpStatusCode.NotFound;
                    return;
                }

                if (path.EndsWith(".html"))
                {
                    res.ContentType = "text/html";
                    res.ContentEncoding = Encoding.UTF8;
                    content = res.ContentEncoding.GetBytes(res.ContentEncoding.GetString(content).Replace("@HOST_PORT@", (domain != null && domain.Length > 0 ? domain : address) + ":" + port.ToString()));
                }
                else if (path.EndsWith(".js"))
                {
                    res.ContentType = "application/javascript";
                    res.ContentEncoding = Encoding.UTF8;
                }

                res.WriteContent(content);
            };

            // TODO : Basic Auth
            /*
            wssv.Realm = "ACTWebSocket";
            wssv.AuthenticationSchemes = AuthenticationSchemes.Basic;
            wssv.UserCredentialsFinder = id => {
                var name = id.Name;

                // Return user name, password, and roles.
                return name == "nobita"
                       ? new NetworkCredential(name, "password", "gunfighter")
                       : null; // If the user credentials aren't found.
            };
            */

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
                foreach (var s in httpServer.WebSocketServices.Hosts)
                {
                    if(Filters.ContainsKey(v) && Filters[v])
                    {
                        if (s.Path.CompareTo(v) == 0)
                        {
                            s.Sessions.Broadcast(text);
                        }
                    }
                }
            }
        }
    }
}
