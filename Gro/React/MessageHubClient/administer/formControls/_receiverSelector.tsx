import * as React from 'react';
function isSafari() {
    if (navigator.userAgent.indexOf('Safari') != -1 && navigator.userAgent.indexOf('Chrome') == -1) {
        console.log('Safari 😱');
        return true;
    }

    return false;
}

var phoneNumberPlaceholder = isSafari() ? "46 71 234 56 78" : "46 71 234 56 78\n46 71 234 56 78";
var emailPlaceholder = isSafari() ? "exempel1@mail.com" : "exempel1@mail.com\nexempel1@mail.com";

interface EmailsInputProps {
    error: string;
}

export class EmailsInput extends React.Component<EmailsInputProps, any>{
    textarea: HTMLTextAreaElement;

    getEmails(): string[] {
        if (!this.textarea.value) { return []; }
        return this.textarea.value.split('\n');
    }

    onFocus() {
        this.textarea.placeholder = "";
    }

    onBlur() {
        this.textarea.placeholder = emailPlaceholder;
    }

    render() {
        return (
            <div>
                <h4>Lägg till E-postadresser manuellt</h4>
                <textarea name="emails" className={this.props.error != null ? 'rubrik error' : ''}
                    ref={r => this.textarea = r} placeholder={emailPlaceholder}
                    onFocus={e => this.onFocus()}
                    onBlur={e => this.onBlur()}></textarea>
                <span className="error-item">{this.props.error}</span>
            </div>

        );
    }
}

interface PhoneNumbersInputProps {
    error: string;
}

export class PhoneNumbersInput extends React.Component<PhoneNumbersInputProps, any>{
    textarea: HTMLTextAreaElement;

    getPhoneNumbers(): string[] {
        if (!this.textarea.value) { return []; }
        return this.textarea.value.split('\n');
    }

    onFocus() {
        this.textarea.placeholder = "";
    }

    onBlur() {
        this.textarea.placeholder = phoneNumberPlaceholder;
    }

    render() {
        return (
            <div>
                <h4>Lägg till telefonnummer manuellt</h4>
                <textarea className={this.props.error != null ? 'rubrik error' : ''}
                    name="phones" ref={r => this.textarea = r} placeholder={phoneNumberPlaceholder}
                    onFocus={e => this.onFocus()}
                    onBlur={e => this.onBlur()}></textarea>
                <span className="error-item">{this.props.error}</span>
            </div>
        );
    }
}

interface ReceiverSelectorProps {
    accordion: boolean;
    errors: string[];
}

interface ReceiverSelectorState {
    open: boolean;
}

export class ReceiverSelector extends React.Component<ReceiverSelectorProps, ReceiverSelectorState>{
    private emailsFromFile: string[] = [];
    private phonesFromFile: string[] = [];

    emailsInput: EmailsInput;
    phoneNumbersInput: PhoneNumbersInput;

    constructor(props) {
        super(props);
        this.state = {
            open: this.props.accordion ? false : true
        };
    }

    onAccordionClick() {
        this.setState({
            open: !this.state.open
        });
    }

    getEmails() {
        return this.emailsFromFile.concat(...this.emailsInput.getEmails());
    }

    getPhoneNumbers() {
        return this.phonesFromFile.concat(...this.phoneNumbersInput.getPhoneNumbers());
    }

    isOpen() {
        return this.state.open;
    }

    render() {
        var outerClass = this.props.accordion ? "meddelanden-accordion" : "";
        outerClass += " receiver-selector";

        var accordionLinkClass = "accordion-control " + (this.state.open ? "expanded" : "");
        var accordionTitle = !this.props.accordion ? null :
            <a className={accordionLinkClass}
                onClick={e => this.onAccordionClick()}>Lägg till mottagare</a>;

        var contentDisplayStyle: React.CSSProperties = {
            display: this.state.open ? "block" : "none"
        };

        var errors: JSX.Element[] = [];
        for (var idx = 0; idx < this.props.errors.length; idx++) {
            var err = this.props.errors[idx];
            errors.push(
                <span className="error-item" key={idx.toString()}>{err}</span>
            );
            errors.push(<br key={idx.toString() + "br"} />);
        }

        var generalError = this.getGeneralError();
        var emailError = this.getEmailError();
        var phoneError = this.getPhoneNumberError();

        return (
            <div className={outerClass}>
                {accordionTitle}
                <div className="accordion-content" style={contentDisplayStyle}>
                    <h3 style={{ marginTop: "0" }}> {"Lägg till mottagare" + (!this.props.accordion && this.state.open ? "*" : "")}</h3>
                    <span className="error-item">{generalError}</span>

                    <h4 className="hidden">Välj mottagare från fil</h4>
                    <div className="file-upload-wrapper hidden" data-back-btn-text="Mottagare från fil">
                        <input type="file" name="mottagare[]" className="file-upload-field" />
                    </div>

                    <EmailsInput ref={r => this.emailsInput = r}
                        error={emailError} />

                    <PhoneNumbersInput ref={r => this.phoneNumbersInput = r}
                        error={phoneError} />
                </div>
            </div>
        );
    }

    getGeneralError() {
        var noreceiverError = this.props.errors.filter(e => e.startsWith("receivers"))[0] || null;
        if (!!noreceiverError) {
            noreceiverError = noreceiverError.substr("receivers:".length);
        }

        return noreceiverError;
    }

    getEmailError() {
        var emailError = this.props.errors.filter(e => e.startsWith("emails:"))[0] || null;
        if (!!emailError) {
            emailError = emailError.substr("emails:".length);
        }

        return emailError;
    }

    getPhoneNumberError() {
        var phoneError = this.props.errors.filter(e => e.startsWith("phones:"))[0] || null;
        if (!!phoneError) {
            phoneError = phoneError.substr("phones:".length);
        }

        return phoneError;
    }
}
