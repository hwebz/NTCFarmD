import * as React from 'react';
import * as ReactDOM from 'react-dom';
import * as service from './service';
import { UserRole } from './models';

interface RoleGroupDisplay {
    description: string;
    roles: UserRole[];
    roleSelectionMap: { [roleId: number]: boolean };

    //events
    roleSelected?: (roleId: number) => void;
}

class RoleGroup extends React.Component<RoleGroupDisplay, any>{
    isChecked() {
        var checked = false;
        for (var r of this.props.roles) {
            if (!!this.props.roleSelectionMap[r.roleid]) {
                checked = true;
                break;
            }
        }

        return checked;
    }

    onOuterCheckboxChange(e: React.FormEvent<HTMLInputElement>) {
        if (this.isChecked()) {
            this.onInnerRoleSelected(null);
            return;
        }

        if (this.props.roles.length == 0) { return; }

        this.onInnerRoleSelected(this.props.roles[0])
    }

    onInnerRoleSelected(role: UserRole) {
        if (this.props.roleSelected) {

            this.props.roleSelected(!!role ? role.roleid : -1);
        }
    }

    render() {
        var checked = this.isChecked();

        return (
            <div className="lm__block-box">
                <div className="parent-checkbox">
                    <div className="lm__checkbox style-3 lm__tick">
                        <input type="checkbox" checked={checked} className="rolegroup-checkbox" onChange={e => this.onOuterCheckboxChange(e)} />
                        <label></label>
                    </div>
                    <h3 className="lm__bold-title">{this.props.description} <a className="link-to-open-popup"><i className="fa fa-info-circle" aria-hidden="true"></i></a></h3>
                </div>
                <div className="sub-checkbox">
                    <ul>
                        {this.props.roles.map(r => {
                            var write = r.RoleName.endsWith("_w");

                            return (
                                <li key={r.RoleName}>
                                    <div className="lm__radio type-4">
                                        <input type="radio" name={this.props.description} checked={!!this.props.roleSelectionMap[r.roleid]}
                                            value={r.roleid}
                                            onChange={e => this.onInnerRoleSelected(r)} />
                                        <p className={checked ? "" : "no-checkbox"}>{write ? "Fullständig behörighet" : "Kan se"}</p>
                                    </div>
                                </li>
                            );
                        })}
                    </ul>
                </div>
            </div>
        );
    }
}

interface CustomRolesProps {
    roleIds: number[];
}

interface CustomRolesState {
    roleSelectionMap: { [roleId: number]: boolean };
}

export class CustomRoles extends React.Component<CustomRolesProps, CustomRolesState>{

    constructor(props: CustomRolesProps) {
        super(props);
        var roleSelectionMap = {};
        for (var roleId of props.roleIds) {
            roleSelectionMap[roleId] = true;
        }
        this.state = {
            roleSelectionMap: roleSelectionMap
        };
    }

    componentWillReceiveProps(nextProps: CustomRolesProps) {
        var roleSelectionMap = {};
        for (var roleId of nextProps.roleIds) {
            roleSelectionMap[roleId] = true;
        }

        this.setState({
            roleSelectionMap: roleSelectionMap
        });
    }

    getRoleGroups(): { [groupName: string]: RoleGroupDisplay } {

        var rg: { [groupName: string]: RoleGroupDisplay } = {};
        var allRoles = service.getRoles();
        for (var role of allRoles) {
            if (!rg[role.RoleDescription]) {
                rg[role.RoleDescription] = {
                    description: role.RoleDescription,
                    roleSelectionMap: this.state.roleSelectionMap,
                    roles: [role]
                };
            } else {
                rg[role.RoleDescription].roles.push(role);
            }
        }

        return rg;
    }

    onRoleChecked(roleId: number, roleGroup: RoleGroupDisplay) {
        var roleSelectionMap = this.state.roleSelectionMap;
        for (var r of roleGroup.roles) {
            roleSelectionMap[r.roleid] = r.roleid == roleId
        }

        this.setState({
            roleSelectionMap: roleSelectionMap
        });
    }

    getSelectedRoles(): number[] {
        var roles = Object.keys(this.state.roleSelectionMap)
            .filter(k => this.state.roleSelectionMap[k] == true)
            .map(k => parseInt(k));
        return roles;
    }

    render() {
        var roleGroups = this.getRoleGroups();
        var roleGroupKeys = Object.keys(roleGroups);

        return (
            <div className="lm__block customer-id-block" style={{ backgroundColor: "#f6f6f6" }}>
                {roleGroupKeys.map(k => {
                    var rg = roleGroups[k];
                    return (
                        <RoleGroup key={k} roles={rg.roles} roleSelectionMap={rg.roleSelectionMap} description={rg.description}
                            roleSelected={rid => this.onRoleChecked(rid, rg)} />
                    );
                })}
            </div>
        );
    }
}
