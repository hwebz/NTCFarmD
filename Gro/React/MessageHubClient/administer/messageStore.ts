import { MessageItem } from './administerModels';
import { EventEmitter } from 'events';
import { Map, List } from 'immutable';

/**
 * Save the key with the format
 * {categoryId1}_{categoryId2}:{type1}_{type2}
 */

function formFilterKey(categoryIds: number[], typeIds: number[]): string {
    var categoryPart = categoryIds.join('_');
    var typePart = typeIds.join('_');
    return `${categoryPart}:${typePart}`;
}

interface FilterStatus {
    totalCount: number;
    loading: boolean;
}

export class MessageStore extends EventEmitter {
    private messages = Map<string, List<MessageItem>>();
    private messageStatus = Map<string, FilterStatus>();
    private selectedTypes: number[] = [];
    private selectedCates: number[] = [];

    getMessages(filterKey: string): List<MessageItem> {
        return this.messages.get(filterKey) || List<MessageItem>();
    }

    setMessages(categoryIds: number[], typeIds: number[], newMessages: List<MessageItem>, totalCount: number, loading: boolean) {
        var filterKey = formFilterKey(categoryIds, typeIds);
        this.messages = this.messages.set(filterKey, newMessages);
        this.messageStatus = this.messageStatus.set(filterKey, {
            loading: loading,
            totalCount: totalCount
        });
        this.emit("messages");
    }

    replaceMessages(categoryIds: number[], typeIds: number[], messages: MessageItem[], totalCount: number, loading: boolean) {
        var messageList = List<MessageItem>(messages);
        this.setMessages(categoryIds, typeIds, messageList, totalCount, loading);
    }

    concatMessages(categoryIds: number[], typeIds: number[], messages: MessageItem[], totalCount: number, loading: boolean) {
        var currentMessages = this.messages.get(formFilterKey(categoryIds, typeIds)) || List<MessageItem>();
        var newMessages = currentMessages.concat(...messages).toList();
        this.setMessages(categoryIds, typeIds, newMessages, totalCount, loading);
    }

    markLoading(categoryIds: number[], typeIds: number[]) {
        var status = this.getFilterStatus(formFilterKey(categoryIds, typeIds));
        status.loading = true;
        this.emit("messages");
    }

    setCurrentFilter(categoryIds: number[], typeIds: number[]) {
        this.selectedCates = categoryIds;
        this.selectedTypes = typeIds;
        this.emit("filter");
    }

    getCurrentFilterKey() {
        return formFilterKey(this.selectedCates, this.selectedTypes);
    }

    getSelectedTypes(): number[] {
        return this.selectedTypes;
    }

    getSelectedCategories(): number[] {
        return this.selectedCates;
    }

    getFilterStatus(filterKey: string): FilterStatus {
        return this.messageStatus.get(filterKey) || {
            loading: false,
            totalCount: 0
        };
    }

    reset() {
        this.messages = Map<string, List<MessageItem>>();
        this.messageStatus = Map<string, FilterStatus>();
        this.selectedCates = [];
        this.selectedCates = [];
    }
}

export var messageStore = new MessageStore();
