// Xóa nhiều câu hỏi bằng cách gửi nhiều request xóa đơn lẻ
export const deleteQuestions = async (ids: number[]) => {
    const results = await Promise.all(ids.map(id => del(endpoints.questions.delete(id))));
    // Nếu tất cả đều thành công thì trả về isSuccess true, ngược lại trả về false và tổng số lỗi
    const failed = results.filter(r => !r.isSuccess);
    return {
        isSuccess: failed.length === 0,
        message: failed.length === 0 ? 'Đã xóa tất cả câu hỏi' : `Có ${failed.length} câu hỏi xóa thất bại`,
        results
    };
};
import { CreateQuestionRequest, Paginated, QuestionListItemDto, SearchQuestionRequest, UpdateQuestionRequest, QuestionDetailDto } from "@/types";
import { endpoints } from "./endpoints";
import { del, get, postForm, putForm } from "./config";

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

export const updateQuestion = async (id: number, request: UpdateQuestionRequest) => {
    return await putForm(endpoints.questions.update(id), request);
}

export const deleteQuestion = async (id: number) => {
    return await del(endpoints.questions.delete(id));
}

export const uploadExcelQuestions = async (file: File) => {
    const formData = new FormData();
    formData.append('file', file);
    return await postForm(endpoints.questions.import, formData);
}