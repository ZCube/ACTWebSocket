var language = {
	"new-url":{"ko":"주소 또는 파일", "ja-JP":"", "en":"URL or File"},
	"new-add":{"ko":"추가", "ja-JP":"", "en":"Add"},
	"start-option":{"ko":"시작 설정", "ja-JP":"", "en":"Start Option"},
	"autorun":{"ko":"ACT와 함께 시작", "ja-JP":"", "en":"Auto Running With ACT"},
	"localhost":{"ko":"이 PC만 사용", "ja-JP":"", "en":"Localhost Only"},
	"randurl":{"ko":"무작위 주소 사용", "ja-JP":"", "en":"Random URL"},
	"url":{"ko":"IP 주소", "ja-JP":"", "en":"URL"},
	"port":{"ko":"포트", "ja-JP":"", "en":"Port"},
	"server-status":{"ko":"서버 설정", "ja-JP":"", "en":"Server Option"},
	"miniparse":{"ko":"전투기록 사용", "ja-JP":"", "en":"Use Mini Parse"},
	"onloglineread":{"ko":"OnLogLineRead 사용", "ja-JP":"", "en":"Use OnLogLineRead"},
	"beforeloglineread":{"ko":"BeforeLogLineRead 사용", "ja-JP":"", "en":"Use BeforeLogLineRead"},
	"miniparseset":{"ko":"전투기록 설정", "ja-JP":"", "en":"Mini Parse Setting"},
	"miniparseSortkey":{"ko":"정렬 키", "ja-JP":"", "en":"Sort Key"},
	"miniparseSorttype":{"ko":"정렬 방법", "ja-JP":"", "en":"Sort Type"},
	"opt-none":{"ko":"없음", "ja-JP":"", "en":"None"},
	"opt-sa":{"ko":"문자-오름차순", "ja-JP":"", "en":"StringAscending"},
	"opt-sd":{"ko":"문자-내림차순", "ja-JP":"", "en":"StringDescending"},
	"opt-na":{"ko":"숫자-오름차순", "ja-JP":"", "en":"NumericAscending"},
	"opt-nd":{"ko":"숫자-내림차순", "ja-JP":"", "en":"NumericDescending"},
	"overlays":{"ko":"오버레이 목록", "ja-JP":"", "en":"Overlays"},
	"overlay-title":{"ko":"제목", "ja-JP":"", "en":"Title"},
	"overlay-url":{"ko":"주소", "ja-JP":"", "en":"URL"},
	"overlay-open":{"ko":"열기", "ja-JP":"", "en":"Open"},
	"overlay-opacity":{"ko":"투명도", "ja-JP":"", "en":"Opacity"},
	"overlay-zoom":{"ko":"확축", "ja-JP":"", "en":"Zoom"},
	"overlay-fps":{"ko":"프레임", "ja-JP":"", "en":"FPS"},
	"overlay-clickthru":{"ko":"클릭 통과 사용", "ja-JP":"", "en":"Enable ClickThru"},
	"overlay-nonfocus":{"ko":"포커스를 주지 않음", "ja-JP":"", "en":"Enable Non focusing"},
	"overlay-dragging":{"ko":"화면 내 드래그 사용", "ja-JP":"", "en":"Enable Dragging"},
	"overlay-dragndrop":{"ko":"드래그 화면 이동 사용", "ja-JP":"", "en":"Enable Drag &amp; Drop"},
	"overlay-hide":{"ko":"오버레이 숨김", "ja-JP":"", "en":"Hide Overlay"},
	"overlay-resize":{"ko":"크기 조절 사용", "ja-JP":"", "en":"Enable Resize"},
	"overlay-x":{"ko":"X좌표", "ja-JP":"", "en":"pos X"},
	"overlay-y":{"ko":"Y좌표", "ja-JP":"", "en":"pos Y"},
	"overlay-width":{"ko":"폭", "ja-JP":"", "en":"Width"},
	"overlay-height":{"ko":"높이", "ja-JP":"", "en":"Height"},
	"overlay-save":{"ko":"저장", "ja-JP":"", "en":"Save"},
	"overlay-delete":{"ko":"삭제", "ja-JP":"", "en":"Delete"},
	"overlay-reload":{"ko":"리로드", "ja-JP":"", "en":"Reload"},
	"overlay-opendevtool":{"ko":"개발자 도구 열기", "ja":"","en":"Open DevTool"},
	"serverstatus":
	{
		"ko":
		{
			"on":"서버 상태 : 작동중 <span style='color:rgba(255,255,255,.5);'> (멈추려면 클릭)</span>",
			"off":"서버 상태 : 멈춤 <span style='color:rgba(255,255,255,.5);'> (시작하려면 클릭)</span>",
		},
		"ja":
		{
			"on":"Server Status : Running <span style='color:rgba(255,255,255,.5);'> (Stop at Click)</span>",
			"off":"Server Status : Stoped <span style='color:rgba(255,255,255,.5);'> (Run at Click)</span>",
		},
		"en":
		{
			"on":"Server Status : Running <span style='color:rgba(255,255,255,.5);'> (Stop at Click)</span>",
			"off":"Server Status : Stoped <span style='color:rgba(255,255,255,.5);'> (Run at Click)</span>",
		}
	}
};

