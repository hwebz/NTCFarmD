import * as React from 'react';
import * as ReactDOM from 'react-dom';

interface ModalProps {
    isOpen: boolean;
    isShowCloseIcon?: boolean;
    isShowSpinner?: boolean;
    modalTitle?: string;

    closeModal: () => void
}

export class Modal extends React.Component<ModalProps, any> {
    constructor() {
        super();
    }

    closeModal(): void{
        this.props.closeModal();
    }

    render() {
        var style = this.props.isOpen ? { display: "block" } : { display: "none" };
        var closeIcon = this.props.isShowCloseIcon ? <a className="modal-close" onClick={_=>this.closeModal()} href="javascript:void(0)"><i className="fa fa-times"></i></a> : null;
        var modalTitle = this.props.modalTitle ?
            <div className="modal-header-title" style={{padding:"10px 50px 10px 30px"}}>
                <h3>{this.props.modalTitle}</h3>
            </div> : null;
        var spinnerStyle = this.props.isShowSpinner ? { display: "block" } : { display: "none" };

        return (
            <div className="lm__modal-alert" style={style}>
                <div className="lm__modal-dialog">
                    {closeIcon}
                    {modalTitle}
                    {this.props.children}
                    <div className="loader-wrapper" style={spinnerStyle}>
                        <div className="loader"></div>
                    </div>
                </div>
            </div>
        );
    }
};
