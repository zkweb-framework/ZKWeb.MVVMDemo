import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './components/app.component';
import { MainSidebarComponent } from './components/main-sidebar.component';
import { ControlSidebarComponent } from './components/control-sidebar.component';
import { HeaderComponent } from './components/header.component';
import { HomeComponent } from './components/home.component';

@NgModule({
	imports: [
		BrowserModule
	],
	declarations: [
		AppComponent,
		MainSidebarComponent,
		ControlSidebarComponent,
		HeaderComponent,
		HomeComponent
	],
	bootstrap: [
		AppComponent
	]
})
export class AppModule { }