function closeMenu()
{
	$(".wideswap").attr("data-checked", "true");
	$(".left").css({"left":"-261px", "box-shadow":"0px 0px 0px transparent"});
	$(".wideswap").css({"left":"8px"});
}

function api_overlaywindow_set(json, callback)
{
	websocket.send(
		JSON.stringify({
			cmd: "set",
			value: json
		})
	);
}

function api_overlaywindow_set_all(json)
{
	websocket.send(
		JSON.stringify({
			cmd: "set_all",
			value: json
		})
	);
}

function api_overlaywindow_close_all()
{
	websocket.send(
		JSON.stringify({
			cmd: "close_all"
		})
	);
}

function api_overlaywindow_get_all()
{
	websocket.send(
		JSON.stringify({
			cmd: "get_all"
		})
	);
}

function api_overlaywindow_close(json)
{
	websocket.send(
		JSON.stringify({
			cmd: "close",
			value: json
		})
	);
}

var wsUri = "ws://localhost:9991/";
var pageActive = true;
var websocket = null;
function connectWebSocket(uri)
{
	websocket = new WebSocket(uri);
	websocket.onopen = function(evt) {
		$(".list").empty();
		api_overlaywindow_get_all();
		// //api_overlaywindow_close_all();
		// var count = $(".list div").length;
		// var objs = {};
		// for(var i = 0; i<count;++i)
		// {
			// var obj = divToJSON(i)
			// objs[obj["id"]] = obj;
		// }
		// api_overlaywindow_set_all(objs);
	}
	websocket.onmessage = function(evt) {
		if (evt.data == ".")
		{
			// ping pong
			websocket.send(".");
		}
		else
		{
			document.dispatchEvent(new CustomEvent('onOverlaySettingChanged', { detail: evt.data }));
		}
	};

	//websocket.onopen = function(evt) { };
	websocket.onclose = function(evt) { 
		//$(".list").empty();
		// 접속 불가 메세지 필요.
		setTimeout(function(){connectWebSocket(uri)}, 5000);
	};
	websocket.onerror = function(evt) {
		websocket.close();
	};
}    

function disconnectWebSocket(){
	pageActive = false;
	websocket.close();
};


$(document).ready(function() {
	pageActive = true;
	connectWebSocket(wsUri);
});

if (document.addEventListener) {
		window.onbeforeunload = function() {
				disconnectWebSocket();
		};
		window.addEventListener("unload", function() {
				disconnectWebSocket();
		}, false);
}
document.addEventListener('onOverlaySettingChanged', onOverlaySettingChanged);
window.addEventListener('message', function (e) 
{
	if (e.data.type === 'onOverlaySettingChanged') 
	{
		onOverlaySettingChanged(e.data);
	}
});
// 변수 이름 변환 부분 추후 정리.
var savedvar = [
	"id",
	"url",
	"opacity",
	"zoom",
	"fps",
	"clickthru",
	"nonfocus",
	"dragging",
	"dragndrop",
	"hide",
	"resize",
	"x",
	"y",
	"width",
	"height"
];
var nativevar = [
	"id",
	"url",
	"opacity",
	"zoom",
	"fps",
	"Transparent",
	"NoActivate",
	"useDragFilter",
	"useDragMove",
	"hide",
	"useResizeGrip",
	"x",
	"y",
	"width",
	"height"
];
var floatvar = [
	"opacity",
	"zoom",
	];
