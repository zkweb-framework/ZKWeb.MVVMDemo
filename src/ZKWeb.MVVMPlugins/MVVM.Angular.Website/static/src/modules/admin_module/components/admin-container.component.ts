import { Component } from '@angular/core';

@Component({
	selector: 'admin-container',
	templateUrl: '../views/admin-container.html',
	styleUrls: [ '../styles/admin-container.scss' ]
})
export class AdminContainerComponent {
	activeMenuId: string;
	themesVisible: boolean = false;
	mobileMenuActive: boolean = false;

	toggleMenu(e) {
		this.mobileMenuActive = !this.mobileMenuActive;
		e.preventDefault();
	}
}
