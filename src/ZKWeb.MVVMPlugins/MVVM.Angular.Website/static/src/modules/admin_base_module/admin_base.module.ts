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
	GrowlModule
} from 'primeng/primeng';

import { BaseModule } from '../base_module/base.module';
import { GeneratedModule } from '../generated_module/generated.module';

import { AuthGuard } from '../auth_module/auth/auth-guard';
import { UserTypes } from '../generated_module/privileges/user-types';

import { AdminContainerComponent } from './components/admin-container.component';
import { AdminToastService } from './services/admin-toast-service';

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
		GrowlModule,
		BaseModule,
		GeneratedModule,
		RouterModule
	],
	declarations: [
		AdminContainerComponent,
	],
	providers: [
		AdminToastService
	],
	exports: [
		RouterModule,
		AdminContainerComponent
	]
})
export class AdminBaseModule { }
