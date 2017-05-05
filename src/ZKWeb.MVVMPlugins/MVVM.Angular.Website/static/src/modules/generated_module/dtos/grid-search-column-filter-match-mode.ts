/** 列过滤模式 */
export enum GridSearchColumnFilterMatchMode {
    /** 默认(包含过滤内容) */
    Default = 0,
    /** 以过滤内容开始 */
    StartsWith = 1,
    /** 以过滤内容结束 */
    EndsWith = 2,
    /** 等于过滤内容 */
    Equals = 3,
    /** 等于过滤值列表的任意一项 */
    In = 4
}
