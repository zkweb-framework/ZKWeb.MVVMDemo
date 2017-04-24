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

import { AdminTenantEditComponent } from './components/admin-tenant-edit.component';
import { AdminTenantListComponent } from './components/admin-tenant-list.component';

const routes: Routes = [
	{
		path: '',
		component: AdminTenantListComponent,
		pathMatch: 'full',
		canActivate: [AuthGuard],
		data: { auth: { requireMasterTenant: true, requireUserType: UserTypes.IAmAdmin } }
	},
	{
		path: 'edit/:id', component: AdminTenantEditComponent,
		canActivate: [AuthGuard],
		data: { auth: { requireMasterTenant: true, requireUserType: UserTypes.IAmAdmin } }
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
		AdminTenantEditComponent,
		AdminTenantListComponent,
	],
	exports: [
		RouterModule
	]
})
export class AdminTenantsModule { }
