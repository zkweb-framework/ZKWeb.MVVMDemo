import { Component, OnInit, Injector } from '@angular/core';
import { AppTranslationService } from '../../base_module/services/app-translation-service';

@Component({
	selector: 'my-app',
	templateUrl: '/modules/app_module/views/app.html'
})
export class AppComponent implements OnInit {
	constructor(private injector: Injector) {
	}

	ngOnInit() {
        // TODO: 删除这里的测试代码
		var a = this.injector.get(AppTranslationService);
		console.log(a.translate("Index"));
	}
}
