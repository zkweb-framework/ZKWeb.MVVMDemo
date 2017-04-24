import { AuthRequirement } from '../../auth_module/auth/auth-requirement';

/** 导航菜单项 */
export class NavMenuItem {
	/** 名称，可以是翻译前的名称 */
	name: string;
	/** 图标，可以是font awesome的class */
	icon: string;
	/** url地址 */
	url: string[];
	/** 要求的权限 */
	auth?: AuthRequirement;
}
