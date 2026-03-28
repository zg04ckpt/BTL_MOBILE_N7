import { PagingRequest } from ".";

export interface SearchUserRequest extends PagingRequest {
    email?: string;
    phone?: string;
    name?: string;
}

export interface CreateUserRequest {
    email: string;
    phoneNumber: string;
    name: string;
    password: string;
    roleId: number;
}

export interface UpdateUserRequest {
    avatar?: File;
    phoneNumber: string;
    name: string;
    email: string;
    status: AccountStatus;
    level: number;
    rank: number;
    rankScore: number;
    exp: number;
    roleId: number;
}

export enum AccountStatus {
    Active,
    Inactive,
    Banned,
    Deleted
}

export enum RoleName {
    SuperAdmin,
    Admin,
    Moderator,
    Editor,
    User
}

export interface UserListItemDto {
    id: number;
    name: string;
    email: string;
    avatarUrl: string;
    status: AccountStatus;
    roleName: string;
}

export interface UserDetailDto {
    id: number;
    avatarUrl: string;
    phoneNumber: string;
    name: string;
    email: string;
    status: AccountStatus;
    level: number;
    rank: number;
    rankScore: number;
    exp: number;
    createdAt: Date;
    roleId: number;
    roleName: string;
}


