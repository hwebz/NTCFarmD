import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Router, Route, IndexRoute, Link, browserHistory, IndexLink } from 'react-router';
import { Category } from './model/categoryModel';
import { Message } from './model/messageModel';
import { CategoryList } from './categoryListComponent';
import { MessageList } from './messageListComponent';
import { messageServices } from './messageServices';
import { globalStore, GlobalMessageModel, MessageModel } from './globalStore';
import { helper } from './helper';
import { Spinner } from '../../components/spinner';
import { isMobile } from '../shared/device-detect';
import { messageSetting } from '../shared/messageSetting';

interface MessageTabContentProps {
    //event
    updateTotalMessagesInfo: () => void;
}

interface MessageTabContentState {
    messages: Message[];
    categories?: Category[];
}

const timeoutInterval = 250;

export class MessageTabContent extends React.Component<MessageTabContentProps, MessageTabContentState>{

    private actionTimeout: number;

    private url: string;
    private tabid: number;
    private totalItems: number;
    private isLoaded: boolean;
    private isMobile: boolean;
    private chooseAction: any;
    private modifiedMessages: Message[];

    constructor(props: MessageTabContentProps) {
        super(props);
        this.isMobile = isMobile();
        this.modifiedMessages = [];
        this.chooseAction = {
            starred: false,
            trash: false
        }
        this.state = {
            messages: [],
            categories: []
        }
    }

    onFilterChanged(selectedCats: Category[]): void {
        //reset current numberPage when filter;
        this.getDataForNewPageIndex(1, true);
    }

    getDataForNewPageIndex(newPageIndex: number, isDontNeedUpdateCategories?: boolean, isShowmore?: boolean): void {
        var tabid = this.props["params"].tabid;
        globalStore.setPageIndex(tabid, newPageIndex);
        this.getMessageAccordingType(this.props, isDontNeedUpdateCategories, isShowmore);
    }

    getMessageAccordingType(updatedProps: MessageTabContentProps, isDontNeedUpdateCategories?: boolean, isShowmore?: boolean): void {
        var messageModel = globalStore.getMessageModel(updatedProps["params"].tabid);
        clearTimeout(this.actionTimeout);
        //this.actionTimeout = window.setTimeout(() => this.loadMessageFromServer(updatedProps, messageModel, isDontNeedUpdateCategories, isShowmore), timeoutInterval);
        this.loadMessageFromServer(updatedProps, messageModel, isDontNeedUpdateCategories, isShowmore);
    }

    loadMessageFromServer(props: MessageTabContentProps, messageModel: MessageModel, isDontNeedUpdateCategories?: boolean, isShowmore?: boolean): void {
        helper.showSpinner(true);
        var catParams = helper.buildCategoryParams(messageModel.categories);
        var pageNumber = messageModel.pageNumber - 1;
        var type = props["params"].tabid;

        var isReloadCategory = !isDontNeedUpdateCategories;
        if (type === "inbox") {
            isReloadCategory = messageModel.categories.length == 0;
        }

        messageServices.getMessages(type, globalStore.getPageSize(), pageNumber, catParams, isReloadCategory).then((data) => {
            helper.showSpinner(false);
            if (!data || !data.total || !data.messages) {
                data = { total: 0, messages: [] }
            }

            for (let msg of data.messages) {
                var result = messageModel.messages.find(function (item) {
                    return item != undefined && item.id == msg.id;
                }, this);

                if (result !== undefined) msg.isSelected = true;
            }

            this.totalItems = data.total;
            var newMessages = [];

            if (isShowmore) {
                newMessages = this.state.messages.concat(data.messages)
            } else {
                newMessages = data.messages;
            }

            this.isLoaded = true;

            if (isReloadCategory) {
                globalStore.setCategoriesByType(type, data.categories)
            }

            var newCategories = globalStore.getCategoriesByType(type);
            this.setState({
                messages: newMessages,
                categories: newCategories
            });
        });
    }

    componentDidMount(): void {
        this.isLoaded = false;
        this.getMessageAccordingType(this.props);
    }

    componentWillReceiveProps(newProps) {
        if (this.props["params"].tabid != newProps["params"].tabid) {
            this.isLoaded = false;

            if (this.isMobile) {
                var tabid = newProps["params"].tabid;
                globalStore.setPageIndex(tabid, 1);
            }

            this.getMessageAccordingType(newProps);
        }
        else if (this.chooseAction.trash || (this.chooseAction.starred && this.props["params"].tabid != "inbox")) {

            this.isLoaded = false;
            globalStore.setPageIndex(this.props["params"].tabid, 1);
            this.getMessageAccordingType(this.props, true);

        } else if (this.modifiedMessages.length > 0) {

            for (let msg of this.modifiedMessages) {
                var index = helper.findIndexOfMessage(this.state.messages, msg);
                if (index >= 0) {
                    this.state.messages[index].isUnRead = msg.isUnRead;
                    this.state.messages[index].isStarred = msg.isStarred;
                    break;
                }
            }
        }
        this.modifiedMessages = [];
        this.chooseAction.trash = false;
        this.chooseAction.starred = false;
    }

    shouldComponentUpdate(nextProps: any, nextState: any) {
        //var yes = this.props["params"].tabid == nextProps["params"].tabid && this.isLoaded;
        var yes = this.isLoaded;
        return yes;
        //return true;
    }

    markToUnRead(messages: Message[], unRead: boolean): void {
        var self = this;
        var msgParams = helper.buildMessageParams(messages);
        messageServices.markToUnRead(msgParams, unRead).then((data) => {
            if (data.success) {
                helper.updateTotalUnread(unRead, messages.length);
            }
        });
    }

