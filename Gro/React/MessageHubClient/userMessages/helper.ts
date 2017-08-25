import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Router, Route, IndexRoute, Link, browserHistory, IndexLink } from 'react-router';
import { Category } from './model/categoryModel';
import { MessageSimple, Message } from './model/messageModel';

var $ = window["$"];

export class Helper {
    //format params as "#cat1#cat2#cat3"
    buildCategoryParams(sourceCats: Category[]): string {
        var catParams = "";

        if (sourceCats && sourceCats.length != 0) {
            for (let cat of sourceCats) {
                if (cat.isSelected)
                    catParams = catParams + ";" + cat.id;
            }
        }

        return catParams;
    }

    //format params as "#cat1#cat2#cat3"
    buildMessageParams(sourceMsgs: Message[]): string {
        var msgParams = "";

        if (sourceMsgs && sourceMsgs.length != 0) {
            for (let msg of sourceMsgs) {
                msgParams = msgParams + ";" + msg.id;
            }
        }

        return msgParams;
    }

    getCategoriesByMessageStatus(newCategories: Category[], sourceCategories: Category[]): Category[] {
        var resultCategories = [];
        if (!newCategories) return sourceCategories;
        if (!sourceCategories) return resultCategories;

        for (let cat of sourceCategories) {
            var result = newCategories.find(function (item) {
                return item.id == cat.id;
            }, this);

            if (result != undefined) {
                resultCategories.push(cat);
            } else {
                cat.isSelected = false;
            }
        }

        return resultCategories;
    }

    findIndexOfMessage(messages: MessageSimple[], searchMessage: MessageSimple): number {
        if (messages === undefined || searchMessage === undefined) return -1;

        for (var i = 0; i < messages.length; i++) {
            if (messages[i] && messages[i].id === searchMessage.id) {
                return i;
            }
        }

        return -1;
    }

    findIndexOfCategory(categories: Category[], searchCategory: Category): number {
        if (!categories || !searchCategory) return -1;

        for (var i = 0; i < categories.length; i++) {
            if (categories[i] && categories[i].id === searchCategory.id) {
                return i;
            }
        }

        return -1;
    }

    updateTotalUnread(unRead: boolean, countNumber: number): void {
        try {
            var totalUnreadElement = document.getElementById('totalUnread');
            var totalUnread = parseInt(totalUnreadElement.innerHTML);
            if (isNaN(totalUnread)) totalUnread = 0;
            totalUnread = unRead ? totalUnread + countNumber : (totalUnread > 0 ? totalUnread - countNumber : totalUnread);
            if (totalUnread > 0) {
                totalUnreadElement.innerHTML = totalUnread.toString();
                totalUnreadElement.className = "lm__messages-count";
            } else {
                totalUnreadElement.innerHTML = '';
                totalUnreadElement.className = '';
            }
        } catch (ex) {
            console.log('can not update the totalUnread');
        }
    }

    updateTotalStarred(starred: boolean, countNumber: number): void {
        try {
            var totalStarredElement = document.getElementById('totalStarred');
            var totalStarred = parseInt(totalStarredElement.innerHTML);
            if (isNaN(totalStarred)) totalStarred = 0;
            totalStarred = starred ? totalStarred + countNumber : (totalStarred > 0 ? totalStarred - countNumber : totalStarred);
            if (totalStarred > 0) {
                totalStarredElement.innerHTML = totalStarred.toString();
                totalStarredElement.className = "lm__messages-stat-count";
            } else {
                totalStarredElement.style.display = 'none';
                totalStarredElement.innerHTML = '';
                totalStarredElement.className = '';
            }

        } catch (ex) {
            console.log('can not update the totalStarred');
        }
    }

    showSpinner(isShow: boolean) {
        if (isShow) {
            $('#loader').show();
            $('.lm__meddelanden-messages, .lm__meddelanden-detail-column').addClass('disabled');
        } else {
            $('#loader').hide();
            $('.lm__meddelanden-messages, .lm__meddelanden-detail-column').removeClass('disabled');
        }
    }

}

export var helper = new Helper();