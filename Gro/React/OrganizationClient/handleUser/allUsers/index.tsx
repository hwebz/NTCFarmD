import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { hashHistory } from 'react-router';
import { BankIdLogin } from '../../shared/_bankIdLogin';
import { View } from '../../shared/view';
import { User } from '../models';
import { store } from '../store';
import * as Immutable from 'immutable';
import * as action from '../action';
import { Spinner } from '../../../components/spinner';

interface AllUsersViewState {
    users: Immutable.List<User>;
}

interface UserGroup {
    [profileKey: string]: User[];
}

function getUserProfileGroups(users: Immutable.List<User>): UserGroup {
    let results: UserGroup = {};
    users.map(user => {
        let groupId = action.matchUserRoleProfile(user);
        if (!results[groupId]) {
            results[groupId] = [];
        }

        results[groupId].push(user);
    });

    return results;
}

export class AllUsersView extends View<any, AllUsersViewState>{
    private fetching = true;

    constructor() {
        super();
        this.state = {
            users: Immutable.List<User>()
        };
    }

    onUsersChange = () => {
        this.fetching = false;
        this.setState({
            users: store.users
        });
    }

    componentDidMount() {
        store.addListener("users", this.onUsersChange);
        store.clearUsers();
        action.fetchUsers();
    }

    componentWillUnmount() {
        store.removeListener("users", this.onUsersChange);
    }

    onUserClick(user: User, profileGroupId: string) {
        if (profileGroupId == "CustomerOwner" || user.roleProfileId == "Owner") {
            return;
        }
        store.selectedUser = user;
        hashHistory.push('/specific');
    }

    getUserGroupDisplay(profileGroupId: string, users: User[]): JSX.Element {
        let userBoxes = users.map(user => (
            <div className={"lm__handle-user-box__inform" + (user.lockedOut ? " user-inactive" : "")} key={user.userName}>
                <a className="user-info-container" onClick={e => this.onUserClick(user, profileGroupId)}>
                    <img src={user.profilePicUrl} alt="User Avatar" className="lm__handle-user-box__avatar" />
                    <div>
                        <p className="lm__bold-title">{user.name}</p>
                        <a className="user-email-link" href={"mailto:" + user.email} target="_top"
                            onClick={e => e.stopPropagation()}>{user.email}</a>
                    </div>
                </a>
            </div>
        ));

        return (
            <div className="lm__handle-user-box" key={profileGroupId}>
                <h3 className="lm__handle-user-box__title">{users[0].roleProfileName}</h3>
                {userBoxes}
            </div>
        );
    }

    getUserGroupsMarkup() {
        let userGroups = getUserProfileGroups(this.state.users);
        let userGroupKeys = Object.keys(userGroups);
        let boxes = userGroupKeys.sort((a, b) => {
            if (a > b) { return 1; }
            if (a < b) { return -1; }
            return 0;
        }).map(k => this.getUserGroupDisplay(k, userGroups[k]));

        return boxes;
    }

    getSearchSpinner() {
        return (
            <div className="author-inform-form__input">
                <Spinner className={"search-user-spinner"} color={"#58a618"} period={0.7} size={32} thickness={3} />
            </div>
        );
    }

    render() {
        let activeclassName = !window["serialNumber"] ? "inactive-area" : "";
        var addNewLinkInput = document.getElementById("addUserToOrganizationPageLink") as HTMLInputElement;
        var addnewLink = !addNewLinkInput ? "" : addNewLinkInput.value;

        var profileInfoInput = document.getElementById("profileInformationPageLink") as HTMLInputElement;
        var profileInfoLink = !profileInfoInput ? "" : profileInfoInput.value;

        return (
            <div className="lm__add-user-form author-inform-form">
                <BankIdLogin />
                <div className={activeclassName}>
                    <div className="author-inform-form__input">
                        <p>Här kan du se och hantera och lägga till användare kopplade till ditt kundnummer. Du kan välja och redigera profiler och behörigheter  till olika tjänster i LM2 för varje användare.</p>
                    </div>
                    <div className="author-inform-form__input">
                        <a href={profileInfoLink} className="lm__link"> Se information om användarprofiler </a>
                        <br />
                        <a href={addnewLink} style={{ color: "inherit" }}>
                            <button className="lm__blue-btn reverse-state block-element create-new">
                                <i className="fa fa-plus" aria-hidden="true"></i>
                                Lägg till användare
                            </button>
                        </a>
                    </div>
                    {this.fetching ? this.getSearchSpinner() : this.getUserGroupsMarkup()}
                </div>
            </div>
        );
    }
}