    markToStarred(message: Message, isStarred: boolean): void {
        var type = this.props["params"].tabid;
        messageServices.markToStarred(message.id, isStarred).then((data) => {
            if (data.success) {
                helper.updateTotalStarred(isStarred, 1);

                this.props.updateTotalMessagesInfo();

                globalStore.setPageIndex("starred", 1);
                globalStore.setCategoriesByType("starred", data.categories);

                if (type === "starred") {
                    if (!isStarred) {
                        message.isSelected = false;
                        globalStore.rememberSelectedStatusToGlobal(type, [message]);
                    }
                    this.getMessageAccordingType(this.props, true);
                }
            }
        });
    }

    moveToTrash(messages: Message[]): void {
        var msgParams = helper.buildMessageParams(messages);
        var type = this.props["params"].tabid;
        helper.showSpinner(true);
        messageServices.moveToTrash(msgParams).then((data) => {
            if (data.success) {
                this.props.updateTotalMessagesInfo();

                globalStore.setPageIndex(type, 1);
                if (type != "starred") {
                    globalStore.setPageIndex("starred", 1);
                }
                globalStore.setCategoriesByType("starred", data.categories);

                if (type === "inbox") {
                    globalStore.setPageIndex(this.props["params"].tabid, 1);
                    this.getMessageAccordingType(this.props, true);
                } else if (type === "starred") {
                    //globalStore.setPageIndex(type, 1);
                    this.getMessageAccordingType(this.props, true);
                }
            } else {
                helper.showSpinner(false);
            }
        });
    }

    removeMessages(messages: Message[]): void {
        var msgParams = helper.buildMessageParams(messages);
        var type = this.props["params"].tabid;
        helper.showSpinner(true);
        messageServices.deleteFromTrash(type, msgParams).then((data) => {
            if (data.success) {

                globalStore.setPageIndex(type, 1);
                globalStore.setCategoriesByType(type, data.categories);
                this.getMessageAccordingType(this.props, true);
            } else {
                helper.showSpinner(false);
            }
        });
    }

    moveToInbox(messages: Message[]): void {
        var msgParams = helper.buildMessageParams(messages);
        var type = this.props["params"].tabid;
        helper.showSpinner(true);
        messageServices.moveToInbox(type, msgParams).then((data) => {
            if (data.success) {
                this.props.updateTotalMessagesInfo();

                globalStore.setPageIndex(type, 1);
                globalStore.setCategoriesByType(type, data.categories);
                this.getMessageAccordingType(this.props, true);
            } else {
                helper.showSpinner(false);
            }
        });
    }

    onMarkToUnreadFromDetail(message: Message): void {
        if (message == undefined) return;

        var index = helper.findIndexOfMessage(this.modifiedMessages, message);

        if (index >= 0) {
            this.modifiedMessages[index].isUnRead == message.isUnRead;
        } else {
            this.modifiedMessages.push(message);
        }
    }

    onMarkToStarredFromDetail(message: Message): void {
        if (message == undefined) return;

        var index = helper.findIndexOfMessage(this.modifiedMessages, message);

        if (index >= 0) {
            this.modifiedMessages[index].isStarred == message.isStarred;
        } else {
            this.modifiedMessages.push(message);
        }
        this.chooseAction.starred = true;
    }

    onMoveToTrashFromDetail(): void {
        this.chooseAction.trash = true;
    }

    render() {

        var messagesContent = this.isLoaded && this.state.messages && this.state.messages.length == 0 ?
            <div className="lm__meddelanden-messages">
                <div className="messages-table">
                    <div className="lm__table-wrapper">
                        {messageSetting.getMessageForEmptyCategory()}
                    </div>
                </div>
            </div> : <MessageList
                messages={this.state.messages}
                type={this.props["params"].tabid}
                totalItems={this.totalItems}
                changePage_onClick={(nv, isShowmore) => this.getDataForNewPageIndex(nv, true, isShowmore)}
                markToUnRead={(msgs, unRead) => this.markToUnRead(msgs, unRead)}
                markToStarred={(msg, isStarred) => this.markToStarred(msg, isStarred)}
                moveToTrash={msgs => this.moveToTrash(msgs)}
                removeMessages={msgs => this.removeMessages(msgs)}
                moveToInbox={msgs => this.moveToInbox(msgs)} />;

        var children = this.props.children == null ?
            <div className="layout layout--large">
                <div className="layout__item u-1/1 u-1/3-tablet u-1/5-desktop">
                    <CategoryList categories={this.state.categories} onFilterChange={nv => this.onFilterChanged(nv)} />
                </div>
                <div className="layout__item u-1/1 u-2/3-tablet u-4/5-desktop">
                    <div className="loader-wrapper" id="loader" style={{ display: "none" }}>
                        <Spinner color="#3498db" period={0.8} thickness={3} size={30} />
                    </div>
                    {messagesContent}
                </div>
            </div> :
            this.props.children && React.cloneElement(this.props.children as React.ReactElement<any>, {
                updateTotalMessagesInfo: nv => this.props.updateTotalMessagesInfo(),
                onMarkToUnreadFromDetail: (message) => this.onMarkToUnreadFromDetail(message),
                onMarkToStarredFromDetail: (message) => this.onMarkToStarredFromDetail(message),
                onMoveToTrashFromDetail: () => this.onMoveToTrashFromDetail(),
            });

        return (
            <div className="lm__meddelanden-contents" style={{ display: "block" }}>
                {children}
            </div>
        )
    }
}
