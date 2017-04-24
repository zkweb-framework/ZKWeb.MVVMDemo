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

import { AuthGuard } from '../auth_module/auth/auth-guard';
import { UserTypes } from '../generated_module/privileges/user-types';

import { AdminContainerComponent } from './components/admin-container.component';

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
		RouterModule
	],
	declarations: [
		AdminContainerComponent,
	],
	exports: [
		RouterModule,
		AdminContainerComponent
	]
})
export class AdminBaseModule { }
