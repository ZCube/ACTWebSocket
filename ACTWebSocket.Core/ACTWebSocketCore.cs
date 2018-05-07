using Advanced_Combat_Tracker;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;

namespace ACTWebSocket_Plugin
{
    public partial class ACTWebSocketCore
    {
        public static String currentVersionString = "";
        ACTWebSocketMain gui = null;
        public ACTWebSocketCore(ACTWebSocketMain gui)
        {
            overlayAPI = new FFXIV_OverlayAPI(this);
            this.gui = gui;
        }
        public Dictionary<string, bool> Filters = new Dictionary<string, bool>();
        public static FFXIV_OverlayAPI overlayAPI;
        public HttpServer httpServer = null;
        System.Timers.Timer updateTimer = null;
        System.Timers.Timer pingTimer = null;
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

        static X509Certificate2 GenerateCertificate(string certName)
        {
            var keypairgen = new RsaKeyPairGenerator();
            keypairgen.Init(new KeyGenerationParameters(new SecureRandom(new CryptoApiRandomGenerator()), 2048));

            var keypair = keypairgen.GenerateKeyPair();

            var gen = new X509V3CertificateGenerator();

            var CN = new X509Name("CN=" + certName);
            var SN = BigInteger.ProbablePrime(120, new Random());

            gen.SetSerialNumber(SN);
            gen.SetSubjectDN(CN);
            gen.SetIssuerDN(CN);
            gen.SetNotAfter(DateTime.MaxValue);
            gen.SetNotBefore(DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)));
            gen.SetSignatureAlgorithm("SHA256WithRSA");
            gen.SetPublicKey(keypair.Public);

            var newCert = gen.Generate(keypair.Private);

            X509Certificate2 cert = new X509Certificate2(DotNetUtilities.ToX509Certificate((Org.BouncyCastle.X509.X509Certificate)newCert));
            cert.PrivateKey = DotNetUtilities.ToRSA((RsaPrivateCrtKeyParameters)keypair.Private);
            return cert;
        }
        internal void StartServer(string address, int port, int extPort, string domain = null, bool skinOnAct = false, bool useSSL = false)
        {
            StopServer();
            
            httpServer = new HttpServer(System.Net.IPAddress.Parse(address), port, useSSL);
            httpServer.ReuseAddress = true;
            if (useSSL)
            {
                httpServer.SslConfiguration.ServerCertificate = GenerateCertificate(domain);
            }
            try
            {
                // for Parser's MemoryError
                var field = typeof(WebSocket).GetField("FragmentLength",
                                    BindingFlags.Static |
                                    BindingFlags.NonPublic);
                int value = (int)field.GetValue(null);
                field.SetValue(null, Int32.MaxValue - 14);
            }
            catch(Exception e)
            {
            }
            // TODO : SSL
            //wssv = new WebSocketServer(System.Net.IPAddress.Parse(address), port, true);
            //wssv.SslConfiguration.ServerCertificate =
            //  new X509Certificate2("/path/to/cert.pfx", "password for cert.pfx");

            string parent_path = "";
            if (randomDir != null)
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

            EventHandler<HttpRequestEventArgs> onget = (sender, e) =>
            {
                byte[] content = null;
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
                    if (path.StartsWith("/github/"))
                    {
                        Regex r = new Regex(@"\/github\/(?<User>[\w]*)\/(?<Other>.*)");
                        // Match the regular expression pattern against a text string.
                        Match m = r.Match(req.Url.PathAndQuery);
                        if(m.Success)
                        {
                            System.Net.WebClient wc = new System.Net.WebClient();
                            wc.Headers["User-Agent"] = "ACTWebSocket (" + ACTWebSocketCore.currentVersionString + ")";
                            string github_url = "https://" + m.Groups[1] + ".github.io/" + m.Groups[2];
                            try
                            {
                                content = wc.DownloadData(github_url);
                                res.StatusCode = 200;
                                res.ContentType = wc.ResponseHeaders["Content-Type"];
                                var tmp = Encoding.UTF8.GetString(content);

                                string host_port = "//"+ req.Url.Host + ":" + req.Url.Port.ToString();
                                string path_dir = "/github/" + m.Groups[1] + "/";

                                tmp = tmp.Replace("href=\'//", "href=\'$//");
                                tmp = tmp.Replace("href=\"//", "href=\"$//");
                                tmp = tmp.Replace("src=\'//", "src=\'$//");
                                tmp = tmp.Replace("src=\"//", "src=\"$//");

                                tmp = tmp.Replace("href=\'/", "href=\'" + host_port + path_dir);
                                tmp = tmp.Replace("href=\"/", "href=\"" + host_port + path_dir);
                                tmp = tmp.Replace("src=\'/", "src=\'" + host_port + path_dir);
                                tmp = tmp.Replace("src=\"/", "src=\"" + host_port + path_dir);

                                tmp = tmp.Replace("href=\'$//", "href=\'//");
                                tmp = tmp.Replace("href=\"$//", "href=\"//");
                                tmp = tmp.Replace("src=\'$//", "src=\'//");
                                tmp = tmp.Replace("src=\"$//", "src=\"//");
                                content = Encoding.UTF8.GetBytes(tmp); 
                            }
                            catch (System.Net.WebException we)
                            {
                                res.StatusCode = (int)we.Status;
                            }
                            catch (Exception)
                            {
                                res.StatusCode = (int)HttpStatusCode.NotFound;
                            }
                        }
                        else
                        {
                            res.StatusCode = (int)HttpStatusCode.NotFound;
                        }
                    }
                    else
                    {
                        httpServer.GetFile(path);

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
                                        res.WriteContent(res.ContentEncoding.GetBytes(clone.ToString(Newtonsoft.Json.Formatting.None)));
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

                            string host_port = req.Url.Host + ":" + req.Url.Port.ToString();// host + ":" + extPort.ToString();
                            if (context.User != null)
                            {
                                string username = context.User.Identity.Name;
                                NetworkCredential cred = httpServer.UserCredentialsFinder(context.User.Identity);
                                string password = cred.Password;
                                host_port = username + ":" + password + "@" + host_port;
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
                    }

                    res.WriteContent(content);
                }
                catch (Exception ex)
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

            pingTimer = new System.Timers.Timer();
            pingTimer.Interval = 2000;
            pingTimer.Elapsed += (o, e) =>
            {
                try
                {
                    if (httpServer != null)
                    {
                        httpServer.WebSocketServices.BroadcastAsync(".", null);
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
                            String str = obj.ToString(Newtonsoft.Json.Formatting.None);
                            s.Sessions.BroadcastAsync(str, null);
                        }
                    }
                }
            }
        }
    }
}
