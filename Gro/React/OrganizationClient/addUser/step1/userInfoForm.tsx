import * as React from 'react';
import { UserInfo } from '../../shared/models';
import { Spinner } from '../../../components/spinner';
import { SearchUserStatus } from '../../shared/models';

function validateTelePhone(phone: string): string {
    if (!phone || phone.trim().length == 0) { return ""; }
    return window["validation"].phoneSE(phone) ? "" : window["validationMessage"].phoneSE.valid;
}

function validateMobilePhone(mobile: string): string {
    if (!mobile || mobile.trim().length == 0) { return window["validationMessage"].mobileSE.required; }
    return window["validation"].mobileSE(mobile) ? "" : window["validationMessage"].mobileSE.valid;
}

interface UserInfoFormProps {
    searchStatus: SearchUserStatus;
    userInfo: UserInfo;
    email?: string;

    onSubmit?: (u: UserInfo) => void;
}

interface UserInfoFormState {
    firstNameError: string;
    lastNameError: string;
    phoneError: string;
    mobileError: string;
}

export class UserInfoForm extends React.Component<UserInfoFormProps, UserInfoFormState>{
    closableMessage: HTMLDivElement;

    firstNameInput: HTMLInputElement;
    lastNameInput: HTMLInputElement;
    telephoneInput: HTMLInputElement;
    mobileInput: HTMLInputElement;
    emailInput: HTMLInputElement;
    telephoneSpan: HTMLSpanElement;
    mobileSpan: HTMLSpanElement;
    firstNameSpan: HTMLSpanElement;
    lastNameSpan: HTMLSpanElement;

    constructor() {
        super();
        this.state = {
            firstNameError: "",
            lastNameError: "",
            mobileError: "",
            phoneError: ""
        };
    }

    closeMessage() {
        if (!this.closableMessage) { return; }
        this.closableMessage.style.display = "none";
    }

    getFindUserStatusBox() {
        if (this.props.searchStatus == SearchUserStatus.searched) { return null; }

        if (this.props.searchStatus == SearchUserStatus.userNotFound) {
            //user does not exist
            return (
                <div className="lm__message-closable error" ref={r => this.closableMessage = r}>
                    <p className="message">Det finns ingen användare registrerad på LM<sup>2</sup> för denna E-postadress.<br />Fyll i uppgifterna nedan.</p>
                    <a className="close-btn" onClick={_ => this.closeMessage()}><i className="fa fa-times"></i></a>
                </div>
            );
        }

        let name = `${this.props.userInfo.firstName} ${this.props.userInfo.lastName}`;
        return (
            <div className="lm__message-closable" ref={r => this.closableMessage = r}>
                <p className="message"><span className="lm__bold-title">{name}</span> är registrerad som användare på LM <sup>2</sup> för denna E-postadress</p>
                <a className="close-btn" ><i className="fa fa-times" onClick={_ => this.closeMessage()}></i></a>
            </div>
        );
    }

    getSearchSpinner() {
        return (
            <div className="author-inform-form__input">
                <Spinner className={"search-user-spinner"} color={"#58a618"} period={0.7} size={32} thickness={3} />
            </div>
        );
    }

    componentWillReceiveProps() {
        this.setState({
            firstNameError: null,
            lastNameError: null,
            mobileError: null,
            phoneError: null
        });
    }

    onSubmitClick(e: React.FormEvent<HTMLFormElement>) {
        e.preventDefault();
        let firstName = this.firstNameInput.value;
        let lastName = this.lastNameInput.value;
        let telephone = this.telephoneInput.value;
        let mobile = this.mobileInput.value;
        let email = this.emailInput.value;

        if (this.props.searchStatus !== SearchUserStatus.userFound) {
            let telephoneValidationResult = validateTelePhone(telephone);
            let mobileValidationResult = validateMobilePhone(mobile);
            let firstNameVR = (!!firstName && firstName.trim().length > 0) ? null : window["validationMessage"].firstName.required;
            let lastNameVR = (!!lastName && lastName.trim().length > 0) ? null : window["validationMessage"].lastName.required;

            this.setState({
                firstNameError: firstNameVR,
                lastNameError: lastNameVR,
                mobileError: mobileValidationResult,
                phoneError: telephoneValidationResult
            });

            if (!!telephoneValidationResult || !!mobileValidationResult || !!firstNameVR || !!lastNameVR) {
                return;
            }
        }

        this.props.onSubmit({
            email: email,
            firstName: firstName,
            lastName: lastName,
            mobile: mobile,
            telephone: telephone,
            userId: this.props.userInfo.userId,
            userName: this.props.userInfo.userName || email
        });
    }

