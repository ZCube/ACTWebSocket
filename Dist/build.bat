CALL "%VS140COMNTOOLS%vsvars32.bat"
pushd ..
devenv ActWebSocket.sln /build "Release"
mkdir Dist\temp
xcopy /hrkysd ACTWebSocket.Loader\bin\Release\ACTWebSocket*.dll Dist\temp
xcopy /hrkysd ACTWebSocket.Loader\bin\Release\websocket*.dll Dist\temp
xcopy /hrkysd ACTWebSocket.Loader\bin\Release\MimeType*.dll Dist\temp
xcopy /hrkysd ACTWebSocket.Loader\bin\Release\Newtonsoft*.dll Dist\temp
xcopy /hrkysd ACTWebSocket.Loader\bin\Release\Newtonsoft*.dll Dist\temp
xcopy /hrkysd ACTWebSocket.Loader\bin\Release\Open.Nat* Dist\temp


xcopy /hrkysd Sample Dist\temp\
popd
del ACTWebSocket_latest.7z
pushd temp
"c:\Program Files\7-Zip\7z.exe" a ..\ACTWebSocket_latest.7z *
popd temp


:SetVariables
FOR /F "tokens=3 delims= " %%G IN ('REG QUERY "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders" /v "Personal"') DO (SET DocumentDir=%%G)
if exist "%APPDATA%\Dropbox\info.json" SET DROP_INFO=%APPDATA%\Dropbox\info.json
if exist "%LOCALAPPDATA%\Dropbox\info.json" SET DROP_INFO=%LOCALAPPDATA%\Dropbox\info.json
for /f "tokens=*" %%a in ( 'powershell -NoProfile -ExecutionPolicy Bypass -Command "( ( Get-Content -Raw -Path %DROP_INFO% | ConvertFrom-Json ).personal.path)" ' ) do set dropBoxRoot=%%a
echo %dropBoxRoot%

mkdir "%dropBoxRoot%\share"
xcopy /hrkysd  "%CD%\ACTWebSocket_latest.7z" /exclude:exclude_files.txt "%dropBoxRoot%\share"
