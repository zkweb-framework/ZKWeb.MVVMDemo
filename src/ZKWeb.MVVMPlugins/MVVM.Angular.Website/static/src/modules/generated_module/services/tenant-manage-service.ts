import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppApiService } from '../../base_module/services/app-api-service';
import { GridSearchResponseDto } from '../dtos/grid-search-response-dto';
import { GridSearchRequestDto } from '../dtos/grid-search-request-dto';

@Injectable()
/** 租户管理服务 */
export class TenantManageService {
	constructor(private appApiService: AppApiService) { }

	/** 搜索租户 */
	Search(dto: GridSearchRequestDto): Observable<GridSearchResponseDto> {
		return this.appApiService.call<GridSearchResponseDto>(
			"/api/TenantManageService/Search",
			{
				dto
			});
	}
}
