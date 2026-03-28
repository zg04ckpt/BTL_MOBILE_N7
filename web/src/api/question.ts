import { CreateQuestionRequest, Paginated, QuestionListItemDto, SearchQuestionRequest, UpdateQuestionRequest, QuestionDetailDto } from "@/types";
import { endpoints } from "./endpoints";
import { delWithBody, get, post, postForm, putForm } from "./config";

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export const getAllQuestions = async (request: SearchQuestionRequest) => {
    var res = await get<Paginated<QuestionListItemDto>>(endpoints.questions.paging(request));
    return res;
}

export const getQuestionDetail = async (id: number) => {
    var res = await get<QuestionDetailDto>(endpoints.questions.detail(id));
    if (res.isSuccess) {
        res.data!.imageUrl = API_BASE_URL + res.data!.imageUrl;
        res.data!.audioUrl = API_BASE_URL + res.data!.audioUrl;
        res.data!.videoUrl = API_BASE_URL + res.data!.videoUrl;
    }
    return res;
}

export const createQuestion = async (request: CreateQuestionRequest) => {
    return await postForm(endpoints.questions.create, request);
}

export const createQuestionsBulk = async (requests: CreateQuestionRequest[]) => {
    return await post(endpoints.questions.bulkCreate, requests);
}

export const updateQuestion = async (id: number, request: UpdateQuestionRequest) => {
    return await putForm(endpoints.questions.update(id), request);
}

export const deleteQuestions = async (ids: number[]) => {
    return await delWithBody(endpoints.questions.bulkDelete, ids);
};

export const uploadExcelQuestions = async (file: File) => {
    const formData = new FormData();
    formData.append('file', file);
    return await postForm(endpoints.questions.import, formData);
}