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
} from 'primeng/primeng';

import { BaseModule } from '../base_module/base.module';
import { GeneratedModule } from '../generated_module/generated.module';
import { AuthModule } from '../auth_module/auth.module';
import { AdminBaseModule } from '../admin_base_module/admin_base.module';

import { AuthGuard } from '../auth_module/auth/auth-guard';
import { UserTypes } from '../generated_module/privileges/user-types';
import { Privileges } from '../generated_module/privileges/privileges';

import { AdminWebsiteSettingsComponent } from './components/admin-website-settings.component';

const routes: Routes = [
    { path: '', redirectTo: 'website_settings', pathMatch: 'full' },
    {
        path: 'website_settings',
        component: AdminWebsiteSettingsComponent,
        canActivate: [AuthGuard],
        data: {
            auth: {
                requireUserType: UserTypes.IAmAdmin,
                requirePrivileges: [Privileges.Settings_WebsiteSettings]
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
        BaseModule,
        GeneratedModule,
        AuthModule,
        AdminBaseModule,
        RouterModule.forChild(routes)
    ],
    declarations: [
        AdminWebsiteSettingsComponent,
    ],
    exports: [
        RouterModule
    ]
})
export class AdminSettingsModule { }
