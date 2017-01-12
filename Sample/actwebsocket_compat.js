/* ACTWebSocket  Begin */

class WebSocketImpl extends ActWebsocketInterface
{
  constructor(uri, path = "MiniParse") {
      super(uri, path);
  }
  //send(to, type, msg)
  //broadcast(type, msg)
  onRecvMessage(e)
  {
  }

  onBroadcastMessage(e)
  {
      if (e.detail.msgtype == "CombatData")
      {
          document.dispatchEvent(new CustomEvent('onOverlayDataUpdate', { detail: e.detail.msg }));
      }
  }
};

var webs = null;
$(document).ready(function() {
  webs = new WebSocketImpl(wsUri);
  webs.connect();
});
if (document.addEventListener) {
  window.onbeforeunload = function() {
      webs.close();
  };
  window.addEventListener("unload", function() {
      webs.close();
  }, false);
}

/* ACTWebSocket  End */