import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import {
	InputTextModule,
	PanelModule,
	ButtonModule,
	MessagesModule,
	BlockUIModule
} from 'primeng/primeng';

import { BaseModule } from '../base_module/base.module';
import { GeneratedModule } from '../generated_module/generated.module';
import { AuthModule } from '../auth_module/auth.module';

import { AuthGuard } from '../auth_module/auth/auth-guard';

import { AdminContainerComponent } from './components/admin-container.component';
import { AdminIndexComponent } from './components/admin-index.component';
import { AdminLoginComponent } from './components/admin-login.component';

const routes: Routes = [
	{ path: '', component: AdminIndexComponent, pathMatch: 'full', canActivate: [AuthGuard] },
	{ path: 'login', component: AdminLoginComponent },
	// { path: 'tenants', loadChildren: '../admin_tenants_module/admin_tenants.module#AdminTenantsModule' },
	// { path: 'users', loadChildren: '../admin_users_module/admin.admin_users.module#AdminUsersModule' },
	// { path: 'roles', loadChildren: '../admin_roles_module/admin_roles.module#AdminRolesModule' },
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
		RouterModule.forChild(routes)
	],
	declarations: [
		AdminContainerComponent,
		AdminIndexComponent,
		AdminLoginComponent
	],
	exports: [
		RouterModule
	]
})
export class AdminModule { }
