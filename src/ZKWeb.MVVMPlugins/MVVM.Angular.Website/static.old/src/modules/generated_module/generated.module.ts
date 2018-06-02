import { NgModule } from '@angular/core';
import { BaseModule } from '../base_module/base.module';
import { CaptchaService } from './services/captcha-service';
import { TenantManageService } from './services/tenant-manage-service';
import { UserManageService } from './services/user-manage-service';
import { RoleManageService } from './services/role-manage-service';
import { WebsiteManageService } from './services/website-manage-service';
import { SessionService } from './services/session-service';
import { UserProfileService } from './services/user-profile-service';
import { UserLoginService } from './services/user-login-service';
import { ExampleDataManageService } from './services/example-data-manage-service';

@NgModule({
    imports: [BaseModule],
    providers: [
        CaptchaService,
        TenantManageService,
        UserManageService,
        RoleManageService,
        WebsiteManageService,
        SessionService,
        UserProfileService,
        UserLoginService,
        ExampleDataManageService
    ]
})
export class GeneratedModule { }
