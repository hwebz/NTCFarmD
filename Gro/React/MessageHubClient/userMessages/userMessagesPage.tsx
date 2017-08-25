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
import { isMobile } from '../shared/device-detect';


export class NavItem extends React.Component<any, any> {
    constructor(props) {
        super(props);
    }

    static contextTypes = {
        router: React.PropTypes.object.isRequired
    }

    render() {
        let isActive = this.context["router"].isActive(this.props.to, false),// func isActive(location, onlyActiveOnIndex)
            className = isActive ? "active" : "";
        return (
            <li className={className}>
                <Link {...this.props}>
                    {this.props.children}
                </Link>
                <i className={this.props.icon} aria-hidden="true" />
            </li>
        );
    }
}

interface TabHeaderProps {
    inboxInfo: InboxInfo;
}

export class TabHeader extends React.Component<TabHeaderProps, any> {

    constructor(props) {
        super(props);
    }

    render() {
        return (
            <ul>
                <NavItem to="/messages/inbox" icon={"fa fa-envelope"}>
                    Inkorg
                    <span className="lm__meddelanden-count">{this.props.inboxInfo.inboxTotal}</span>
                </NavItem>
                <NavItem to="/messages/starred" icon={"fa fa-star-o"}>
                    Stjärnmärkta
                    <span className="lm__meddelanden-count">{this.props.inboxInfo.starredTotal}</span>
                </NavItem>
                <NavItem to="/messages/trash" icon={"fa fa-trash"}>
                    Papperskorg
                </NavItem>
            </ul>
        );
    }
}

interface InboxInfo {
    inboxTotal: number;
    starredTotal: number;
}

export class UserMessagesPage extends React.Component<any, any> {

    private inboxInfo: InboxInfo;
    private tabHeader: TabHeader;

    constructor(props: any) {
        super(props);
        this.inboxInfo = {
            inboxTotal: 0,
            starredTotal: 0
        }
    }

    componentDidMount(): void {
        this.updateTotalMessagesInfo();
    }

    updateTotalMessagesInfo(): void {
        messageServices.getTotalOfMessages().then((data) => {
            this.inboxInfo.inboxTotal = data.inboxTotal;
            this.inboxInfo.starredTotal = data.starredTotal;

            this.tabHeader && this.tabHeader.forceUpdate();
        });
    }

    render() {
        return (
            <div className="layout layout--large">
                <div className="layout__item u-1/1">
                    <div className="lm__meddelanden-tabs">
                        <nav>
                            <TabHeader ref={r => this.tabHeader = r} inboxInfo={this.inboxInfo} />
                        </nav>
                    </div>
                </div>
                <div className="layout__item u-1/1">
                    {this.props.children && React.cloneElement(this.props.children as React.ReactElement<any>, {
                        updateTotalMessagesInfo: nv => this.updateTotalMessagesInfo()
                    })}
                </div>
            </div>
        );
    }
}
