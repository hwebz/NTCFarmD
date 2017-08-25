import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Router, Route, IndexRoute, Link, browserHistory, IndexLink } from 'react-router';
import { Category } from './model/categoryModel';
import { Message } from './model/messageModel';
import { CategoryList } from './categoryListComponent';
import { MessageList } from './messageListComponent';

var $ = window["$"];

export class MessageServices {
    makeGetRequest(url: string, data?: any): Promise<any> {
        return new Promise(function (resolve) {
            $.ajax({ url: url, type: "get", data: data }).done(resolve).fail(function (jqXHR, textStatus, errorThrown) {
                console.error(url, textStatus, errorThrown.toString());
            });
        });
    }

    makePostRequest(url: string, data?: any): Promise<any> {
        return new Promise(function (resolve) {
            $.ajax({ url: url, type: "post", data: data }).done(resolve).fail(function (jqXHR, textStatus, errorThrown) {
                console.error(url, textStatus, errorThrown.toString());
            });
        });
    }

    getCategoriesByStatus(type: string): any {
        return this.makePostRequest('/api/user/get-categories-by-status', { type: type });
    }

    getMessages(type: string, pageSize: number, pageNumber: number, categories: string, isReloadCategory?: boolean): any {
        return this.makePostRequest('/api/user/get-messages', { type: type, pageSize: pageSize, pageIndex: pageNumber, cats: categories, isReloadCategory: isReloadCategory });
    }

    getMessage(msgId: number): any {
        return this.makePostRequest('/api/user/get-message', { msgId: msgId });
    }    

    markToUnRead(msgIds: string, unRead: boolean): any {
        return this.makePostRequest('/api/user/mark-to-unread', { msgIds: msgIds, unRead: unRead });
    }

    markToStarred(msgId: number, isStarred: boolean): any {
        return this.makePostRequest('/api/user/mark-to-starred', { msgId: msgId, isStarred: isStarred });
    }

    moveToTrash(msgIds: string): any {
        return this.makePostRequest('/api/user/move-to-trash', { msgIds: msgIds });
    }

    deleteFromTrash(type: string, msgIds: string): any {
        return this.makePostRequest('/api/user/delete-from-trash', { type: type, msgIds: msgIds });
    }

    getTotalOfMessages(): any {
        return this.makePostRequest('/api/user/get-total');
    }

    moveToInbox(type: string, msgIds: string): any {
        return this.makePostRequest('/api/user/move-to-inbox', { type: type, msgIds: msgIds });
    }
}

export var messageServices = new MessageServices();