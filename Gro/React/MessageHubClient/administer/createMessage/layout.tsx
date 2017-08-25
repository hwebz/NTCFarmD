import * as React from 'react';


export class CreateMessageLayout extends React.Component<any, any>{
    getIntroduction() {
        if (this.props.location.pathname == "/create-message/free") {
            let freemessageHeader = document.getElementById("free-message-header").innerHTML;

            //free message
            return (
                <div className="administrera-introduction" dangerouslySetInnerHTML={{ __html: freemessageHeader }}>
                </div>
            );
        } else {
            //pre-defined message
            let standardMessageHeader = document.getElementById("standard-message-header").innerHTML;

            return (
                <div className="administrera-introduction" dangerouslySetInnerHTML={{ __html: standardMessageHeader }}>
                </div>
            );
        }
    }

    render() {
        var introduction = this.getIntroduction();

        return (
            <div className="layout__item u-1/1">
                <div className="lm__administrera-meddelanden">
                    <div className="lm__administrera-wrapper">
                        {introduction}
                        {this.props.children}
                    </div>
                </div>
            </div>
        );
    }
}
