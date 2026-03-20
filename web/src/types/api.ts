export interface ApiResult<T = void> {
	isSuccess: boolean;
	message: string | null;
	data?: T | null;
}

export interface Paginated<T>
{
    totalItems: number;
    totalPages: number;
    pageIndex: number;
    pageSize: number;
    items: T[];
}

export interface PagingRequest {
    pageIndex: number;
    pageSize: number;
    orderBy?: string;
    isAsc: boolean;
}
