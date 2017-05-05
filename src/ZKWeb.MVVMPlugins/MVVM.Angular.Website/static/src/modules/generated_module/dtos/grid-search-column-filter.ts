import { GridSearchColumnFilterMatchMode } from './grid-search-column-filter-match-mode';

/** 列过滤信息 */
export class GridSearchColumnFilter {
    /** 列名 */
    public Column: string;
    /** 匹配模式 */
    public MatchMode: GridSearchColumnFilterMatchMode;
    /** 过滤值 */
    public Value: any;
}
