import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppApiService } from '../../base_module/services/app-api-service';

@Injectable()
/** 验证码服务 */
export class CaptchaService {
    constructor(private appApiService: AppApiService) { }

    /** 获取验证码图片的Base64 */
    GetCaptchaImageBase64(key: string): Observable<string> {
        return this.appApiService.call<string>(
            "/api/CaptchaService/GetCaptchaImageBase64",
            {
                key
            });
    }
}
