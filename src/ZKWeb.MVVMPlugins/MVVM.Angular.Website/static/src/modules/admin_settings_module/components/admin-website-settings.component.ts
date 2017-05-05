import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Message } from 'primeng/primeng';
import { WebsiteManageService } from '../../generated_module/services/website-manage-service';

@Component({
    selector: 'admin-website-settings',
    templateUrl: '../views/admin-website-settings.html',
})
export class AdminWebsiteSettingsComponent implements OnInit {
    msgs: Message[] = [];
    isSubmitting = false;
    websiteSettingsForm = new FormGroup({
        WebsiteName: new FormControl('', Validators.required),
    });

    constructor(private websiteManageService: WebsiteManageService) {
    }

    ngOnInit() {
        this.websiteSettingsForm.reset();
        this.websiteManageService.GetWebsiteSettings().subscribe(
            s => this.websiteSettingsForm.patchValue(s),
            e => this.msgs = [{ severity: "error", detail: e }]);
    }

    onSubmit() {
        this.isSubmitting = true;
        this.websiteManageService.SaveWebsiteSettings(this.websiteSettingsForm.value).subscribe(
            s => {
                this.msgs = [{ severity: "info", detail: s.Message }];
                this.isSubmitting = false;
            },
            e => {
                this.msgs = [{ severity: "error", detail: e }];
                this.isSubmitting = true;
            });
    }
}
