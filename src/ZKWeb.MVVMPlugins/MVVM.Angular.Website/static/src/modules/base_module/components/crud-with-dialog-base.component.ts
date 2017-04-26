import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup } from '@angular/forms';
import { ConfirmationService } from 'primeng/primeng';
import { AppTranslationService } from '../../base_module/services/app-translation-service';
import { GridSearchRequestDto } from '../../generated_module/dtos/grid-search-request-dto';
import { GridSearchResponseDto } from '../../generated_module/dtos/grid-search-response-dto';
import { GridSearchColumnFilter } from '../../generated_module/dtos/grid-search-column-filter';
import { GridSearchColumnFilterMatchMode } from '../../generated_module/dtos/grid-search-column-filter-match-mode';
import { ActionResponseDto } from '../../generated_module/dtos/action-response-dto';
import { AuthRequirement } from '../../auth_module/auth/auth-requirement';
import { AppPrivilegeService } from '../../auth_module/services/app-privilege-service';
import { AppSessionService } from '../../auth_module/services/app-session-service';
import { CrudBaseComponent } from './crud-base.component';
import { Message } from 'primeng/primeng';
import { Observable } from 'rxjs/Observable';

/** 使用弹出框的增删查改页的组件基类 */
export abstract class CrudWithDialogBaseComponent extends CrudBaseComponent {
	/** 编辑弹出框是否可见 */
	editDialogVisible = false;
	/** 确认是否删除使用的消息模板 */
	removeConfirmTemplate = "Are you sure to remove '{0}'?";
	/** 编辑表单 */
	editForm = new FormGroup({});
	/** 编辑表单是否正在提交 */
	editFormSubmitting = false;
	/** 编辑表单使用的消息列表 */
	editMsgs = [];

	/** 提交编辑到服务器 */
	abstract submitEdit(obj: any): Observable<ActionResponseDto>;
	/** 提交删除到服务器 */
	abstract submitRemove(obj: any): Observable<ActionResponseDto>;

	constructor(
		protected confirmationService: ConfirmationService,
		appSessionService: AppSessionService,
		appPrivilegeService: AppPrivilegeService,
		appTranslationService: AppTranslationService) {
		super(appSessionService, appPrivilegeService, appTranslationService);
	}

	/** 在编辑表单显示消息 */
	displayEditMessage(severity: string, detail: string) {
		this.editMsgs = [{ severity: severity, detail: this.appTranslationService.translate(detail) }];
	}

	/** 添加数据 */
	add() {
		this.editForm.reset();
		this.editDialogVisible = true;
	}

	/** 编辑数据 */
	edit(obj: any) {
		this.editForm.reset();
		this.editForm.patchValue(obj);
		this.editDialogVisible = true;
	}

	/* 关闭编辑框 */
	editDialogClose() {
		this.editForm.reset();
		this.editDialogVisible = false;
	}

	/* 提交编辑表单 */
	editDialogSubmit() {
		this.editFormSubmitting = true;
		console.log("submit form", JSON.parse(JSON.stringify(this.editForm.value)));
		this.submitEdit(this.editForm.value).subscribe(
			s => {
				this.displayMessage("info", s.Message);
				this.searchWithLastParameters();
				this.editFormSubmitting = false;
				this.editDialogVisible = false;
			},
			e => {
				this.displayEditMessage("error", e);
				this.editFormSubmitting = false;
			});
	}

	/** 删除数据 */
	remove(obj: any) {
		// 弹出确认框
		var name = obj.Name || obj.DisplayName || obj.Username || obj.Title || obj.Serial;
		var confirmMessage = this.appTranslationService.translate(this.removeConfirmTemplate)
			.replace("{0}", name);
		this.confirmationService.confirm({
			message: confirmMessage,
			accept: () => {
				this.submitRemove(obj).subscribe(
					s => {
						this.displayMessage("info", s.Message);
						this.searchWithLastParameters();
					},
					e => this.displayMessage("error", e));
			}
		});
	}
}
