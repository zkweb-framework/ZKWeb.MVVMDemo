# 前端模块的建立

创建一个前端模块需要以下步骤

- 在`src\modules`下添加模块文件夹
- 添加模块的主文件`名称.module.ts`
- 添加模块的组件和视图到`components`和`views`下
- 添加模块的内部路由
- 添加模块的外部路由到`AdminModule`中

以示例数据的管理模块`admin_example_datas_module`为例

### **模块的主文件内容**

``` typescript
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import {
    InputTextModule,
    PanelModule,
    ButtonModule,
    MessagesModule,
    BlockUIModule,
    DataTableModule,
    DropdownModule,
    MultiSelectModule,
    DialogModule,
    ConfirmDialogModule
} from 'primeng/primeng';

import { BaseModule } from '../base_module/base.module';
import { GeneratedModule } from '../generated_module/generated.module';
import { AuthModule } from '../auth_module/auth.module';
import { AdminBaseModule } from '../admin_base_module/admin_base.module';

import { AuthGuard } from '../auth_module/auth/auth-guard';
import { UserTypes } from '../generated_module/privileges/user-types';
import { Privileges } from '../generated_module/privileges/privileges';

import { AdminExampleDataListComponent } from './components/admin-example-data-list.component';

const routes: Routes = [
    {
        path: '',
        component: AdminExampleDataListComponent,
        pathMatch: 'full',
        canActivate: [AuthGuard],
        data: {
            auth: {
                requireUserType: UserTypes.IAmAdmin,
                requirePrivileges: [Privileges.ExampleData_View]
            }
        }
    }
];

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        InputTextModule,
        PanelModule,
        ButtonModule,
        MessagesModule,
        BlockUIModule,
        DataTableModule,
        DropdownModule,
        MultiSelectModule,
        DialogModule,
        ConfirmDialogModule,
        BaseModule,
        GeneratedModule,
        AuthModule,
        AdminBaseModule,
        RouterModule.forChild(routes)
    ],
    declarations: [
        AdminExampleDataListComponent,
    ],
    exports: [
        RouterModule
    ]
})
export class AdminExampleDatasModule { }
```

这个模块导入了PrimeNG中的用到的界面元素模块和BaseModule, GeneratedModule, AuthModule, AdminBaseModule还有路由模块

这个模块包含了一个页面(示例数据列表)，使用的组件是AdminExampleDataListComponent，需要的权限是管理员和查看示例数据的权限

### **页面组件的内容**

``` typescript
import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { ConfirmationService } from 'primeng/primeng';
import { CrudWithDialogBaseComponent } from '../../base_module/components/crud-with-dialog-base.component';
import { GridSearchRequestDto } from '../../generated_module/dtos/grid-search-request-dto';
import { AppTranslationService } from '../../base_module/services/app-translation-service';
import { ExampleDataManageService } from '../../generated_module/services/example-data-manage-service';
import { UserTypes } from '../../generated_module/privileges/user-types';
import { Privileges } from '../../generated_module/privileges/privileges';
import { AppPrivilegeService } from '../../auth_module/services/app-privilege-service';
import { AppSessionService } from '../../auth_module/services/app-session-service';

@Component({
    selector: 'admin-example-data-list',
    templateUrl: '../views/admin-example-data-list.html',
    providers: [ConfirmationService]
})
export class AdminExampleDataListComponent extends CrudWithDialogBaseComponent {
    constructor(
        confirmationService: ConfirmationService,
        appSessionService: AppSessionService,
        appPrivilegeService: AppPrivilegeService,
        appTranslationService: AppTranslationService,
        private exampleDataManageService: ExampleDataManageService) {
        super(confirmationService, appSessionService, appPrivilegeService, appTranslationService);
    }

    ngOnInit() {
        super.ngOnInit();
        this.editForm.addControl("Id", new FormControl(""));
        this.editForm.addControl("Name", new FormControl("", Validators.required));
        this.editForm.addControl("Description", new FormControl([]));
    }

    submitSearch(request: GridSearchRequestDto) {
        return this.exampleDataManageService.Search(request);
    }

    getAddRequirement() {
        return {
            requireUserType: UserTypes.IAmAdmin,
            requirePrivileges: [Privileges.ExampleData_Edit]
        };
    }

    getEditRequirement() {
        return {
            requireUserType: UserTypes.IAmAdmin,
            requirePrivileges: [Privileges.ExampleData_Edit]
        };
    }

    getRemoveRequirement() {
        return {
            requireMasterExampleData: true,
            requireUserType: UserTypes.IAmAdmin,
            requirePrivileges: [Privileges.ExampleData_Remove]
        };
    }

    submitEdit(obj: any) {
        return this.exampleDataManageService.Edit(obj);
    }

    submitRemove(obj: any) {
        return this.exampleDataManageService.Remove(obj.Id);
    }
}
```

这个组件继承了`base_module`提供的`CrudWithDialogBaseComponent`组件，可以简化增删查改页面的代码，上面的函数的作用如下

