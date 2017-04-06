import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { AdminModule } from '../admin_module/admin.module';
import { AppComponent } from './components/app.component';

@NgModule({
	imports: [
		BrowserModule,
		AdminModule
	],
	declarations: [
		AppComponent
	],
	bootstrap: [
		AppComponent
	]
})
export class AppModule { }
