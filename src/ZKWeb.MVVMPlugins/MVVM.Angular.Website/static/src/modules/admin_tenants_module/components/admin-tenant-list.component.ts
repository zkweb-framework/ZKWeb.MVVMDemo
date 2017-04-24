import { Component, OnInit } from '@angular/core';
import { SelectItem } from 'primeng/primeng';
import { CrudListBaseComponent } from '../../base_module/components/crud-list-base.component'
import { GridSearchRequestDto } from '../../generated_module/dtos/grid-search-request-dto';
import { GridSearchResponseDto } from '../../generated_module/dtos/grid-search-response-dto';

@Component({
    selector: 'admin-tenant-list',
    templateUrl: '../views/admin-tenant-list.html'
})
export class AdminTenantListComponent extends CrudListBaseComponent {
    isMasterOptions: SelectItem[];

    ngOnInit() {
        this.isMasterOptions = [
            { label: this.appTranslationService.translate("Please Select"), value: null },
            { label: this.appTranslationService.translate("Yes"), value: true },
            { label: this.appTranslationService.translate("No"), value: false },
        ];
    }

    submitSearch(request: GridSearchRequestDto) {
        return null;
    }

    submitEdit(obj: any) {
        return null;
    }

    submitRemove(obj: any) {
        return null;
    }
}
