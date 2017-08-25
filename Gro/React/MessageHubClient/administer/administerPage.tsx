import * as React from 'react';
import { MessageCategory, MessageType, MessageItem } from './administerModels';
import { AdministerMessageList } from './administerMessageList';
import { Router, Route, IndexRoute, Link, hashHistory } from 'react-router';
import { messageSetting } from '../shared/messageSetting';
import { MessageFilterPanel } from './messageFilterPanel';
import { messageStore } from './messageStore';
import { changeFilter } from './services';

interface NewMessageButtonProps {
    onNewMessageSelect: (type: string) => void;
}

interface NewMessageButtonState {
    dropdownDisplayStyle: string;
}

class NewMessageButton extends React.Component<NewMessageButtonProps, NewMessageButtonState>{
    constructor(props) {
        super(props);
        this.state = {
            dropdownDisplayStyle: "none"
        };
    }

    onButtonClick() {
        this.setState({
            dropdownDisplayStyle: this.state.dropdownDisplayStyle == "none" ? "block" : "none"
        });
    }

    render() {
        return (
            <div className="creating-meddelande">
                <ul className="lm__form-dropdown type-3">
                    <li className="showcase">
                        <a onClick={e => this.onButtonClick()}>Nytt meddelande</a>
                        <ul style={{ display: this.state.dropdownDisplayStyle }}>
                            <li onClick={e => this.props.onNewMessageSelect("standard")}><a >Styrt meddelande <small>Fasta mottagare</small></a></li>
                            <li onClick={e => this.props.onNewMessageSelect("free")}><a>Fritt meddelande <small>Valfria mottagare</small></a></li>
                        </ul>
                    </li>
                </ul>
            </div>
        );
    }
}

interface AdministerMessagePageState {
    listMessageAreas: MessageCategory[];
}

export class AdministerPage extends React.Component<any, AdministerMessagePageState>{
    listMessageAreas: MessageCategory[];
    selectedType: MessageType;
    selectedCate: MessageCategory;
    selectedMessageId: number;
    createMessageSelector: HTMLSelectElement;

    constructor() {
        super();
        // default value
        this.listMessageAreas = [];
        var pageSize = messageSetting.getAdminPageSize();
    }

    componentDidMount(){
        document.getElementById("mainlist-backbutton").style.display = "none";
    }

    componentWillUnmount(){
        document.getElementById("mainlist-backbutton").style.display = "block";
    }

    onNewMessageSelect(value) {
        if (value == "0") {
            return;
        }

        hashHistory.push(`/create-message/${value}`);
    }

    render() {
        return (
            <div>
                <div className="layout__item u-1/1 u-1/3-mobile u-1/5-desktop">
                    <div className="lm__administrera-meddelanden-nav">
                        <NewMessageButton onNewMessageSelect={t => this.onNewMessageSelect(t)} />
                        <MessageFilterPanel />
                    </div>
                </div >
                {this.props.children}
            </div >
        );
    }
}
