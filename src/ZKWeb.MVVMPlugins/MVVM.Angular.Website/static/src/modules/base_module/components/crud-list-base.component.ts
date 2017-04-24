import { Component, OnInit } from '@angular/core';
import { AppTranslationService } from '../../base_module/services/app-translation-service';
import { GridSearchRequestDto } from '../../generated_module/dtos/grid-search-request-dto';
import { GridSearchResponseDto } from '../../generated_module/dtos/grid-search-response-dto';
import { ActionResponseDto } from '../../generated_module/dtos/action-response-dto';
import { Message } from 'primeng/primeng';
import { Observable } from 'rxjs/Observable';

/** 增删查改的列表页的组件基类 */
export abstract class CrudListBaseComponent implements OnInit {
    /** 消息列表 */
    msgs: Message[] = [];
    /** 是否载入中 */
    loading = false;
    /** 是否重复载入，如果是需要在载入后重新刷新 */
    loadingDuplicated = false;
    /** 当前页数据 */
    value: any[] = [];
    /** 单页最多显示的数量 */
    rows = 10;
    /** 总记录数量 */
    totalRecords = 0;
    /** 单页数量的选项 */
    rowsPerPageOptions = [10, 25, 50, 100, 500];
    /** 找不到数据时的文本 */
    emptyMessage: string;

    abstract submitSearch(request: GridSearchRequestDto): Observable<GridSearchResponseDto>;
    abstract submitEdit(obj: any): Observable<ActionResponseDto>;
    abstract submitRemove(obj: any): Observable<ActionResponseDto>;
    abstract ngOnInit();

    constructor(protected appTranslationService: AppTranslationService) {
        this.emptyMessage = this.appTranslationService.translate("No records found");
    }

    /** 显示消息 */
    displayMessage(severity: string, detail: string) {
        this.msgs = [{ severity: severity, detail: detail }];
    }

	/** 搜索数据
		应该绑定表格的onLazyLoad事件
		例如(onLazyLoad)="search($event)" */
    search(e) {
        // 避免重复搜索，也可以防止ExpressionChangedAfterItHasBeenCheckedError
        if (this.loading) {
            return;
        }
        // 设置载入中
        console.log("search", e);
        this.loading = true;
        setTimeout(() => {
            this.displayMessage("info", "搜索完成");
            this.value = [];
            this.totalRecords = 100;
            this.loading = false;
        }, 3000);
    }

	/** 添加数据
		应该绑定添加按钮的点击事件
		例如 TODO */
    add() {
        alert("add");
    }

	/** 编辑数据
		应该绑定编辑按钮的点击事件
		例如 TODO */
    edit(obj: any) {
        alert("edit: " + JSON.stringify(obj));
    }

	/** 删除数据
		应该绑定删除按钮的点击事件
		例如 TODO */
    remove(obj: any) {
        alert("remove: " + JSON.stringify(obj));
    }
}
