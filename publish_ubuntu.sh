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

if [ ! -d "../ZKWeb/ZKWeb.Toolkits" ]; then
    echo "ERROR:"
    echo "please download ZKWeb and put it in the same directory as ZKWeb.MVVMDemo"
    exit
fi

echo "building project..."
cd src/ZKWeb.MVVMDemo.AspNetCore
dotnet restore
dotnet build -c Release -f netcoreapp1.1
dotnet publish -c Release -f netcoreapp1.1 -r ubuntu.16.04-x64
cd ../..

echo "building plugins..."
cd src/ZKWeb.MVVMDemo.Console
dotnet restore
dotnet run -c Release -f netcoreapp1.1
cd ../..

echo "publishing website..."
cd ../ZKWeb/ZKWeb.Toolkits/ZKWeb.Toolkits.WebsitePublisher.Cmd
dotnet restore
dotnet run -c Debug -f netcoreapp1.1 -- -f netcoreapp1.1 -x ".*node_modules.*" -r ../../../ZKWeb.MVVMDemo/src/ZKWeb.MVVMDemo.AspNetCore -n "publish" -o "../../../ZKWeb.MVVMDemo"
