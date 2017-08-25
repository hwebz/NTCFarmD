import * as React from 'react';
import { MessageCategory, MessageType, MessageItem } from './administerModels';
import { Link, hashHistory } from 'react-router';

interface MessageItemRowProps {
    item: MessageItem;
}

export class MessageRow extends React.Component<MessageItemRowProps, {}>{

    shouldComponentUpdate(newProps: MessageItemRowProps) {
        return this.props.item != newProps.item;
    }

    render() {
        let link = `/message-detail/${this.props.item.MessageId}`;

        return (
            <tr className="message-item-row">
                <td><Link to={link}>{this.props.item.AreaDescription}</Link> </td>
                <td><Link to={link}>{this.props.item.TypeDescription} </Link> </td>
                <td><Link to={link}>{this.props.item.HeadLine} </Link> </td>
                <td><Link to={link}>{!this.props.item.SendDate ? "" : this.props.item.SendDate.toLocaleDateString("sv-SV")} </Link> </td>
            </tr>
        );
    }
}
