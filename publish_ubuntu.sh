#!/usr/bin/env bash

if [ ! -f "/usr/lib/gdiplus.dll" ]; then
    echo "ERROR:"
    echo "please install libgdiplus first:"
    echo "================================"
    echo "sudo apt-get install libgdiplus"
    echo "cd /usr/lib"
    echo "ln -s libgdiplus.so gdiplus.dll"
    echo "================================"
    exit
fi

if [ ! -d "../ZKWeb/Tools" ]; then
    echo "ERROR:"
    echo "please download ZKWeb and put it in the same directory as ZKWeb.MVVMDemo"
    exit
fi

echo "building project..."
cd src/ZKWeb.MVVMDemo.AspNetCore
dotnet restore
dotnet build -c Release -f netcoreapp2.0
dotnet publish -c Release -f netcoreapp2.0 -r ubuntu.16.04-x64
cd ../..

echo "building plugins..."
cd src/ZKWeb.MVVMDemo.Console
dotnet restore
dotnet run -c Release -f netcoreapp2.0
cd ../..

echo "publishing website..."
dotnet ../ZKWeb/Tools/WebsitePublisher.Cmd.NetCore/ZKWeb.Toolkits.WebsitePublisher.Cmd.dll -f netcoreapp2.0 -x ".*node_modules.*" -r "src/ZKWeb.MVVMDemo.AspNetCore" -n "zkweb.mvvm" -o "../../publish"
