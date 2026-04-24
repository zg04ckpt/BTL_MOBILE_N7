import { CreateTopicRequest, TopicListItemDto, UpdateTopicRequest } from "@/types"
import { del, get, post, put } from "./config"
import { endpoints } from "./endpoints"

export const getAllTopic = async () => {
    return await get<TopicListItemDto[]>(endpoints.topics.all);
}

export const createTopic = async (request: CreateTopicRequest) => {
    return await post(endpoints.topics.create, request);
}

export const updateTopic = async (id: number, request: UpdateTopicRequest) => {
    return await put(endpoints.topics.update(id), request);
}

export const deleteTopic = async (id: number) => {
    return await del(endpoints.topics.update(id));
}