var intvar = [
	"x",
	"y",
	"width",
	"height",
	"fps",
];
nameNativeToJSMap = {};
nameJSToNativeMap = {};
for(i=0;i<savedvar.length;++i)
{
	nameJSToNativeMap[savedvar[i]] = nativevar[i];
	nameNativeToJSMap[nativevar[i]] = savedvar[i];
}

function JSONToDivPos(index, obj)
{
	var selectedIndex = parseInt($(".list").attr("data-selected-index"));
	$($(".list div")[index]).find("span")[0].innerText = obj["title"];
	for(var i in intvar)
	{
		obj[intvar[i]] = parseFloat(obj[intvar[i]]);
	}
	for(var i in intvar)
	{
		if($("*[data-flag=overlay-"+intvar[i]+"]").is("[data-checked]"))
		{
			$($(".list div")[index]).attr("data-"+intvar[i], obj[nameJSToNativeMap[intvar[i]]])
			if(index == selectedIndex)
			{
				$("*[data-flag=overlay-"+intvar[i]+"]").attr("data-checked", obj[nameJSToNativeMap[intvar[i]]]);
			}
			//$($(".list div")[index]).attr("data-"+savedvar[i], $("*[data-flag=overlay-"+savedvar[i]+"]").attr("data-checked")=="true"?"true":"false");
		}
		else
		{
			$($(".list div")[index]).attr("data-"+intvar[i], obj[nameJSToNativeMap[intvar[i]]]);
			if(index == selectedIndex)
			{
				$("*[data-flag=overlay-"+intvar[i]+"]").val(obj[nameJSToNativeMap[intvar[i]]]);
			}
			//$($(".list div")[index]).attr("data-"+savedvar[i], $("*[data-flag=overlay-"+savedvar[i]+"]").val());
		}
	}
}

