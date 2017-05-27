if not exist temp2 mkdir temp2
set filename=%1
set newver=%2
set dir=%3
"%ILDASM%" temp\%1.dll /out="temp2\%1.il"
py -2 version_change.py "temp2\%1.il" ".ver %newver%"
"%ILASM%" /DLL "temp2\%1.il" /RESOURCE="temp2\%1.res" /out="%dir%\%1.dll"
del /Q temp2\*
