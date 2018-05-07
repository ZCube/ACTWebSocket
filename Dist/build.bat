@echo off
if exist config.bat call config.bat

REM CALL "%VS140COMNTOOLS%vsvars32.bat"
set PATH=D:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin;%PATH%
pushd ..
if not exist nuget.exe curl https://dist.nuget.org/win-x86-commandline/latest/nuget.exe -o nuget.exe
if %errorlevel% neq 0 exit /b %errorlevel%
nuget restore -SolutionDirectory .
if %errorlevel% neq 0 exit /b %errorlevel%
msbuild ActWebSocket.sln /tv:15.0 /t:Build /p:Configuration=Release /p:TargetFramework=v4.5
if %errorlevel% neq 0 exit /b %errorlevel%
if not exist Dist\temp mkdir Dist\temp
if %errorlevel% neq 0 exit /b %errorlevel%
xcopy /hrkysd ACTWebSocket.Loader\bin\Release\BouncyCastle.*.dll Dist\temp
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
xcopy /hrkysd ACTWebSocket.Loader\bin\Release\SharpCompress*.dll Dist\temp
if %errorlevel% neq 0 exit /b %errorlevel%
xcopy /hrkysd ACTWebSocket.Loader\bin\Release\Tmds*.dll Dist\temp
if %errorlevel% neq 0 exit /b %errorlevel%

popd

set /p version=<..\Common\version
set /p tag=<..\Common\tag

xcopy /hrkysd ..\Sample /exclude:exclude_files.txt temp\
if %errorlevel% neq 0 exit /b %errorlevel%
if exist ACTWebSocket_latest.7z del ACTWebSocket_latest.7z
pushd temp
"c:\Program Files\7-Zip\7z.exe" a ..\ACTWebSocket_latest.7z *
if %errorlevel% neq 0 exit /b %errorlevel%
popd temp

if exist ACTWebSocket_latest.zip del ACTWebSocket_latest.zip
pushd temp
"c:\Program Files\7-Zip\7z.exe" a ..\ACTWebSocket_latest.zip *
if %errorlevel% neq 0 exit /b %errorlevel%
popd temp

xcopy /hrkyd ACTWebSocket_latest.zip ACTWebSocket_%version%.*

SET OWNER=ZCube
SET REPO=ACTWebSocket

echo ==========================================================================================
set actversion=3.3.0.254
echo %actversion%
if not exist %actversion% mkdir %actversion%
xcopy /hrkysd temp %actversion%
if %errorlevel% neq 0 exit /b %errorlevel%
call version_change.bat ACTWebSocket %actversion% %actversion%
if %errorlevel% neq 0 exit /b %errorlevel%
call version_change.bat ACTWebSocket_Plugin %actversion% %actversion%
if %errorlevel% neq 0 exit /b %errorlevel%

if exist ACTWebSocket_latest_actv3_%actversion%.zip del ACTWebSocket_latest_actv3_%actversion%.zip
pushd %actversion%
"c:\Program Files\7-Zip\7z.exe" a ..\ACTWebSocket_latest_actv3_%actversion%.zip *
if %errorlevel% neq 0 exit /b %errorlevel%
popd %actversion%

xcopy /hrkyd ACTWebSocket_latest_actv3_%actversion%.zip ACTWebSocket_%version%_actv3_%actversion%.*
@py -2 github_uploader.py %GITHUB_TOKEN% %OWNER% %REPO% %tag% ACTWebSocket_%version%_actv3_%actversion%.zip
echo ==========================================================================================
set actversion=3.3.1.255
echo %actversion%
if not exist %actversion% mkdir %actversion%
xcopy /hrkysd temp %actversion%
if %errorlevel% neq 0 exit /b %errorlevel%
call version_change.bat ACTWebSocket %actversion% %actversion%
if %errorlevel% neq 0 exit /b %errorlevel%
call version_change.bat ACTWebSocket_Plugin %actversion% %actversion%
if %errorlevel% neq 0 exit /b %errorlevel%

if exist ACTWebSocket_latest_actv3_%actversion%.zip del ACTWebSocket_latest_actv3_%actversion%.zip
pushd %actversion%
"c:\Program Files\7-Zip\7z.exe" a ..\ACTWebSocket_latest_actv3_%actversion%.zip *
if %errorlevel% neq 0 exit /b %errorlevel%
popd %actversion%

xcopy /hrkyd ACTWebSocket_latest_actv3_%actversion%.zip ACTWebSocket_%version%_actv3_%actversion%.*
@py -2 github_uploader.py %GITHUB_TOKEN% %OWNER% %REPO% %tag% ACTWebSocket_%version%_actv3_%actversion%.zip
echo ==========================================================================================


:SetVariables
FOR /F "tokens=3 delims= " %%G IN ('REG QUERY "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders" /v "Personal"') DO (SET DocumentDir=%%G)
if exist "%APPDATA%\Dropbox\info.json" SET DROP_INFO=%APPDATA%\Dropbox\info.json
if exist "%LOCALAPPDATA%\Dropbox\info.json" SET DROP_INFO=%LOCALAPPDATA%\Dropbox\info.json
for /f "tokens=*" %%a in ( 'powershell -NoProfile -ExecutionPolicy Bypass -Command "( ( Get-Content -Raw -Path %DROP_INFO% | ConvertFrom-Json ).personal.path)" ' ) do set dropBoxRoot=%%a
echo %dropBoxRoot%

if not exist "%dropBoxRoot%\share" mkdir "%dropBoxRoot%\share"
if %errorlevel% neq 0 exit /b %errorlevel%
set actversion=3.3.1.255
xcopy /hrkysd  "%CD%\ACTWebSocket_latest_actv3_%actversion%.zip" /exclude:exclude_files.txt "%dropBoxRoot%\share"
set actversion=3.3.0.254
xcopy /hrkysd  "%CD%\ACTWebSocket_latest_actv3_%actversion%.zip" /exclude:exclude_files.txt "%dropBoxRoot%\share"
if %errorlevel% neq 0 exit /b %errorlevel%
xcopy /hrkysd  "%CD%\ACTWebSocket_latest.zip" /exclude:exclude_files.txt "%dropBoxRoot%\share"
if %errorlevel% neq 0 exit /b %errorlevel%
xcopy /hrkysd  "%CD%\ACTWebSocket_latest.7z" /exclude:exclude_files.txt "%dropBoxRoot%\share"
if %errorlevel% neq 0 exit /b %errorlevel%
copy /y "%CD%\..\Common\version" "%dropBoxRoot%\share\ACTWebSocket_version"
if %errorlevel% neq 0 exit /b %errorlevel%