function JSONToDiv(index, obj)
{
	var selectedIndex = parseInt($(".list").attr("data-selected-index"));
	$($(".list div")[index]).find("span")[0].innerText = obj["title"];
	if(index == selectedIndex)
	{
		$("*[data-flag=overlay-title]").val(obj["title"]);
		$("*[data-flag=overlay-id]").val(obj["id"]);
		$("*[data-flag=overlay-url]").val(obj["url"]);
	}
	$($(".list div")[index]).attr("data-id", obj["id"]);
	$($(".list div")[index]).attr("data-url", obj["url"]);
	//obj["title"] = $($(".list div")[index]).find("span").html($("*[data-flag=overlay-title]").val());
	for(var i in floatvar)
	{
		obj[floatvar[i]] = parseFloat(obj[floatvar[i]]) * 100.0;
	}
	for(var i in intvar)
	{
		obj[intvar[i]] = parseFloat(obj[intvar[i]]);
	}
	for(var i in savedvar)
	{
		if($("*[data-flag=overlay-"+savedvar[i]+"]").is("[data-checked]"))
		{
			$($(".list div")[index]).attr("data-"+savedvar[i], obj[nameJSToNativeMap[savedvar[i]]])
			if(index == selectedIndex)
			{
				$("*[data-flag=overlay-"+savedvar[i]+"]").attr("data-checked", obj[nameJSToNativeMap[savedvar[i]]]);
			}
			//$($(".list div")[index]).attr("data-"+savedvar[i], $("*[data-flag=overlay-"+savedvar[i]+"]").attr("data-checked")=="true"?"true":"false");
		}
		else
		{
			$($(".list div")[index]).attr("data-"+savedvar[i], obj[nameJSToNativeMap[savedvar[i]]]);
			if(index == selectedIndex)
			{
				$("*[data-flag=overlay-"+savedvar[i]+"]").val(obj[nameJSToNativeMap[savedvar[i]]]);
			}
			//$($(".list div")[index]).attr("data-"+savedvar[i], $("*[data-flag=overlay-"+savedvar[i]+"]").val());
		}
	}
}
// overlay window의 json으로 정리.
function divToJSON(index)
{
	var selectedIndex = parseInt($(".list").attr("data-selected-index"));
	var count = $(".list div").length;
	var obj = null;
	if(index < count && index >= 0)
	{
		obj = {};
		obj["title"] = $($(".list div")[index]).find("span")[0].innerText;
		obj["id"] = $($(".list div")[index]).attr("data-id");
		obj["url"] = $($(".list div")[index]).attr("data-url");
		//obj["title"] = $($(".list div")[index]).find("span").html($("*[data-flag=overlay-title]").val());
		for(var i in savedvar)
		{
			if($("*[data-flag=overlay-"+savedvar[i]+"]").is("[data-checked]"))
			{
				if(index == selectedIndex)
				{
					$($(".list div")[index]).attr("data-"+savedvar[i], $("*[data-flag=overlay-"+savedvar[i]+"]").attr("data-checked")=="true"?"true":"false");
				}
				obj[nameJSToNativeMap[savedvar[i]]] = $($(".list div")[index]).attr("data-"+savedvar[i])=="true"?true:false;
			}
			else
			{
				if(index == selectedIndex)
				{
					$($(".list div")[index]).attr("data-"+savedvar[i], $("*[data-flag=overlay-"+savedvar[i]+"]").val());
				}
				obj[nameJSToNativeMap[savedvar[i]]] = $($(".list div")[index]).attr("data-"+savedvar[i]);
			}
		}
		for(var i in floatvar)
		{
			obj[floatvar[i]] = parseFloat(obj[floatvar[i]]) / 100.0;
		}
		for(var i in intvar)
		{
			obj[intvar[i]] = parseFloat(obj[intvar[i]]);
		}
	}
	return obj;
}
function addHostname(hostname)
{
	var html ="<option value=\""+hostname+"\">"+hostname+"</div>";
	$("*[data-flag=hostnames]").append(html);
}
function Hex2Str(hexx) {
    var hex = hexx.toString();//force conversion
    var str = '';
    for (var i = 0; i < hex.length; i += 2)
        str += String.fromCharCode(parseInt(hex.substr(i, 2), 16));
    return str;
}

function update_all(vals)
{
	for(var key in vals)
	{
		var val = vals[key];
		update(val);
	}
}

function update(obj)
{
	try{
		var count = $(".list div").length;
		var find = false;
		for(var index=0;index<count;++index)
		{
			if (obj["id"] == $($(".list div")[index]).attr("data-id"))
			{
				JSONToDiv(index, obj);
				find = true;
				break;
			}
		}
		if(!find)
		{
			var html = "<div ";
			html+='data-url="'+ obj["url"] +'"'
				+' data-id="' + obj["id"] +'"'
				+' data-opacity="' + obj["opacity"]*100 +'"'
				+' data-zoom="' + obj["zoom"]*100 +'"'
				+' data-fps="' + obj["fps"] +'"'
				+' data-x="' + obj["x"] +'"'
				+' data-y="' + obj["y"] +'"'
				+' data-width="' + obj["width"] +'"'
				+' data-height="' + obj["height"] +'"'
				+' data-clickthru="' + (obj["Transparent"]?'true':'false') +'"'
				+' data-nonfocus="' + (obj["NoActivate"]?'true':'false') +'"'
				+' data-dragging="' + (obj["useDragFilter"]?'true':'false') +'"'
				+' data-dragndrop="' + (obj["useDragMove"]?'true':'false') +'"'
				+' data-hide="' + (obj["hide"]?'true':'false') +'"'
				+' data-resize="' + (obj["useResizeGrip"]?'true':'false') +'"'
				;
			
			html+="><span>"+obj["title"]+"</span></div>";
			$(".list").append(html);
			actAttach();
		}
	}
	catch(e)
	{
		alert(e);
	}
}
function update_pos(obj)
{
	try{
		var count = $(".list div").length;
		var find = false;
		for(var index=0;index<count;++index)
		{
			if (obj["id"] == $($(".list div")[index]).attr("data-id"))
			{
				JSONToDivPos(index, obj);
				find = true;
				break;
			}
		}
	}
	catch(e)
	{
		alert(e);
	}
}

