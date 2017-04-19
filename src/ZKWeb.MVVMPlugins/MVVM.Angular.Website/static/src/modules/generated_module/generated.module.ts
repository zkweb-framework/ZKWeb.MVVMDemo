import { NgModule } from '@angular/core';
import { BaseModule } from '../base_module/base.module';
import { ExampleService } from './services/example-service';
import { SessionService } from './services/session-service';

@NgModule({
	imports: [BaseModule],
	providers: [
		ExampleService,
		SessionService
	]
})
export class GeneratedModule { }
