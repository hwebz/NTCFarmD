import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Router, Route, IndexRoute, Link, browserHistory, IndexLink } from 'react-router';
import { Category } from './model/categoryModel';
import { MessageSimple, Message, MessageTable, MessageRow } from './model/messageModel';
import { DeliveryItem } from './model/deliveryModel';
import { messageServices } from './messageServices';
import { globalStore, GlobalMessageModel, MessageModel } from './globalStore';
import { Spinner } from '../../components/spinner';
import { Modal } from '../shared/modal';
import { helper } from './helper';

var $ = window["$"];

interface ActionPanelProps {
    message: Message;
    tabid: string;
    markToUnread: (isUnread: boolean) => void;
    markToStarred: (isStarred: boolean) => void;
    moveToTrash: () => void;
    moveToInbox: () => void;
    removeMessage: () => void;
}

interface ActionPanelState {
    isOpenModal: boolean;
}

export class ActionPanel extends React.Component<ActionPanelProps, ActionPanelState>{
    constructor(props: ActionPanelProps) {
        super(props);
        this.state = {
            isOpenModal: false
        }
    }

    openModal() {
        this.setState({ isOpenModal: true });
    }

    closeModal() {
        this.setState({ isOpenModal: false });
    }

    markToUnread(): void {
        if (this.props.message === undefined) return;

        this.props.message.isUnRead = !this.props.message.isUnRead;
        this.forceUpdate();
        this.props.markToUnread(this.props.message.isUnRead);
    }

    markToStarred(): void {
        if (this.props.message === undefined) return;

        this.props.message.isStarred = !this.props.message.isStarred;
        this.forceUpdate();
        this.props.markToStarred(this.props.message.isStarred);
    }

    moveToTrash(): void {
        if (this.props.message === undefined) return;
        this.props.moveToTrash();
    }

    moveToInbox(): void {
        if (this.props.message === undefined) return;
        this.props.moveToInbox();
    }

    removeMessage(): void {
        if (this.props.message === undefined) return;
        this.props.removeMessage();
        this.closeModal();
    }

    getClassForUnreadLink(): string {
        return this.props.message != undefined && this.props.message.isUnRead ? "unread" : "";
    }

    getClassForStarredLink(): string {
        return this.props.message != undefined && this.props.message.isStarred ? "lm__meddelanden-favorite marked" : "lm__meddelanden-favorite";
    }

    render() {
        if (!this.props.message) {
            return (<tr></tr>);
        }

        if (this.props.message != undefined && this.props.message.isTrash) {
            return (
                <tr>
                    <td>
                        <Link to={"/messages/trash"} className="lm__back-link"><i className="fa fa-arrow-left" aria-hidden="true"></i>Till Meddelanden</Link>
                    </td>
                    <td>
                        <div className="lm__meddelanden-actions">
                            <Modal isOpen={this.state.isOpenModal} isShowCloseIcon={true} closeModal={() => this.closeModal()} modalTitle={"Vill du ta bort markerade meddelanden permanent?"}>
                                <div className="modal-content">
                                    Du kommer inte kunna se eller läsa dessa meddelanden igen
                                </div>
                                <div className="lm__block meddelanden-block" style={{ marginBottom: "10px" }}>
                                    <div className="author-inform-form__input" style={{ padding: "0px" }}>
                                        <input type="button" className="author-inform-form__btn" value="Nej, ta inte bort" style={{ marginRight: "10px", minWidth: "100px" }} onClick={_ => this.closeModal()} />
                                        <input type="button" className="author-inform-form__btn" value="Ja, ta bort" style={{ minWidth: "100px" }} onClick={_ => this.removeMessage()} />
                                    </div>
                                </div>
                            </Modal>
                            <a href="javascript:void(0)" id="markera-som-olast" className="lm__meddelanden-action-btn btn-restore" onClick={_ => this.moveToInbox()}><div className="radio-icon unread"></div>Flytta till Inkorg</a>
                            <a href="javascript:void(0)" id="markera-som-last" className="lm__meddelanden-action-btn btn-delete" onClick={_ => this.openModal()}><i className="fa fa-trash"></i>Ta bort permanent</a>
                        </div>
                    </td>
                </tr>
            );

        }

        return (
            <tr>
                <td>
                    <Link to={"/messages/" + this.props.tabid} className="lm__back-link"><i className="fa fa-arrow-left" aria-hidden="true"></i>Till Meddelanden</Link>
                </td>
                <td>
                    <ul>
                        <li>
                            <a href="javascript:void(0)" onClick={_ => this.markToUnread()} className={this.getClassForUnreadLink()}>
                                <div className="lm__meddelanden-radio"></div>
                                Markera som oläst
                            </a>
                        </li>
                        <li><a href="javascript:void(0)" onClick={_ => this.markToStarred()}>
                            <div className={this.getClassForStarredLink()}></div>
                            Stjärnmärk detta meddelande
                        </a>
                        </li>
                        <li>
                            <a href="javascript:void(0)" onClick={_ => this.moveToTrash()}>
                                <i className="fa fa-trash" aria-hidden="true"></i>Radera</a>
                        </li>
                    </ul>
                </td>
            </tr>
        )
    }
}

