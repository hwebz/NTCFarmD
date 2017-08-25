import * as React from 'react';
import { hashHistory, Link } from 'react-router';
import { View } from '../../shared/view';
import { addUserStore } from '../store';
import { UserInfo, RoleProfile, Role } from '../../shared/models';
import * as Immutable from 'immutable';
import { ProfileSelector } from '../../shared/_profileSelector';
import { RoleViewer } from '../../shared/_roleViewer';
import { CustomRoles } from '../../shared/_customRoles';

interface Step2ViewState {
    profileMap?: Immutable.Map<string, RoleProfile>;
    selectedProfile?: string;
}

export class Step2View extends View<any, Step2ViewState>{
    constructor(props) {
        super(props);
        let profiles = addUserStore.getAllRoleProfiles();
        let selectedProfile = addUserStore.getSelectedProfileId();
        this.state = {
            profileMap: profiles,
            selectedProfile: selectedProfile
        };
    }

    onRoleProfileChange = () => {
        let selectedProfile = addUserStore.getSelectedProfileId();
        this.setState({
            selectedProfile: selectedProfile
        });
    };

    componentDidMount() {
        super.componentDidMount();
        let step1Done = addUserStore.isStep1Done()
        if (!step1Done) {
            hashHistory.push('/step1');
            return;
        }
        addUserStore.addListener("roleProfile", this.onRoleProfileChange);
        addUserStore.setCurrentStep(2);
    }

    componentWillUnmount() {
        addUserStore.removeListener("roleProfile", this.onRoleProfileChange);
    }

    onProfileSelect(profileId: string) {
        addUserStore.setSelectedProfileId(profileId);
    }

    onSettingGroupCheckedChange(headline: string, newValue: boolean) {
        console.log("onSettingGroupCheckedChange " + headline + " " + newValue);
        addUserStore.customRoleGroupChanaged(headline, newValue);
    }

    onRoleSettingChange(headline: string, newRole: string) {
        console.log("onSettingGroupCheckedChange " + headline + " " + newRole);
        addUserStore.customRoleChanged(headline, newRole);
    }

    onSubmit(e: React.FormEvent<HTMLFormElement>) {
        e.preventDefault();
        addUserStore.step2Done = true;
        hashHistory.push('/step3');
    }

    render() {
        let rolesArea: JSX.Element = null;
        if (this.state.selectedProfile == "Custom") {
            let customConfig = addUserStore.getCustomConfig();
            let customConfigSelectedMap = addUserStore.getcustomConfigSelectedMap();
            rolesArea = (
                <CustomRoles customConfig={customConfig} customConfigSelectedMap={customConfigSelectedMap}
                    onGroupCheckedChange={(hl, nv) => this.onSettingGroupCheckedChange(hl, nv)}
                    onRoleSettingChange={(hl, nv) => this.onRoleSettingChange(hl, nv)} />
            );
        } else {
            let selectedProfile = addUserStore.getRoleProfile(this.state.selectedProfile);
            rolesArea = (
                <RoleViewer profile={selectedProfile} />
            );
        }

        return (
            <form className="lm__add-user-form author-inform-form" onSubmit={e => this.onSubmit(e)}>
                <div className="author-inform-form__input">
                    <h2 className="heading-title-2">Välj användarens profil och behörigheter i LM2</h2>

                    <ProfileSelector profileMap={this.state.profileMap} selectedProfile={this.state.selectedProfile}
                        onProfileSelect={pId => this.onProfileSelect(pId)} />
                </div>
                <div className="author-inform-form__input">
                    <h2 className="heading-title-2">Den anställdes behörigheter</h2>
                    {rolesArea}
                </div>

                <div className="author-inform-form__input align-right-text block-button-on-mobile">
                    <Link to="/step1" className="lm__form-btn left-button">Bakåt</Link>
                    <a className="lm__form-btn" href={document.referrer}>Avbryt</a>
                    <input type="submit" className="lm__form-btn reverse-state" value="Nästa" />
                </div>
            </form>
        );
    }
}
