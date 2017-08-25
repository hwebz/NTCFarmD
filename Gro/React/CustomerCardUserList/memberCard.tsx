import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { OrganizationUser, UserRole, Profile } from './models';
import { Combobox, ComboBoxItem } from './components';
import { CustomRoles } from './customRoles';
import * as service from './service';

interface MemberCardProps {
    user: OrganizationUser;
    customRoles: number[];

    //events
    onCustomRoleRequested: (roles: number[]) => void;
    onProfileUpdate: (profileId: string) => void;
    onRemove: () => void;
}

interface MemberCardState {
    profile: string;
    profileName: string;
}

export class MemberCard extends React.Component<MemberCardProps, MemberCardState>{
    profileItems: ComboBoxItem[];
    constructor(props: MemberCardProps) {
        super(props);
        this.state = {
            profile: this.props.user.RoleProfileId,
            profileName: this.props.user.RoleProfileName,
        };

        this.profileItems = service.getProfiles().map(p => {
            var cbi: ComboBoxItem = {
                name: p.Description,
                value: p.Id
            };
            return cbi;
        });
    }

    shouldComponentUpdate(props: MemberCardProps, state: MemberCardState) {
        return this.props.user != props.user
            || this.props.customRoles != props.customRoles
            || this.state.profile != state.profile
            || this.state.profileName != state.profileName;
    }

    onProfileChange(profile: string) {
        var selectedProfile = service.getProfiles().filter(p => p.Id == profile)[0];

        this.setState({
            profile: profile,
            profileName: !selectedProfile ? "" : selectedProfile.Description
        });
    }

    onCustomRoleClick() {
        var roles = service.getRolesOfProfile(this.state.profile);
        if (!roles || roles.length == 0) {
            this.props.onCustomRoleRequested(this.props.customRoles);
            return;
        }

        this.props.onCustomRoleRequested(roles.map(r => r.roleid));
    }

    onProfileUpdate() {
        this.props.onProfileUpdate(this.state.profile);
    }

    render() {
        var deleteIcon = this.props.user.Roles.filter(r => r.RoleName.toLowerCase() == "customerowner").length > 0 ? null : (
            <a href="javascript:void(0)" className="lm__icon-btn"
                onClick={e => this.props.onRemove()}><i className="fa fa-trash-2"></i> </a>
        );

        return (
            <tr className="lm__listing-block__item" style={{ height: "220px" }}>
                <td>
                    <div style={{ position: "absolute", height: "100%", width: "100%", paddingTop: "15px", top: 0 }}>
                        <div style={{ position: "relative" }}>
                            <a href="#" className="lm__link" style={{ color: "darkgreen", marginBottom: "5px" }}>{this.props.user.Name}</a>

                            <Combobox selectedItem={{
                                name: this.state.profileName,
                                value: this.state.profile
                            }}
                                onChange={i => this.onProfileChange(i)}
                                items={this.profileItems} />

                            <div style={{
                                opacity: this.props.user.RoleProfileId == this.state.profile ? 0 : 1,
                                width: "80%",
                                padding: 0,
                                marginLeft: "-5px"
                            }}>
                                <input type="button" className="lm__form-btn reverse-state width-100" value="Spara"
                                    onClick={e => this.onProfileUpdate()} />
                            </div>

                            <p className="setting-link small-distance-top">
                                <a href="javascript:void(0)" className="lm__link"
                                    onClick={e => this.onCustomRoleClick()}>Behörigheter</a>
                            </p>
                        </div>
                    </div>
                </td>

                <td>
                    <p>E-post: {this.props.user.Email}</p>
                    <p>Mobil: {this.props.user.Mobile}</p>

                    <p className="setting-link small-distance-top">
                        <i className="fa fa-cog"></i>
                        <a href="" className="lm__link" target="_self">Meddelandeinställningar</a>
                    </p>

                    <div className="loader-wrapper" style={{ display: "none" }}>
                        <div className="loader"></div>
                    </div>
                </td>
                <td>
                    {deleteIcon}
                </td>

            </tr >
        );
    }
}