    render() {
        if (this.props.searchStatus == SearchUserStatus.notSearched) {
            return (
                <form className="lm__add-user-form author-inform-form" >
                    <div className="author-inform-form__input align-right-text" style={{ padding: "15px" }}>
                        <a className="lm__form-btn" href={document.referrer}>Avbryt</a>
                        <input disabled type="submit" className="lm__form-btn reverse-state" style={{ marginLeft: "6px" }} value="Nästa" />
                    </div>
                </form>
            );
        }

        if (this.props.searchStatus == SearchUserStatus.searching) {
            return this.getSearchSpinner();
        }

        if (this.props.searchStatus == SearchUserStatus.userConflict) {
            return (
                <form className="lm__add-user-form author-inform-form" >
                    <div className="author-inform-form__input"  >
                        <div className="lm__message-closable error" ref={r => this.closableMessage = r} >
                            <p className="message">Användaren finns redan för den här organisationen</p>
                        </div>
                    </div>
                </form>
            );
        }

        let findUserStatusBox = this.getFindUserStatusBox();
        let displayClass = this.props.searchStatus == SearchUserStatus.userFound ? "e-post__detail-result disabled-form" : "e-post__detail-result";
        let email = this.props.email || this.props.userInfo.email;
        return (
            <form className="lm__add-user-form author-inform-form" onSubmit={e => this.onSubmitClick(e)}>
                <div className="author-inform-form__input">
                    <div className={displayClass}>
                        {findUserStatusBox}
                        <div className="author-inform-form__column author-inform-form__column-large">
                            <label htmlFor="firstName">Förnamn{this.props.searchStatus == SearchUserStatus.userFound ? "" : "*"}</label>
                            <input type="text" name="firstName" className="fornamn" placeholder="Användarens förnamn"
                                defaultValue={this.props.userInfo.firstName} ref={r => this.firstNameInput = r} />
                            <span className="error-item" ref={r => this.firstNameSpan = r}>{this.state.firstNameError}</span>
                        </div>

                        <div className="author-inform-form__column author-inform-form__column-large">
                            <label htmlFor="lastName">Efternamn{this.props.searchStatus == SearchUserStatus.userFound ? "" : "*"}</label>
                            <input type="text" name="lastName" className="efternamn" placeholder="Användarens efternamn"
                                defaultValue={this.props.userInfo.lastName} ref={r => this.lastNameInput = r} />
                            <span className="error-item" ref={r => this.lastNameSpan = r}>{this.state.lastNameError}</span>
                        </div>

                        <div className="author-inform-form__column author-inform-form__column-large">
                            <label htmlFor="telephone">Telefon</label>
                            <input type="text" name="telephone" className="telefon" placeholder={this.props.searchStatus == SearchUserStatus.userFound ? "" : "+46 12 34 56"}
                                defaultValue={this.props.userInfo.telephone} ref={r => this.telephoneInput = r} />
                            <span className="error-item" ref={r => this.telephoneSpan = r}>{this.state.phoneError}</span>
                        </div>

                        <div className="author-inform-form__column author-inform-form__column-large">
                            <label htmlFor="mobile">Mobil{this.props.searchStatus == SearchUserStatus.userFound ? "" : "*"}</label>
                            <input type="text" name="mobile" className="mobil" placeholder={this.props.searchStatus == SearchUserStatus.userFound ? "" : "+46 123 456 789"}
                                defaultValue={this.props.userInfo.mobile} ref={r => this.mobileInput = r} />
                            <span className="error-item" ref={r => this.mobileSpan = r}>{this.state.mobileError}</span>
                        </div>

                        <div className="author-inform-form__column author-inform-form__column-large">
                            <label htmlFor="email">E-post{this.props.searchStatus == SearchUserStatus.userFound ? "" : "*"}</label>
                            <input required type="email" name="e-post" className="e-post" placeholder="example@mail.com"
                                defaultValue={email} ref={r => this.emailInput = r} disabled />
                        </div>
                    </div>
                </div>
                <div className="author-inform-form__input align-right-text" style={{ padding: "15px" }}>
                    <a className="lm__form-btn" href={document.referrer}>Avbryt</a>
                    <input type="submit" className="lm__form-btn reverse-state" style={{ marginLeft: "6px" }} value="Nästa" />
                </div>
            </form>
        );
    }
}
