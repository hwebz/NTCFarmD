import * as React from 'react';
import { MessageTypeSelector, ReceiverSelector, SenderPhoneNumberInput, MessageComposer } from '../formControls';
import { hashHistory, withRouter } from 'react-router';
import { MessageType, MessageCategory, StandardMessageValidationResult } from '../administerModels';
import * as adminService from '../services';
import { trySaveStandardMessage } from './actions';
import { Spinner } from '../../../components/spinner';

interface StandardMessageFormState {
    messageCategories: MessageCategory[];

    typeSelectorErrors?: string[];
    receiverSelectorErrors?: string[];
    senderPhoneNumberErrors?: string[];
    messageBodyErrors?: string[];
}

var withRouterWrapper = (withRouter as Function);

@withRouterWrapper
export class StandardMessageForm extends React.Component<any, StandardMessageFormState>{
    messageTypeSelector: MessageTypeSelector;
    receiverSelector: ReceiverSelector;
    senderPhoneNumberInput: SenderPhoneNumberInput;
    messageComposer: MessageComposer;
    created = false;
    fieldSet: HTMLDivElement;

    static contextTypes: any = {
        router: React.PropTypes.object.isRequired
    };

    constructor(props, context) {
        super(props, context);
        this.state = {
            messageCategories: [{
                CategoryId: -1,
                CategoryName: "Typ av meddelande",
                Types: []
            }],
            typeSelectorErrors: [],
            messageBodyErrors: [],
            receiverSelectorErrors: [],
            senderPhoneNumberErrors: [],
        };
    }

    routerWillLeave = () => {
        if (this.created || !this.userDidInteract()) {
            return true;
        }
        return confirm("Vill du verkligen lämna den här sidan utan att ha sparat ändringar?");
    }

    onbeforeunload = (e: BeforeUnloadEvent) => {
        if (!this.created && this.userDidInteract()) {
            e.returnValue = "Vill du verkligen lämna den här sidan utan att ha sparat ändringar?";
            return "Vill du verkligen lämna den här sidan utan att ha sparat ändringar?";
        }
    }

    componentDidMount() {
        this.fieldSet.dataset["disabled"] = "false";
        adminService.getAllCategories(false).then(c => {
            var categories: MessageCategory[] = this.state.messageCategories;
            categories.push(...c);

            this.setState({
                messageCategories: categories,
            });
        }).catch(r => {
            alert("Error connecting to server, please try again");
        });

        this.props.router.setRouteLeaveHook(
            this.props.route,
            this.routerWillLeave
        );

        window.addEventListener("beforeunload", e => this.onbeforeunload(e));
    }

    componentWillUnmount() {
        window.removeEventListener("beforeunload", e => this.onbeforeunload(e));
    }

    componentDidUpdate() {
        window.scrollTo(0, 0);
    }

    hasErrors() {
        return this.state.messageBodyErrors.length > 0
            || this.state.typeSelectorErrors.length > 0
            || (this.state.receiverSelectorErrors.length > 0 && this.receiverSelector.isOpen())
            || this.state.senderPhoneNumberErrors.length > 0
    }

    onFormSubmit(e: React.FormEvent<HTMLFormElement>) {
        e.preventDefault();

        //gather field data
        var selectedType = this.messageTypeSelector.getSelectedType();
        var emails = this.receiverSelector.getEmails();
        var phoneNumbers = this.receiverSelector.getPhoneNumbers();
        var smsSender = this.senderPhoneNumberInput.getValue();
        var title = this.messageComposer.getTitle();
        var body = this.messageComposer.getBody();
        var smsText = this.messageComposer.getSmsText();
        var moreReceivers = this.receiverSelector.isOpen();

        this.fieldSet.dataset["disabled"] = "true"

        trySaveStandardMessage(selectedType, emails, phoneNumbers, smsSender, title, body, smsText, moreReceivers).then(id => {
            //clear to reload?
            adminService.clearMessages();
            this.created = true;
            hashHistory.push(`/message-detail/${id}`);
        }).catch(r => {
            this.fieldSet.dataset["disabled"] = "false";
            if (typeof r == "string") {
                alert(r);
                return;
            }

            var errors = r as StandardMessageValidationResult;
            this.setState({
                messageCategories: this.state.messageCategories,
                typeSelectorErrors: errors.typeErrors,
                messageBodyErrors: errors.bodyErrors,
                receiverSelectorErrors: errors.receiverErrors,
                senderPhoneNumberErrors: errors.smsSenderErrors
            });
        });
    }

