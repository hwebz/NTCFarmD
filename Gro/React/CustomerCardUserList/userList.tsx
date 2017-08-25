import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { MemberCard } from './memberCard';
import { OrganizationUser, AddUserInfo } from './models';
import * as service from './service';
import { CustomRoles } from "./customRoles";
import { AddUserComponent } from './addUser';
import 'isomorphic-fetch';
import "./styles.scss";

var DeteleConfirmDialog = (props: {
    userName: string;

    onResult: (confirm: boolean) => void;
}) => (
        <div className="lm__modal-alert" id="dg-del-user-confirmation" style={{ display: "block" }}>
            <div className="lm__modal-dialog">
                <div className="modal-content-inform">
                    <span className="alert-icon warning"></span>
                    <h3 className="success-header-title">Är du säker på att du vill ta bort användaren?</h3>
                    <div className="button-confirm wider-buttons align-center-text">
                        <button className="success-confirm-inform no" onClick={e => props.onResult(false)}>Nej, spara den</button>
                        <button className="success-confirm-inform reverse-state yes" onClick={e => props.onResult(true)}>Ja, ta bort</button>
                    </div>
                    <div className="lm__information-modal__close-btn">
                        <a href="javascript:void(0)" onClick={e => props.onResult(false)}></a>
                    </div>
                </div>
            </div>
        </div>
    );

interface LayoutState {
    customRoleRequestUser: string;
    customRoles: number[];
    loading: boolean;
    deletingUser: string;
    addingUser: boolean;
}

export class Layout extends React.Component<any, LayoutState>{
    private customeRoleComponent: CustomRoles;

    constructor() {
        super();
        this.state = {
            customRoleRequestUser: null,
            customRoles: [],
            loading: true,
            deletingUser: null,
            addingUser: false
        };
    }

    componentDidMount() {
        this.reload();
    }

    reload() {
        service.loadUsers().then(users => {
            this.setState({
                customRoleRequestUser: null,
                customRoles: [],
                loading: false,
                deletingUser: null,
                addingUser: false
            })
        });
    }

    onCustomRoleRequested(userName: string, roles: number[]) {
        if (this.state.customRoleRequestUser == userName) {
            this.setState({
                customRoleRequestUser: null,
                customRoles: []
            });
            return;
        }

        this.setState({
            customRoleRequestUser: userName,
            customRoles: roles
        });
    }

    updateUserRoles(userName: string, roles: number[]) {
        this.setState({
            loading: true
        });
        var promise = service.updateUserRole(userName, roles, service.getCustomerNumber());
        promise.then(r => {
            this.reload();
        });

        promise.catch(err => {
            alert(err);
            this.setState({ loading: false });
        });
    }

    onCustomRoleSubmitted(roles: number[]) {
        var userRequestedCustomeRole = this.state.customRoleRequestUser;
        this.setState({
            customRoles: roles
        });
        this.updateUserRoles(userRequestedCustomeRole, roles);
    }

    onProfileUpdate(userName: string, profileId: string) {
        var roles = service.getRolesOfProfile(profileId).map(r => r.roleid);
        this.updateUserRoles(userName, roles);
    }

    onRemoveUser(userName: string) {
        this.setState({
            loading: true
        });

        var promise = service.removeUser(userName, service.getCustomerNumber());
        promise.then(r => {
            this.reload();
        });
        promise.catch(err => {
            alert(err);
            this.setState({ loading: false });
        });
    }

    onRemoveDialogResult(r: boolean, userName: string) {
        this.setState({
            deletingUser: null
        });

        if (r == true) {
            this.onRemoveUser(userName);
        }
    }

    onRemoveClicked(userName: string) {
        this.setState({
            deletingUser: userName
        });
    }

    onAddUserClick() {
        this.setState({
            addingUser: !this.state.addingUser
        });
    }


    onAddExistingUser(userName: string, roleIds: number[]) {
        this.setState({
            loading: true
        });

        var promise = service.addExistingUser(userName, service.getCustomerNumber(), roleIds);
        promise.then(r => {
            this.reload();
        });

        promise.catch(err => {
            alert(err);
            this.setState({ loading: false });
        });
    }

    onAddNewUser(info: AddUserInfo, roleIds: number[]) {
        this.setState({
            loading: true
        });

        var promise = service.addNewUser(info, roleIds, service.getCustomerNumber());
        promise.then(r => {
            this.reload();
        });

        promise.catch(err => {
            alert(err);
            this.setState({ loading: false });
        });
    }

    getUsersDisplay() {
        var users = service.getUsers();
        var components: JSX.Element[] = [];
        for (let u of users) {
            components.push(
                <MemberCard key={u.UserName} user={u}
                    customRoles={u.Roles.map(r => r["Roleid"])} onCustomRoleRequested={r => this.onCustomRoleRequested(u.UserName, r)}
                    onProfileUpdate={r => this.onProfileUpdate(u.UserName, r)}
                    onRemove={() => this.onRemoveClicked(u.UserName)} />
            );
            if (u.UserName == this.state.customRoleRequestUser) {
                components.push(
                    <tr key={"custom"} className="lm__listing-block__item__options">
                        <td colSpan={3}>
                            <CustomRoles roleIds={this.state.customRoles} ref={r => this.customeRoleComponent = r} />

                            <input type="button" className="lm__form-btn reverse-state width-100" value="Spara"
                                onClick={e => this.onCustomRoleSubmitted(this.customeRoleComponent.getSelectedRoles())} />
                        </td>
                    </tr>
                );
            }
        }

        return components;
    }

    render() {
        var deteleConfirmDialog = !this.state.deletingUser ? null : <DeteleConfirmDialog userName={this.state.deletingUser}
            onResult={r => this.onRemoveDialogResult(r, this.state.deletingUser)} />;

        var addUserComponent = !this.state.addingUser ? null : (
            <AddUserComponent onClose={() => this.setState({ addingUser: false })}
                onAddExistingUser={(u, r) => this.onAddExistingUser(u, r)}
                onAddNewUser={(u, r) => this.onAddNewUser(u, r)} />
        );

        return (
            <div style={{
                opacity: this.state.loading ? 0.6 : 1,
                pointerEvents: this.state.loading ? "none" : "auto"
            }}>
                <div className="lm__listing-block__header">
                    <div className="layout layout--small">
                        <div className="layout__item u-1/2">
                            <h1 className="heading-title-1 no-margin">Användare</h1>
                        </div>
                        <div className="layout__item u-1/2 align-right-text">
                            <a onClick={e => this.onAddUserClick()} className="lm__blue-btn"> <i className="fa fa-plus"></i> Lägg till användare </a>
                        </div>
                    </div>
                </div>

                <div className="lm__listing-block__items">
                    <div className="lm__table-wrapper">
                        <table style={{ border: 0 }}>
                            <colgroup>
                                <col width="50%" />
                                <col width="40%" />
                                <col width="10%" />
                            </colgroup>

                            <tbody>
                                {addUserComponent}
                                {this.getUsersDisplay()}
                            </tbody>
                        </table>
                    </div>
                </div>
                {deteleConfirmDialog}
            </div>
        );
    }
}

ReactDOM.render(<Layout />, document.getElementById("userListApp"));
