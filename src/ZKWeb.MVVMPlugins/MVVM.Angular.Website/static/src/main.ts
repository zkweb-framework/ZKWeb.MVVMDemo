import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from './modules/app_module/app.module';

enableProdMode();

platformBrowserDynamic().bootstrapModule(AppModule);
