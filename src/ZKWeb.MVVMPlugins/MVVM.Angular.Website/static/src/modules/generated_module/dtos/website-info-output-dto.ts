import { PluginInfoOutputDto } from './plugin-info-output-dto';

/** 网站信息 */
export class WebsiteInfoOutputDto {
    /** ZKWeb版本 */
    public ZKWebVersion: string;
    /** ZKWeb完整版本 */
    public ZKWebFullVersion: string;
    /** 使用内存 */
    public MemoryUsage: string;
    /** 插件列表 */
    public Plugins: PluginInfoOutputDto[];
}
