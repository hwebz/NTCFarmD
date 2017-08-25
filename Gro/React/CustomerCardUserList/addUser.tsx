import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { AddUserInfo } from './models';
import { CustomRoles } from "./customRoles";
import { Combobox, ComboBoxItem } from './components';
import * as service from './service';

function validateEmail(email: string) {
    if (!email || email.trim().length == 0) {
        return false;
    }

    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

function validateTelePhone(phone: string): string {
    if (!phone || phone.trim().length == 0) { return ""; }
    return window["validation"].phoneSE(phone) ? "" : window["validationMessage"].phoneSE.valid;
}

function validateMobilePhone(mobile: string): string {
    if (!mobile || mobile.trim().length == 0) { return window["validationMessage"].mobileSE.required; }
    return window["validation"].mobileSE(mobile) ? "" : window["validationMessage"].mobileSE.valid;
}

var profileItems = service.getProfiles().map(p => {
    var cbi: ComboBoxItem = {
        name: p.Description,
        value: p.Id
    };
    return cbi;
});

profileItems.push({
    name: "Anpassad profil",
    value: "Custom"
});

var ExistingUserBox = (props: { user: AddUserInfo }) => {
    return !props.user ? null : (
        <div className="existing-user" >
            <h3 className="heading-title-3" > Användaren finns redan i LM2</h3>
            <p> <strong>Namn:</strong> {props.user.firstName} {props.user.lastName}</p>
            <p> <strong>Telefon:</strong> {props.user.telePhone} </p>
            <p> <strong>Mobil:</strong> {props.user.mobile}</p>
            <p> <strong>E-post:</strong> {props.user.email}</p>
        </div>
    );
};

var addUserMounted = false;

interface UserInfoFormpProps {
    defaultEmail: string;
}

interface UserInfoFormState {
    firstNameError: string;
    lastNameError: string;
    phoneError: string;
    mobileError: string;
}

class UserInfoForm extends React.Component<UserInfoFormpProps, UserInfoFormState>{
    firstNameInput: HTMLInputElement;
    lastNameInput: HTMLInputElement;
    telephoneInput: HTMLInputElement;
    mobileInput: HTMLInputElement;
    emailInput: HTMLInputElement;
    telephoneSpan: HTMLSpanElement;
    mobileSpan: HTMLSpanElement;
    firstNameSpan: HTMLSpanElement;
    lastNameSpan: HTMLSpanElement;

    constructor(props: UserInfoFormpProps) {
        super(props);
        this.state = {
            firstNameError: null,
            lastNameError: null,
            phoneError: null,
            mobileError: null,
        };
    }

    public validateGetUserInfo(): AddUserInfo {
        let firstName = this.firstNameInput.value;
        let lastName = this.lastNameInput.value;
        let telephone = this.telephoneInput.value;
        let mobile = this.mobileInput.value;
        let email = this.emailInput.value;

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
            return null;
        }

        return {
            email: email,
            firstName: firstName,
            lastName: lastName,
            mobile: mobile,
            telePhone: telephone,
            userId: null,
            userName: email
        };
    }

    render() {
        return (
            <div className="lm__add-user-form author-inform-form">
                <div className="author-inform-form__input">
                    <p>Alla stjärnmärkta fällt är obligatoriska</p>
                    <div className="author-inform-form__column author-inform-form__column-large">
                        <label htmlFor="firstName">Förnamn*</label>
                        <input type="text" name="firstName" className="fornamn" placeholder="Användarens förnamn"
                            ref={r => this.firstNameInput = r} />
                        <span className="error-item" ref={r => this.firstNameSpan = r}>{this.state.firstNameError}</span>
                    </div>

                    <div className="author-inform-form__column author-inform-form__column-large">
                        <label htmlFor="lastName">Efternamn*</label>
                        <input type="text" name="lastName" className="efternamn" placeholder="Användarens efternamn"
                            ref={r => this.lastNameInput = r} />
                        <span className="error-item" ref={r => this.lastNameSpan = r}>{this.state.lastNameError}</span>
                    </div>

                    <div className="author-inform-form__column author-inform-form__column-large">
                        <label htmlFor="telephone">Telefon</label>
                        <input type="text" name="telephone" className="telefon" placeholder="+46 12 34 56"
                            ref={r => this.telephoneInput = r} />
                        <span className="error-item" ref={r => this.telephoneSpan = r}>{this.state.phoneError}</span>
                    </div>

                    <div className="author-inform-form__column author-inform-form__column-large">
                        <label htmlFor="mobile">Mobil*</label>
                        <input type="text" name="mobile" className="mobil" placeholder="+46 123 456 789"
                            ref={r => this.mobileInput = r} />
                        <span className="error-item" ref={r => this.mobileSpan = r}>{this.state.mobileError}</span>
                    </div>

                    <div className="author-inform-form__column author-inform-form__column-large">
                        <label htmlFor="email">E-post</label>
                        <input required type="email" name="e-post" className="e-post" style={{ backgroundColor: "lightgray" }}
                            defaultValue={this.props.defaultEmail} ref={r => this.emailInput = r} disabled />
                    </div>
                </div>
            </div>
        );
    }
}

interface AddUserComponentProps {
    onClose: () => void;

    //events
    onAddExistingUser: (userName: string, roleIds: number[]) => void;
    onAddNewUser: (info: AddUserInfo, roleIds: number[]) => void;
}

