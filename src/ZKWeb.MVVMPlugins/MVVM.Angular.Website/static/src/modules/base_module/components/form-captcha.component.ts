import { Component, Input, EventEmitter } from '@angular/core';
import { FormFieldBaseComponent } from './form-field-base.component';
import { CaptchaService } from '../../generated_module/services/captcha-service';
import { AppTranslationService } from '../../base_module/services/app-translation-service';

@Component({
    selector: 'z-form-captcha',
    templateUrl: '../views/form-captcha.html',
    host: { 'class': 'ui-grid-row' }
})
export class FormCaptchaComponent extends FormFieldBaseComponent {
    @Input() captchaKey: string;
    @Input() captchaGridWidth: number = 3;
    @Input() captchaRefreshEvent: EventEmitter<any>;
    captchaImageBase64: string;
    captchaLoadingText: string;

    constructor(
        appTranslationService: AppTranslationService,
        private captchaService: CaptchaService) {
        super(appTranslationService);
    }

    ngOnInit() {
        super.ngOnInit();
        // 加载时刷新验证码
        this.refreshCaptcha();
        // 外部刷新验证码
        if (this.captchaRefreshEvent) {
            this.captchaRefreshEvent.subscribe(_ => this.refreshCaptcha());
        }
    }

    /** 刷新验证码 */
    refreshCaptcha() {
        this.captchaImageBase64 = null;
        this.captchaLoadingText = this.appTranslationService.translate("Loading");
        this.captchaService.GetCaptchaImageBase64(this.captchaKey).subscribe(
            s => this.captchaImageBase64 = s,
            e => this.captchaLoadingText = this.appTranslationService.translate("Load Failed"));
    }
}
