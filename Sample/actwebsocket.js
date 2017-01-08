
class ActWebsocketInterface
{
	constructor(uri, path = "MiniParse") {
		// url check
		if(uri == undefined || uri == null )
		{
			querySet = getQuerySet();
			if(querySet["HOST_PORT"] != undefined)
			{
				uri = querySet["HOST_PORT"] + path;
			}
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
		if(this.websocket != undefined && this.websocket != null)
			close();
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
		if(this.websocket != null && this.websocket != undefined)
		{
			this.websocket.close();
		}
	}
	onopen(evt) {
		// get id from useragent
		if(this.id != null && this.id != undefined)
		{
			set_id(this.id);
		}
		else
		{
			var r = new RegExp('[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}');
			var id = r.exec(navigator.userAgent);
			if(id != null && id.length == 1)
			{
				set_id(id[0]);
				self.id = id;
			}
		}
	}
	onclose(evt) {
		this.websocket = null;
		if(this.activate)
		{
			setTimeout(this.connect, 5000);
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
	static getQuerySet() {
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
		var obj = {};
		obj["type"] = "broadcast";
		obj["msgtype"] = type;
		obj["msg"] = msg;
		this.websocket.send(JSON.stringify(obj));
	}

	send(to, type, msg){
		var obj = {};
		obj["type"] = "send";
		obj["to"] = to;
		obj["msgtype"] = type;
		obj["msg"] = msg;
		this.websocket.send(JSON.stringify(obj));
	}
	
	set_id(id){
		var obj = {};
		obj["type"] = "set_id";
		obj["id"] = id;
		this.websocket.send(JSON.stringify(obj));
	}

	onRecvMessage(e)
	{
	}
	
	onBroadcastMessage(e)
	{
	}
};

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
