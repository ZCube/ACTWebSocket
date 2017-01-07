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
            public WebSocketCommunicateBehavior()
            {
                overlayAPI = ACTWebSocketCore.overlayAPI;
                id = Guid.NewGuid().ToString();
            }
            protected override async void OnOpen()
            {
                base.OnOpen();
            }
            protected override void OnClose(CloseEventArgs e)
            {
                base.OnClose(e);
            }

            public void Broadcast(String from, String type, JToken message)
            {
                JObject obj = new JObject();
                obj["type"] = "broadcast";
                obj["from"] = from;
                obj["msgtype"] = type;
                obj["msg"] = message;
                String str = obj.ToString();
                foreach (WebSocketCommunicateBehavior s in Sessions.Sessions)
                {
                    if (s.id != from)
                    {
                        s.Send(str);
                    }
                }
            }

            public void Send(String from, String to, String type, JToken message)
            {
                JObject obj = new JObject();
                obj["type"] = "send";
                obj["msgtype"] = type;
                obj["from"] = from;
                obj["to"] = to;
                obj["msg"] = message;
                String str = obj.ToString();
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
                            if (e.Data == ".")
                                return;
                            try
                            {
                                JObject o = JObject.Parse(e.Data);
                                String type = o["type"].ToString();
                                String from = id;
                                if (type == "broadcast")
                                {
                                    JToken msg = o["msg"].ToString();
                                    String msgtype = o["msgtype"].ToString();
                                    Broadcast(from, msgtype, msg);
                                }
                                if (type == "send")
                                {
                                    JToken msg = o["msg"];
                                    String to = o["to"].ToString();
                                    String msgtype = o["msgtype"].ToString();
                                    Send(from, to, msgtype, msg);
                                }
                                if (type == "set_id")
                                {
                                    String before = id;
                                    id = o["id"].ToString();
                                    String to = id;
                                    JObject t = new JObject();
                                    t["before"] = before;
                                    t["after"] = to;
                                    Broadcast(id, "set_id", t);
                                }
                                overlayAPI.OnMessage(this, type, from, o);
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
