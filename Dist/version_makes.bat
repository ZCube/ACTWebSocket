rem 3:3:1:255
rem 3:3:0:254
if not exist 3.3.0.254 mkdir 3.3.0.254
call version_change.bat ACTWebSocket "3:3:0:254" "3.3.0.254"
call version_change.bat ACTWebSocket_Plugin "3:3:0:254" "3.3.0.254"
