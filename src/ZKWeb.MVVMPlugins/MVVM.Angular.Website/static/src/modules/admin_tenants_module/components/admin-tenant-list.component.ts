import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SelectItem } from 'primeng/primeng';
import { CrudListBaseComponent } from '../../base_module/components/crud-list-base.component'
import { GridSearchRequestDto } from '../../generated_module/dtos/grid-search-request-dto';
import { GridSearchResponseDto } from '../../generated_module/dtos/grid-search-response-dto';
import { AppTranslationService } from '../../base_module/services/app-translation-service';
import { TenantManageService } from '../../generated_module/services/tenant-manage-service';

@Component({
	selector: 'admin-tenant-list',
	templateUrl: '../views/admin-tenant-list.html'
})
export class AdminTenantListComponent extends CrudListBaseComponent {
	isMasterOptions: SelectItem[];

	constructor(
		router: Router,
		appTranslationService: AppTranslationService,
		private tenantManageService: TenantManageService) {
		super(router, appTranslationService);
	}

	ngOnInit() {
		this.isMasterOptions = [
			{ label: this.appTranslationService.translate("Please Select"), value: null },
			{ label: this.appTranslationService.translate("Yes"), value: true },
			{ label: this.appTranslationService.translate("No"), value: false },
		];
	}

	/** 提交搜索请求到服务器 */
	submitSearch(request: GridSearchRequestDto) {
		return this.tenantManageService.Search(request);
	}

	/** 获取添加地址 */
	getAddUrl() {
		return ["/admin", "tenants", "add"];
	}

	/** 获取编辑地址 */
	getEditUrl(obj: any) {
		return ["/admin", "tenants", "edit", obj.Id];
	}

	/** 提交删除请求到服务器 */
	submitRemove(obj: any) {
		return this.tenantManageService.Remove(obj);
	}
}
