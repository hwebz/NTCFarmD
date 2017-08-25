import { UserInfo, RoleProfile, Role, CustomRoleSetting } from "../shared/models";
import { User } from "./models";
import { store } from "./store";
import "isomorphic-fetch"

/**
 * Return the id of the role profile that user belongs
 */
export function matchUserRoleProfile(user: User): string {
    return !user.roleProfileId || !user.roleProfileId.trim() ? "Custom" : user.roleProfileId;
}

function getUsersOfThisOrganization(): Promise<User[]> {
    return new Promise<User[]>((resolve, reject) => {
        fetch("/api/organization/get-users?pageId=" + window["pageId"], {
            credentials: "same-origin",
            cache: "reload"
        }).then(r => {
            if (r.status !== 200) {
                r.text().then(t => reject(t));
                return;
            }

            r.json().then(j => resolve(j));
        });
    });
}

export function fetchUsers() {
    getUsersOfThisOrganization().then(users => {
        store.setUsers(users);
    }).catch(err => {
        alert(`fetch users failed: ${err}`);
        console.error(err);
    });
}
