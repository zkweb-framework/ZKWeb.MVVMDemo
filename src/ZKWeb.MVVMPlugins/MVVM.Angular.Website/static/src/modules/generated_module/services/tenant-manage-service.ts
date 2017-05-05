import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppApiService } from '../../base_module/services/app-api-service';
import { GridSearchResponseDto } from '../dtos/grid-search-response-dto';
import { GridSearchRequestDto } from '../dtos/grid-search-request-dto';
import { ActionResponseDto } from '../dtos/action-response-dto';
import { TenantInputDto } from '../dtos/tenant-input-dto';

@Injectable()
/** 租户管理服务 */
export class TenantManageService {
    constructor(private appApiService: AppApiService) { }

    /** 搜索租户 */
    Search(request: GridSearchRequestDto): Observable<GridSearchResponseDto> {
        return this.appApiService.call<GridSearchResponseDto>(
            "/api/TenantManageService/Search",
            {
                request
            });
    }

    /** 编辑租户 */
    Edit(dto: TenantInputDto): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/TenantManageService/Edit",
            {
                dto
            });
    }

    /** 删除租户 */
    Remove(id: string): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/TenantManageService/Remove",
            {
                id
            });
    }
}
