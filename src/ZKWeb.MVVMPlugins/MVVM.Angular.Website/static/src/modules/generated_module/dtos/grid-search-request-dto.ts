import { GridSearchColumnFilter } from './grid-search-column-filter';

/** 表格搜索请求 */
export class GridSearchRequestDto {
	/** 关键字 */
	public Keyword: string;
	/** 当前页，从0开始 */
	public Page: number;
	/** 单页数量 */
	public PageSize: number;
	/** 排序字段 */
	public OrderBy: string;
	/** 是否升序 */
	public Ascending: boolean;
	/** 列过滤条件 */
	public ColumnFilters: GridSearchColumnFilter[];
	/** 附加数据 */
	public Extra: any;
}
