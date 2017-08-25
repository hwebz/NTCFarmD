import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Router, Route, IndexRoute, Link, browserHistory, IndexLink } from 'react-router';
import { Category } from './model/categoryModel';
import { MessageSimple, Message } from './model/messageModel';
import { Pager } from '../shared/pager';
import { Modal } from '../shared/modal';
import { globalStore, GlobalMessageModel, MessageModel } from './globalStore';

interface MessageRowProps {
    message: Message;
    type: string;

    //event
    toggleSelected: (mesage: Message) => void;
    markToUnread: (mesage: Message) => void;
    markToStarred: (mesage: Message) => void;
}

export class MessageRow extends React.Component<MessageRowProps, any>{

    constructor(props: any) {
        super(props);
    }

    setSelectedMessage(mes: Message): void {
        this.props.toggleSelected(mes);
    }

    markToUnread(mes: Message): void {
        mes.isUnRead = !mes.isUnRead;
        this.props.markToUnread(mes);
    }

    markToStarred(mes: Message): void {
        mes.isStarred = !mes.isStarred;
        this.forceUpdate();
        this.props.markToStarred(mes);
    }

    render() {
        return (this.props.type !== "trash" ?
            <tr className={this.props.message.isUnRead ? "unread" : ""}>
                <td><div className="lm__meddelanden-radio" onClick={_ => this.markToUnread(this.props.message)} /></td>
                <td>
                    <div className={this.props.message.isStarred ? "lm__meddelanden-favorite marked" : "lm__meddelanden-favorite"} onClick={_ => this.markToStarred(this.props.message)} />
                </td>
                <td><Link to={"/messages/" + this.props.type + "/" + this.props.message.id}>{this.props.message.categoryName}</Link></td>
                <td><Link to={"/messages/" + this.props.type + "/" + this.props.message.id}>{this.props.message.header}</Link></td>
                <td><Link to={"/messages/" + this.props.type + "/" + this.props.message.id}>{this.props.message.receivedDate}</Link></td>
                <td><div onClick={_ => this.setSelectedMessage(this.props.message)} className={this.props.message.isSelected ? "lm__meddelanden-checkbox checked" : "lm__meddelanden-checkbox"} /></td>
            </tr> :
            <tr className={this.props.message.isUnRead ? "unread" : ""}>
                <td><div className="lm__meddelanden-radio" onClick={_ => this.markToUnread(this.props.message)} /></td>
                <td><Link to={"/messages/" + this.props.type + "/" + this.props.message.id}>{this.props.message.categoryName}</Link></td>
                <td><Link to={"/messages/" + this.props.type + "/" + this.props.message.id}>{this.props.message.header}</Link></td>
                <td><Link to={"/messages/" + this.props.type + "/" + this.props.message.id}>{this.props.message.receivedDate}</Link></td>
                <td><div onClick={_ => this.setSelectedMessage(this.props.message)} className={this.props.message.isSelected ? "lm__meddelanden-checkbox checked" : "lm__meddelanden-checkbox"} /></td>
            </tr>);
    }
}

interface MessageListState {
    isOpenModal: boolean;
}

interface MessageListProps {
    messages: Message[];
    type: string;
    totalItems: number;

    //event
    markToUnRead: (messages: Message[], unRead: boolean) => void;
    markToStarred: (message: Message, isStarred: boolean) => void;
    moveToTrash: (messages: Message[]) => void;
    moveToInbox: (messages: Message[]) => void;
    removeMessages: (messages: Message[]) => void;
    changePage_onClick?: (newPageNumber: number, isShowmore?: boolean) => void;
}


export class MessageList extends React.Component<MessageListProps, MessageListState>{

    constructor(props: any) {
        super(props);
        this.state = {
            isOpenModal: false
        }
    }

    disabledReadButton(): string {
        var foundItem = this.props.messages["find"](function (item) {
            return item.isSelected && item.isUnRead;
        }, this)

        return foundItem != undefined ? "lm__meddelanden-action-btn" : "lm__meddelanden-action-btn disabled";
    }

    disabledUnReadButton(): string {
        var foundItem = this.props.messages["find"](function (item) {
            return item.isSelected && !item.isUnRead;
        }, this)

        return foundItem != undefined ? "lm__meddelanden-action-btn" : "lm__meddelanden-action-btn disabled";
    }

    updateStatusButton(): string {
        var foundItem = this.props.messages["find"](function (item) {
            return item.isSelected;
        }, this)

        return foundItem != undefined ? "lm__meddelanden-action-btn" : "lm__meddelanden-action-btn disabled";
    }

    updateStatusCheckboxAll(): string {
        return this.props.messages != undefined && this.props.messages.length > 0 && this.props.messages.every(item => item.isSelected) ? "lm__meddelanden-checkbox check-all checked" : "lm__meddelanden-checkbox check-all";
    }

    markToUnRead(isUnread: boolean, selectedMsgs?: Message[]): void {
        if (selectedMsgs == undefined) {
            selectedMsgs = [];
            for (let msg of this.props.messages) {
                if (msg.isSelected && msg.isUnRead != isUnread) {
                    msg.isUnRead = isUnread;
                    selectedMsgs.push(msg);
                }
            }
        }

        this.forceUpdate();
        this.props.markToUnRead(selectedMsgs, isUnread);
    }

    moveToTrash(selectedMsgs?: Message[]): void {
        if (selectedMsgs == undefined) {
            selectedMsgs = [];
            for (let msg of this.props.messages) {
                if (msg.isSelected) {
                    msg.isSelected = false;
                    selectedMsgs.push(msg);
                }
            }
        }
        globalStore.rememberSelectedStatusToGlobal(this.props.type, selectedMsgs);
        this.props.moveToTrash(selectedMsgs);
    }

