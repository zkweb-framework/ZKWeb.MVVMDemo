# 前端如何检查权限

### **手动检查权限**

前端检查权限时首先要获取当前的会话信息，调用`AppSessionService.getSessionInfo`可以获取到`SessionInfoDto`，内容如下

``` json
{
  "User": {
    "Id": "00838e19-8103-1033-ab23-25ae7ea814f7",
    "Type": "SuperAdmin",
    "Username": "admin",
    "OwnerTenantId": "00838e19-80d7-1027-849d-7e38e7e7218e",
    "CreateTime": "2017/05/05 12:10:13",
    "UpdateTime": "2017/05/05 12:10:13",
    "Remark": null,
    "Deleted": false,
    "RoleIds": [],
    "Roles": [],
    "OwnerTenantName": "Master",
    "OwnerTenantIsMasterTenant": true,
    "AvatarImageBase64": null,
    "ImplementedTypes": [
      "IAmSuperAdmin",
      "IAmAdmin",
      "IAmUser",
      "IUserType",
      "ICanUseAdminPanel",
      "SuperAdminUserType"
    ],
    "Privileges": []
  }
}
```

然后你需要构建一个`AuthRequirement`对象，这个对象的定义如下

``` typescript
/** 验证要求信息 */
export class AuthRequirement {
    /** 是否要求主租户 */
    requireMasterTenant?: boolean;
    /** 要求的用户类型 */
    requireUserType?: string;
    /** 要求的权限列表 */
    requirePrivileges?: string[];
}
```

例如

``` typescript
import { UserTypes } from '../../generated_module/privileges/user-types';
import { Privileges } from '../../generated_module/privileges/privileges';

var requirement = { requireUserType: UserTypes.IAmAdmin, requirePrivileges: [ Privileges.Role_View ] };
```

构建完成后把会话信息中的`User`和`requirement`传给`AppPrivilegeService.isAuthorized`即进行权限检查

``` typescript
var result = this.appPrivilegeService.isAuthorized(sessionInfo.User, requirement);
if (!result.success) {
  alert(result.errorMessage);
  return;
}
```

注意`isAuthorized`函数返回的是一个对象，包含了`success`和`errorMessage`成员，切勿使用`if (service.isAuthorized())`这样的代码检查

### **路由检查权限**

在路由中检查权限时需要使用`auth_module\auth\auth-guard.ts`中的`AuthGuard`，例如

``` typescript
{
    path: 'about_me', component: AdminAboutMeComponent,
    canActivate: [AuthGuard],
    data: { auth: { requireUserType: UserTypes.ICanUseAdminPanel } }
}
```

定义路由规则时把`AuthGuard`加到`canActivate`中，并且把`AuthRequirement`对象放到`data.auth`即可启用路由检查权限

`AuthGuard`在检查失败时会跳转到登录页面，如果当前用户已登录只是权限不足还会弹出提示框

### **导航菜单检查权限**

在`admin_base_module\navigation\admin-nav-menu.ts`定义的导航菜单也支持检查权限，如下

``` typescript
{
    name: "Role Manage",
    icon: "fa fa-legal",
    url: ["/admin", "roles"],
    auth: {
        requireUserType: UserTypes.IAmAdmin,
        requirePrivileges: [Privileges.Role_View]
    }
}
```

把`AuthRequirement`对象放到`auth`中即可实现权限不足时隐藏菜单项
