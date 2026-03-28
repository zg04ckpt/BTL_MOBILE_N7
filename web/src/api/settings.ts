
import { SettingsDto, UpdateSettingsRequest } from "@/types";
import { get, put } from "./config"
import { endpoints } from "./endpoints"

export const getAllSettings = async () => {
    return get<SettingsDto>(endpoints.settings.getAll);
}

export const updateSetting = async (request: UpdateSettingsRequest) => {
    return put(endpoints.settings.update, request);
}