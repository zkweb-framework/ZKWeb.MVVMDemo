# 在Ubuntu上开发和打包

这个Demo支持在Ubuntu上开发和打包，在开发前需要安装.net core sdk和nodejs 6.x

我使用的环境是Ubuntu 16.04 LTS，以下的步骤也按这个环境为准

### **安装.Net Core**

参考[微软官方的教程](https://www.microsoft.com/net/core#linuxubuntu)

### **安装nodejs 6.x**

因为Ubuntu官方源中的Node.Js还是4.x的，需要加官方源安装，执行以下的命令

``` sh
cd ~
curl -sL https://deb.nodesource.com/setup_6.x -o nodesource_setup.sh
sudo bash nodesource_setup.sh
sudo apt-get install nodejs
sudo apt-get install build-essential
```

## **安装libgdiplus**

处理图片，例如生成验证图片时需要这个库，执行以下的命令

``` sh
sudo apt-get install libgdiplus
cd /usr/lib
sudo ln -s libgdiplus.so gdiplus.dll
```

### **编译前端**

按之前[给出的步骤](README.md)编译前端文件即可，编译成功后可以看到`dist`文件夹

### **编译和运行后端**

按照普通.Net Core项目的方式编译和运行后端即可，进入项目文件夹并执行以下命令

``` sh
cd src/ZKWeb.MVVMDemo.AspNetCore
dotnet restore
dotnet run -c Release -f netcoreapp1.1
```

### **发布网站**

发布网站需要使用ZKWeb专门的发布工具，你需要[下载ZKWeb](https://github.com/zkweb-framework/zkweb)到Demo项目所在的文件夹

请确保有以下的文件夹结构

- ZKWeb
  - ZKWeb.Toolkits
- ZKWeb.MVVMDemo
  - src
  - publish_ubuntu.sh

完成后运行publish_ubuntu.sh，运行完以后会发布网站和插件文件到`publish`文件夹下
