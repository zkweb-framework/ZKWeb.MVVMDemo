# SPA demo based on ZKWeb framework [![Build status](https://ci.appveyor.com/api/projects/status/412kh8yf7yd915j1?svg=true)](https://ci.appveyor.com/project/303248153/zkweb-mvvmdemo)

Different to [ZKWeb.Demo](https://github.com/zkweb-framework/ZKWeb.Demo), this demo use Entity Framework Core and it's a single page application.<br/>
Also it supports linux (docker) hosting, which [ZKWeb.Demo](https://github.com/zkweb-framework/ZKWeb.Demo) doesn't for now.

# Screenshots

![preview](./docs/preview.jpg)

# Features

- Use AngularJS 6.0 + NgCli
    - Support automatic generate script form dto and application service
    - Support return pre-compressed script bundle
    - Support CORS api request
- Use Entity Framework Core
- Use Swagger
- Use AutoMapper
- Support .Net Core
- Support multi-tenant
- Support multi-language
- Support multi-timezone
- Support scheduled task
- Support unit of work
    - Support query filter
    - Support operation filter
- Support automatic validate DTOs from request
- Provide default admin panel with PrimeNG
    - Support manage tenants
    - Support manage roles
    - Support manage users
- Provide command line compile and publish script
- Provide Dockerfile, support running in docker

# How to start

**Use taobao npm mirror (optional)**

If you're in china, use taobao registry will make the npm command much happier.

```
npm config set registry http://registry.npm.taobao.org
npm config set sass_binary_site http://npm.taobao.org/mirrors/node-sass
```

**Build website files**

```
cd src/ZKWeb.MVVMPlugins/MVVM.Angular.Website/static
npm install -g @angular/cli
npm install
ng build --prod --aot
```

**Modify database configuration (optional)**

Open `ZKWeb.MVVMDemo.AspNetCore\App_Data\config.json` and modify database configuration.<br/>
By default it uses sqlite.

**Start website**

Open `ZKWeb.MVVMDemo.sln` with visual studio (>= 2017) and run the project.<br/>
If you're using linux, please install libgdiplus by following the instruction on [here](https://github.com/zkweb-framework/ZKWeb.System.Drawing).

**Use ng-serve (optional)**

If you want more verbose error message and faster recompilation you can use ng-serve, <br/>
First, modify `apiUrlBase` in `src/ZKWeb.MVVMPlugins/MVVM.Angular.Website/static/index.html`, use the proper api server address, then run `ng serve` under `src/ZKWeb.MVVMPlugins/MVVM.Angular.Website/static`.

# Documents

- [Chinese Documents(中文文档)](./docs/cn)

# LICENSE

LICENSE: MIT LICENSE<br/>
Copyright © 2017-2018 303248153@github<br/>
If you have any license issue please contact 303248153@qq.com.<br/>
