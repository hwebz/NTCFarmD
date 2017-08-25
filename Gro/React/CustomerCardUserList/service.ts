import { OrganizationUser, Profile, UserRole, AddUserInfo } from './models';

export function updateUserRole(userName: string, roles: number[], customerNumber: string): Promise<boolean> {
    var url = "/api/customer-card/update-roles";
    var formData = new FormData();
    formData.append("userName", userName);
    formData.append("roles", roles.join(","));
    formData.append("customerNumber", customerNumber);
    return new Promise<boolean>((resolve, reject) => {
        fetch(url, {
            method: "post",
            body: formData,
            credentials: 'same-origin',
        }).then(res => {
            if (res.status == 200) {
                resolve(true);
                return;
            }
            reject("cannot update profile for " + userName);
        });
    });
}

export function findExistingUser(email: string): Promise<AddUserInfo> {
    var url = "/api/add-user-to-org/find-existing?email=" + email;
    return new Promise<AddUserInfo>((resolve, reject) => {
        fetch(url, {
            method: "get",
            credentials: 'same-origin'
        }).then(res => {
            if (res.status == 409) {
                reject("conflict");
                return;
            }
            if (res.status != 200) {
                reject("badrequest");
                return;
            }

            res.json().then(js => {
                if (!js || !js["userName"]) {
                    resolve(null);
                    return;
                }

                resolve(js as AddUserInfo);
            });
        });
    });
}

export function addExistingUser(userName: string, customerNumber: string, roleIds: number[]): Promise<boolean> {
    var url = "/api/customer-card/add-existing/" + customerNumber;
    var formData = new FormData();
    formData.append("userName", userName);
    formData.append("roleIds", roleIds.join(","));
    return new Promise<boolean>((resolve, reject) => {
        fetch(url, {
            method: "post",
            body: formData,
            credentials: 'same-origin',
        }).then(res => {
            if (res.status == 200) {
                resolve(true);
                return;
            }
            reject(`cannot add user ${userName} to ${customerNumber}`);
        });
    });
}

export function addNewUser(info: AddUserInfo, roleIds: number[], customerNumber: string) {
    var url = "/api/customer-card/add-new/" + customerNumber;
    var formData = new FormData();
    formData.append("Email", info.email);
    formData.append("FirstName", info.firstName);
    formData.append("LastName", info.lastName);
    formData.append("Telephone", info.telePhone);
    formData.append("Mobile", info.mobile);
    formData.append("Roles", roleIds.join(","));
    return new Promise<boolean>((resolve, reject) => {
        fetch(url, {
            method: "post",
            body: formData,
            credentials: 'same-origin',
        }).then(res => {
            if (res.status == 200) {
                resolve(true);
                return;
            }

            reject(`Cannot add ${info.email} to ${customerNumber}`)
        });
    });
}

export function removeUser(userName: string, customerNumber: string): Promise<boolean> {
    var url = "/api/customer-card/remove-user";
    var formData = new FormData();
    formData.append("userName", userName);
    formData.append("customerNo", customerNumber);

    return new Promise<boolean>((resolve, reject) => {
        fetch(url, {
            method: "post",
            body: formData,
            credentials: 'same-origin',
        }).then(res => {
            if (res.status != 200) {
                reject(`cannot delete ${userName} from ${customerNumber}`);
                return;
            }

            resolve(true);
        });
    });
}

var roles = window["roles"].map(r => {
    var userRole: UserRole = {
        RoleName: r.RoleName,
        roleid: r.RoleId,
        RoleDescription: r.RoleDescription
    };
    return userRole;
});
var users: OrganizationUser[] = [];
var profiles = window["profileRoles"];

export function getUsers(): OrganizationUser[] {
    return users;
}

export function getProfiles(): Profile[] {
    return profiles;
}

export function getRoles(): UserRole[] {
    return roles;
}

export function getRoleById(roleId: number): UserRole {
    var rs = getRoles().filter(r => r.roleid == roleId);
    return rs.length == 0 ? null : rs[0];
}

export function getRolesOfProfile(profileId: string): UserRole[] {
    var profile = window["profileRoles"].filter(p => p.Id == profileId)[0];
    if (!profile) {
        return [];
    }

    return !!profile.ProfileRoles ? profile.ProfileRoles.map(p => {
        var r: UserRole = {
            RoleName: p.RoleName,
            roleid: p.RoleId,
            RoleDescription: p.ProfileHeadline
        };

        return r;
    }) : [];
}

export function getCustomerNumber(): string {
    return window["customerNumber"];
}


var firstLoad = true;
export function loadUsers(): Promise<OrganizationUser[]> {
    return new Promise<OrganizationUser[]>((res, rej) => {
        if (firstLoad && !!window["initialUsers"]) {
            users = window["initialUsers"];
            firstLoad = false;
            res(users);
            return;
        }

        fetch("/api/customer-card/users-of-customers/" + getCustomerNumber(), {
            method: "get",
            credentials: 'same-origin'
        }).then(r => {
            if (r.status != 200) {
                rej("error loading users");
                return;
            }

            r.json().then(js => {
                users = js;
                console.log(js);
                res(js);
            });
        });
    });
}
