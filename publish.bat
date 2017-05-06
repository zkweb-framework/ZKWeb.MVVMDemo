@echo off

echo building project...
cd src\ZKWeb.MVVMDemo.AspNetCore
dotnet build -c Release -f netcoreapp1.1
dotnet publish -c Release -f netcoreapp1.1 -r win10-x64
cd ..\..

echo building plugins...
cd src\ZKWeb.MVVMDemo.Console
dotnet run -c Release -f netcoreapp1.1
cd ..\..

echo publishing website...
..\ZKWeb\Tools\WebsitePublisher.Cmd.exe -f netcoreapp1.1 -x ".*node_modules.*" -r src\ZKWeb.MVVMDemo.AspNetCore -n "zkweb.mvvm" -o "..\..\publish"
pause
