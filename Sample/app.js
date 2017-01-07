// ACTWebSocket 적용
function connectWebSocket(uri)
{
	websocket = new WebSocket(uri);

	websocket.onmessage = function(evt) {
		if (evt.data == ".")
		{
			// ping pong
			websocket.send(".");
		}
		else
		{
			try{
				var obj = JSON.parse(evt.data);
				var type = obj["type"];
				if(type == "broadcast")
				{
					var from = obj["from"];
					var type = obj["msgtype"];
					var msg = obj["msg"];
					document.dispatchEvent(new CustomEvent('onBroadcastMessage', { detail: obj }));
				}
				if(type == "send")
				{
					var from = obj["from"];
					var type = obj["msgtype"];
					var msg = obj["msg"];
					document.dispatchEvent(new CustomEvent('onRecvMessage', { detail: obj }));
				}
				if(type == "set_id")
				{
					//document.dispatchEvent(new CustomEvent('onIdChanged', { detail: obj }));
				}
			}
			catch(e)
			{
			}
		}
	};

	websocket.onclose = function(evt) 
	{ 
		setTimeout(function(){connectWebSocket(uri)}, 5000);
	};

	websocket.onerror = function(evt) 
	{
		websocket.close();
	};
}    


$(document).ready(function() {
	connectWebSocket(wsUri);
});

function broadcast(type, msg){
	var obj = {};
	obj["type"] = "broadcast";
	obj["msgtype"] = type;
	obj["msg"] = msg;
	websocket.send(JSON.stringify(obj));
};

function send(to, type, msg){
	var obj = {};
	obj["type"] = "send";
	obj["to"] = to;
	obj["msgtype"] = type;
	obj["msg"] = msg;
	websocket.send(JSON.stringify(obj));
};
function set_id(id){
	var obj = {};
	obj["type"] = "set_id";
	obj["id"] = id;
	websocket.send(JSON.stringify(obj));
};

document.addEventListener('onBroadcastMessage', onBroadcastMessage);
document.addEventListener('onRecvMessage', onRecvMessage);
window.addEventListener('message', function (e) 
{
	if (e.data.type === 'onBroadcastMessage') 
	{
		onBroadcastMessage(e.data);
	}
	if (e.data.type === 'onRecvMessage') 
	{
		onRecvMessage(e.data);
	}
});
function onRecvMessage(e)
{
}
function onBroadcastMessage(e)
{
	if(e.detail.msgtype == "CombatData")
	{
		document.dispatchEvent(new CustomEvent('onOverlayDataUpdate', { detail: e.detail.msg }));
	}
}
