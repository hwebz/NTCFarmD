import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Link } from 'react-router';
import { MessageItem } from './administerModels';
import { Pager } from '../shared/pager';
import { messageSetting } from '../shared/messageSetting';
import { messageStore } from './messageStore';
import { Map, List } from 'immutable';
import * as adminService from './services';
import { MessageRow } from './messageRow';
import { Spinner } from '../../components/spinner';
import { isMobile } from '../shared/device-detect';

export interface AdministerMessageListState {
    messageItemList: List<MessageItem>;
    currentPageNumber: number;
    totalCount: number;
    loading: boolean;
}

export class AdministerMessageList extends React.Component<any, AdministerMessageListState>{
    pageNumber = 1;

    constructor(props) {
        super(props);
        this.state = {
            messageItemList: List<MessageItem>(),
            currentPageNumber: this.pageNumber,
            totalCount: 0,
            loading: true
        };
    }

    onMessageChange = () => {
        var currentKey = messageStore.getCurrentFilterKey();
        var messageFilterResult = messageStore.getMessages(currentKey);
        var filterStatus = messageStore.getFilterStatus(currentKey);

        var totalCountDisplay = filterStatus.loading ? this.state.totalCount : filterStatus.totalCount;

        var displayMessage = filterStatus.loading ? this.state.messageItemList : messageFilterResult;
        this.setState({
            messageItemList: displayMessage,
            currentPageNumber: this.pageNumber,
            totalCount: totalCountDisplay,
            loading: filterStatus.loading
        });
    }

    onFilterChange = () => {
        this.pageNumber = 1;
        var selectedTypes = messageStore.getSelectedTypes();
        var selectedCates = messageStore.getSelectedCategories();
        adminService.loadFilteredMessages(selectedCates, selectedTypes, this.pageNumber, messageSetting.getAdminPageSize(), false);
    };

    componentDidMount() {
        messageStore.addListener("messages", this.onMessageChange);
        messageStore.addListener("filter", this.onFilterChange);

        adminService.clearMessages();
        this.onFilterChange();
    }

    componentWillUnmount() {
        messageStore.removeListener("messages", this.onMessageChange);
        messageStore.removeListener("filter", this.onFilterChange);
    }

    pageNumber_onClick(updatedNumber: number, isShowMore: boolean) {
        this.pageNumber = updatedNumber;

        var selectedTypes = messageStore.getSelectedTypes();
        var selectedCates = messageStore.getSelectedCategories();
        adminService.loadFilteredMessages(selectedCates, selectedTypes, this.pageNumber, messageSetting.getAdminPageSize(), isShowMore);
    }

    getBody(): JSX.Element | JSX.Element[] {
        if (!this.state.loading && this.state.messageItemList.count() == 0) {
            return <tr><td colSpan={4} style={{ textAlign: "center" }}>{messageSetting.getMessageForEmptyCategory()}</td></tr>;
        }

        if (this.state.loading && !isMobile()) {
            return null;
        }

        var rows = this.state.messageItemList.map((item, idx) => (
            <MessageRow key={item.MessageId} item={item} />
        )).toArray();

        return rows;
    }

    render() {
        let pageSize = messageSetting.getAdminPageSize();

        let noClickStyle: React.CSSProperties = {
            pointerEvents: "none"
        };

        let body = this.getBody();

        //#3498db
        let spinner = !this.state.loading ? null : (
            <Spinner color="#3498db" period={0.8} thickness={3} size={30} />
        );

        return (
            <div className="layout__item u-1/1 u-2/3-mobile u-4/5-desktop">
                <div className="lm__administrera-meddelanden">
                    <div className="lm__table-wrapper" style={this.state.loading ? noClickStyle : {}}>
                        <table className="tablesaw" data-tablesaw-mode="columntoggle" >
                            <thead>
                                <tr>
                                    <th data-tablesaw-priority="persist">Område</th>
                                    <th data-tablesaw-priority="persist">Meddelandetyp</th>
                                    <th data-tablesaw-priority="1" className="tablesaw-priority-1" >Ämne</th>
                                    <th data-tablesaw-priority="2" className="tablesaw-priority-2">Datum</th>
                                </tr>
                            </thead>
                            <colgroup>
                                <col width="15%" />
                                <col width="25%" />
                                <col width="45%" />
                                <col width="15%" />
                            </colgroup>
                            <tbody >
                                {body}
                            </tbody>
                        </table>

                    </div>
                    <div style={{ width: "100%", marginTop: "5px", marginBottom: "5px" }}>
                        {spinner}
                    </div>
                    <Pager preventClick={this.state.loading}
                        totalItemsForDisplaying={this.state.messageItemList.count()}
                        currentPageNumber={this.state.currentPageNumber}
                        totalItems={this.state.totalCount}
                        pageSize={pageSize}
                        changePage_onClick={(np, isShowmore) => this.pageNumber_onClick(np, isShowmore)} />
                </div>
            </div>
        );
    }
}
