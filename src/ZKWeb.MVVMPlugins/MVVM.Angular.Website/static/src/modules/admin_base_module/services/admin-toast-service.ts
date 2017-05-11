import { Injectable } from '@angular/core';
import { Message } from 'primeng/components/common/api';

/** 在后台页面显示悬浮信息的服务 */
@Injectable()
export class AdminToastService {
    /** 悬浮信息列表 */
    private toastMessages: Message[] = [];

    /** 获取悬浮信息列表 */
    getToastMessages(): Message[] {
        return this.toastMessages;
    }

    /** 清理所有悬浮消息 */
    clearToastMessages() {
        while (this.toastMessages.length > 0) {
            this.toastMessages.pop();
        }
    }

    /** 显示悬浮信息，显示前清理之前的消息 */
    showToastMessage(severity: string, detail: string) {
        this.clearToastMessages();
        this.toastMessages.push({ severity: severity, detail: detail });
    }

    /** 添加悬浮消息，之前的悬浮消息会保留 */
    appendToastMessage(severity: string, detail: string) {
        this.toastMessages.push({ severity: severity, detail: detail });
    }
}
