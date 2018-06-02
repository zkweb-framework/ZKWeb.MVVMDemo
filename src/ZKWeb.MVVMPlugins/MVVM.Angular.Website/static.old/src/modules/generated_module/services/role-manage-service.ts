import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppApiService } from '../../base_module/services/app-api-service';
import { GridSearchResponseDto } from '../dtos/grid-search-response-dto';
import { GridSearchRequestDto } from '../dtos/grid-search-request-dto';
import { ActionResponseDto } from '../dtos/action-response-dto';
import { RoleInputDto } from '../dtos/role-input-dto';
import { RoleOutputDto } from '../dtos/role-output-dto';

@Injectable()
/** 角色管理服务 */
export class RoleManageService {
    constructor(private appApiService: AppApiService) { }

    /** 搜索角色 */
    Search(request: GridSearchRequestDto): Observable<GridSearchResponseDto> {
        return this.appApiService.call<GridSearchResponseDto>(
            "/api/RoleManageService/Search",
            {
                request
            });
    }

    /** 编辑角色 */
    Edit(dto: RoleInputDto): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/RoleManageService/Edit",
            {
                dto
            });
    }

    /** 删除角色 */
    Remove(id: string): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/RoleManageService/Remove",
            {
                id
            });
    }

    /** 获取所有角色 */
    GetAllRoles(): Observable<RoleOutputDto[]> {
        return this.appApiService.call<RoleOutputDto[]>(
            "/api/RoleManageService/GetAllRoles",
            {
            });
    }
}
