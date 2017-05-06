基于ZKWeb网页框架的MVVM示例

![preview](./docs/preview.jpg)

### 功能一览

- 使用Angular JS 4.0 + Webpack 2.0
    - 支持自动生成Api服务和Dto的TypeScript脚本
    - 支持返回预压缩好的脚本包
    - 支持跨站Api请求
- 使用Entity Framework Core查询数据
- 使用Swagger浏览和测试Api
- 使用AutoMapper转换Dto
- 支持.Net Core
- 支持多租户
- 支持多语言
- 支持多时区
- 支持工作单元
    - 支持使用查询过滤器
    - 支持使用操作过滤器
- Support automatic validate DTOs from request
- Provide default admin panel with PrimeNG
    - Support manage tenants
    - Support manage roles
    - Support manage users
- Provide command line compile and publish script

### How to start

**Use taobao npm mirror if needed**

```
npm config set registry http://registry.npm.taobao.org
npm config set sass_binary_site http://npm.taobao.org/mirrors/node-sass
```

**Build website files**

```
cd src/ZKWeb.MVVMPlugins/MVVM.Angular.Website/static
npm install
npm run watch
```

**Modify database configuration"

Open `ZKWeb.MVVMDemo.AspNetCore\App_Data\config.json` and modify database configuration.

**Start website**

Open `ZKWeb.MVVMDemo.sln` with VS2017 and run the project.

### Documents

- [Chinese Documents(中文文档)](./docs/cn)

### LICENSE

MIT LICENSE<br/>
Copyright © 2017 303248153@github<br/>
If you have any license issue please contact 303248153@qq.com.<br/>
