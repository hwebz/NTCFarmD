export interface UserInfo {
    userId: number;
    userName: string;
    firstName: string;
    lastName: string;
    telephone: string;
    mobile: string;
    email: string;
}

export enum SearchUserStatus {
    notSearched, searching, userNotFound, userFound, userConflict, searched
}

export interface Role {
    RoleId: number;
    RoleName: string;
}

export interface CustomRoleSetting extends Role {
    selected: boolean;
}

export interface ProfileRole {
    RoleId: number;
    RoleName: string;
    ProfileId: string;
    ProfileDescription: string;
    ProfileHeadline: string;
    RoleRights: string;
}

export interface RoleProfile {
    Id: string;
    Description: string;
    ProfileRoles: ProfileRole[];
}
