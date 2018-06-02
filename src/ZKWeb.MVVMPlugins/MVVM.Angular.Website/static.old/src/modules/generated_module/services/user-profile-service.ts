import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppApiService } from '../../base_module/services/app-api-service';
import { ActionResponseDto } from '../dtos/action-response-dto';
import { UserChangePasswordInputDto } from '../dtos/user-change-password-input-dto';
import { UserUploadAvatarInputDto } from '../dtos/user-upload-avatar-input-dto';

@Injectable()
/** 用户资料服务 */
export class UserProfileService {
    constructor(private appApiService: AppApiService) { }

    /** 修改密码 */
    ChangePassword(dto: UserChangePasswordInputDto): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/UserProfileService/ChangePassword",
            {
                dto
            });
    }

    /** 上传头像 */
    UploadAvatar(dto: UserUploadAvatarInputDto): Observable<ActionResponseDto> {
        return this.appApiService.call<ActionResponseDto>(
            "/api/UserProfileService/UploadAvatar",
            {
                dto
            });
    }
}
