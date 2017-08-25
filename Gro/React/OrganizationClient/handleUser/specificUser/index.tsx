import * as React from 'react';
import { View } from '../../shared/view';
import { RoleProfile, CustomRoleSetting, Role } from '../../shared/models';
import { User } from '../models';
import { store } from '../store';
import { hashHistory } from 'react-router';
import { ProfileSelector } from '../../shared/_profileSelector';
import { RoleViewer } from '../../shared/_roleViewer';
import { CustomRoles } from '../../shared/_customRoles';
import * as action from '../action';
import * as Immutable from 'immutable';

interface UserInfoBoxProps {
    user: User;
}

export class UserInfoBox extends React.Component<UserInfoBoxProps, any> {
    deleteForm: HTMLFormElement;

    shouldComponentUpdate(newProps: UserInfoBoxProps) {
        return this.props.user != newProps.user;
    }

    onDeleteClick(e: React.MouseEvent<HTMLAnchorElement>) {
        var r = confirm("Är du säker på att du vill ta bort användaren från ditt KundID?");
        if (!r) { return; }

        this.deleteForm.submit();
    }

    render() {
        if (!this.props.user) { return null; }
        var selectedProfileId = !store.selectedUser ? store.profiles.first().Id :
            action.matchUserRoleProfile(store.selectedUser);
        var profile = store.profiles.get(selectedProfileId);
        var profileName = !!profile ? profile.Description : "Anpassad profil";
        if (selectedProfileId == "CustomerOwner") {
            profileName = "Ägare";
        }

        return (
            <form action="." method="post" className="auhtor-inform-form__input lm__gray-box wider-padding" ref={r => this.deleteForm = r}>
                <img src={this.props.user.profilePicUrl} alt="User Avatar" className="lm__gray-box__avatar" />
                <div className="lm__gray-box__inform">
                    <p className="lm__bold-title">{this.props.user.name}</p>
                    <p>{profileName + " vid " + window["currentOrganization"]}</p>

                    <table>
                        <tbody>
                            <tr>
                                <td>Telefon:</td>
                                <td>{!this.props.user.phone ? "" : this.props.user.phone}</td>
                            </tr>
                            <tr>
                                <td>Mobil:</td>
                                <td>{!this.props.user.mobile ? "" : this.props.user.mobile}</td>
                            </tr>
                            <tr>
                                <td>E-post:</td>
                                <td>{this.props.user.email}</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <input type="hidden" hidden name="action" value="removeUser" />
                <input type="hidden" hidden name="userName" value={this.props.user.userName} />
                <a className="lm__form-btn"
                    onClick={e => this.onDeleteClick(e)}>
                    <i className="fa fa-trash" aria-hidden="true"></i>
                    Ta bort användare
                </a>
            </form>
        );
    }
}

interface SpecificUserViewState {
    selectedProfileId?: string;
    customConfig?: Immutable.Map<string, Immutable.List<CustomRoleSetting>>;
    customConfigSelectedMap?: Immutable.Map<string, boolean>;
}

export class SpecificUserView extends View<any, SpecificUserViewState>{
    private setRolesAndProfiles(): SpecificUserViewState {
        let customConfig = Immutable.Map<string, Immutable.List<CustomRoleSetting>>();
        let customConfigSelectedMap = Immutable.Map<string, boolean>();
        var selectedProfileId = !store.selectedUser ? store.profiles.first().Id :
            action.matchUserRoleProfile(store.selectedUser);

        if (!store.profiles.get(selectedProfileId)) {
            selectedProfileId = store.profiles.first().Id;
        }

        var adminRoles = store.profiles.get("Admin");
        for (var adminRole of adminRoles.ProfileRoles) {
            let roleGroupChecked = false;
            let fullRole = store.roles.get(adminRole.RoleName);
            let fullRoleSelected = !store.selectedUser ? false : store.selectedUser.roles.filter(ur => ur.RoleName == fullRole.RoleName).length > 0
            let customFullRole: CustomRoleSetting = {
                RoleId: fullRole.RoleId,
                RoleName: fullRole.RoleName,
                selected: fullRoleSelected
            };
            roleGroupChecked = roleGroupChecked || fullRoleSelected;
            let groupRoleList = Immutable.List<CustomRoleSetting>([customFullRole]);

            if (adminRole.RoleName.endsWith("_w")) {
                let readOnlyRoleName = adminRole.RoleName.substr(0, adminRole.RoleName.indexOf("_w"));
                let readOnlyRole = store.roles.get(readOnlyRoleName);
                let readOnlyRoleSelected = !store.selectedUser ? false : store.selectedUser.roles.filter(ur => ur.RoleName == readOnlyRole.RoleName).length > 0
                let customReadOnlyRole: CustomRoleSetting = {
                    RoleId: readOnlyRole.RoleId,
                    RoleName: readOnlyRole.RoleName,
                    selected: !fullRoleSelected && readOnlyRoleSelected
                };
                if (!!readOnlyRole) {
                    groupRoleList = groupRoleList.push(customReadOnlyRole);
                }

                roleGroupChecked = roleGroupChecked || readOnlyRoleSelected;
            }

            customConfig = customConfig.set(adminRole.ProfileHeadline, groupRoleList);
            customConfigSelectedMap = customConfigSelectedMap.set(adminRole.ProfileHeadline, roleGroupChecked);
        }

        return {
            customConfig: customConfig,
            customConfigSelectedMap: customConfigSelectedMap,
            selectedProfileId: selectedProfileId
        };
    }

    constructor() {
        super();
        this.state = this.setRolesAndProfiles();
    }

    onRadioSelectionChange(profileId: string) {
        this.setState({
            selectedProfileId: profileId
        });
    }