- ngOnInit: 组件初始化时给FormGroup添加字段的定义
  - 关于FormGroup请查看Angular的[Reactive Forms文档](https://angular.io/docs/ts/latest/guide/reactive-forms.html)
- submitSearch: 提交搜索请求
- getAddRequirement： 获取添加数据需要的权限，如果不满足则添加按钮不应该显示
- getEditRequirement: 获取编辑数据需要的权限，如果不满足则编辑按钮不应该显示
- getRemoveRequirement: 获取删除数据需要的权限，如果不满足则删除按钮不应该显示
- submitEdit: 提交编辑请求
- submitRemove: 提交删除请求

### **视图的内容**

``` typescript
<div class="portlet">
    <h3 class="portlet-title">
        <i class="fa fa-legal"></i>
        <span class="caption-subject">{{ "ExampleData Manage" | trans }}</span>
    </h3>
    <div class="portlet-body">
        <p-messages [value]="msgs"></p-messages>
        <p-dataTable [value]="value"
                     [paginator]="true"
                     [rows]="rows"
                     [totalRecords]="totalRecords"
                     [rowsPerPageOptions]="rowsPerPageOptions"
                     [globalFilter]="globalFilter"
                     [resizableColumns]="true"
                     [reorderableColumns]="true"
                     [emptyMessage]="emptyMessage"
                     scrollable="true"
                     scrollHeight="300px"
                     sortField="CreateTime"
                     sortOrder="-1"
                     [lazy]="true"
                     (onLazyLoad)="search($event)"
                     [loading]="loading" #table>
            <p-header>
                <div class="ui-helper-clearfix">
                    <span class="pull-left">
                        <button type="button" pButton icon="fa-plus" (click)="add()" *ngIf="allowAdd" [label]="'Add' | trans"></button>
                    </span>
                    <span class="pull-right">
                        <i class="fa fa-search search-icon"></i>
                        <input #globalFilter type="text" pInputText [placeholder]="'Name/Remark' | trans">
                    </span>
                </div>
            </p-header>
            <p-column field="Name" [header]="'Name' | trans" [filter]="true" [sortable]="true" [filterPlaceholder]="'Search' | trans"></p-column>
            <p-column field="Description" [header]="'Description' | trans" [filter]="true" [sortable]="true" [filterPlaceholder]="'Search' | trans"></p-column>
            <p-column field="CreateTime" [header]="'CreateTime' | trans" [sortable]="true"></p-column>
            <p-column field="UpdateTime" [header]="'UpdateTime' | trans" [sortable]="true"></p-column>
            <p-column [header]="'Actions' | trans" styleClass="col-button" *ngIf="allowEdit || allowRemove">
                <ng-template let-row="rowData" pTemplate="body">
                    <button type="button" pButton (click)="edit(row)" *ngIf="allowEdit" icon="fa-edit"></button>
                    <button type="button" pButton (click)="remove(row)" *ngIf="allowRemove" icon="fa-remove"></button>
                </ng-template>
            </p-column>
        </p-dataTable>
    </div>
</div>
<p-confirmDialog [header]="'Confirmation' | trans" [acceptLabel]="'Yes' | trans" [rejectLabel]="'No' | trans" icon="fa fa-question-circle" width="380"></p-confirmDialog>
<p-dialog [header]="'Edit ExampleData' | trans" [(visible)]="editDialogVisible" modal="modal" width="500" responsive="true">
    <form [formGroup]="editForm" (ngSubmit)="editDialogSubmit()">
        <p-messages [value]="editMsgs"></p-messages>
        <z-form-grid>
            <z-form-hidden [formGroup]="editForm" fieldName="Id"></z-form-hidden>
            <z-form-text [formGroup]="editForm" fieldName="Name"></z-form-text>
            <z-form-textarea [formGroup]="editForm" fieldName="Description"></z-form-textarea>
        </z-form-grid>
    </form>
    <p-footer>
        <div class="ui-dialog-buttonpane ui-widget-content ui-helper-clearfix">
            <button type="button" pButton icon="fa-close" (click)="editDialogClose()" [label]="'Cancel' | trans"></button>
            <button type="button" pButton icon="fa-check" (click)="editDialogSubmit()" [label]="'Submit' | trans" [disabled]="!editForm.valid || editFormSubmitting"></button>
        </div>
    </p-footer>
</p-dialog>
```

这个视图的结构如下

- div.portlet: 一个包装用的div
  - h3.portlet-title: 头部文本
  - div.portlet-body: 包装的内容
    - p-messages: 消息列表，请查看[PrimeNG的文档](https://www.primefaces.org/primeng/#/messages)
    - p-dataTable: 表格，数据从远程载入，请查看[PrimeNG的文档](https://www.primefaces.org/primeng/#/datatable)
      - p-header: 表格头部，包含添加按钮和关键字搜索栏
      - p-column: 各个表格列的定义
- p-confirmDialog: 删除数据使用的确认框，请查看[PrimeNG的文档](https://www.primefaces.org/primeng/#/confirmdialog)
- p-dialog: 添加或编辑数据使用的弹出框，请查看[PrimeNG的文档](https://www.primefaces.org/primeng/#/dialog)
  - form: 表单
    - p-messages: 表单消息列表，请查看[PrimeNG的文档](https://www.primefaces.org/primeng/#/messages)
    - z-form-grid: 表单内容的包装组件，在`base_module\components\form-grid.component.ts`中定义
      - z-form-hidden: 隐藏字段的组件，在`base_module\components\form-hidden.component.ts`中定义
      - z-form-text: 文本框的组件，在`base_module\components\form-text.component.ts`中定义
      - z-form-textarea: 多行文本框的组件，在`base_module\components\form-textarea.component.ts`中定义
  - p-footer: 弹出框底部，包含关闭和提交按钮

### 修改其他模块

建立完以上的模块中的内部文件后还需要修改其他模块

添加路由，打开`admin_module\admin.module.ts`并添加以下行到`routes`下

``` javascript
 { path: 'example_datas', loadChildren: '../admin_example_datas_module/admin_example_datas.module#AdminExampleDatasModule' }
```

添加后如下

``` typescript
const routes: Routes = [
    {
        path: "",
        component: AdminContainerComponent,
        canActivate: [AuthGuard],
        data: { auth: { requireUserType: UserTypes.ICanUseAdminPanel } },
        children:
        [
            {
                path: '', component: AdminIndexComponent, pathMatch: "full"
            },
            {
                path: 'about_website', component: AdminAboutWebsiteComponent,
                canActivate: [AuthGuard],
                data: { auth: { requireUserType: UserTypes.ICanUseAdminPanel } }
            },
            {
                path: 'about_me', component: AdminAboutMeComponent,
                canActivate: [AuthGuard],
                data: { auth: { requireUserType: UserTypes.ICanUseAdminPanel } }
            },
            { path: 'tenants', loadChildren: '../admin_tenants_module/admin_tenants.module#AdminTenantsModule' },
            { path: 'users', loadChildren: '../admin_users_module/admin_users.module#AdminUsersModule' },
            { path: 'roles', loadChildren: '../admin_roles_module/admin_roles.module#AdminRolesModule' },
            { path: 'settings', loadChildren: '../admin_settings_module/admin_settings.module#AdminSettingsModule' },
            { path: 'scheduled_tasks', loadChildren: '../admin_scheduled_tasks_module/admin_scheduled_tasks.module#AdminScheduledTasksModule' },
            { path: 'example_datas', loadChildren: '../admin_example_datas_module/admin_example_datas.module#AdminExampleDatasModule' }

        ]
    },
    { path: 'login', component: AdminLoginComponent } 
];
```

添加导航栏菜单，打开`admin_base_module\navigation\admin-nav-menu.ts`并添加以下行到`AdminNavMenu`

``` typescript
 {
    name: "Example Datas",
    icon: "fa fa-database",
    items: [
        {
            name: "Example Datas",
            icon: "fa fa-database",
            url: ["/admin", "example_datas"],
            auth: {
                requireUserType: UserTypes.IAmAdmin,
                requirePrivileges: [Privileges.ExampleData_View]
            }
        },
    ]
}
```

添加后的效果如下

``` typescript
export const AdminNavMenu: NavMenuGroup[] = [
    {
        name: "Admin Index",
        icon: "fa fa-home",
        items: [],
        url: ["/admin"]
    },
    {
        name: "System Manage",
        icon: "fa fa-gear",
        items: [
            {
                name: "Tenant Manage",
                icon: "fa fa-copy",
                url: ["/admin", "tenants"],
                auth: {
                    requireMasterTenant: true,
                    requireUserType: UserTypes.IAmAdmin,
                    requirePrivileges: [Privileges.Tenant_View]
                }
            },
            {
                name: "User Manage",
                icon: "fa fa-user",
                url: ["/admin", "users"],
                auth: {
                    requireUserType: UserTypes.IAmAdmin,
                    requirePrivileges: [Privileges.User_View]
                }
            },
            {
                name: "Role Manage",
                icon: "fa fa-legal",
                url: ["/admin", "roles"],
                auth: {
                    requireUserType: UserTypes.IAmAdmin,
                    requirePrivileges: [Privileges.Role_View]
                }
            }
        ]
    },
    {
        name: "Example Datas",
        icon: "fa fa-database",
        items: [
            {
                name: "Example Datas",
                icon: "fa fa-database",
                url: ["/admin", "example_datas"],
                auth: {
                    requireUserType: UserTypes.IAmAdmin,
                    requirePrivileges: [Privileges.ExampleData_View]
                }
            },
        ]
    }
];
```

以上步骤都完成以后，重新编译前端并且刷新后台就可以看到新添加的页面了
