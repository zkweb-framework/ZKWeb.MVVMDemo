@echo off
echo this script is for build and publish mvvm demo site
echo please ensure you have this directory layout
echo - zkweb
echo   - tools
echo - zkweb.demo
echo   - publish.bat
echo.

echo building project...
cd src\ZKWeb.MVVMDemo.AspNetCore
dotnet restore
dotnet build -c Release -f netcoreapp2.0
dotnet publish -c Release -f netcoreapp2.0 -r win10-x64
cd ..\..

echo building plugins...
cd src\ZKWeb.MVVMDemo.Console
dotnet restore
dotnet run -c Release -f netcoreapp2.0
cd ..\..

echo publishing website...
..\ZKWeb\Tools\WebsitePublisher.Cmd.Windows\ZKWeb.Toolkits.WebsitePublisher.Cmd.exe -f netcoreapp2.0 -x ".*node_modules.*" -r src\ZKWeb.MVVMDemo.AspNetCore -n "zkweb.mvvm" -o "..\..\publish"
pause
