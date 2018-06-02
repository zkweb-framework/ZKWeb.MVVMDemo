import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { InputTextModule } from 'primeng/components/inputtext/inputtext';
import { PanelModule } from 'primeng/components/panel/panel';
import { ButtonModule } from 'primeng/components/button/button';
import { MessagesModule } from 'primeng/components/messages/messages';
import { BlockUIModule } from 'primeng/components/blockui/blockui';
import { DataTableModule } from 'primeng/components/datatable/datatable';
import { TabViewModule } from 'primeng/components/tabview/tabview';

import { BaseModule } from '../base_module/base.module';
import { GeneratedModule } from '../generated_module/generated.module';
import { AuthModule } from '../auth_module/auth.module';
import { AdminBaseModule } from '../admin_base_module/admin_base.module';

import { AuthGuard } from '../auth_module/auth/auth-guard';
import { UserTypes } from '../generated_module/privileges/user-types';

import { AdminAboutMeComponent } from './components/admin-about-me.component';
import { AdminAboutWebsiteComponent } from './components/admin-about-website.component';
import { AdminIndexComponent } from './components/admin-index.component';
import { AdminLoginComponent } from './components/admin-login.component';
import { AdminContainerComponent } from '../admin_base_module/components/admin-container.component';

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
        TabViewModule,
        BaseModule,
        GeneratedModule,
        AuthModule,
        AdminBaseModule,
        RouterModule.forChild(routes)
    ],
    declarations: [
        AdminAboutMeComponent,
        AdminAboutWebsiteComponent,
        AdminIndexComponent,
        AdminLoginComponent
    ],
    exports: [
        RouterModule,
    ]
})
export class AdminModule { }