interface MessageDeliveryListProps {
    messageTable: MessageTable,
    customerName: string,
    customerAddress: string,
    customerZipAndCity: string,
    messageType: number

}

export class MessageTableView extends React.Component<MessageDeliveryListProps, any>{
    constructor(props: MessageDeliveryListProps) {
        super(props);
    }

    render() {

        var tableInfor = this.props.messageTable;
        var tableInfo = this.props.messageTable != null && this.props.messageType != 15 ?
            <div className="grid">
                <div className="column-50">
                    <ul>
                        <li>{this.props.customerName}</li>
                        <li>{this.props.customerAddress}</li>
                        <li>{this.props.customerZipAndCity}</li>
                    </ul>
                </div>
                <div className="column-50">
                    <div className="grid inform-table-wrapper">
                        <div className="column-50">
                            <table className="inform-table">
                                <tbody><tr>
                                    <td>Sändning</td>
                                    <td>{tableInfor.freightNo}</td>
                                </tr>
                                    <tr>
                                        <td>Transportör</td>
                                        <td>{tableInfor.carrier}</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>

                        <div className="column-50">
                            <table className="inform-table">
                                <tbody><tr>
                                    <td>Bil</td>
                                    <td>{tableInfor.carNo}</td>
                                </tr>
                                    <tr>
                                        <td>Telefon bil</td>
                                        <td>{tableInfor.mobileNo}</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div> : "";

        var firstTblRows = tableInfo != null && this.props.messageType == 15 && this.props.messageTable.firstTblRows != null ? this.props.messageTable.firstTblRows.map((item, idx) => {
            return <tr key={idx}>
                {item.items.map((rowItem, rowIdx) => {
                    if (rowIdx == 0 || rowIdx == 1) {
                        return <td key={rowIdx} className="tablesaw-priority-persit">{rowItem}</td>
                    }
                    return <td key={rowIdx} className={"tablesaw-priority-" + (idx - 1)}>{rowItem}</td>
                })
                }
            </tr>
        }) : <tr></tr>

        var firstTbl = tableInfo != null && this.props.messageType == 15 ?
            <div className="lm__table-wrapper">

                <table className="tablesaw" data-tablesaw-mode="columntoggle">
                    <thead>
                        <tr>
                            {this.props.messageTable.firstTblHeaders.map((headerItem, idx) => {
                                if (idx == 0 || idx == 1) {
                                    return <th key={idx} scope="col" data-tablesaw-priority="persist">{headerItem}</th>
                                }
                                return <th key={idx} scope="col" data-tablesaw-priority={idx - 1}>{headerItem}</th>
                            })}
                        </tr>
                    </thead>
                    <tbody>
                        {firstTblRows}
                    </tbody>
                </table>
            </div> : "";

        var secondTblRows = this.props.messageTable != null && this.props.messageTable.secondTableRows != null ? this.props.messageTable.secondTableRows.map((item, idx) => {
            return <tr key={idx}>
                {item.items.map((rowItem, rowIdx) => {
                    if (rowIdx == 0 || rowIdx == 1) {
                        return <td key={rowIdx} className="tablesaw-priority-persit">{rowItem}</td>
                    }
                    return <td key={rowIdx} className={"tablesaw-priority-" + (idx - 1)}>{rowItem}</td>
                })
                }
            </tr>
        }) : <tr></tr>


        return (
            <div>
                {tableInfo}
                {firstTbl}
                <div className="lm__table-wrapper">
                    <table className="tablesaw" data-tablesaw-mode="columntoggle">
                        <thead>
                            <tr>
                                {this.props.messageTable.secondHeaderItems.map((headerItem, idx) => {
                                    if (idx == 0 || idx == 1) {
                                        return <th key={idx} scope="col" data-tablesaw-priority="persist">{headerItem}</th>
                                    }
                                    return <th key={idx} scope="col" data-tablesaw-priority={idx - 1}>{headerItem}</th>
                                })}
                            </tr>
                        </thead>
                        <tbody>
                            {secondTblRows}
                        </tbody>
                    </table>
                </div>
            </div>
        )
    }
}

