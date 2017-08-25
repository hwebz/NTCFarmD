import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { MessageCategory, MessageType, MessageItem, MessageDetail } from './administerModels';
import { messageStore } from './messageStore';

const getListMessagesUrl = "/api/message-admin/get-messages";
const getTotalMessageUrl = "/api/message-admin/get-totalmessage"

function toJavaScriptDate(value: string | Date): Date {
    if (!value) {
        return null;
    }

    if (value instanceof Date) {
        return value as Date;
    }

    var pattern = /Date\(([^)]+)\)/;
    var results = pattern.exec(value as string);
    var dt = new Date(parseFloat(results[1]));
    return dt;
}

/**
 * Query the total number of sent messages
 */
export function getTotalMessages(cateIds: number[], typeIds: number[]): Promise<number> {
    return new Promise<number>(function (resolve, reject) {
        var form = new FormData();
        typeIds.map((item, idx) => {
            form.append("messageTypes", item);
        });

        for (var cateId of cateIds) {
            form.append("categories", cateId);
        }
        fetch(getTotalMessageUrl, {
            method: "POST",
            body: form,
            credentials: 'same-origin',
            cache: "no-cache"
        }).then(rs => {
            if (rs.status == 401) {
                reject("Unauthorized");
                return;
            }
            return rs.json();
        }).then(result => {
            if (result == undefined) { return; }
            resolve(<any>result);
        })
    });
}

/**
 * Query the list of sent messages
 */
export function getMessages(cateIds: number[], typeIds: number[], currentPageNumber: number, pageSize: number): Promise<MessageItem[]> {
    var self = this;
    return new Promise<MessageItem[]>(function (resolve, reject) {
        var form = new FormData();
        form.append("pageSize", pageSize);
        form.append("pageIndex", currentPageNumber - 1);
        typeIds.map((item, idx) => {
            form.append("messageTypes", item);
        })

        for (var cateId of cateIds) {
            form.append("categories", cateId);
        }

        fetch(getListMessagesUrl, {
            method: "POST",
            body: form,
            credentials: 'same-origin',
            cache: "no-cache"
        }).then(rs => {
            if (rs.status == 401) {
                reject("Unauthorized");
                return;
            }
            return rs.json();
        }).then(result => {
            if (!result) { return; }
            for (var r of <any>result) {
                r.ValidFrom = toJavaScriptDate(r.ValidFrom);
                r.ValidTo = toJavaScriptDate(r.ValidTo);
                r.SendDate = toJavaScriptDate(r.SendDate);
            }
            resolve(<any>result);
        });
    });
}

/**
 * Get the detail of a message including list of receivers
 */
export function getMessageDetail(messageId: number): Promise<MessageDetail> {
    return new Promise<MessageDetail>(function (resolve, reject) {
        fetch(`/api/message-admin/message-detail/${messageId}`, {
            credentials: 'same-origin',
            cache: "no-cache"
        }).then(rp => {
            if (rp.status != 200) {
                reject("Failed to get the Message Detail");
                return;
            }
            return rp.json();
        }).then(rs => {
            var m = <any>rs;
            if (!m) { return; }
            var messageDetail = m as MessageDetail;
            messageDetail.Message.ValidFrom = toJavaScriptDate(m.Message.ValidFrom);
            messageDetail.Message.ValidTo = toJavaScriptDate(m.Message.ValidTo);
            messageDetail.Message.SendDate = toJavaScriptDate(m.Message.SendDate);
            for (var ei of messageDetail.ExtendedInfo) {
                ei.DeliveryDate = toJavaScriptDate(ei.DeliveryDate);
                ei.PlannedDeliveryDate = toJavaScriptDate(ei.PlannedDeliveryDate);
            }

            messageDetail.receivers = [];
            var receivers = !!m.Receivers ? m.Receivers : [];
            messageDetail.receivers = messageDetail.receivers.concat(receivers);
            resolve(messageDetail);
        });
    });
}

export function getFreeMessageCategories(): Promise<MessageCategory[]> {
    return new Promise<MessageCategory[]>((resolve, reject) => {
        fetch("/api/message-admin/free-categories", {
            credentials: 'same-origin',
            cache: "no-cache"
        }).then(r => {
            if (r.status != 200) {
                reject("FetchFailed");
                return;
            }
            return r.json();
        }).then(result => {
            if (!result) { return; }
            var categories: MessageCategory[] = [];
            for (var c of <any>result) {
                var category: MessageCategory = {
                    CategoryId: c.Categoryid,
                    CategoryName: c.CategoryName,
                    Types: []
                };

                categories.push(category);
            }
            resolve(categories);
        });
    });
}

/**
 * Get all possible message categories
 */
export function getAllCategories(showTipsAndInfo: boolean): Promise<MessageCategory[]> {
    return new Promise<MessageCategory[]>((resolve, reject) => {
        fetch(`/api/message-admin/all-categories?showTipsAndInfo=${showTipsAndInfo}`, {
            credentials: 'same-origin',
            cache: "no-cache"
        }).then(r => {
            if (r.status != 200) {
                reject("FetchFailed");
                return;
            }
            return r.json();
        }).then(result => {
            if (!result) { return; }
            var categories: MessageCategory[] = [];
            for (var c of <any>result) {
                var category: MessageCategory = {
                    CategoryId: c.Categoryid,
                    CategoryName: c.CategoryName,
                    Types: []
                };

                for (var t of c.MessageType) {
                    category.Types.push({
                        CategoryId: category.CategoryId,
                        TypeId: t.Id,
                        TypeName: t.Name
                    });
                }

                categories.push(category);
            }
            resolve(categories);
        });
    });
}

export function clearMessages() {
    messageStore.reset();
}

export function loadFilteredMessages(cateIds: number[], typeIds: number[], pageIndex: number, pageSize: number, isShowMore: boolean) {
    //clear if reload
    if (!isShowMore) {
        messageStore.replaceMessages(cateIds, typeIds, [], 0, true);
    } else {
        messageStore.concatMessages(cateIds, typeIds, [], 0, true);
    }

    let listMessages: MessageItem[] = [];

    var getMess = getMessages(cateIds, typeIds, pageIndex, pageSize)
        .then(messages => listMessages = messages);

    let totalItems = 0;
    var getTotalItems = getTotalMessages(cateIds, typeIds)
        .then(total => totalItems = total);

    Promise.all<any>([getMess, getTotalItems]).then(_ => {
        if (isShowMore) {
            messageStore.concatMessages(cateIds, typeIds, listMessages, totalItems, false);
            return;
        }
        messageStore.replaceMessages(cateIds, typeIds, listMessages, totalItems, false);
    });
}

export function changeFilter(cateIds: number[], typeIds: number[]) {
    messageStore.setCurrentFilter(cateIds, typeIds);
}
