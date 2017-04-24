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
import { UserTypes } from '../generated_module/privileges/user-types';

import { AdminRoleEditComponent } from './components/admin-role-edit.component';
import { AdminRoleListComponent } from './components/admin-role-list.component';

const routes: Routes = [
	{
		path: '',
		component: AdminRoleListComponent,
		pathMatch: 'full',
		canActivate: [AuthGuard],
		data: { auth: { requireUserType: UserTypes.IAmAdmin } }
	},
	{
		path: 'edit/:id', component: AdminRoleEditComponent,
		canActivate: [AuthGuard],
		data: { auth: { requireUserType: UserTypes.IAmAdmin } }
	},
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
		AdminRoleEditComponent,
		AdminRoleListComponent,
	],
	exports: [
		RouterModule
	]
})
export class AdminRolesModule { }
