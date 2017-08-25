import * as React from 'react';
import { MessageCategorySelector, ReceiverSelector, SenderPhoneNumberInput, MessageComposer } from '../formControls';
import { hashHistory, withRouter } from 'react-router';
import { MessageType, MessageCategory, FreeMessageValidationResult } from '../administerModels';
import * as adminService from '../services';
import { trySaveFreeMessage } from './actions';
import { Spinner } from '../../../components/spinner';

interface FreeMessageFormState {
    messageCategories: MessageCategory[];

    categorySelectorErrors?: string[];
    receiverSelectorErrors?: string[];
    senderPhoneNumberErrors?: string[];
    messageBodyErrors?: string[];
}

var withRouterWrapper = (withRouter as Function);

@withRouterWrapper
export class FreeMessageForm extends React.Component<any, FreeMessageFormState>{
    receiverSelector: ReceiverSelector;
    senderPhoneNumberInput: SenderPhoneNumberInput;
    messageCategorySelector: MessageCategorySelector;
    messageComposer: MessageComposer;
    created = false;
    fieldSet: HTMLDivElement;

    static contextTypes: any = {
        router: React.PropTypes.object.isRequired
    };

    constructor() {
        super();
        this.state = {
            messageCategories: [{
                CategoryId: -1,
                CategoryName: "Kategori av meddelande",
                Types: []
            }],
            categorySelectorErrors: [],
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

        adminService.getFreeMessageCategories().then(c => {
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

    onFormSubmit(e: React.MouseEvent<HTMLInputElement>) {
        e.preventDefault();

        //gather field data
        var selectedCategory = this.messageCategorySelector.getSelectedCategory();
        var emails = this.receiverSelector.getEmails();
        var phoneNumbers = this.receiverSelector.getPhoneNumbers();
        var smsSender = this.senderPhoneNumberInput.getValue();
        var title = this.messageComposer.getTitle();
        var body = this.messageComposer.getBody();
        var smsText = this.messageComposer.getSmsText();

        this.fieldSet.dataset["disabled"] = "true"

        trySaveFreeMessage(selectedCategory, emails, phoneNumbers, smsSender, title, body, smsText).then(id => {
            adminService.clearMessages();
            this.created = true;
            hashHistory.push(`/message-detail/${id}`);
        }).catch(r => {
            this.fieldSet.dataset["disabled"] = "false";
            if (typeof r == "string") {
                alert(r);
                return;
            }

            var errors = r as FreeMessageValidationResult;
            this.setState({
                messageCategories: this.state.messageCategories,
                categorySelectorErrors: errors.categoryErrors,
                messageBodyErrors: errors.bodyErrors,
                receiverSelectorErrors: errors.receiverErrors,
                senderPhoneNumberErrors: errors.smsSenderErrors,
            });
        });
    }

    componentDidUpdate() {
        window.scrollTo(0, 0);
    }

    hasErrors() {
        return this.state.messageBodyErrors.length > 0
            || this.state.categorySelectorErrors.length > 0
            || this.state.receiverSelectorErrors.length > 0
            || this.state.senderPhoneNumberErrors.length > 0
    }

    userDidInteract() {
        function hasChanged(...input: string[]) {
            for (var i of input) {
                if (!(!i || !i.trim())) return true;
            }

            return false;
        }

        var selectedCategory = !!this.messageCategorySelector.getSelectedCategory();
        var emails = hasChanged(...this.receiverSelector.getEmails());
        var phoneNumbers = hasChanged(...this.receiverSelector.getPhoneNumbers());
        var smsSender = hasChanged(this.senderPhoneNumberInput.getValue());
        var title = hasChanged(this.messageComposer.getTitle());
        var body = hasChanged(this.messageComposer.getBody());
        var smsText = hasChanged(this.messageComposer.getSmsText());

        return selectedCategory || emails || phoneNumbers || smsSender || title || body || smsText;
    }

    render() {
        var generalErrors = this.getGeneralErrors();

        return (
            <form className="lm__administrera-form create-message-form" >
                {generalErrors}
                <div className="fieldset" ref={r => this.fieldSet = r || this.fieldSet}>
                    <div className="lm__administrera-form__input">
                        <MessageCategorySelector ref={r => this.messageCategorySelector = r || this.messageCategorySelector}
                            errors={this.state.categorySelectorErrors}
                            categories={this.state.messageCategories} />
                    </div>

                    <div className="lm__administrera-form__input">
                        <ReceiverSelector ref={r => this.receiverSelector = r || this.receiverSelector}
                            errors={this.state.receiverSelectorErrors}
                            accordion={false} />
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
                        <input style={{ marginRight: "4px" }} type="button" className="lm__administrera-form__btn" defaultValue="Avbryt"
                            onClick={e => hashHistory.push('/')} />
                        <input type="button" className="lm__administrera-form__btn" defaultValue="Skicka meddelande"
                            onClick={e => this.onFormSubmit(e)} />
                        <div className="submit-spinner">
                            <Spinner color="#3498db" period={0.8} thickness={2} size={20} />
                        </div>
                    </div>
                </div>
            </form>
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

        if (this.state.categorySelectorErrors.length > 0) {
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
