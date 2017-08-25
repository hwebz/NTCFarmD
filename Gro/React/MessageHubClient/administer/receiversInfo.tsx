import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Link } from 'react-router';
import { MessageItem, MessageExtendedInfo, MessageDetail } from './administerModels';

function isPointInsideRect(x: number, y: number, rect: ClientRect) {
    return x >= rect.left && x <= rect.right
        && y <= rect.bottom && y >= rect.top;
}

interface ReceiverInfoState {
    isFlyoutShowing: boolean;
}

export interface ReceiverInfoProp {
    receivers: string[];
}

export class ReceiverInfo extends React.Component<ReceiverInfoProp, ReceiverInfoState>{
    area: HTMLLabelElement;
    flyout: HTMLDivElement;

    constructor(props) {
        super(props);
        this.state = {
            isFlyoutShowing: false
        };
    }

    componentDidMount() {
        document.addEventListener('click', this.onDocumentClick);
    }

    componentWillUnmount() {
        document.removeEventListener('click', this.onDocumentClick);
    }

    onDocumentClick = (e: MouseEvent) => {
        if (!this.state || (e.clientX <= 0 && e.clientY <= 0)) {
            return;
        }

        var area = this.area;
        var flyout = this.flyout;
        var areaRect = ReactDOM.findDOMNode(area).getBoundingClientRect();
        var flyoutRect = ReactDOM.findDOMNode(flyout).getBoundingClientRect();

        if (isPointInsideRect(e.clientX, e.clientY, areaRect)
            || isPointInsideRect(e.clientX, e.clientY, flyoutRect)) {
            return;
        }

        if (this.state.isFlyoutShowing == true) {
            this.closeFlyout();
        }
    }

    closeFlyout() {
        this.setState({
            isFlyoutShowing: false
        });
    }

    openFlyout() {
        this.setState({
            isFlyoutShowing: true
        });
    }

    onTriggerClick(e: React.MouseEvent<HTMLLabelElement>) {
        if (this.state.isFlyoutShowing) {
            this.closeFlyout();
        } else {
            this.openFlyout();
        }
    }

    render() {
        var flyOutStyles: React.CSSProperties = {
            display: this.state.isFlyoutShowing ? "block" : "none",
            visibility: this.state.isFlyoutShowing ? "visible" : "hidden",
            opacity: this.state.isFlyoutShowing ? 1 : 0,
            minHeight: "25px"
        };

        var receiverDetails = this.props.receivers.map((item, idx) =>
            <p key={idx} className="reciever-info"><span className="email">{item}</span></p>
        );

        return (
            <div className="lm__tooltip">
                <div className="lm__tooltip__wrapper">
                    <input type="checkbox" name="toggle" id="toggle" />
                    <label htmlFor="toggle" onClick={e => this.onTriggerClick(e)} ref={r => this.area = r}></label>

                    <div className="lm__tooltip__content" ref={r => this.flyout = r} style={flyOutStyles}>
                        <div className="user-emails">
                            {receiverDetails}
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}