function onOverlaySettingChanged(e)
{
	try{
		var obj = JSON.parse(e.detail);
		var cmd = obj["cmd"];
		if(cmd == "update")
		{
			var val = obj["value"];
			update_pos(val);
		}
		if(cmd == "get" || cmd == "set")
		{
			var val = obj["value"];
			update(val);
		}
		if(cmd == "close_all")
		{
			$(".list").empty();
		}
		if(cmd == "get_all")
		{
			var vals = obj["value"];
			for(var key in vals)
			{
				var val = vals[key];
				update(val);
			}
			
		}
	}
	catch(e)
	{
		alert(e);
	}
}
function settingsTojson()
{
	var count = $(".list div").length;
	
	$(".list div span")[0].innerText
	var savedvar = [
		"url",
		"opacity",
		"zoom",
		"fps",
		"clickthru",
		"nonfocus",
		"dragging",
		"dragndrop",
		"hide",
		"resize",
		"x",
		"y",
		"width",
		"height"
	];
	var savedvar = [
		"url",
		"opacity",
		"zoom",
		"fps",
		"Transparent",
		"NoActivate",
		"useDragFilter",
		"useDragMove",
		"hide",
		"useResizeGrip",
		"x",
		"y",
		"width",
		"height"
	];
	var divs = $(".list div");
	
}

function saveOption()
{
	var savedvar = [
		"url",
		"opacity",
		"zoom",
		"fps",
		"clickthru",
		"nonfocus",
		"dragging",
		"dragndrop",
		"hide",
		"resize",
		"x",
		"y",
		"width",
		"height"
	];
	var index = parseInt($(".list").attr("data-selected-index"));
	$($(".list div")[index]).find("span").html($("*[data-flag=overlay-title]").val());
	for(var i in savedvar)
	{
		if($("*[data-flag=overlay-"+savedvar[i]+"]").is("[data-checked]"))
		{
			$($(".list div")[index]).attr("data-"+savedvar[i], $("*[data-flag=overlay-"+savedvar[i]+"]").attr("data-checked")=="true"?"true":"false");
		}
		else
		{
			$($(".list div")[index]).attr("data-"+savedvar[i], $("*[data-flag=overlay-"+savedvar[i]+"]").val());
		}
	}
}

