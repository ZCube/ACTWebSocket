# ACTWebSocket Plugin
![N|Solid](https://raw.githubusercontent.com/laiglinne-ff/ACTWebSocket/master/logo.png)
WebSocket Plugin for Advanced Combat Tracker v3

[![Build Status](https://jenkins.zcube.kr/buildStatus/icon?job=ACTWebSocket)](https://jenkins.zcube.kr/job/ACTWebSocket/)

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

1. Overlay MiniParse에 보내는 것을 웹소켓으로 구현한 예
``` javascript
  ...
	<script src="https://ZCube.github.io/ACTWebSocket/Sample/actwebsocket.js"></script>
	<script src="https://ZCube.github.io/ACTWebSocket/Sample/actwebsocket_compat.js"></script>
  ...
```

## 요구 사항 ##

* .Net Framework 4.5

## 사용 시 주의 사항 ##

* ravahn/FFXIV_ACT_Plugin 

LogLine을 수정함으로 이 플러그인 먼저 로드 되는지 후에 로드되는지에 따라 로그 메세지가 바뀜.
아래와 같은 경로에서 설정 파일을 수정하여 ACTWebSocket이 먼저 로드 될 필요가 있음.

%APPDATA%\Advanced Combat Tracker\Config\Advanced Combat Tracker.config.xml

``` xml
    <ActPlugins>
        <Plugin Enabled="True" Path="...\ACTWebSocket.dll" />
        <Plugin Enabled="True" Path="...\FFXIV_ACT_Plugin.dll" />
    </ActPlugins>
```

* 사용, 재배포에 의해 발생하는 모든 결과에 대한 책임은 사용자 본인에게 있습니다.

## 빌드 방법 ##

1. external/websocket-sharp를 빌드후 dll 파일을 external 디렉토리에 복사
2. ACTWebSocket.sln으로 빌드.

## 경로 ##

* Skin Directory : [ACT 경로]/OverlaySkin

## 해야할 일 ##

1. 웹서버의 보안관련 문제 개선

## Binary ##

latest : [download](https://www.dropbox.com/s/3lrsetatf9mrmnp/ACTWebSocket_latest.7z?dl=1)

Release : [download](https://github.com/ZCube/ACTWebSocket/releases)

## License ##

ACTWebSocket is provided under The MIT License.
