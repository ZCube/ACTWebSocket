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
        ACTWebSocketMain gui = null;
        public ACTWebSocketCore(ACTWebSocketMain gui)
        {
            overlayAPI = new FFXIV_OverlayAPI(this);
            this.gui = gui;
        }
        public Dictionary<string, bool> Filters = new Dictionary<string, bool>();
        public static FFXIV_OverlayAPI overlayAPI;
        public HttpServer httpServer = null;
        Timer updateTimer = null;
        Timer pingTimer = null;
        internal IntPtr hwnd;
        public JObject skinObject = new JObject();
        static public string randomDir = null;
        static public List<String> addrs = new List<String>();

        public void SetAddress(List<String> addrs)
        {
            ACTWebSocketCore.addrs = addrs;
        }

        void InitUpdate()
        {
            System.Threading.Thread.Sleep(1000);
            Broadcast("/MiniParse", SendMessageType.CombatData.ToString(), overlayAPI.CreateEncounterJsonData());
        }

        //static X509Certificate2 GenerateCertificate(string certName)
        //{
        //    var keypairgen = new RsaKeyPairGenerator();
        //    keypairgen.Init(new KeyGenerationParameters(new SecureRandom(new CryptoApiRandomGenerator()), 2048));

        //    var keypair = keypairgen.GenerateKeyPair();

        //    var gen = new X509V3CertificateGenerator();

        //    var CN = new X509Name("CN=" + certName);
        //    var SN = BigInteger.ProbablePrime(120, new Random());

        //    gen.SetSerialNumber(SN);
        //    gen.SetSubjectDN(CN);
        //    gen.SetIssuerDN(CN);
        //    gen.SetNotAfter(DateTime.MaxValue);
        //    gen.SetNotBefore(DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)));
        //    gen.SetSignatureAlgorithm("SHA256WithRSA");
        //    gen.SetPublicKey(keypair.Public);

        //    var newCert = gen.Generate(keypair.Private);
            
        //    X509Certificate2 cert = new X509Certificate2(DotNetUtilities.ToX509Certificate((Org.BouncyCastle.X509.X509Certificate)newCert));
        //    cert.PrivateKey = DotNetUtilities.ToRSA((RsaPrivateCrtKeyParameters) keypair.Private);
        //    return cert;
        //}
        internal void StartServer(string address, int port, int extPort, string domain = null, bool skinOnAct = false, bool useSSL = false)
        {
            StopServer();

            httpServer = new HttpServer(System.Net.IPAddress.Parse(address), port, useSSL);
            httpServer.ReuseAddress = true;
            //if (useSSL)
            //{
            //    httpServer.SslConfiguration.ServerCertificate = GenerateCertificate(domain);
            //}

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

            if (randomDir != null)
            {
                httpServer.AddWebSocketService<WebSocketCommunicateBehavior>(parent_path + "/Communicate");
                httpServer.AddWebSocketService<WebSocketCommunicateBehavior>(parent_path + "/MiniParse");
                httpServer.AddWebSocketService<WebSocketCommunicateBehavior>(parent_path + "/BeforeLogLineRead");
                httpServer.AddWebSocketService<WebSocketCommunicateBehavior>(parent_path + "/OnLogLineRead");
            }

            if (skinOnAct)
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

            EventHandler < HttpRequestEventArgs > onget = (sender, e) =>
            {
                try
                {
                    var req = e.Request;
                    var res = e.Response;
                    HttpListenerContext context = (HttpListenerContext)req.GetType().GetField("_context", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(req);
                    var path = req.RawUrl;

                    bool localConnection = false;
                    localConnection = addrs.Contains(req.RemoteEndPoint.Address.ToString());

                    if (randomDir != null && !localConnection)
                    {
                        if (!path.StartsWith(parent_path))
                        {
                            res.StatusCode = (int)HttpStatusCode.NotFound;
                            return;
                        }
                    }

                    if (path.StartsWith(parent_path))
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
                        if (path == "/skins.json" || path == "/pages.json")
                        {
                            if (skinObject != null)
                            {
                                lock (skinObject)
                                {
                                    res.ContentType = "text/html";
                                    res.ContentEncoding = Encoding.UTF8;
                                    var clone = skinObject.DeepClone();

                                    JArray array = (JArray)clone["URLList"];
                                    if (array != null)
                                    {
                                        foreach (JToken obj in array)
                                        {
                                            obj["URL"] = gui.getURLPath(obj["URL"].ToObject<String>(), gui.RandomURL);
                                        }
                                    }
                                    res.WriteContent(res.ContentEncoding.GetBytes(clone.ToString()));
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
                        else if (domain != null && domain.Length > 0)
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
                        String co = res.ContentEncoding.GetString(content).Replace("@HOST_PORT@", host_port);
                        if (e.Request.Url.Scheme == "https")
                        {
                            co = co.Replace("ws://", "wss://").Replace("http://", "https://");
                        }
                        content = res.ContentEncoding.GetBytes(co);

                    }

                    res.WriteContent(content);
                }
                catch(Exception ex)
                {
                    // TODO:
                }
            };
            httpServer.OnGet += onget;

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
                foreach (var s in httpServer.WebSocketServices.Hosts)
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
                    if (Filters.ContainsKey(v) && Filters[v])
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
