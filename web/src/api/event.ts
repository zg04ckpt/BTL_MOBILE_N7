import { EventRewardInfoDto, UpdateEventRequest } from "@/types/event";
import { get, post, put } from "./config"
import { endpoints } from "./endpoints"

export const getAllEvents = async () => {
    const events =  await get<any[]>(endpoints.events.getAll);
    // events.data!.forEach(e => e.startTime = new Date(e.startTime));
    // events.data!.forEach(e => e.endTime = new Date(e.endTime));
    return events;
}

export const updateEvent = async (id: number, request: UpdateEventRequest) => {
    return await put(endpoints.events.update(id), request);
}

export const getAllRewards = async () => {
    return await get<EventRewardInfoDto[]>(endpoints.events.rewardMapping);
}