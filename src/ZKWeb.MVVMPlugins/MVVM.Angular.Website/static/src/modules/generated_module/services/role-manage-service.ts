import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppApiService } from '../../base_module/services/app-api-service';

@Injectable()
/** 租户管理服务 */
export class RoleManageService {
	constructor(private appApiService: AppApiService) { }

}