interface MessagesDetailProps {
    //event
    updateTotalMessagesInfo: () => void;
    onMarkToUnreadFromDetail: (message: Message) => void;
    onMarkToStarredFromDetail: (message: Message) => void;
    onMoveToTrashFromDetail: () => void;
}

interface MessageDetailState {
    message: Message;
}

export class MessageDetail extends React.Component<MessagesDetailProps, MessageDetailState>{
    private isLoaded: boolean;
    private ignoreLastFetch: boolean;

    constructor(props: MessagesDetailProps) {
        super(props);
        this.state = {
            message: null
        }
    }

    getDetailMesasge(id: number): void {
        this.isLoaded = false;
        helper.showSpinner(true);
        messageServices.getMessage(id).then((data) => {

            if (!this.ignoreLastFetch) {
                if (data == undefined) {
                    data = { message: undefined }
                }
                this.isLoaded = true;

                if (data.isNeedUpdate) {
                    this.props.onMarkToUnreadFromDetail(data.message);
                    helper.updateTotalUnread(false, 1);
                }

                this.setState({ message: data.message });
            }
             helper.showSpinner(false);
        });
    }

    componentDidMount(): void {
        this.ignoreLastFetch = false;
        this.getDetailMesasge(this.props["params"].mesid);
        helper.showSpinner(false);
    }

    componentWillReceiveProps(newProps: MessagesDetailProps) {
        if (this.props["params"].mesid != newProps["params"].mesid) {
            this.getDetailMesasge(newProps["params"].mesid);
        }
    }

    shouldComponentUpdate(nextProps: any, nextState: any) {
        var yes = this.isLoaded;
        return yes;
    }

    componentDidUpdate(prevProps: any, prevState: any): void {
        if (this.isLoaded) {
            $(document).trigger("enhance.tablesaw");
        }
    }

    componentWillUnmount() {
        this.ignoreLastFetch = true;
    }

    markToUnread(isUnRead: boolean): void {
        messageServices.markToUnRead(";" + this.state.message.id, isUnRead).then((data) => {
            if (data.success) {
                helper.updateTotalUnread(isUnRead, 1);
                this.props.onMarkToUnreadFromDetail(this.state.message);
            } else {
                alert("Unable to update read mode for this message.")
                this.state.message.isUnRead = !this.state.message.isUnRead;
                this.setState({ message: this.state.message });
            }
        });
    }

    markToStarred(isStarred: boolean): void {
        var type = this.props["params"].tabid;
        messageServices.markToStarred(this.state.message.id, isStarred).then((data) => {
            if (data.success) {

                helper.updateTotalStarred(isStarred, 1);
                if (this.state.message.isStarred == false && type == "starred") {
                    globalStore.rememberSelectedStatusToGlobal(type, [this.state.message]);
                }
                globalStore.setPageIndex("starred", 1);
                globalStore.setCategoriesByType("starred", data.categories);
                this.props.onMarkToStarredFromDetail(this.state.message)
                this.props.updateTotalMessagesInfo();
            } else {
                alert("Unable to update starred mode for this message.")
                this.state.message.isStarred = !this.state.message.isStarred;
                this.setState({ message: this.state.message });
            }
        });
    }

    moveToTrash(): void {
        if (!this.state.message) return;

        var type = this.props["params"].tabid;
        var msgParams = ";" + this.state.message.id;
        helper.showSpinner(true);
        messageServices.moveToTrash(msgParams).then((data) => {
            if (data.success) {
                //console.log("moveToTrash successful");
                globalStore.rememberSelectedStatusToGlobal(type, [this.state.message]);

                globalStore.setPageIndex("starred", 1);
                globalStore.setCategoriesByType("starred", data.categories);
                this.props.updateTotalMessagesInfo();
                this.props.onMoveToTrashFromDetail();
                this.context["router"].push("/messages/" + type);
            } else {
                helper.showSpinner(false);
            }
        });
    }

