import * as React from 'react';
import { Link, hashHistory } from 'react-router';
import * as adminService from './services'
import { MessageItem, MessageExtendedInfo, MessageDetail } from './administerModels';
import { ReceiverInfo } from "./receiversInfo";
import { messageStore } from './messageStore';
import { Spinner } from '../../components/spinner';

export interface MessageProp {
    params: {
        messageId: number;
    }
}

interface MessageDetailPageState {
    message: MessageDetail;
}

export class MessageDetailPage extends React.Component<MessageProp, MessageDetailPageState>{
    ignoreLastFetch: boolean = false;

    constructor(props) {
        super(props);
        this.state = {
            message: null,
        };
    }

    onFilterChange = () => {
        hashHistory.push("/");
    };

    componentDidMount() {
        messageStore.addListener("filter", this.onFilterChange);

        this.queryForMessage();
    }

    componentWillReceiveProps(newProps: MessageProp) {
        if (newProps.params.messageId == this.props.params.messageId) {
            return;
        }

        this.setState({
            message: null
        });
    }

    componentWillUnmount() {
        this.ignoreLastFetch = true;
        messageStore.removeListener("filter", this.onFilterChange);
    }

    componentDidUpdate() {
        this.queryForMessage();
    }

    queryForMessage() {
        if (!!this.state.message) { return; }
        adminService.getMessageDetail(this.props.params.messageId).then(rs => {
            if (this.ignoreLastFetch) { return; }
            this.setState({
                message: rs
            });
        }).catch(reason => {
            alert(`Cannot display message: ${reason}`)
        });
    }

    render() {
        let body: JSX.Element = null;
        if (!this.state.message) {
            body = <Spinner color="#3498db" period={0.8} thickness={3} size={30} />
        } else {
            var smsText = this.state.message ? this.state.message.Message.MsgText : null;
            var sendDate = this.state.message ? this.state.message.Message.ValidFrom.toLocaleDateString("sv-SV") : "";
            var receivers = this.state.message ? this.state.message.receivers : [];
            var headline = !this.state.message ? "" : this.state.message.Message.HeadLine;
            var mailBody = !this.state.message ? "" : this.state.message.Message.MailMessage;
            var categoryDescription = !this.state.message ? "" : this.state.message.Message.AreaDescription;
            var typeDescription = !this.state.message ? "" : this.state.message.Message.TypeDescription;

            body = (
                <div>
                    <div className="meddelanden-metadata meddelande-box">
                        <ul className="meddelanden-cate">
                            <li><a href="#">{categoryDescription}</a></li>
                            <li><a href="#">{typeDescription}</a></li>
                        </ul>
                        <span className="meddelanden-date">{sendDate}</span>
                    </div>
                    <div className="lm2-meddelande-epost meddelande-box">
                        <div className="epost-header">
                            <h3 className="lm2-meddelande-epost__title meddelande-title">LM2-meddelande & E-post</h3>
                            <ReceiverInfo receivers={receivers} />
                            <div className="lm2-meddelande-epost__content detail-main-content">
                                <h1>{headline}</h1>
                                <div className="richtext-display" dangerouslySetInnerHTML={{ __html: mailBody }}></div>
                            </div>
                        </div>
                    </div>
                    <div className="meddelande-sms meddelande-box">
                        <h3 className="meddelande__title meddelande-title">SMS</h3>
                        <div className="meddelande-sms__content detail-main-content">
                            <p>{smsText}</p>
                        </div>
                    </div>
                </div>
            );
        }

        return (
            <div className="layout__item u-1/1 u-2/3-mobile u-4/5-desktop">
                <div className="lm__administrera-meddelanden lm__administrera-meddelanden-detail">
                    <div className="listing-link meddelande-box">
                        <Link to="/" style={{ paddingLeft: "10px" }}><i className="fa fa-arrow-left" aria-hidden="true" ></i>Till Meddelanden</Link>
                    </div>
                    {body}
                </div>
            </div>
        );
    }
}
