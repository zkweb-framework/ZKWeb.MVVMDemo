import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppApiService } from '../../base_module/services/app-api-service';
import { ActionResponseDto } from '../dtos/action-response-dto';
import { UserLoginRequestDto } from '../dtos/user-login-request-dto';

@Injectable()
// 用户登录服务
export class UserLoginService {
	constructor(private appApiService: AppApiService) { }

	// 获取当前的会话信息
	Login(request: UserLoginRequestDto): Observable<ActionResponseDto> {
		return this.appApiService.call<ActionResponseDto>(
			"/api/UserLoginService/Login",
			{
				request
			});
	}
}
