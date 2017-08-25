import * as React from 'react';
import * as Immutable from 'immutable';
import { UserInfo, RoleProfile, Role, ProfileRole, CustomRoleSetting } from './models';
import * as infoPopup from './_roleInfoPopup';

interface RoleGroupProps {
    headline: string;
    roleList: Immutable.List<CustomRoleSetting>;
    selected: boolean;

    //events
    onRoleChange: (newRoleName: string) => void;
    onCheckedChange: (newValue: boolean) => void;
    onInfoClick: (headline: string) => void;
}

export class RoleGroup extends React.Component<RoleGroupProps, any>{
    shouldComponentUpdate(newProps: RoleGroupProps) {
        return this.props.headline != newProps.headline
            || this.props.roleList != newProps.roleList
            || this.props.selected != newProps.selected;
    }

    onInfoClick(hl: string) {
        console.log(hl);
        this.props.onInfoClick(hl);
    }

    render() {
        console.log("render RoleGroup");

        let radioButtons = this.props.roleList.map(role =>
            <div className={"lm__radio type-2"} key={role.RoleId}>
                <input type="radio" name={role.RoleId + "-sub-type"} checked={role.selected}
                    onChange={e => !role.selected && this.props.onRoleChange(role.RoleName)} />
                <p>{role.RoleName.endsWith("_w") ? "Fullständig behörighet" : "Kan se"} </p>
            </div>
        ).toArray();

        return (
            <div className="sub-type">
                <div className={"lm__checkbox lm__tick " + (this.props.headline == "Basfunktioner" ? "disabled" : "")}>
                    <input type="checkbox" checked={this.props.selected}
                        onChange={e => this.props.onCheckedChange(!this.props.selected)} />
                    <label></label>
                </div>
                <p className="title lm__bold-title">
                    {this.props.headline}
                    <a className="link-to-open-popup" onClick={e => this.onInfoClick(this.props.headline)}
                        style={{
                            cursor: "pointer",
                            top: "20px",
                            zIndex: 10
                        }}>
                        <i className="fa fa-info-circle" aria-hidden="true" ></i>
                    </a>
                </p>
                <div className={"type-of-sub-type" + (this.props.headline == "Basfunktioner" ? " hidden" : "")}>
                    {radioButtons}
                </div>
            </div>
        );
    }
}

interface CustomRolesProps {
    customConfig: Immutable.Map<string, Immutable.List<CustomRoleSetting>>;
    customConfigSelectedMap: Immutable.Map<string, boolean>;

    //events
    onGroupCheckedChange: (headline: string, newValue: boolean) => void;
    onRoleSettingChange: (headline: string, newRole: string) => void;
}

interface CustomRolesState {
    displayingRoleInfo: string;
}

export class CustomRoles extends React.Component<CustomRolesProps, CustomRolesState>{
    constructor(props) {
        super(props);
        this.state = {
            displayingRoleInfo: null
        };
    }

    getRoleInfoPopup(): JSX.Element {
        if (!this.state.displayingRoleInfo) { return null; }

        var roleHeadline = this.state.displayingRoleInfo;
        var profilePopup: JSX.Element;

        switch (roleHeadline) {
            default:
                profilePopup = null;
                break;
            case "Basfunktioner":
                profilePopup = infoPopup.BasfunktionerInfo;
                break;
            case "Prenumeration":
                profilePopup = infoPopup.Prenumeration;
                break;
            case "Spannmalsavtal":
                profilePopup = infoPopup.SpannmalsAvtal;
                break;
            case "Leveransinformation":
                profilePopup = infoPopup.Leveransinfo;
                break;
            case "Beställning":
                profilePopup = infoPopup.Bestallning;
                break;
            case "Maskin":
                profilePopup = infoPopup.Maskin;
                break;
            case "Ekonomi":
                profilePopup = infoPopup.Ekonomi;
                break;
        }

        return (
            <infoPopup.InfoPopup onclose={() => this.setState({ displayingRoleInfo: null })}>
                {profilePopup}
            </infoPopup.InfoPopup>
        );
    }

    shouldComponentUpdate(newProps: CustomRolesProps, newState: CustomRolesState) {
        return this.props.customConfig != newProps.customConfig
            || this.props.customConfigSelectedMap != newProps.customConfigSelectedMap
            || this.state.displayingRoleInfo != newState.displayingRoleInfo;
    }

    showRoleInfoPopup(roleHeadline: string) {
        this.setState({
            displayingRoleInfo: roleHeadline
        });
    }

    render() {
        console.log("render CustomRoles");

        let configs = this.props.customConfig.map((roleGroup, headline) =>
            <RoleGroup roleList={roleGroup} headline={headline} key={headline}
                onRoleChange={n => this.props.onRoleSettingChange(headline, n)}
                selected={this.props.customConfigSelectedMap.get(headline) || false}
                onCheckedChange={nv => this.props.onGroupCheckedChange(headline, nv)}
                onInfoClick={hl => this.showRoleInfoPopup(hl)} />
        ).toArray();

        return (
            <div>
                {this.getRoleInfoPopup()}
                {configs}
            </div>
        );
    }
}
