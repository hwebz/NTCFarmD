import { EventEmitter } from 'events';
import * as Immutable from 'immutable';
import { UserInfo, RoleProfile, Role, CustomRoleSetting } from '../shared/models';
import { User } from './models';

//Map tab: DisplayOrder -> SettingTab
//Map category : id -> category
//List : [index] => MessageType (item)

export class HandleUserStore extends EventEmitter {
    public users = Immutable.List<User>();
    public roles = Immutable.Map<string, Role>();
    public profiles = Immutable.Map<string, RoleProfile>();
    public selectedUser: User = null;

    constructor() {
        super();
        var profiles: RoleProfile[] = window["profileRoles"];
        for (var profile of profiles) {
            this.profiles = this.profiles.set(profile.Id, profile);
        }

        var roles: Role[] = window["roles"];
        for (var role of roles) {
            this.roles = this.roles.set(role.RoleName, role);
        }
    }

    setUsers(users: User[]) {
        this.users = Immutable.List(users);
        this.emit("users");
    }

    clearUsers() {
        this.users = Immutable.List<User>();
        this.selectedUser = null;
    }
}

export var store = new HandleUserStore();
