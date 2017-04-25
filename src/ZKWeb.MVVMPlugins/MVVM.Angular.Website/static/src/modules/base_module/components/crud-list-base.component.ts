import { Component, OnInit } from '@angular/core';
import { AppTranslationService } from '../../base_module/services/app-translation-service';
import { GridSearchRequestDto } from '../../generated_module/dtos/grid-search-request-dto';
import { GridSearchResponseDto } from '../../generated_module/dtos/grid-search-response-dto';
import { GridSearchColumnFilter } from '../../generated_module/dtos/grid-search-column-filter';
import { GridSearchColumnFilterMatchMode } from '../../generated_module/dtos/grid-search-column-filter-match-mode';
import { ActionResponseDto } from '../../generated_module/dtos/action-response-dto';
import { Message } from 'primeng/primeng';
import { Observable } from 'rxjs/Observable';

/** 增删查改的列表页的组件基类 */
export abstract class CrudListBaseComponent implements OnInit {
	/** 消息列表 */
	msgs: Message[] = [];
	/** 是否载入中 */
	loading = false;
	/** 是否重复搜索，如果是需要在上一次结束后重新开始 */
	loadingDuplicated = false;
	/** 延迟搜索的句柄 */
	delaySearchHandler: any;
	/** 延迟搜索的时间 */
	delaySearchInterval = 500;
	/** 搜索条件的Json，防止多余搜索使用 */
	searchConditionJson = null;
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

	/** 提交搜索请求到服务器 */
	abstract submitSearch(request: GridSearchRequestDto): Observable<GridSearchResponseDto>;
	/** 提交编辑请求到服务器 */
	abstract submitEdit(obj: any): Observable<ActionResponseDto>;
	/** 提交删除请求道服务器 */
	abstract submitRemove(obj: any): Observable<ActionResponseDto>;
	/** 初始化时的处理 */
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
	search(e, noDelay = false) {
		// 检查是否多余搜索
		// 如果搜索条件和上次的一致则跳过搜索
		var json = JSON.stringify(e);
		if (json == this.searchConditionJson) {
			return;
		}
		e = JSON.parse(json);
		// 延迟搜索
		if (!noDelay) {
			if (this.delaySearchHandler) {
				clearTimeout(this.delaySearchHandler);
			}
			this.delaySearchHandler = setTimeout(() => this.search(e, true), 100);
			return;
		}
		// 检测是否重复搜索
		// 注意搜索在开发模式会引发ExpressionChangedAfterItHasBeenCheckedError错误，只能切换到生成模式避免
		// https://github.com/angular/angular/issues/6005
		if (this.loading) {
			this.loadingDuplicated = true;
			return;
		}
		// 设置载入中
		this.loading = true;
		this.searchConditionJson = json;
		console.log("search datatable", e);
		// 构建搜索请求
		var request = new GridSearchRequestDto();
		request.Keyword = e.globalFilter;
		request.Page = e.first / e.rows;
		request.PageSize = e.rows;
		request.OrderBy = e.sortField;
		request.Ascending = e.sortOrder > 0;
		request.ColumnFilters = [];
		var filters = e.filters || {};
		for (var key in filters) {
			if (!filters.hasOwnProperty(key)) {
				continue;
			}
			var value = filters[key];
			var columnFilter = new GridSearchColumnFilter();
			columnFilter.Column = key;
			columnFilter.MatchMode = GridSearchColumnFilterMatchMode.Default;
			if (value.matchMode == "startsWith") {
				columnFilter.MatchMode = GridSearchColumnFilterMatchMode.StartsWith;
			} else if (value.matchMode == "endsWidth") {
				columnFilter.MatchMode = GridSearchColumnFilterMatchMode.EndsWith;
			} else if (value.matchMode == "equals") {
				columnFilter.MatchMode = GridSearchColumnFilterMatchMode.Equals;
			} else if (value.matchMode = "in") {
				columnFilter.MatchMode = GridSearchColumnFilterMatchMode.In;
			}
			columnFilter.Value = value.value;
			request.ColumnFilters.push(columnFilter);
		}
		// 调用搜索函数
		var setSearchResult = (value: any[], totalRecords: number) => {
			this.value = value;
			this.totalRecords = totalRecords;
			// 设置已载入
			this.loading = false;
			// 如果在搜索途中条件有改变需要再搜索一次
			if (this.loadingDuplicated) {
				this.loadingDuplicated = false;
				setTimeout(() => this.search(e), 1);
			}
		};
		this.submitSearch(request).subscribe(
			r => setSearchResult(r.Result, r.TotalCount),
			e => {
				this.displayMessage("error", e);
				setSearchResult([], 0);
			});
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
