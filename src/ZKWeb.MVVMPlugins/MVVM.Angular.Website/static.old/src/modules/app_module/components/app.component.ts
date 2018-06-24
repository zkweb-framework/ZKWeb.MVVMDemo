import { Component } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';

@Component({
    selector: 'my-app',
    templateUrl: '../views/app.html'
})
export class AppComponent {
    routerActivated: boolean;

    constructor(private router: Router) {
        router.events.subscribe(e => {
            this.routerActivated = (e instanceof NavigationEnd);
        });
    }
}
