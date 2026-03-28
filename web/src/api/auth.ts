import { LoginSesssionDto } from "@/types/auth"
import { del, get, post } from "./config"
import { endpoints } from "./endpoints"

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL

export const login = async (email: string, password: string) => {
    return await post<LoginSesssionDto>(endpoints.auth.login, { email, password });
}

export const getLoginInfo = async () => {
    const res = await get<LoginSesssionDto>(endpoints.auth.info);
    if (res.isSuccess) {
        res.data!.avatarUrl = API_BASE_URL + res.data!.avatarUrl;
    }
    return res;
}

export const logout = async () => {
    return await del(endpoints.auth.logout);
}