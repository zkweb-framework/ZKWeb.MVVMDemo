// import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from './modules/app_module/app.module';

// 启用生产模式可以取消注释以下行
// enableProdMode();

platformBrowserDynamic().bootstrapModule(AppModule);
