export interface UserRole {
    RoleName: string;
    roleid: number;
    RoleDescription?: string;
}

export interface AddUserInfo {
    userId: number;
    userName: string;
    firstName: string;
    lastName: string;
    telePhone: string;
    mobile: string;
    email: string;
}

export interface OrganizationUser {
    UserName: string;
    Email: string;
    Name: string;
    Mobile: string;
    Phone: string;
    ProfilePicUrl: string;
    Roles: UserRole[];
    LockedOut: boolean;
    RoleProfileId: string;
    RoleProfileName: string;
    Userid: number;
}

export interface Profile {
    Description: string;
    Id: string;
}
