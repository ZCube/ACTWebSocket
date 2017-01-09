using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace ACTWebSocket_Plugin
{
    public partial class ACTWebSocketCore
    {
        public class WebSocketCommunicateBehavior : WebSocketBehavior
        {
            FFXIV_OverlayAPI overlayAPI;
            public String id = null;
            private bool isfirst = true;
            public Dictionary<string, Action<WebSocketCommunicateBehavior, JObject>> handle = new Dictionary<string, Action<WebSocketCommunicateBehavior, JObject>>();
            public WebSocketCommunicateBehavior()
            {
                overlayAPI = ACTWebSocketCore.overlayAPI;
                id = Guid.NewGuid().ToString();
                handle["broadcast"] = (session, o) => {
                    String from = id;
                    JToken msg = o["msg"].ToString();
                    String msgtype = o["msgtype"].ToString();
                    Broadcast(from, msgtype, msg);
                };
                handle["send"] = (session, o) => {
                    String from = id;
                    JToken msg = o["msg"].ToString();
                    String msgtype = o["msgtype"].ToString();
                    Broadcast(from, msgtype, msg);
                };
                handle["set_id"] = (session, o) => {
                    String before = id;
                    id = o["id"].ToString();
                    String to = id;
                    JObject t = new JObject();
                    t["before"] = before;
                    t["after"] = to;
                    Broadcast(id, "set_id", t);
                };
                overlayAPI.InstallMessageHandle(ref handle);
            }
            protected override async void OnOpen()
            {
                base.OnOpen();
                overlayAPI.OnOpen(id, this);
            }

            protected override void OnClose(CloseEventArgs e)
            {
                base.OnClose(e);
            }

            public static JObject GenMessage(String type, String msgtype, JToken message)
            {
                JObject obj = new JObject();
                obj["type"] = type;
                obj["msgtype"] = msgtype;
                obj["msg"] = message;
                return obj;
            }

            public static JObject GenMessage(String type, String from, String msgtype, JToken message)
            {
                JObject obj = new JObject();
                obj["type"] = type;
                obj["msgtype"] = msgtype;
                obj["from"] = from;
                obj["msg"] = message;
                return obj;
            }

            public static JObject GenMessage(String type, String from, String to, String msgtype, JToken message)
            {
                JObject obj = new JObject();
                obj["type"] = type;
                obj["msgtype"] = msgtype;
                obj["from"] = from;
                obj["to"] = to;
                obj["msg"] = message;
                return obj;
            }

            public void Broadcast(String from, String msgtype, JToken message)
            {
                JObject obj = new JObject();
                String str = GenMessage("broadcast", from, msgtype, message).ToString();
                foreach (WebSocketCommunicateBehavior s in Sessions.Sessions)
                {
                    if (s.id != from)
                    {
                        s.Send(str);
                    }
                }
            }

            public void Broadcast(String msgtype, JToken message)
            {
                String from = id;
                String str = GenMessage("broadcast", from, msgtype, message).ToString();
                foreach (WebSocketCommunicateBehavior s in Sessions.Sessions)
                {
                    if (s.id != from)
                    {
                        s.Send(str);
                    }
                }
            }

            public void Send(String to, String msgtype, JToken message)
            {
                String from = id;
                String str = GenMessage("send", from, to, msgtype, message).ToString();
                foreach (WebSocketCommunicateBehavior s in Sessions.Sessions)
                {
                    if (s.id == to)
                    {
                        s.Send(str);
                        break;
                    }
                }
            }

            public void Send(String from, String to, String msgtype, JToken message)
            {
                JObject obj = new JObject();
                String str = GenMessage("send", from, to, msgtype, message).ToString();
                foreach (WebSocketCommunicateBehavior s in Sessions.Sessions)
                {
                    if (s.id == to)
                    {
                        s.Send(str);
                        break;
                    }
                }
            }

            protected override void OnMessage(MessageEventArgs e)
            {
                switch (e.Type)
                {
                    case Opcode.Text:
                        {
                            if (e.Data.StartsWith("."))
                            {
                                return;
                            }

                            try
                            {
                                JObject o = JObject.Parse(e.Data);
                                String type = o["type"].ToString();
                                try
                                {
                                    handle[type](this, o);
                                }
                                catch (Exception ex)
                                {
                                    Send(this.id, this.id, "Error", ex.Message);
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        break;
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
    }
}
