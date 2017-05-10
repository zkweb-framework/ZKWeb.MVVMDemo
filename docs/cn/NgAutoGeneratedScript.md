# 自动生成脚本的说明

Demo的后端会在网站启动时自动生成一些脚本供前端使用，包括

- 根据Dto生成脚本
- 根据权限列表生成脚本
- 根据用户类型列表生成脚本
- 根据应用服务(API)生成脚本
- 根据翻译列表生成脚本

生成的脚本例子如下

**Dto**，如果一个Dto有引用其他Dto会自动import

``` typescript
import { UserOutputDto } from './user-output-dto';

/** 当前会话信息 */
export class SessionInfoDto {
    /** 用户信息 */
    public User: UserOutputDto;
}
```

**权限列表**，都会存在一个类里面

``` typescript
export class Privileges {
    /** 角色:查看 */
    public static Role_View = "Role:View";
    /** 角色:编辑 */
    public static Role_Edit = "Role:Edit";
    /** 角色:删除 */
    public static Role_Remove = "Role:Remove";
}
```

**用户类型列表**，都会存在一个类里面

``` typescript
export class UserTypes {
    /** IAmAdmin */
    public static IAmAdmin = "IAmAdmin";
    /** IAmUser */
    public static IAmUser = "IAmUser";
    /** IUserType */
    public static IUserType = "IUserType";
}
```

**应用服务(API)**，根据类型和里面的函数生成

``` typescript
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppApiService } from '../../base_module/services/app-api-service';
import { GridSearchResponseDto } from '../dtos/grid-search-response-dto';
import { GridSearchRequestDto } from '../dtos/grid-search-request-dto';
import { ActionResponseDto } from '../dtos/action-response-dto';
import { ExampleDataInputDto } from '../dtos/example-data-input-dto';

@Injectable()
/** 示例数据管理服务 */
export class ExampleDataManageService {
    constructor(private appApiService: AppApiService) { }

    /** 搜索数据 */
    Search(request: GridSearchRequestDto): Observable<GridSearchResponseDto> {
        return this.appApiService.call<GridSearchResponseDto>(
            "/api/ExampleDataManageService/Search",
            {
                request
            });
    }
}
```

**翻译**，按语言合并到一个文件

``` typescript
/** 中文 */
export class Translation_zh_CN {
    public static language = "zh-CN";
    public static translations: { [key: string]: string } = {
		"Submit": "提交",
		"Cancel": "取消",
		"{0} is required": "请填写{0}",
    }
}
```

这些脚本都会保存在`generated_module`下，需要生成的服务时请引用`GeneratedModule`模块

这些脚本会在**网站启动**的时候生成，也就是你修改完代码以后刷新一次网页就可以看到生成的文件变化

生成脚本的实现在`MVVM.Angular.Support`插件的`src\Components\ScriptGenerator下`