    getProfileDisplay() {
        let profiles = store.profiles.map(pf => (
            <div className="lm__radio type-2 width-40" key={pf.Id}>
                <input type="radio" name="user-type" id={pf.Id} checked={this.state.selectedProfileId == pf.Id}
                    onChange={e => this.onRadioSelectionChange(pf.Id)} />
                <p>{pf.Description}</p>
            </div>
        )).toArray();

        profiles.push(
            <div className="lm__radio type-2 width-40" key="Custom">
                <input type="radio" name="user-type" id="Custom" checked={this.state.selectedProfileId == "Custom"}
                    onChange={e => this.onRadioSelectionChange("Custom")} />
                <p>Anpassad profil</p>
            </div>
        );

        return profiles;
    }

    componentDidMount() {
        if (!store.selectedUser) {
            hashHistory.push('/');
        }
    }

    onSettingGroupCheckedChange(headline: string, newValue: boolean) {
        console.log("onSettingGroupCheckedChange " + headline + " " + newValue);
        let checkedMap = this.state.customConfigSelectedMap;
        checkedMap = checkedMap.set(headline, newValue);
        let roleConfig = this.state.customConfig;

        if (newValue == true) {
            let roleList = roleConfig.get(headline);
            let firstRole = roleList.first();
            roleList = roleList.set(0, {
                RoleId: firstRole.RoleId,
                RoleName: firstRole.RoleName,
                selected: true
            });
            roleConfig = roleConfig.set(headline, roleList);
        } else {
            let roleList = roleConfig.get(headline);
            for (var i = 0, count = roleList.count(); i < count; i++) {
                let role = roleList.get(i);
                roleList = roleList.set(i, {
                    RoleId: role.RoleId,
                    RoleName: role.RoleName,
                    selected: false
                });
            }
            roleConfig = roleConfig.set(headline, roleList);
        }

        this.setState({
            customConfigSelectedMap: checkedMap,
            customConfig: roleConfig
        });
    }

    onRoleSettingChange(headline: string, newRole: string) {
        console.log("onSettingGroupCheckedChange " + headline + " " + newRole);

        let roleConfig = this.state.customConfig;
        let roleList = roleConfig.get(headline);
        let length = roleList.count();
        for (var i = 0; i < length; i++) {
            if (roleList.get(i).RoleName != newRole) {
                roleList = roleList.set(i, {
                    RoleId: roleList.get(i).RoleId,
                    RoleName: roleList.get(i).RoleName,
                    selected: false
                });
            } else {
                roleList = roleList.set(i, {
                    RoleId: roleList.get(i).RoleId,
                    RoleName: roleList.get(i).RoleName,
                    selected: true
                });
            }
        }
        roleConfig = roleConfig.set(headline, roleList);
        this.setState({
            customConfig: roleConfig
        });
    }

    calculateSelectedRoles(): Role[] {
        let roles: Role[] = [];

        if (this.state.selectedProfileId == "Custom") {
            this.state.customConfig
                .filter((c, h) => this.state.customConfigSelectedMap.get(h) == true)
                .map(group => {
                    group.map(roleSetting => {
                        if (roleSetting.selected) { roles.push(roleSetting); }
                    });
                });
        } else {
            let profile = store.profiles.get(this.state.selectedProfileId);
            if (!profile) {
                profile = store.profiles.first();
            }
            profile.ProfileRoles.map(pr => roles.push({
                RoleId: pr.RoleId, RoleName: pr.RoleName
            }));
        }
        return roles;
    }

    render() {
        let user = store.selectedUser;
        let rolesArea: JSX.Element = null;
        if (this.state.selectedProfileId == "Custom") {
            rolesArea = (
                <CustomRoles customConfig={this.state.customConfig} customConfigSelectedMap={this.state.customConfigSelectedMap}
                    onGroupCheckedChange={(hl, nv) => this.onSettingGroupCheckedChange(hl, nv)}
                    onRoleSettingChange={(hl, nv) => this.onRoleSettingChange(hl, nv)} />
            );
        } else {
            let selectedProfile = store.profiles.get(this.state.selectedProfileId);
            rolesArea = (
                <RoleViewer profile={selectedProfile} />
            );
        }

        let selectedRoles = this.calculateSelectedRoles();

        return (
            <div className="lm__add-user-form author-inform-form">
                <UserInfoBox user={user} />
                <div className="author-inform-form__input">
                    <h2 className="heading-title-2">Välj användarens profil och behörigheter i LM2</h2>

                    <div className="author-inform-form__column author-inform-form__column-full">
                        <div className="choose-user-type">
                            {this.getProfileDisplay()}
                        </div>
                    </div>
                </div>
                <div className="author-inform-form__input">
                    <h2 className="heading-title-2">Den anställdes behörigheter</h2>
                    {rolesArea}
                </div>
                <div className="author-inform-form__input">
                    <div className="lm__gray-box">
                        <h3 className="heading-title-3">Bekräftelse och instruktioner</h3>
                        <p>En bekräftelse och instruktioner för inloggning kommer skickas till den nya användarens E-post.</p>
                    </div>
                </div>

                <form action="." method="post" className="author-inform-form__input align-right-text block-button-on-mobile">
                    <input type="button" className="lm__form-btn left-button" value="Bakåt"
                        onClick={e => hashHistory.push("/")} />
                    <input type="hidden" name="roles" hidden value={selectedRoles.map(r => r.RoleId).join(',')} />
                    <input type="hidden" name="action" hidden value="updateRoles" />
                    <input type="hidden" name="userName" hidden value={!store.selectedUser ? "" : store.selectedUser.userName} />
                    <input type="reset" className="lm__form-btn left-button" value="Avbryt" />
                    <input type="submit" className="lm__form-btn reverse-state" value="Uppdatera" />
                </form>
            </div>
        );
    }
}
