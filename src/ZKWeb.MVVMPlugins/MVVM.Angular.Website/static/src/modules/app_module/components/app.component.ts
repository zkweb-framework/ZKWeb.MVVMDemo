import { Component } from '@angular/core';
import {
    Router,
    NavigationStart,
    NavigationEnd,
    NavigationCancel,
    NavigationError
} from '@angular/router';

@Component({
    selector: 'my-app',
    templateUrl: '../views/app.html'
})
export class AppComponent {
    routerActivated: boolean;

    constructor(private router: Router) {
        router.events.subscribe(e => {
            if (e instanceof NavigationStart) {
                this.routerActivated = false;
            } else if ((e instanceof NavigationEnd) ||
                (e instanceof NavigationCancel) ||
                (e instanceof NavigationError)) {
                this.routerActivated = true;
            }
        });
    }
}
