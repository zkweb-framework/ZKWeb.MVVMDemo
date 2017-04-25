import { Component, OnInit } from '@angular/core';
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
	appTranslationService: AppTranslationService,
		private tenantManageService: TenantManageService) {
		super(appTranslationService);
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

	/** 提交编辑请求到服务器 */
	submitEdit(obj: any) {
		return this.tenantManageService.Edit(obj);
	}

	/** 提交删除请求道服务器 */
	submitRemove(obj: any) {
		return this.tenantManageService.Remove(obj);
	}
}
