import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppApiService } from '../../base_module/services/app-api-service';
import { GridSearchResponseDto } from '../dtos/grid-search-response-dto';
import { GridSearchRequestDto } from '../dtos/grid-search-request-dto';
import { ActionResponseDto } from '../dtos/action-response-dto';
import { ExampleDataInputDto } from '../dtos/example-data-input-dto';

@Injectable()
/** 示例数据管理服务 */
export class ExampleDataManageService {
    constructor(private appApiService: AppApiService) { }

    /** 搜索数据 */
    Search(request: GridSearchRequestDto): Observable<GridSearchResponseDto> {
        return this.appApiService.call<GridSearchResponseDto>(
            "/api/ExampleDataManageService/Search",
            {
                request
            });
    }

    /** 编辑数据 */
    Edit(dto: ExampleDataInputDto): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/ExampleDataManageService/Edit",
            {
                dto
            });
    }

    /** 删除数据 */
    Remove(id: string): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/ExampleDataManageService/Remove",
            {
                id
            });
    }
}