    userDidInteract() {
        function hasChanged(...input: string[]) {
            for (var i of input) {
                if (!(!i || !i.trim())) return true;
            }

            return false;
        }

        var selectedType = !!this.messageTypeSelector.getSelectedType();
        var emails = hasChanged(...this.receiverSelector.getEmails());
        var phoneNumbers = hasChanged(...this.receiverSelector.getPhoneNumbers());
        var smsSender = hasChanged(this.senderPhoneNumberInput.getValue());
        var title = hasChanged(this.messageComposer.getTitle());
        var body = hasChanged(this.messageComposer.getBody());
        var smsText = hasChanged(this.messageComposer.getSmsText());

        return selectedType || emails || phoneNumbers || smsSender || title || body || smsText;
    }

    render() {
        var generalErrors = this.getGeneralErrors();

        return (
            <form className="lm__administrera-form create-message-form" onSubmit={e => this.onFormSubmit(e)}>
                {generalErrors}
                <div className="fieldset" ref={r => this.fieldSet = r || this.fieldSet} style={{ border: "none" }}>
                    <div className="lm__administrera-form__input">
                        <MessageTypeSelector ref={r => this.messageTypeSelector = r || this.messageTypeSelector}
                            categories={this.state.messageCategories}
                            errors={this.state.typeSelectorErrors} />
                    </div>

                    <div className="lm__administrera-form__input">
                        <ReceiverSelector accordion={true} errors={this.state.receiverSelectorErrors}
                            ref={r => this.receiverSelector = r || this.receiverSelector} />
                    </div>

                    <div className="lm__administrera-form__input">
                        <SenderPhoneNumberInput ref={r => this.senderPhoneNumberInput = r || this.senderPhoneNumberInput}
                            errors={this.state.senderPhoneNumberErrors} />
                    </div>
                    <div className="lm__administrera-form__input">
                        <MessageComposer ref={r => this.messageComposer = r || this.messageComposer}
                            errors={this.state.messageBodyErrors} />
                    </div>

                    <div className="lm__administrera-form__buttons">
                        <input style={{ marginRight: "4px" }} type="reset" className="lm__administrera-form__btn" defaultValue="Avbryt"
                            onClick={e => hashHistory.push('/')} />
                        <input type="submit" className="lm__administrera-form__btn" defaultValue="Skicka meddelande" />
                        <div className="submit-spinner">
                            <Spinner color="#3498db" period={0.8} thickness={2} size={20} />
                        </div>
                    </div>
                </div>
            </form >
        );
    }

    private getGeneralErrors() {
        if (!this.hasErrors()) {
            return [];
        }

        let keyCount = 0;

        let generalErrors: JSX.Element[] = [
            <strong className="error-item" key={keyCount++}>Du måste se över</strong>
        ];

        if (this.state.typeSelectorErrors.length > 0) {
            generalErrors.push(
                <p className="error-item" style={{ margin: "0px auto" }} key={keyCount++}> - Välj meddelandeområde</p>
            );
        }

        if (this.state.receiverSelectorErrors.length > 0) {
            generalErrors.push(
                <p className="error-item" style={{ margin: "0px auto" }} key={keyCount++}> - Lägg till mottagare</p>
            );
        }

        if (this.state.senderPhoneNumberErrors.length > 0) {
            generalErrors.push(
                <p className="error-item" style={{ margin: "0px auto" }} key={keyCount++}> - Avsändare SMS-meddelande</p>
            );
        }

        if (this.state.messageBodyErrors.length > 0) {
            generalErrors.push(
                <p className="error-item" style={{ margin: "0px auto" }} key={keyCount++}> - Skapa meddelande</p>
            );
        }

        return generalErrors;
    }
}
