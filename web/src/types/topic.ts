import { PagingRequest } from ".";

export interface CreateTopicRequest {
    name: string;
}

export interface SearchTopicRequest extends PagingRequest {
    name?: string;
}

export interface UpdateTopicRequest {
    name: string;
}

export interface TopicDetailDto {
    id: number;
    name: string;
    slug: string;
    questionCount: number;
}

export interface TopicListItemDto {
    id: number;
    name: string;
    slug: string;
    questionCount: number;
}
