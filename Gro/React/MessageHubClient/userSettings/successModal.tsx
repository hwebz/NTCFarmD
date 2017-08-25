import * as React from 'react';
import * as ReactDOM from 'react-dom';

interface SavingSuccessModalProps {
    state: string;

    confirmClick: () => void;
}

export class SavingSuccessModal extends React.Component<SavingSuccessModalProps, any> {
    confirmClick() {
        this.props.confirmClick();
    }

    render() {
        var title = this.props.state == "saving" ? "Sparande" : "Klart - Ändringar sparade";
        var button = this.props.state == "saving" ? null :
            <button className="success-confirm-inform"
                onClick={e => this.confirmClick() }>Stäng</button>;
        var successModal = this.props.state == "saving" ? null :
            <span className="success-icon"></span>;

        return (
            <div className="lm__modal-alert" style={{ display: "block" }}>
                <div className="lm__modal-dialog">
                    <div className="modal-content-inform">
                        {successModal}
                        <h3 className="success-header-title" style={{ textAlign: "center" }}>
                            {title}
                        </h3>
                        <div className="button-confirm">
                            {button}
                        </div>
                    </div>
                </div>
            </div>
        );
    }
};
