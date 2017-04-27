import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppApiService } from '../../base_module/services/app-api-service';
import { GridSearchResponseDto } from '../dtos/grid-search-response-dto';
import { GridSearchRequestDto } from '../dtos/grid-search-request-dto';
import { ActionResponseDto } from '../dtos/action-response-dto';
import { UserInputDto } from '../dtos/user-input-dto';
import { UserTypeOutputDto } from '../dtos/user-type-output-dto';

@Injectable()
/** 用户管理服务 */
export class UserManageService {
	constructor(private appApiService: AppApiService) { }

	/** 搜索用户 */
	Search(request: GridSearchRequestDto): Observable<GridSearchResponseDto> {
		return this.appApiService.call<GridSearchResponseDto>(
			"/api/UserManageService/Search",
			{
				request
			});
	}

	/** 编辑用户 */
	Edit(dto: UserInputDto): Observable<ActionResponseDto> {
		return this.appApiService.call<ActionResponseDto>(
			"/api/UserManageService/Edit",
			{
				dto
			});
	}

	/** 删除用户 */
	Remove(id: string): Observable<ActionResponseDto> {
		return this.appApiService.call<ActionResponseDto>(
			"/api/UserManageService/Remove",
			{
				id
			});
	}

	/** 获取所有用户类型 */
	GetAllUserTypes(): Observable<UserTypeOutputDto[]> {
		return this.appApiService.call<UserTypeOutputDto[]>(
			"/api/UserManageService/GetAllUserTypes",
			{
			});
	}
}
