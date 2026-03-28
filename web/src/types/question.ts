import { PagingRequest } from ".";

export interface AnswersDto {
    correctAnswers: string[];
    stringAnswers: string[];
}

export interface CreateQuestionRequest {
    stringContent: string;
    image?: File;
    audio?: File;
    video?: File;
    type: QuestionType;
    level: QuestionLevel;
    topicId: number;
    correctAnswers: string[];
    stringAnswers: string[];
}

export interface QuestionDetailDto {
    id: number;
    slug: string;
    stringContent: string;
    imageUrl: string|null;
    audioUrl: string|null;
    videoUrl: string|null;
    type: QuestionType;
    level: QuestionLevel;
    status: QuestionStatus;
    topicId: number;
    topicName: string;
    createdAt: Date;
    correctAnswers: string[];
    stringAnswers: string[];
}

export interface QuestionListItemDto {
    id: number;
    slug: string;
    stringContent: string;
    type: QuestionType;
    level: QuestionLevel;
    status: QuestionStatus;
    topicName: string;
    createdAt: Date;
}

export interface SearchQuestionRequest extends PagingRequest {
    stringContent: string;
    topicId: number|null;
    type: QuestionType|null;
    level: QuestionLevel|null;
    status: QuestionStatus|null;
}



export interface UpdateQuestionRequest {
    stringContent: string;
    image?: File;
    audio?: File;
    video?: File;
    type: QuestionType;
    level: QuestionLevel;
    status: QuestionStatus;
    topicId: number;
    correctAnswers: string[];
    stringAnswers: string[];
}

export enum QuestionLevel {
    Normal,
    Medium,
    Hard
}

export const QuestionLevelLabel: Record<QuestionLevel, string> = {
    [QuestionLevel.Normal]: 'Normal',
    [QuestionLevel.Medium]: 'Medium',
    [QuestionLevel.Hard]: 'Hard',
};

export enum QuestionStatus {
    Draft,
    Pending,
    Approved,
    Rejected
}

export const QuestionStatusLabel: Record<QuestionStatus, string> = {
    [QuestionStatus.Draft]: 'Draft',
    [QuestionStatus.Pending]: 'Pending',
    [QuestionStatus.Approved]: 'Approved',
    [QuestionStatus.Rejected]: 'Rejected',
};

export enum QuestionType {
    SingleChoice,
    MultipleChoice,
    TrueFalse
}

export const QuestionTypeLabel: Record<QuestionType, string> = {
    [QuestionType.SingleChoice]: 'Single Choice',
    [QuestionType.MultipleChoice]: 'Multiple Choice',
    [QuestionType.TrueFalse]: 'True False',
};
