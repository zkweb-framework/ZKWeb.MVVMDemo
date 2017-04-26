import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
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

	add() {
		alert("add");
	}

	edit(obj: any) {
		alert("edit");
	}

	remove(obj: any) {
		alert("remove");
	}
}
