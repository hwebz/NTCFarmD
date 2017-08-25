import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Router, Route, IndexRoute, Link, browserHistory, IndexLink } from 'react-router';
import { Category } from './model/categoryModel';
import { MessageSimple, Message } from './model/messageModel';
import { CategoryList } from './categoryListComponent';
import { MessageList } from './messageListComponent';
import { messageSetting } from '../shared/messageSetting';
import { helper } from './helper';

export interface MessageModel {
    categories: Category[];
    pageNumber: number;
    messages: MessageSimple[];
}

export interface GlobalMessageModel {
    inboxMes: MessageModel;
    starredMes: MessageModel;
    trashMes: MessageModel;
    pageSize: number;
}

export class GlobalStore {
    private globalMessageModel: GlobalMessageModel;

    constructor() {
        var pageSize = messageSetting.getUserPageSize();
        this.globalMessageModel = {
            inboxMes: {
                categories: [],
                pageNumber: 1,
                messages: []
            },
            starredMes: {
                categories: [],
                pageNumber: 1,
                messages: []
            },
            trashMes: {
                categories: [],
                pageNumber: 1,
                messages: []
            },
            pageSize: pageSize > 0 ? pageSize : 10
        }
    }

    getPageSize(): number {
        return this.globalMessageModel.pageSize;
    }

    getMessageModel(type): MessageModel {
        if (type == "starred") {
            return this.globalMessageModel.starredMes;
        } else if (type == "trash") {
            return this.globalMessageModel.trashMes;
        } else { //default = inbox
            return this.globalMessageModel.inboxMes;
        }
    }

    getPageIndex(type: string): number {
        if (type === "starred") {
            return this.globalMessageModel.starredMes.pageNumber;
        } else if (type === "trash") {
            return this.globalMessageModel.trashMes.pageNumber;
        }

        return this.globalMessageModel.inboxMes.pageNumber;
    }

    private updateSelectedCategories(oldCategories: Category[], newCategories: Category[]): void {
        if (!oldCategories) return;

        for (let cat of oldCategories) {
            if (cat.isSelected) {
                var index = helper.findIndexOfCategory(newCategories, cat);
                if (index >= 0) {
                    newCategories[index].isSelected = true;
                }
            }
        }
    }

    setCategoriesByType(type: string, categories: Category[]): void {
        if (!categories) categories = [];

        if (type === "inbox") {
            if (this.globalMessageModel.inboxMes.categories.length == 0) {
                this.globalMessageModel.inboxMes.categories = categories;
            }
        } else if (type === "starred") {
            this.updateSelectedCategories(this.globalMessageModel.starredMes.categories, categories);
            this.globalMessageModel.starredMes.categories = categories;
        } else if (type === "trash") {
            this.updateSelectedCategories(this.globalMessageModel.trashMes.categories, categories);
            this.globalMessageModel.trashMes.categories = categories;
        }
    }

    getCategoriesByType(type: string): Category[] {
        if (type == "starred") {
            return this.globalMessageModel.starredMes.categories;
        } else if (type == "trash") {
            return this.globalMessageModel.trashMes.categories;
        } else { //default = inbox
            return this.globalMessageModel.inboxMes.categories;
        }
    }

    setPageIndex(type: string, pageIndex: number): void {
        if (type === "inbox") {
            this.globalMessageModel.inboxMes.pageNumber = pageIndex;
        } else if (type === "starred") {
            this.globalMessageModel.starredMes.pageNumber = pageIndex;
        } else if (type === "trash") {
            this.globalMessageModel.trashMes.pageNumber = pageIndex;
        }
    }

    private rememberSelectedStatus(storeMessages: MessageSimple[], messages: Message[]): void {
        if (!messages) return;

        for (let msg of messages) {

            var index = helper.findIndexOfMessage(storeMessages, msg);
            if (msg.isSelected) {
                if (index == -1) { //don't exist
                    storeMessages.push({
                        id: msg.id
                    });
                }
            } else {
                if (index >= 0) {
                    storeMessages[index] = undefined;
                }
            }
        }
    }

    rememberSelectedStatusToGlobal(type: string, messages: Message[]): void {
        if (type === "inbox") {
            this.rememberSelectedStatus(this.globalMessageModel.inboxMes.messages, messages);
        } else if (type === "starred") {
            this.rememberSelectedStatus(this.globalMessageModel.starredMes.messages, messages);
        } else if (type === "trash") {
            this.rememberSelectedStatus(this.globalMessageModel.trashMes.messages, messages);
        }
    }

}

export var globalStore = new GlobalStore();