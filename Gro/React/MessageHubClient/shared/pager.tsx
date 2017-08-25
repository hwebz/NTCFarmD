import * as React from 'react';
import * as ReactDOM from 'react-dom';
import {isMobile} from './device-detect';

export interface PagerProp {
    currentPageNumber: number;
    pageSize: number;
    // totalPages: number;
    totalItemsForDisplaying: number;
    totalItems: number;
    preventClick?: boolean;

    // event:
    changePage_onClick?: (newPageNumber: number, isShowmore: boolean) => void
    showMore_onClick?: (newPageNumber: number) => void
}

export class Pager extends React.Component<PagerProp, any>{
    startIndex: number;
    endIndex: number;
    pageNumbers: number[];
    totalPages: number;
    isMobile: boolean;

    constructor() {
        super();
        this.startIndex = 0;
        this.endIndex = 0;
        this.pageNumbers = [];
        this.totalPages = 0;
        this.isMobile = isMobile();
    }

    pageNumber_onClick(newPageNumber: number, isShowmore?: boolean) {
        if (!!this.props.preventClick) { return; }

        this.props.changePage_onClick(newPageNumber, isShowmore);
    }

    getIndexsClassName(): string {
        return "pageIndex float-right" + (this.props.totalItemsForDisplaying <= 1 ? ' hidden' : '');
    }

    calculateTotalPages(totalItems: number, pageSize: number) {
        if (totalItems == 0 || pageSize == 0) {
            return 0;
        }
        return totalItems % pageSize == 0 ? totalItems / pageSize : Math.floor(totalItems / pageSize) + 1;
    }

    getInforToRenderPager(upToDateprops: PagerProp) {
        var props = upToDateprops || this.props;
        if (props.totalItemsForDisplaying == 0) {
            return [];

        }
        var currentPageNumber = props.currentPageNumber;
        var pageSize = props.pageSize;
        // var totalPages = props.totalItems % pageSize == 0 ? props.totalItems / pageSize : Math.floor(props.totalItems / pageSize) + 1;
        var totalPages = this.calculateTotalPages(props.totalItems, props.pageSize);

        var startPage, endPage;
        if (totalPages <= 10) {
            // less than 10 total pages so show all
            startPage = 1;
            endPage = totalPages;
        } else {
            // more than 10 total pages so calculate start and end pages
            if (props.currentPageNumber <= 6) {
                startPage = 1;
                endPage = 10;
            } else if (props.currentPageNumber + 4 >= totalPages) {
                startPage = totalPages - 9;
                endPage = totalPages;
            } else {
                startPage = props.currentPageNumber - 5;
                endPage = props.currentPageNumber + 4;
            }
        }

        this.startIndex = (props.currentPageNumber - 1) * pageSize;
        this.endIndex = this.startIndex + Math.min(pageSize, props.totalItemsForDisplaying) - 1;
        // this.pageNumbers = Array.from(Array(endPage + 1).keys()).slice(startPage, endPage + 1);
        this.pageNumbers = Array.apply(null, { length: endPage + 1 }).map(Number.call, Number).slice(startPage, endPage + 1);
        this.totalPages = totalPages;
    }

    shouldComponentUpdate(newProps: PagerProp, newState) {
        var shouldUpdate = this.props.totalItems != newProps.totalItems
            || this.props.totalItemsForDisplaying != newProps.totalItemsForDisplaying
            || this.props.currentPageNumber != newProps.currentPageNumber
            || this.props.preventClick != newProps.preventClick;
        return shouldUpdate;
    }

    render() {
        if (!this.isMobile) {
            let summaryStyle: React.CSSProperties = this.props.preventClick ? {
                display: "none"
            } : {};

            this.getInforToRenderPager(this.props);
            return (
                <div className={this.props.totalItemsForDisplaying == 0 ? ' lm__additional hidden' : 'lm__additional'}>
                    <div className="lm__pagination">
                        <ul>
                            <li className={this.props.currentPageNumber == 1 ? 'hidden' : ''}
                                onClick={_ => this.pageNumber_onClick(this.props.currentPageNumber - 1, false) }>
                                <a href="javascript:void(0)"><i className="fa fa-arrow-left"></i></a>
                            </li>
                            { this.pageNumbers.map((pageNumber, idx) =>
                                <li key={idx} className={pageNumber == this.props.currentPageNumber ? 'active' : ''} onClick={_ => this.pageNumber_onClick(pageNumber, false) }><a href="javascript:void(0)">{pageNumber}</a> </li>
                            ) }
                            <li className={this.props.currentPageNumber == this.totalPages ? 'hidden' : ''} onClick={_ => this.pageNumber_onClick(this.props.currentPageNumber + 1, false) }><a href="javascript:void(0)"><i className="fa fa-arrow-right"></i></a></li>
                        </ul>
                    </div>
                    <span className="foljesedlar-summary" style={summaryStyle}>
                        Visar {this.startIndex + 1} - {this.endIndex + 1} av {this.props.totalItems} meddelanden
                    </span>
                </div>
            );
        }

        let showMoreStyle: React.CSSProperties = !this.props.preventClick ? {} : {
            display: "none"
        };

        var totalPage = this.calculateTotalPages(this.props.totalItems, this.props.pageSize);
        var showMore = this.props.currentPageNumber != totalPage && totalPage != 0 ? (
            <a className="lm__blue-btn" href="javascript:void(0)" onClick={_ => this.pageNumber_onClick(this.props.currentPageNumber + 1, true) }
                style={showMoreStyle}>
                Visa fler meddelanden
            </a>
        ) : "";

        return (
            <div className="lm__view-more-messages"  style={{
                opacity: !this.props.preventClick ? 1 : 0.6
            }}>
                {showMore}
            </div>
        );
    }
}
