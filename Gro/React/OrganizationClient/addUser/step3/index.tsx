import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { hashHistory, Link } from 'react-router';
import { View } from '../../shared/view';
import { addUserStore } from '../store';
import { UserInfo, RoleProfile, Role } from '../../shared/models';

interface Step3ViewState {
    userInfo: UserInfo;
    selectedProfileId: string;
}

export class Step3View extends View<any, Step3ViewState>{
    submitInput: HTMLInputElement;

    constructor(props) {
        super(props);
        this.state = {
            userInfo: addUserStore.getUserInfo(),
            selectedProfileId: addUserStore.getSelectedProfileId()
        };
    }

    componentDidMount() {
        super.componentDidMount();
        if (!addUserStore.isStep1Done()) {
            hashHistory.push('/step1');
            return;
        }

        if (!addUserStore.step2Done) {
            hashHistory.push('/step2');
        }

        addUserStore.setCurrentStep(3);
    }

    getRightsDisplayForCustomProfile() {
        let displays = new Array<JSX.Element>();
        let rightGroups = addUserStore.getCustomConfig();
        let rightsSelected = addUserStore.getcustomConfigSelectedMap();
        let roleIdArrays = new Array<number>();
        rightGroups.map((rg, headline) => {
            if (!rightsSelected.get(headline)) { return; }
            rg.map(role => {
                if (!role.selected) { return; }
                roleIdArrays.push(role.RoleId);
                displays.push(
                    <tr key={headline}>
                        <td>{headline}</td>
                        <td>{role.RoleName.endsWith("_w") ? "Fullständig behörighet" : "Kan se"}</td>
                    </tr>
                );
            });
        });

        displays.push(<input key="Roles" name="Roles" hidden defaultValue={roleIdArrays.join(',')} />);

        return displays;
    }

    getRightsDisplayForProfile() {
        let profile = addUserStore.getRoleProfile(this.state.selectedProfileId);
        let displays = new Array<JSX.Element>();
        let roleIdArrays = new Array<number>();
        displays.push(
            <tr key="description">
                <td>Profil:</td>
                <td>{profile.Description}</td>
            </tr>
        );
        for (let profileRole of profile.ProfileRoles) {
            roleIdArrays.push(profileRole.RoleId);

            displays.push(
                <tr key={profileRole.RoleId}>
                    <td>{profileRole.ProfileHeadline + ":"}</td>
                    <td>{profileRole.RoleRights}</td>
                </tr>
            );
        }
        displays.push(<input key="Roles" name="Roles" hidden defaultValue={roleIdArrays.join(',')} />);
        return displays;
    }

    onFormSubmit(e: React.FormEvent<HTMLFormElement>) {
        this.submitInput.disabled = true;
    }

    render() {
        if (!this.state.selectedProfileId || !this.state.userInfo) {
            return null;
        }

        let rightsDisplay = this.state.selectedProfileId == "Custom" ?
            this.getRightsDisplayForCustomProfile() : this.getRightsDisplayForProfile();

        return (
            <form action="." method="post" className="lm__add-user-form author-inform-form" onSubmit={e => this.onFormSubmit(e)}>
                <input hidden name="FirstName" defaultValue={this.state.userInfo.firstName} />
                <input hidden name="LastName" defaultValue={this.state.userInfo.lastName} />
                <input hidden name="Telephone" defaultValue={this.state.userInfo.telephone} />
                <input hidden name="Mobile" defaultValue={this.state.userInfo.mobile} />
                <input hidden name="Email" defaultValue={this.state.userInfo.email} />

                <div className="author-inform-form__input">
                    <h2 className="heading-title-2">Granska uppgifter</h2>
                    <div className="table-information">
                        <p className="lm__bold-title">Personlig information</p>
                        <table className="lm__information-table">
                            <tbody>
                                <tr>
                                    <td>Förnamn:</td>
                                    <td>{this.state.userInfo.firstName}</td>
                                </tr>
                                <tr>
                                    <td>Efternamn:</td>
                                    <td>{this.state.userInfo.lastName}</td>
                                </tr>
                                <tr>
                                    <td>Telefon:</td>
                                    <td>{this.state.userInfo.telephone}</td>
                                </tr>
                                <tr>
                                    <td>Mobil:</td>
                                    <td>{this.state.userInfo.mobile}</td>
                                </tr>
                                <tr>
                                    <td>E-post:</td>
                                    <td>{this.state.userInfo.email}</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div className="table-information">
                        <p className="lm__bold-title">Profil & behörighet</p>
                        <table className="lm__information-table">
                            <tbody>
                                {rightsDisplay}
                            </tbody>
                        </table>
                    </div>
                </div>
                <div className="author-inform-form__input">
                    <h3 className="heading-title-3">Bekräftelse och instruktioner</h3>
                    <p>En bekräftelse och instruktioner för inloggning kommer skickas till den nya användarens E-post.</p>
                </div>
                <div className="author-inform-form__input align-right-text block-button-on-mobile">
                    <Link to="/step2" className="lm__form-btn left-button">Bakåt</Link>
                    <a className="lm__form-btn" href={document.referrer}>Avbryt</a>
                    <input type="submit" className="lm__form-btn reverse-state" value="Nästa"
                        ref={r => this.submitInput = r} />
                </div>
            </form>
        );
    }
}
