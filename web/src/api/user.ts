import { CreateUserRequest, Paginated, SearchUserRequest, UpdateUserRequest, UserDetailDto, UserListItemDto } from "@/types"
import { del, get, post, putForm } from "./config"
import { endpoints } from "./endpoints"

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export const getAllUsers = async (request: SearchUserRequest) => {
    var users = await get<Paginated<UserListItemDto>>(endpoints.users.paging(request));
    if (users.isSuccess) {
        users.data!.items.forEach(u => u.avatarUrl = API_BASE_URL + u.avatarUrl)
    }
    return users;
}

export const createUser = async (request: CreateUserRequest) => {
    return await post(endpoints.users.create, request);
}

export const deleteUser = async (id: number) => {
    return await del(endpoints.users.delete(id));
}

export const updateUser = async (id: number, request: UpdateUserRequest) => {
    return await putForm(endpoints.users.update(id), request);
}

export const getUserDetail = async (id: number) => {
    var res = await get<UserDetailDto>(endpoints.users.detail(id));
    if (res.isSuccess) {
        res.data!.avatarUrl = API_BASE_URL + res.data!.avatarUrl;
    }
    return res;
}
