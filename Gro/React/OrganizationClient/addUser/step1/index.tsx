import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { hashHistory } from 'react-router';
import { View } from '../../shared/view';
import { UserInfo, SearchUserStatus } from '../../shared/models';
import { UserSearchBox } from './userSearchBox';
import { UserInfoForm } from './userInfoForm';
import { addUserStore } from '../store';
import * as service from '../service';

export class Step1ViewState {
    userInfo: UserInfo;
    searchStatus: SearchUserStatus;
}

export class Step1View extends View<any, Step1ViewState>{
    private searchingEmail: string = null;

    constructor(props) {
        super(props);

        this.state = {
            userInfo: null,
            searchStatus: SearchUserStatus.notSearched
        };
    }

    userSet = () => {
        let userInfo = addUserStore.getUserInfo();
        if (!userInfo) {
            this.setState({
                userInfo: null,
                searchStatus: SearchUserStatus.notSearched
            });
        }

        this.setState({
            userInfo: userInfo,
            searchStatus: !userInfo.userName ? SearchUserStatus.userNotFound : SearchUserStatus.userFound
        });
    };

    userConflict = () => {
        this.setState({
            userInfo: null,
            searchStatus: SearchUserStatus.userConflict
        });
    };

    componentDidMount() {
        super.componentDidMount();
        addUserStore.setCurrentStep(1);

        //add event listeners
        addUserStore.addListener("user", this.userSet);
        addUserStore.addListener("userExists", this.userConflict);

        if (addUserStore.isStep1Done()) {
            this.setState({
                userInfo: addUserStore.getUserInfo(),
                searchStatus: SearchUserStatus.searched
            });
        }
    }

    componentWillUnmount() {
        addUserStore.removeListener("user", this.userSet);
        addUserStore.removeListener("userExists", this.userConflict);
    }

    onEmailSearch(email: string) {
        this.searchingEmail = email;
        service.searchUserInfo(email);
        this.setState({
            userInfo: this.state.userInfo,
            searchStatus: SearchUserStatus.searching
        });
    }

    onUserInfoSubmit(u: UserInfo) {
        addUserStore.setUserInfo(u, false);
        hashHistory.push('/step2');
    }

    render() {

        return (
            <div className="lm__add-user-form author-inform-form">
                <div className="author-inform-form__input">
                    <h2 className="heading-title-2">Kontrollera om användaren redan har LM<sup>2</sup>-konto</h2>
                    <p>Fyll i användarens E-postadress för att kontrollera om personen redan är registrerad på LM<sup>2</sup> </p>
                    <UserSearchBox commitSearch={e => this.onEmailSearch(e)} disabled={this.state.searchStatus == SearchUserStatus.searching} />
                </div>
                <UserInfoForm userInfo={this.state.userInfo} searchStatus={this.state.searchStatus}
                    onSubmit={u => this.onUserInfoSubmit(u)} email={this.state.searchStatus == SearchUserStatus.userNotFound ? this.searchingEmail : null} />
            </div>
        );
    }
}
