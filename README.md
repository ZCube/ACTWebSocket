# ACTWebSocket Plugin

WebSocket Plugin for Advanced Combat Tracker v3

## 기능 ##

1. 간이 웹서버.
   플러그인이 포함된 디렉토리를 웹에서 받을 수 있도록 전달

2. OverlayPlugin의 MiniParse에서 사용되는 데이터 포맷으로 웹소켓을 사용해서 전달.
 
3. ACT에서 입력받는 로그를 웹소켓을 사용해서 전달.


## 웹서버에서 적용되는 변수들 ##

* HOST_PORT

  주소와 포트.
  localhost:10501
  
  @HOST_PORT@ 문자열을 치환한다.


## HTML에 적용하는 샘플 코드 ##

1. ACT LogLine을 OverlayPlugin의 SendMessage로 보내는 것을 웹소켓으로 구현한 예
``` javascript
  ...
  var wsUri = "ws://@HOST_PORT@/BeforeLogLineRead";
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
        document.dispatchEvent(new CustomEvent('onBroadcastMessageReceive', { detail: evt.data }));
      }
    };

    //websocket.onopen = function(evt) { };
    websocket.onclose = function(evt) { 
      setTimeout(function(){connectWebSocket(uri)}, 5000);
    };
    websocket.onerror = function(evt) {
      websocket.close();
    };
  }    
  connectWebSocket(wsUri);
  ...
```    

2. Overlay MiniParse에 보내는 것을 웹소켓으로 구현한 예
``` javascript
  ...
  var wsUri = "ws://@HOST_PORT@/MiniParse";
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
        document.dispatchEvent(new CustomEvent('onOverlayDataUpdate', { detail: JSON.parse(evt.data) }));
      }
    };

    //websocket.onopen = function(evt) { };
    websocket.onclose = function(evt) { 
      setTimeout(function(){connectWebSocket(uri)}, 5000);
    };
    websocket.onerror = function(evt) {
      websocket.close();
    };
  }    
  connectWebSocket(wsUri);
  ...
```

## 요구 사항 ##

* .Net Framework 4.5

## 빌드 방법 ##

1. external/websocket-sharp를 빌드후 dll 파일을 external 디렉토리에 복사
2. ACTWebSocket.Loader에서 ACTWebSocket.sln으로 빌드.

## 해야할 일 ##

1. 웹서버의 보안관련 문제 개선

## License ##

ACTWebSocket is provided under The MIT License.
