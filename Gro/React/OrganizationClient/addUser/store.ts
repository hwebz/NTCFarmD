import { EventEmitter } from 'events';
import { UserInfo, RoleProfile, Role, CustomRoleSetting } from '../shared/models';
import { onRouteChange, RouteStore } from '../../components/routeStore';
import * as Immutable from 'immutable';

export class AddUserStore extends RouteStore {
    private currentStep = 0;
    private userInfo: UserInfo = null;
    private roleProfilesMap = Immutable.Map<string, RoleProfile>();
    private rolesMap = Immutable.Map<string, Role>();
    private customConfig = Immutable.Map<string, Immutable.List<CustomRoleSetting>>();
    private customConfigSelectedMap = Immutable.Map<string, boolean>();
    private selectedProfileId: string = null;

    public step2Done = false;

    constructor() {
        super();
        let roles = JSON.parse(window["roles"]) as Role[];
        let roleProfiles = JSON.parse(window["profileRoles"]) as RoleProfile[];
        this.setRolesAndProfiles(roles, roleProfiles);
        let defaultProfile = this.roleProfilesMap.filter(p => p.Id == "Anstalld");
        this.selectedProfileId = defaultProfile.count() > 0 ? defaultProfile.first().Id : roleProfiles[0].Id || null;
    }

    public canNavigate(route: string): boolean {
        if (route == "/step2") {
            if (this.getCurrentStep() == 2 || this.isStep1Done() == false) {
                return false;
            }
        } else if (route == "/step3") {
            if (this.getCurrentStep() == 3 || this.isStep1Done() == false) {
                return false;
            }
        }

        return true;
    }

    private setRolesAndProfiles(roles: Role[], roleProfiles: RoleProfile[]) {
        //set profiles
        for (var profile of roleProfiles) {
            this.roleProfilesMap = this.roleProfilesMap.set(profile.Id, profile);
        }

        //set roles
        for (var role of roles) {
            this.rolesMap = this.rolesMap.set(role.RoleName, role);
            // check for roles
        }

        var adminRoles = this.getRoleProfile("Admin");
        for (var adminRole of adminRoles.ProfileRoles) {
            let fullRole = this.rolesMap.get(adminRole.RoleName);
            let customFullRole: CustomRoleSetting = {
                RoleId: fullRole.RoleId,
                RoleName: fullRole.RoleName,
                selected: true
            };
            let groupRoleList = Immutable.List<CustomRoleSetting>([customFullRole]);

            if (adminRole.RoleName.endsWith("_w")) {
                let readOnlyRoleName = adminRole.RoleName.substr(0, adminRole.RoleName.indexOf("_w"));
                let readOnlyRole = this.rolesMap.get(readOnlyRoleName);
                let customReadOnlyRole: CustomRoleSetting = {
                    RoleId: readOnlyRole.RoleId,
                    RoleName: readOnlyRole.RoleName,
                    selected: false
                };
                if (!!readOnlyRole) {
                    groupRoleList = groupRoleList.push(customReadOnlyRole);
                }
            }

            this.customConfig = this.customConfig.set(adminRole.ProfileHeadline, groupRoleList);
            this.customConfigSelectedMap = this.customConfigSelectedMap.set(adminRole.ProfileHeadline, true);
        }
    }

    setSelectedProfileId(profileId: string) {
        this.selectedProfileId = profileId;
        this.emit("roleProfile");
    }

    getSelectedProfileId(): string {
        return this.selectedProfileId;
    }

    setCurrentStep(stepName: number) {
        this.currentStep = stepName;
        this.emit("step");
    }

    getCurrentStep() {
        return this.currentStep;
    }

    setUserInfo(userInfo: UserInfo, emit: boolean = true) {
        this.userInfo = userInfo;
        if (emit) { this.emit("user"); }
    }

    getUserInfo(): UserInfo {
        return this.userInfo;
    }

    isStep1Done(): boolean {
        let userInfo = this.getUserInfo();
        return !!userInfo && !!userInfo.userName && !!userInfo.userName.trim();
    }

    getRoleProfile(profileId: string): RoleProfile {
        return this.roleProfilesMap.get(profileId) || null;
    }

    getAllRoleProfiles() {
        return this.roleProfilesMap;
    }

    getCustomConfig() {
        return this.customConfig;
    }

    getcustomConfigSelectedMap() {
        return this.customConfigSelectedMap;
    }

    customRoleChanged(headline: string, newRole: string) {
        let roleList = this.customConfig.get(headline);
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

        this.customConfig = this.customConfig.set(headline, roleList);
        this.emit("roleProfile");
    }

    customRoleGroupChanaged(headline: string, newValue: boolean) {
        this.customConfigSelectedMap = this.customConfigSelectedMap.set(headline, newValue);
        this.emit("roleProfile");
    }
}

export var addUserStore = new AddUserStore();
