import * as React from 'react';

function validateEmail(email: string) {
    if (!email || email.trim().length == 0) {
        return false;
    }

    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

export interface UserSearchBoxProps {
    commitSearch: (email: string) => void;
    disabled: boolean;
}

export interface UserSearchBoxState {
    errorMessage: string;
}

export class UserSearchBox extends React.Component<UserSearchBoxProps, UserSearchBoxState>{
    emailInput: HTMLInputElement;
    constructor(props) {
        super(props);
        this.state = {
            errorMessage: ""
        };
    }

    onEmailSearch(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();

        if (!validateEmail(this.emailInput.value)) {
            this.setState({
                errorMessage: "Ange en giltig e-postadress"
            });
            return;
        }

        this.setState({
            errorMessage: ""
        });
        this.props.commitSearch(this.emailInput.value);
    }

    render() {
        return (
            <div required className="author-inform-form__column author-inform-form__column-full" >
                <form onSubmit={e => this.onEmailSearch(e)}>
                    <input type="text" id="e-post" name="e-post" className="input3 inline-block-input" placeholder="example@mail.com"
                        ref={r => this.emailInput = r} disabled={this.props.disabled} style={{
                            backgroundColor: "rgba(102, 204, 102, 0.266666)",
                            color: "black"
                        }} />
                    <input type="submit" className="lm__form-btn" value="Kontrollera" disabled={this.props.disabled} />
                    <br />
                    <span className="error-item">{this.state.errorMessage}</span>
                </form>
            </div>
        );
    }
}
