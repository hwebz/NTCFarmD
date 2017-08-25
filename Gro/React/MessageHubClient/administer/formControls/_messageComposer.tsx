import * as React from 'react';
import {RichEditor} from './_richEditor';

// File
// <div className="attachment-wrapper">
//     <div className="attachment-form">
//         <input type="file" name="attachments" id="attachments" multiple />
//     </div>
//     <ul className="attachment-files">
//         <li>file1.pdf<a ><i className="fa fa-times-circle" aria-hidden="true"></i></a></li>
//         <li>file2.pdf<a ><i className="fa fa-times-circle" aria-hidden="true"></i></a></li>
//         <li>file3.pdf<a ><i className="fa fa-times-circle" aria-hidden="true"></i></a></li>
//     </ul>
// </div>

const maxSmsLength = 160;

interface SmsTextBoxState {
    remainingSmsTextLength: number;
}

class SmsTextBox extends React.Component<any, SmsTextBoxState> {
    smsText: HTMLTextAreaElement;

    constructor() {
        super();
        this.state = {
            remainingSmsTextLength: maxSmsLength
        };
    }

    smsText_onChange() {
        var currentLength = !this.smsText.value ? 0 : this.smsText.value.length;
        this.setState({
            remainingSmsTextLength: maxSmsLength - currentLength
        });
    }

    getValue() {
        return this.smsText.value;
    }

    render() {
        var smsTextHeader = `SMS-meddelande* (max ${this.state.remainingSmsTextLength} tecken)`;

        return (
            <div className="lm__administrera-form__column">
                <label htmlFor="sms">{smsTextHeader}</label>
                <textarea name="sms" placeholder="Skriv ett meddelande" className="sms" maxLength={maxSmsLength}
                    ref={r => this.smsText = r}
                    onChange={e => this.smsText_onChange() }></textarea>
            </div>
        );
    }
}

// <textarea name="epost-meddelande" className="epost-meddelande" required={true}
//     placeholder="Skriv ett meddelande"
//     ref={r => this.bodyTextArea = r}></textarea>

interface MessageComposerProps {
    errors: string[];
}

export class MessageComposer extends React.Component<MessageComposerProps, any>{
    titleInput: HTMLInputElement;
    smsTextBox: SmsTextBox;
    editor: RichEditor;

    getTitle() {
        return this.titleInput.value;
    }

    getBody() {
        return this.editor.getMarkup();
    }

    getSmsText() {
        return this.smsTextBox.getValue();
    }

    render() {
        let titleError = this.getTitleError();
        let bodyError = this.getBodyError();
        let smsError = this.getSmsError();

        return (
            <div>
                <h3>Skapa meddelande</h3>
                <div className="lm__administrera-form__column">
                    <label htmlFor="rubrik">Rubrik*</label>
                    <input type="text" name="rubrik" className={'rubrik' + (titleError != null ? ' error' : '')}
                        placeholder="Skriv en rubrik"
                        ref={r => this.titleInput = r}/>
                    <span className="error-item">{titleError}</span>
                </div>
                <div className="lm__administrera-form__column richtext-editor richtext-display">
                    <label htmlFor="epost-meddelande">LM2 och E-postmeddelande*</label>
                    <RichEditor ref={r => this.editor = r}/>
                    <span className="error-item">{bodyError}</span>
                </div>
                <SmsTextBox className={smsError != null ? 'rubrik error' : '' } ref={r => this.smsTextBox = r}/>
                <span className="error-item">{smsError}</span>
            </div>
        );
    }

    private getTitleError() {
        var titleError = this.props.errors.filter(e => e.startsWith("title:"))[0] || null;
        if (!!titleError) {
            titleError = titleError.substr("title:".length);
        }

        return titleError;
    }

    private getSmsError() {
        var smsError = this.props.errors.filter(e => e.startsWith("sms:"))[0] || null;
        if (!!smsError) {
            smsError = smsError.substr("sms:".length);
        }

        return smsError;
    }

    private getBodyError() {
        var titleError = this.props.errors.filter(e => e.startsWith("body:"))[0] || null;
        if (!!titleError) {
            titleError = titleError.substr("body:".length);
        }

        return titleError;
    }
}
