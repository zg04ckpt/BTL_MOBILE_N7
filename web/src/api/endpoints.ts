import { SearchQuestionRequest, SearchUserRequest } from "@/types";
import { toQueryParams } from "@/utils/helper";

export const endpoints = {
    auth: {
        login: "/api/auth/login",
        logout: "/api/auth/logout",
        info: "/api/auth/info",
    },
    users: {
        paging: (request: SearchUserRequest) => "/api/users/paging?" + toQueryParams(request),
        create: "/api/users",
        detail: (id: number) => `/api/users/${id}`,
        delete: (id: number) => `/api/users/${id}`,
        update: (id: number) => `/api/users/${id}`,
    },
    topics: {
        all: "/api/topics",
        create: "/api/topics",
        delete: (id: number) => `/api/topics/${id}`,
        update: (id: number) => `/api/topics/${id}`,
    },
    questions: {
        paging: (request: SearchQuestionRequest) => "/api/questions/paging?" + toQueryParams(request),
        create: "/api/questions",
        bulkCreate: "/api/questions/bulk",
        detail: (id: number) => `/api/questions/${id}`,
        update: (id: number) => `/api/questions/${id}`,
        import: "/api/questions/import-excel",
        bulkDelete: "/api/questions/bulk",
    },
    events: {
        getAll: "/api/events",
        update: (id: number) => `/api/events/${id}`,
        rewardMapping: "/api/events/rewards/mappings"
    },
    settings: {
        getAll: "/api/settings",
        update: "/api/settings"
    }
}