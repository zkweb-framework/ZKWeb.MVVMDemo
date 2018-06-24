import { AuthRequirement } from '../../auth_module/auth/auth-requirement';
import { NavMenuItem } from './nav-menu-item';

/** 导航菜单组 */
export class NavMenuGroup {
    /** 名称，可以是翻译前的名称 */
    name: string;
    /** 图标，可以是font awesome的class */
    icon: string;
    /** 菜单项列表 */
    items: NavMenuItem[];
    /** Url地址，如果有url时会把这个组当成是单独的菜单 */
    url?: string[];
    /** 要求的权限 */
    auth?: AuthRequirement;
}
