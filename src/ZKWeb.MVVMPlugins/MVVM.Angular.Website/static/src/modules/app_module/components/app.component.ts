import { Component, OnInit, Injector } from '@angular/core';
import { AppTranslationService } from '../services/app-translation-service';

@Component({
	selector: 'my-app',
	templateUrl: '/modules/app_module/views/app.html'
})
export class AppComponent implements OnInit {
	constructor(private injector: Injector) {
	}

	ngOnInit() {
		var a = this.injector.get(AppTranslationService);
		console.log(a.translate("Index"));
	}
}