    moveToInbox(selectedMsgs?: Message[]): void {
        if (selectedMsgs == undefined) {
            selectedMsgs = [];
            for (let msg of this.props.messages) {
                if (msg.isSelected) {
                    msg.isSelected = false;
                    selectedMsgs.push(msg);
                }
            }
        }
        globalStore.rememberSelectedStatusToGlobal(this.props.type, selectedMsgs);
        this.props.moveToInbox(selectedMsgs);
    }

    removeMessages(selectedMsgs?: Message[]): void {

        if (selectedMsgs == undefined) {
            selectedMsgs = [];
            for (let msg of this.props.messages) {
                if (msg.isSelected) {
                    selectedMsgs.push(msg);
                }
            }
        }

        this.props.removeMessages(selectedMsgs);
        this.closeModal();
    }

    pageNumber_onClick(updatedNumber: number, isShowmore?: boolean) {
        this.props.changePage_onClick(updatedNumber, isShowmore);
    }

    getCurrentPageNumber(): number {
        return globalStore.getPageIndex(this.props.type);
    }

    selectAllMessage(): void {
        var isSelectAll = this.props.messages.every(item => item.isSelected);

        for (let mes of this.props.messages) {
            mes.isSelected = !isSelectAll;
        }

        this.forceUpdate();
        globalStore.rememberSelectedStatusToGlobal(this.props.type, this.props.messages);
    }

    toggleSelected(mes: Message): void {
        mes.isSelected = !mes.isSelected;
        this.forceUpdate();
        globalStore.rememberSelectedStatusToGlobal(this.props.type, [mes]);
    }

    openModal() {
        this.setState({ isOpenModal: true });
    }

    closeModal() {
        this.setState({ isOpenModal: false });
    }

    render() {
        var self = this;
        var messageRows = this.props.messages != null ? this.props.messages.map(function (mes) {
            return (
                <MessageRow key={mes.id}
                    type={self.props.type}
                    message={mes}
                    toggleSelected={(nv) => self.toggleSelected(nv)}
                    markToUnread={(msg) => self.markToUnRead(msg.isUnRead, [msg])}
                    markToStarred={msg => self.props.markToStarred(msg, msg.isStarred)} />
            );
        }) : <tr></tr>;

        var pager = this.props.totalItems != undefined && this.props.totalItems > 0 ?
            <Pager key={1} totalItemsForDisplaying={this.props.messages.length} currentPageNumber={this.getCurrentPageNumber()} totalItems={this.props.totalItems}
                pageSize={globalStore.getPageSize()} changePage_onClick={(np, isShowmore) => this.pageNumber_onClick(np, isShowmore)} /> : "";

        var actionPanel = this.props.type !== "trash" ?
            <div className="lm__meddelanden-actions">
                <a href="javascript:void(0)" id="markera-som-olast" className={this.disabledReadButton()} onClick={_ => this.markToUnRead(false)}><div className="radio-icon"></div>Markera som läst</a>
                <a href="javascript:void(0)" id="markera-som-last" className={this.disabledUnReadButton()} onClick={_ => this.markToUnRead(true)}><div className="radio-icon unread"></div>Markera som oläst</a>
                <a href="javascript:void(0)" id="ta-bort-markerade" className={this.updateStatusButton()} onClick={_ => this.moveToTrash()}><i className="fa fa-trash"></i>Ta bort markerade</a>
                <div className={this.updateStatusCheckboxAll()} onClick={_ => this.selectAllMessage()}></div>
            </div> :
            <div className="lm__meddelanden-actions">
                <Modal isOpen={this.state.isOpenModal} isShowCloseIcon={true} closeModal={() => this.closeModal()} modalTitle={"Vill du ta bort markerade meddelanden permanent?"}>
                    <div className="modal-content">
                        Du kommer inte kunna se eller läsa dessa meddelanden igen
                    </div>
                    <div className="lm__block meddelanden-block" style={{ marginBottom: "0px" }}>
                        <div className="author-inform-form__input" style={{ padding: "0px" }}>
                            <input type="button" className="author-inform-form__btn" value="Nej, ta inte bort" style={{ marginRight: "10px", minWidth: "100px" }} onClick={_ => this.closeModal()} />
                            <input type="button" className="author-inform-form__btn" value="Ja, ta bort" style={{ minWidth: "100px" }} onClick={_ => this.removeMessages()} />
                        </div>
                    </div>
                </Modal>
                <a href="javascript:void(0)" id="markera-som-olast" className={this.updateStatusButton()} onClick={_ => this.moveToInbox()}>Flytta till Inkorg</a>
                <a href="javascript:void(0)" id="markera-som-last" className={this.updateStatusButton()} onClick={_ => this.openModal()}>Radera markerade</a>
                <div className={this.updateStatusCheckboxAll()} onClick={_ => this.selectAllMessage()}></div>
            </div>;

        var headRow = this.props.type !== "trash" ?
            <tr>
                <th></th>
                <th></th>
                <th>Område</th>
                <th>Ämne</th>
                <th>Datum</th>
                <th></th>
            </tr> :
            <tr>
                <th></th>
                <th>Område</th>
                <th>Ämne</th>
                <th>Datum</th>
                <th></th>
            </tr>;

        return (
            <div className="lm__meddelanden-messages">
                {actionPanel}
                <div className="messages-table">
                    <div className="lm__table-wrapper">
                        <table>
                            <thead>
                                {headRow}
                            </thead>
                            <colgroup>
                                <col width="5%" />
                                <col width="5%" />
                                <col width="20%" />
                                <col width="45%" />
                                <col width="20%" />
                                <col width="5%" />
                            </colgroup>
                            <tbody>{messageRows}</tbody>
                        </table>
                    </div>
                </div>
                {pager}
            </div>
        )
    }
}