interface AddUserComponentState {
    loading: boolean;
    existingUser: AddUserInfo;
    selectedProfile: ComboBoxItem;
    customRoleIds: number[];
    state: "initial" | "existingUser" | "newUser";

    invalidEmailMessage?: string;
}

export class AddUserComponent extends React.Component<AddUserComponentProps, AddUserComponentState>{
    emailInput: HTMLInputElement;
    customRoles: CustomRoles;
    newUserForm: UserInfoForm;

    constructor(props: AddUserComponentProps) {
        super(props);
        this.state = {
            existingUser: null,
            selectedProfile: profileItems[0],
            customRoleIds: [],
            loading: false,
            invalidEmailMessage: null,
            state: "initial"
        };
    }

    onFindUser(email: string) {
        if (!validateEmail(email)) {
            this.setState({
                invalidEmailMessage: "Invalid email"
            });
            return;
        }

        this.setState({
            loading: true,
            invalidEmailMessage: null
        });

        service.findExistingUser(email).then(user => {
            if (!addUserMounted) { return; }

            if (!user) {
                this.setState({
                    loading: false,
                    existingUser: null,
                    state: "newUser"
                });
                return;
            }

            this.setState({
                existingUser: user,
                loading: false,
                state: "existingUser"
            });
        }).catch(err => {
            this.setState({
                loading: false
            });
            if (err == "conflict") {
                alert("Användaren finns redan för den här organisationen");
            } else {
                alert(err);
            }
        });
    }

    componentDidMount() {
        addUserMounted = true;
    }

    componentWillUnmount() {
        addUserMounted = false;
    }

    onProfileChange(profileId: string) {
        var selectedProfile = profileItems.filter(p => p.value == profileId)[0];
        this.setState({
            selectedProfile: selectedProfile
        });
    }

    onSubmit() {
        if (this.state.state == "initial") { return; }

        var roleIds: number[] = [];
        if (this.state.selectedProfile.value == "Custom") {
            roleIds = this.customRoles.getSelectedRoles();
        } else {
            roleIds = service.getRolesOfProfile(this.state.selectedProfile.value).map(r => r.roleid);
        }
        if (this.state.state == "existingUser") {
            this.props.onAddExistingUser(this.state.existingUser.email, roleIds);
            return;
        }

        var user = this.newUserForm.validateGetUserInfo();
        if (!user) { return; }
        this.props.onAddNewUser(user, roleIds);
    }

    getUserInfoComponent() {
        if (this.state.state == "initial") { return null; }

        if (this.state.state == "existingUser") {
            return <ExistingUserBox user={this.state.existingUser} />;
        }

        return <UserInfoForm defaultEmail={this.emailInput.value}
            ref={r => this.newUserForm = r} />
    }

    render() {
        var profileComboBox = this.state.state == "initial" ? null : (
            < div className="gray-background small-distance-bottom" >
                <div className="author-inform-form__input" >
                    <div className="author-inform-form__column author-inform-form__column-large" >
                        <label htmlFor="user-type" > Välj användarens profil och behörigheter</label>

                        <Combobox items={profileItems} selectedItem={this.state.selectedProfile}
                            onChange={i => this.onProfileChange(i)} />
                    </div>
                </div>
            </div>
        );

        var customRoleBox = this.state.state == "initial" || !this.state.selectedProfile || (this.state.selectedProfile.value != "Custom") ? null : (
            <CustomRoles roleIds={this.state.customRoleIds} ref={r => this.customRoles = r} />
        );

        return (
            <tr className="lm__listing-block__add-item" style={{
                opacity: this.state.loading ? 0.6 : 1,
                pointerEvents: this.state.loading ? "none" : "auto"
            }}>
                <td colSpan={3} >
                    <div className="add-item-form" >
                        <a onClick={e => this.props.onClose()} className="close-btn" > <i className="fa fa-times" > </i></a>
                        <h2 className="heading-title-2" > Lägg till användare till kundID</h2>
                        <form className="author-inform-form" onSubmit={e => e.preventDefault()}>
                            <div className="gray-background small-distance-bottom" >
                                <p className="no-margin" > Fyll i användarens E- postadress för att kontrollera om</p>
                                <p> personen redan är registrerad på LM2</p>

                                <div className="author-inform-form__input" >
                                    <div className="author-inform-form__column author-inform-form__column-large" >
                                        <label htmlFor="epost"> E-post </label>
                                        <input type="text" name="epost" className="epost" id="epost" ref={r => this.emailInput = r} />
                                    </div>
                                    <div className="author-inform-form__column author-inform-form__column-custom-1" >
                                        <label htmlFor="check-btn" > Kontrollera </label>
                                        <div className="lm__form-btn no-margin" onClick={e => this.onFindUser(this.emailInput.value)}> Kontrollera </div>
                                    </div>
                                    <p style={{ color: "red" }}>{this.state.invalidEmailMessage}</p>
                                </div>

                                {this.getUserInfoComponent()}
                            </div>

                            {profileComboBox}

                            {customRoleBox}

                            <p> En bekräftelse kommer skickas till den nya användarens e- post.</p>
                            <div className="lm__form-btn reverse-state" onClick={e => this.onSubmit()}> Lägg till</div>
                        </form>
                    </div>
                </td>
            </tr>
        );
    }
}
