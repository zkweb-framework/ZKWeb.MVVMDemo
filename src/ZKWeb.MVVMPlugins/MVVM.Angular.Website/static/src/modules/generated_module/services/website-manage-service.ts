import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppApiService } from '../../base_module/services/app-api-service';
import { ActionResponseDto } from '../dtos/action-response-dto';
import { WebsiteInfoOutputDto } from '../dtos/website-info-output-dto';

@Injectable()
/** 网站管理服务 */
export class WebsiteManageService {
	constructor(private appApiService: AppApiService) { }

	/** 清理缓存 */
	ClearCache(): Observable<ActionResponseDto> {
		return this.appApiService.call<ActionResponseDto>(
			"/api/WebsiteManageService/ClearCache",
			{
			});
	}

	/** 获取网站信息 */
	GetWebsiteInfo(): Observable<WebsiteInfoOutputDto> {
		return this.appApiService.call<WebsiteInfoOutputDto>(
			"/api/WebsiteManageService/GetWebsiteInfo",
			{
			});
	}
}
