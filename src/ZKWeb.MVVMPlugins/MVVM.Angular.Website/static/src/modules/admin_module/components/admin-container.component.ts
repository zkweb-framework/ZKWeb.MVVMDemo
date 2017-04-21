import { Component } from '@angular/core';
import { AdminNavMenu } from '../navigation/admin-nav-menu';

@Component({
	selector: 'admin-container',
	templateUrl: '../views/admin-container.html',
	styleUrls: [ '../styles/admin-container.scss' ]
})
export class AdminContainerComponent {
	logoUrl = require("../../../vendor/images/logo.png");
	activeMenuId: string;
	themesVisible: boolean = false;
	mobileMenuActive: boolean = false;

	toggleMenu(e) {
		this.mobileMenuActive = !this.mobileMenuActive;
		e.preventDefault();
	}
}
