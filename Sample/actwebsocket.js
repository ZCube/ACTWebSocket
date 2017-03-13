var QueryString = function () 
{
	// This function is anonymous, is executed immediately and 
	// the return value is assigned to QueryString!
	var query_string = {};
	var query = window.location.search.substring(1);
	var vars = query.split("&");
	for (var i=0;i<vars.length;i++) 
	{
		var pair = vars[i].split("=");
			// If first entry with this name
		if (typeof query_string[pair[0]] === "undefined") 
		{
			query_string[pair[0]] = decodeURIComponent(pair[1]);
			// If second entry with this name
		} 
		else if (typeof query_string[pair[0]] === "string") 
		{
			var arr = [ query_string[pair[0]],decodeURIComponent(pair[1]) ];
			query_string[pair[0]] = arr;
			// If third or later entry with this name
		} 
		else 
		{
			query_string[pair[0]].push(decodeURIComponent(pair[1]));
		}
	} 
	return query_string;
}();

// webs
var host_port = QueryString["HOST_PORT"];

var host_port = "192.168.0.2///";
var wsUri = "@HOST_PORT@/MiniParse"; /*DO NOT EDIT THIS VALUE*/

while(host_port.endsWith('/')) {
	host_port = host_port.substring(0, host_port.length - 1);
}

if(wsUri.indexOf("//") == 0) {
	wsUri = wsUri.substring(2);
}

if(wsUri.indexOf("ws://") == 0 || wsUri.indexOf("wss://") == 0)
{
	if(host_port.indexOf("ws://") == 0 || host_port.indexOf("wss://") == 0)
	{
		wsUri = wsUri.replace(/ws:\/\/@HOST_PORT@/im, host_port);
		wsUri = wsUri.replace(/wss:\/\/@HOST_PORT@/im, host_port);
	}
	else
	{
		wsUri = wsUri.replace(/@HOST_PORT@/im, host_port);
	}
}
else
{
	if(host_port.indexOf("ws://") == 0 || host_port.indexOf("wss://") == 0)
	{
		wsUri = wsUri.replace(/@HOST_PORT@/im, host_port);
	}
	else
	{
		wsUri = "ws://" + wsUri.replace(/@HOST_PORT@/im, host_port);
	}
}

class ActWebsocketInterface
{
	constructor(uri, path = "MiniParse") {
		// url check
		var querySet = this.getQuerySet();
		if(typeof querySet["HOST_PORT"] != 'undefined')
		{
		    uri = querySet["HOST_PORT"] + path;
		}
		this.uri = uri;
		this.id = null;
		this.activate = false;
		
		var This = this;
		document.addEventListener('onBroadcastMessage', function(evt) {
			This.onBroadcastMessage(evt);
		});
		document.addEventListener('onRecvMessage', function(evt) {
			This.onRecvMessage(evt);
		});
		window.addEventListener('message', function (e) 
		{
			if (e.data.type === 'onBroadcastMessage') 
			{
				This.onBroadcastMessage(e.data);
			}
			if (e.data.type === 'onRecvMessage') 
			{
				This.onRecvMessage(e.data);
			}
		});
	}
	connect() {
		if(typeof this.websocket != "undefined" && this.websocket != null)
			this.close();
		this.activate = true;
		var This = this;
		this.websocket = new WebSocket(this.uri);
		this.websocket.onopen = function(evt) {This.onopen(evt);};
		this.websocket.onmessage = function(evt) {This.onmessage(evt);};
		this.websocket.onclose = function(evt) {This.onclose(evt);};
		this.websocket.onerror = function(evt) {This.onerror(evt);};
	}
	close() {
		this.activate = false;
		if(this.websocket != null && typeof this.websocket != "undefined")
		{
			this.websocket.close();
		}
	}
	onopen(evt) {
		// get id from useragent
		if(this.id != null && typeof this.id != "undefined")
		{
			this.set_id(this.id);
		}
		else
		{
			if(typeof overlayWindowId != "undefined")
			{
				this.set_id(overlayWindowId);
			}
			else
			{
				var r = new RegExp('[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}');
				var id = r.exec(navigator.userAgent);
				if(id != null && id.length == 1)
				{
					this.set_id(id[0]);
				}
			}
		}
	}
	onclose(evt) {
		this.websocket = null;
		if(this.activate)
		{
			var This = this;
			setTimeout(function() {This.connect();}, 5000);
		}
	}
	onmessage(evt) {
		if (evt.data == ".")
		{
			// ping pong
			this.websocket.send(".");
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
	}
	onerror(evt) {
		this.websocket.close();
		console.log(evt);
	}
	getQuerySet() {
		var querySet = {};
		// get query 
		var query = window.location.search.substring(1);
		var vars = query.split('&');
		for (var i = 0; i < vars.length; i++) {
			try{
				var pair = vars[i].split('=');
				querieSet[decodeURIComponent(pair[0])] = decodeURIComponent(pair[1]);
			}
			catch(e)
			{
			}
		}
		return querySet;
	}
	
	
	broadcast(type, msg){
		if(typeof overlayWindowId != 'undefined' && this.id != overlayWindowId)
		{
			this.set_id(overlayWindowId);
		}
		var obj = {};
		obj["type"] = "broadcast";
		obj["msgtype"] = type;
		obj["msg"] = msg;
		this.websocket.send(JSON.stringify(obj));
	}

	send(to, type, msg){
		if(typeof overlayWindowId != 'undefined' && this.id != overlayWindowId)
		{
			this.set_id(overlayWindowId);
		}
		var obj = {};
		obj["type"] = "send";
		obj["to"] = to;
		obj["msgtype"] = type;
		obj["msg"] = msg;
		this.websocket.send(JSON.stringify(obj));
	}
	
	overlayAPI(type, msg){
		var obj = {};
		if(typeof overlayWindowId != 'undefined' && this.id != overlayWindowId)
		{
			this.set_id(overlayWindowId);
		}
		obj["type"] = "overlayAPI";
		obj["to"] = overlayWindowId;
		obj["msgtype"] = type;
		obj["msg"] = msg;
		this.websocket.send(JSON.stringify(obj));
	}
	
	set_id(id){
		var obj = {};
		obj["type"] = "set_id";
		obj["id"] = id;
		this.id = overlayWindowId;
	this.websocket.send(JSON.stringify(obj));
	}

	onRecvMessage(e)
	{
	}
	
	onBroadcastMessage(e)
	{
	}
};

/*
class ActWebSocketImpl extends ActWebsocketInterface
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
		if(e.detail.msgtype == "CombatData")
		{
			document.dispatchEvent(new CustomEvent('onOverlayDataUpdate', { detail: e.detail.msg }));
		}
	}
};
*/