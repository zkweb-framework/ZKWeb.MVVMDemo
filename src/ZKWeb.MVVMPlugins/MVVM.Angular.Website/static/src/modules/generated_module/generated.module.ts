import { NgModule } from '@angular/core';
import { ExampleService } from './services/example-service';
import { SessionService } from './services/session-service';

@NgModule({
	providers: [
		ExampleService,
		SessionService
	]
})
export class GeneratedModule { }
