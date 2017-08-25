import * as React from 'react';
import * as Immutable from 'immutable';
import { UserInfo, RoleProfile, Role } from './models';

interface ProfileSelectorProps {
    profileMap: Immutable.Map<string, RoleProfile>;
    selectedProfile: string;

    //events
    onProfileSelect: (profileId: string) => void;
}

export class ProfileSelector extends React.Component<ProfileSelectorProps, any> {

    shouldComponentUpdate(newProps: ProfileSelectorProps) {
        return this.props.profileMap != newProps.profileMap
            || this.props.selectedProfile != newProps.selectedProfile;
    }

    onProfileSelect(profile: RoleProfile) {
        this.props.onProfileSelect(profile.Id);
    }

    render() {
        console.log("render ProfileSelector");

        let displayProfiles = this.props.profileMap.set("Custom", {
            Id: "Custom",
            Description: "Anpassad profil",
            ProfileRoles: []
        }).toArray();

        let profiles = displayProfiles.map(p => {
            return (
                <div className="lm__radio type-2 width-40" key={p.Id}>
                    <input type="radio" name="user-type" id={p.Id} checked={this.props.selectedProfile == p.Id}
                        value={p.Id}
                        onChange={e => this.onProfileSelect(p)} />
                    <p>{p.Description}</p>
                </div>
            );
        });

        return (
            <div className="author-inform-form__column author-inform-form__column-full">
                <div className="choose-user-type">
                    {profiles}
                </div>
            </div>
        );
    }
}