function forceChange(obj)
{
	$(obj).attr("data-checked", $(obj).attr("data-checked")=="true"?"false":"true");
	if($(obj).is("[data-status-off]") && $(obj).attr("data-checked") == "false")
	{
		$(obj).html($(obj).attr("data-status-off"));
		if($(obj).is("[data-status-off-css]"))
		{
			$(obj).css(JSON.parse($(obj).attr("data-status-off-css").replace(/'/ig, "\"")));
		}
	}
	else if($(obj).is("[data-status-on]") && $(obj).attr("data-checked") == "true")
	{
		$(obj).html($(obj).attr("data-status-on"));
		if($(obj).is("[data-status-on-css]"))
		{
			$(obj).css(JSON.parse($(obj).attr("data-status-on-css").replace(/'/ig, "\"")));
		}
	}

	if($(obj).is("[data-disableobject]"))
	{
		if($(obj).attr("data-checked")=="true")
		{
			var obj = JSON.parse($(obj).attr("data-disableobject").replace(/'/ig, "\""));
			for(var i in obj.objects)
			{
				$("*[data-id=\""+obj.objects[i]+"\"]").attr("disabled","disabled");
				$("*[data-flag=\""+obj.objects[i]+"\"]").attr("disabled","disabled");
			}
		}
		else
		{
			var obj = JSON.parse($(obj).attr("data-disableobject").replace(/'/ig, "\""));
			for(var i in obj.objects)
			{
				$("*[data-id=\""+obj.objects[i]+"\"]").removeAttr("disabled");
				$("*[data-flag=\""+obj.objects[i]+"\"]").removeAttr("disabled");
			}
		}
	}
}

$(document).ready(function(){
	{
		$("*[data-flag=hostnames]").change(function() {
			$("*[data-flag=hostname]").val($("*[data-flag=hostnames]").val());
		});
	}
	try{
		$(".list").empty();
	}
	catch(e)
	{
		alert(e);
	}
	setTimeout(function(){$(".left").css("transition", ".2s"); $(".wideswap").css("transition", ".2s");},1000)
	
	// 초기 로드 시 선택된 Overlay가 없으므로 disable 시킨다.
	$(".setting *[data-flag]").attr("disabled", "disabled");
	$(".setting *[data-id]").attr("disabled", "disabled");
	
	$(".listhead .newbtn").click(function(){
		// api_skin_get_list(function(data){
			// console.log(data.skins);
		// });
		if($(".newwindow").css("display") == "none")
		{
			$(".disableall").show();
			$(".newwindow").show();
		}
		else
		{
			$(".disableall").hide();
			$(".newwindow").hide();
		}
	});

	$("*[data-flag=new-add]").click(function(){
		try
		{
			var url = $("*[data-flag=new-url]").val();
			var title = $("*[data-flag=new-url]").val();
			var obj = {
				"Transparent": false,
				"NoActivate": false,
				"hide": false,
				"useDragFilter": true,
				"useDragMove": true,
				"useResizeGrip": true,
				"opacity": 1.0,
				"zoom": 1.0,
				"url": url,
				"title": title,
				"fps": 30.0,
				"x": 0,
				"y": 0,
				"width": 300,
				"height": 300
			};
			api_overlaywindow_set(obj);
			$("*[data-flag=new-url]").val("");
			$(".disableall").hide();
			$(".newwindow").hide();
		}
		catch(ex)
		{
			alert(ex);
		}
	});

	$("*[data-flag=overlay-open]").click(function(){
		var index = parseInt($(".list").attr("data-selected-index"));
		var obj = divToJSON(index);
		if(obj != null)
		{
			api_overlaywindow_set(obj);
		}
	});
	$("*[data-flag=overlay-save]").click(function(){
		api_overlaywindow_get_all();
	});
	$("*[data-flag=overlay-delete]").click(function(){
		var index = parseInt($(".list").attr("data-selected-index"));
		var obj = divToJSON(index);
		api_overlaywindow_close(obj);
		$(".setting *[data-flag]").attr("disabled", "disabled");
		$(".setting *[data-id]").attr("disabled", "disabled");
		$(".setting *[data-flag]").each(function(){
			if($(this).is("[data-checked]"))
			{
				$(this).attr("data-checked", "false");
			}
			else
			{
				$(this).val("");
			}
		});
		$($(".list div")[parseInt($(".list").attr("data-selected-index"))]).remove();
		$(".list").attr("data-selected-index", "-1");
	});

	$("*[data-flag]").change(function(){
		saveOption();
	});

	$("*[data-checked]").click(function(){
		if($(this).is("[disabled]")) return;
		if($(this).parent().is("[disabled]")) return;

		$(this).attr("data-checked", $(this).attr("data-checked")=="true"?"false":"true");
		if($(this).is("[data-status-off]") && $(this).attr("data-checked") == "false")
		{
			$(this).html($(this).attr("data-status-off"));
			if($(this).is("[data-status-off-css]"))
			{
				$(this).css(JSON.parse($(this).attr("data-status-off-css").replace(/'/ig, "\"")));
			}
		}
		else if($(this).is("[data-status-on]") && $(this).attr("data-checked") == "true")
		{
			$(this).html($(this).attr("data-status-on"));
			if($(this).is("[data-status-on-css]"))
			{
				$(this).css(JSON.parse($(this).attr("data-status-on-css").replace(/'/ig, "\"")));
			}
		}

		if($(this).is("[data-disableobject]"))
		{
			if($(this).attr("data-checked")=="true")
			{
				var obj = JSON.parse($(this).attr("data-disableobject").replace(/'/ig, "\""));
				for(var i in obj.objects)
				{
					$("*[data-id=\""+obj.objects[i]+"\"]").attr("disabled","disabled");
					$("*[data-flag=\""+obj.objects[i]+"\"]").attr("disabled","disabled");
				}
			}
			else
			{
				var obj = JSON.parse($(this).attr("data-disableobject").replace(/'/ig, "\""));
				for(var i in obj.objects)
				{
					$("*[data-id=\""+obj.objects[i]+"\"]").removeAttr("disabled");
					$("*[data-flag=\""+obj.objects[i]+"\"]").removeAttr("disabled");
				}
			}
		}

		var index = parseInt($(".list").attr("data-selected-index"));
		var obj = divToJSON(index);
		if(obj != null)
		{
			api_overlaywindow_set(obj);
		}

		if($(this).parent().hasClass("setting"))
		{
			saveOption();
		}
	});

	$("input[type=range]").change(function(){
		$(this).parent().parent().find(".valueview").html($(this).val()+$(this).attr("data-str"));
		var index = parseInt($(".list").attr("data-selected-index"));
		var obj = divToJSON(index);
		api_overlaywindow_set(obj);
	});

	actAttach();
	var userLang = navigator.language || navigator.userLanguage;

	userLang = userLang+"-";
	$("*[data-id]").each(function(){
		try
		{
			$("*[data-id="+$(this).attr("data-id")+"]").html(language[$(this).attr("data-id")][userLang.substr(0, userLang.indexOf("-"))]);
		}
		catch(ex)
		{

		}
	});

	$("*[data-id-ext]").each(function(){
		$(this).attr("data-status-on", language[$(this).attr("data-id-ext")][userLang.substr(0, userLang.indexOf("-"))].on);
		$(this).attr("data-status-off", language[$(this).attr("data-id-ext")][userLang.substr(0, userLang.indexOf("-"))].off);

		if($(this).attr("data-checked"))
			$(this).html(language[$(this).attr("data-id-ext")][userLang.substr(0, userLang.indexOf("-"))].off);
		else
			$(this).html(language[$(this).attr("data-id-ext")][userLang.substr(0, userLang.indexOf("-"))].on);
	});

	$(".wideswap").click(function(){
		if($(this).attr("data-checked") == "true")
		{
			$(".left").css({"left":"-261px", "box-shadow":"0px 0px 0px transparent"});
			$(".wideswap").css({"left":"8px"});
			$(".disablemenu").hide();
		}
		else
		{
			$(".left").css({"left":"0px", "box-shadow":"0px 0px 10px rgba(0,0,0,0.5)"});
			$(".wideswap").css({"left":"228px"});
			$(".disablemenu").show();
		}
		localStorage.setItem("mordernizer_collapse", $(this).attr("data-checked"));
	});
});

function actAttach()
{
	// 파일 선택 목록, list 아이템이 추가될 때 한번씩 호출하면 되는 함수
	$(".lists>div>.select").click(function(){
		$("*[data-flag=new-url]").val($(".lists").attr("data-parent")+$(this).parent().text());
		$("*[data-flag=new-url]").focus();
	});

	$(".list div").click(function(){
		$(".list div").each(function(){$(this).removeClass("selected");});
		$(this).addClass("selected");
		var i = 0;
		$(".list div").each(function(){
			if($(this).hasClass("selected")) $(".list").attr("data-selected-index", i);
			i++;
		});
		$("*[data-id]").removeAttr("disabled");
		$("*[data-flag]").removeAttr("disabled");

		$(".setting *[data-flag=overlay-title]").val($(this).find("span").html());
		$.each(this.attributes, function(){
			if(this.specified)
			{
				if($(".setting *[data-flag=\""+this.name.replace("data", "overlay")+"\"]").is("[data-checked]"))
				{
					$(".setting *[data-flag=\""+this.name.replace("data", "overlay")+"\"]").attr("data-checked", this.value);
				}
				else
				{
					$(".setting *[data-flag=\""+this.name.replace("data", "overlay")+"\"]").val(this.value.replace(/%20/ig, " "));
					$(".setting *[data-flag=\""+this.name.replace("data", "overlay")+"\"]").parent().parent().find(".valueview").html($(".setting *[data-flag=\""+this.name.replace("data", "overlay")+"\"]").val()+$(".setting *[data-flag=\""+this.name.replace("data", "overlay")+"\"]").attr("data-str"));
				}
			}
		});
	});
}