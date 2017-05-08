基于ZKWeb网页框架的MVVM示例

这份示例使用了ZKWeb提供的插件系统和AngularJS 4.0编写的模块化前端，支持前后端分离，并且提供了一套基本的后台管理系统

这份示例的开源协议是MIT，你可以随意的修改并用于个人或商业用途

![preview](../preview.jpg)

### 功能一览

- 使用AngularJS 4.0 + Webpack 2.0
    - 支持自动生成Api服务和DTO的TypeScript脚本
    - 支持返回预压缩好的脚本包
    - 支持跨站Api请求
- 使用Entity Framework Core查询数据
- 使用Swagger浏览和测试API
- 使用AutoMapper转换Dto
- 支持.Net Core
- 支持多租户
- 支持多语言
- 支持多时区
- 支持定时任务
- 支持工作单元
    - 支持使用查询过滤器
    - 支持使用操作过滤器
- 支持自动验证传入到Api服务的的DTO对象
- 提供基于PrimeNG开发的管理后台
    - 支持管理租户
    - 支持管理角色
    - 支持管理用户
- 提供编译和发布的命令行脚本

### 如何开始

**如果你访问官方npm源较慢，可以设置使用淘宝的npm镜像**

```
npm config set registry http://registry.npm.taobao.org
npm config set sass_binary_site http://npm.taobao.org/mirrors/node-sass
```

**编译网站前端文件**

```
cd src/ZKWeb.MVVMPlugins/MVVM.Angular.Website/static
npm install
npm run watch
```

**修改数据库配置**

打开`ZKWeb.MVVMDemo.AspNetCore\App_Data\config.json`并修改到你使用的数据库连接

**启动网站**

使用VS2017打开`ZKWeb.MVVMDemo.sln`并启动项目即可

### 更多内容

- [ZKWeb的官方文档](http://zkweb-framework.github.io)
- [ZKWeb的官方文档(备用))](http://zkweb.org/static/docs/index.html)
- [后端的项目结构](./BackendStruction.md)
- [管理后台的说明](./AdminPanel.md)
- [插件的建立](./CreatePlugin.md)
- [实体的建立](./CreateEntity.md)
- [数据的增删查改](./CRUD.md)
- [添加和测试Api](./ApplicationService.md)
- [多租户的说明](./MultiTenant.md)
- [组织架构的说明](./Organization.md)
- [多语言和多时区](./TODO.md)
- [添加定时任务](./TODO.md)
- [在Ubuntu上开发和打包](./TODO.md)
- AngularJS
	- [前端的项目结构](./TODO.md)
	- [PrimeNG的官方文档](https://www.primefaces.org/primeng/#/setup)
	- [自动生成脚本的说明](./TODO.md)
	- [前端模块的建立](./TODO.md)
	- [前端如何检查权限](./TODO.md)
	- [前端如何请求Api](./TODO.md)

欢迎加入ZKeb的官方QQ群522083886共同讨论
