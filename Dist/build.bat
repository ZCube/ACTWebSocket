@echo off
CALL "%VS140COMNTOOLS%vsvars32.bat"
pushd ..
if not exist nuget.exe wget https://dist.nuget.org/win-x86-commandline/latest/nuget.exe
if %errorlevel% neq 0 exit /b %errorlevel%
nuget restore -SolutionDirectory .
if %errorlevel% neq 0 exit /b %errorlevel%
devenv ActWebSocket.sln /build "Release"
if %errorlevel% neq 0 exit /b %errorlevel%
if not exist Dist\temp mkdir Dist\temp
if %errorlevel% neq 0 exit /b %errorlevel%
xcopy /hrkysd ACTWebSocket.Loader\bin\Release\ACTWebSocket*.dll Dist\temp
if %errorlevel% neq 0 exit /b %errorlevel%
xcopy /hrkysd ACTWebSocket.Loader\bin\Release\websocket*.dll Dist\temp
if %errorlevel% neq 0 exit /b %errorlevel%
xcopy /hrkysd ACTWebSocket.Loader\bin\Release\MimeType*.dll Dist\temp
if %errorlevel% neq 0 exit /b %errorlevel%
xcopy /hrkysd ACTWebSocket.Loader\bin\Release\Newtonsoft*.dll Dist\temp
if %errorlevel% neq 0 exit /b %errorlevel%
xcopy /hrkysd ACTWebSocket.Loader\bin\Release\Open.Nat*.dll Dist\temp
if %errorlevel% neq 0 exit /b %errorlevel%
xcopy /hrkysd ACTWebSocket.Loader\bin\Release\DotNetZip*.dll Dist\temp
if %errorlevel% neq 0 exit /b %errorlevel%
xcopy /hrkysd ACTWebSocket.Loader\bin\Release\Tmds*.dll Dist\temp
if %errorlevel% neq 0 exit /b %errorlevel%


popd
xcopy /hrkysd ..\Sample /exclude:exclude_files.txt temp\
if %errorlevel% neq 0 exit /b %errorlevel%
if exist ACTWebSocket_latest.7z del ACTWebSocket_latest.7z
pushd temp
"c:\Program Files\7-Zip\7z.exe" a ..\ACTWebSocket_latest.7z *
if %errorlevel% neq 0 exit /b %errorlevel%
popd temp


:SetVariables
FOR /F "tokens=3 delims= " %%G IN ('REG QUERY "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders" /v "Personal"') DO (SET DocumentDir=%%G)
if exist "%APPDATA%\Dropbox\info.json" SET DROP_INFO=%APPDATA%\Dropbox\info.json
if exist "%LOCALAPPDATA%\Dropbox\info.json" SET DROP_INFO=%LOCALAPPDATA%\Dropbox\info.json
for /f "tokens=*" %%a in ( 'powershell -NoProfile -ExecutionPolicy Bypass -Command "( ( Get-Content -Raw -Path %DROP_INFO% | ConvertFrom-Json ).personal.path)" ' ) do set dropBoxRoot=%%a
echo %dropBoxRoot%

if not exist "%dropBoxRoot%\share" mkdir "%dropBoxRoot%\share"
if %errorlevel% neq 0 exit /b %errorlevel%
xcopy /hrkysd  "%CD%\ACTWebSocket_latest.7z" /exclude:exclude_files.txt "%dropBoxRoot%\share"
if %errorlevel% neq 0 exit /b %errorlevel%
