import { Component, OnInit, Injector } from '@angular/core';

@Component({
	selector: 'my-app',
	templateUrl: '../views/app.html'
})
export class AppComponent {
	routerActivated: boolean;

	onRouterActivate() {
		this.routerActivated = true;
		console.log("router activated");
	}

	onRouterDeactivate() {
		this.routerActivated = false;
		console.log("router deactivated");
	}
}
