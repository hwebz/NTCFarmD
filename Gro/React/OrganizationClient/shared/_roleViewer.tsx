import * as React from 'react';
import * as ReactDOM from 'react-dom';
import * as Immutable from 'immutable';
import { UserInfo, RoleProfile, Role, ProfileRole } from './models';
import * as infoPopup from './_roleInfoPopup';

interface RoleViewerProps {
    profile: RoleProfile;
}

interface RoleViewerState {
    displayingRoleInfo: string;
}

export class RoleViewer extends React.Component<RoleViewerProps, RoleViewerState>{
    constructor(props) {
        super(props);
        this.state = {
            displayingRoleInfo: null
        };
    }

    showRoleInfoPopup(roleHeadline: string) {
        this.setState({
            displayingRoleInfo: roleHeadline
        });
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

    shouldComponentUpdate(newProps: RoleViewerProps, newState: RoleViewerState) {
        return this.props.profile != newProps.profile
            || this.state.displayingRoleInfo != newState.displayingRoleInfo;
    }

    render() {
        console.log("render RoleViewer");
        let popup = this.getRoleInfoPopup();
        let roles = this.props.profile.ProfileRoles.map(r =>
            <div className="sub-type" key={r.RoleId}>
                <div className="lm__checkbox lm__tick disabled">
                    <input type="checkbox" defaultChecked />
                    <label></label>
                </div>
                <p className="title lm__bold-title">{r.ProfileHeadline}
                    <a className="link-to-open-popup" >
                        <i className="fa fa-info-circle" aria-hidden="true"
                            onClick={e => this.showRoleInfoPopup(r.ProfileHeadline)}
                            style={{ cursor: "pointer" }}></i>
                    </a>
                </p>
                <p className={"sub-title " + (r.RoleName == "Basfunktioner" ? "hidden" : "")}>{r.RoleRights}</p>
            </div>
        );

        return (
            <div >
                {popup}
                {roles}
            </div>
        );
    }
}
