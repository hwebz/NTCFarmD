import { UserInfo, RoleProfile, ProfileRole, Role } from './../shared/models';
import { addUserStore } from './store';
import "isomorphic-fetch"

var $ = window["$"];

function getUserInfoByEmail(email: string): Promise<UserInfo> {
    let url = `/api/add-user-to-org/find-existing?email=${encodeURIComponent(email)}`;
    return new Promise<UserInfo>((resolve, reject) => {
        fetch(url, {
            credentials: 'same-origin',
            cache: "reload"
        }).then(r => {
            if (r.status == 409) {
                //user exists
                reject("UserExists")
            }

            if (r.status != 200) {
                r.text().then(t => reject(t));
                return;
            }

            r.json().then(j => resolve(j));
        });
    });
}

export function searchUserInfo(email: string) {
    addUserStore.step2Done = false;
    addUserStore.setUserInfo(null, false);
    addUserStore.emit("step");

    getUserInfoByEmail(email).then(user => {
        if (!user || !user.userName) {
            //no user with that email
            addUserStore.setUserInfo({
                email: null,
                firstName: null,
                lastName: null,
                mobile: null,
                telephone: null,
                userId: 0,
                userName: null
            });
            return;
        }

        addUserStore.setUserInfo(user);
    }).catch(err => {
        if (err == "UserExists") {
            addUserStore.emit("userExists");
            return;
        }

        alert("failed request " + err);
        addUserStore.setUserInfo(null);
    });
}