    moveToInbox(): void {
        if (!this.state.message) return;

        var type = 'trash';
        var msgParams = ";" + this.state.message.id
        helper.showSpinner(true);
        messageServices.moveToInbox(type, msgParams).then((data) => {
            helper.showSpinner(false);
            if (data.success) {
                globalStore.rememberSelectedStatusToGlobal(type, [this.state.message]);
                globalStore.setPageIndex(type, 1);
                globalStore.setCategoriesByType(type, data.categories);
                this.props.updateTotalMessagesInfo();
                this.props.onMoveToTrashFromDetail();
            }
        });
    }

    removeMessage(): void {
        if (!this.state.message) return;

        var type = 'trash';
        var msgParams = ";" + this.state.message.id
        helper.showSpinner(true);
        messageServices.deleteFromTrash(type, msgParams).then((data) => {
            if (data.success) {

                globalStore.setPageIndex(type, 1);
                globalStore.setCategoriesByType(type, data.categories);
                this.props.onMoveToTrashFromDetail();
                this.context["router"].push("/messages/" + type);
            } else {
                helper.showSpinner(false);
            }
        });
    }

    render() {

        var isDelete = this.state.message && this.state.message.isDelete;
        var categoryName = this.state.message ? this.state.message.categoryName : "";
        var header = this.state.message ? this.state.message.header : "";
        var receivedDate = this.state.message ? this.state.message.receivedDate : "";
        var content = this.state.message ? this.state.message.content : "";
        var customerName = this.state.message ? this.state.message.customerName : "";
        var customerAddress = this.state.message ? this.state.message.customerAddress : "";
        var customerZipAndCity = this.state.message ? this.state.message.customerZipAndCity : "";
        var messageType = this.state.message ? this.state.message.messageType : 0;
        var deliveryView = this.state.message ? this.state.message.deliveriesView : "";

        //the delivery table:
        var deliveryTableInfor = this.state.message && this.state.message.messageTable ? this.state.message.messageTable : null;
        var deiveryTableComponent = deliveryTableInfor != null && this.state.message.messageType != 15
            ? <MessageTableView messageTable={this.state.message.messageTable} messageType={messageType} customerName={customerName} customerAddress={customerAddress} customerZipAndCity={customerZipAndCity} />
            : "";
        if (this.state.message && this.state.message.messageType == 15) {
            deiveryTableComponent = <div dangerouslySetInnerHTML={{ __html: deliveryView }}></div>;
        }

        var messageSignElement = document.getElementById('message-signature');
        var messageSignature = messageSignElement ? messageSignElement.innerHTML : "";

        if (isDelete) {
            return (
                <div className="lm__meddelanden-detail-column">
                    <table className="detail-nav">
                        <tbody>
                            <tr>
                                <td>
                                    <Link to={"/messages/" + this.props["params"].tabid}><i className="fa fa-arrow-left" aria-hidden="true"></i>Till Meddelanden</Link>
                                </td>
                                <td>

                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <div className="detail-content">
                        <div className="detail-main-content">
                            <h1>This message is deleted!</h1>
                        </div>

                    </div>
                </div>
            )
        }
        return (
            <div className="lm__meddelanden-detail-column">
                <div className="loader-wrapper" id="loader" style={{ display: "none" }}>
                    <Spinner color="#3498db" period={0.8} thickness={3} size={30} />
                </div>
                <table className="detail-nav">
                    <tbody>
                        <ActionPanel
                            tabid={this.props["params"].tabid}
                            message={this.state.message}
                            markToStarred={(nv) => this.markToStarred(nv)}
                            markToUnread={(nv) => this.markToUnread(nv)}
                            moveToTrash={() => this.moveToTrash()}
                            moveToInbox={() => this.moveToInbox()}
                            removeMessage={() => this.removeMessage()} />
                    </tbody>
                </table>
                <div className="detail-content">
                    <table className="detail-meta">
                        <tbody>
                            <tr>
                                <td><p>Område: <span className="detail-cat">{categoryName}</span></p></td>
                                <td><p className="detail-date">{receivedDate}</p></td>
                            </tr>
                        </tbody>
                    </table>
                    <div className="detail-main-content">
                        <h1>{header}</h1>
                        <div className="richtext-display" dangerouslySetInnerHTML={{ __html: content }}></div>
                        {deiveryTableComponent}
                        <div dangerouslySetInnerHTML={{ __html: messageSignature }}></div>
                    </div>
                </div>
            </div>
        )
    }
}

MessageDetail["contextTypes"] = {
    router: React.PropTypes.object.isRequired
}