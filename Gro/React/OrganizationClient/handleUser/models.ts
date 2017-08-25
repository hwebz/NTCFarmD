export interface UserRole {
    CustomerId: number;
    CustomerNumber: string;
    RoleName: string;
    roleid: number;
}

export interface User {
    email: string;
    id: number;
    mobile: string;
    name: string;
    phone: string;
    profilePicUrl: string;
    userName: string;
    roles: UserRole[];
    lockedOut: boolean;
    roleProfileId: string;
    roleProfileName: string;
}
