import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppApiService } from '../../base_module/services/app-api-service';
import { SessionInfoDto } from '../dtos/session-info-dto';

@Injectable()
/** 会话服务 */
export class SessionService {
    constructor(private appApiService: AppApiService) { }

    /** 获取当前的会话信息 */
    GetSessionInfo(): Observable<SessionInfoDto> {
        return this.appApiService.call<SessionInfoDto>(
            "/api/SessionService/GetSessionInfo",
            {
            });
    }
}
