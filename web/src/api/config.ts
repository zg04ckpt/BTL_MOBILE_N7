import { ApiResult } from '@/types/api';
import { convertToFormData, toCamelCase } from '@/utils/helper';
import axios, { AxiosError, type AxiosInstance, type AxiosRequestConfig, type AxiosResponse } from 'axios'

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL

const apiClient: AxiosInstance = axios.create({
	baseURL: API_BASE_URL,
	withCredentials: true,
});

// Request interceptor
apiClient.interceptors.request.use((config) => {
	config.withCredentials = true;
	return config;
});

// Response interceptor
apiClient.interceptors.response.use(
	(res) => {
		return res;
	},
	async (error: AxiosError) => {
		const apiError = error as AxiosError;

		if (apiError.response?.status === 403) {
			window.location.href = "/403";
			return;
		}

		if (apiError.response?.status === 401) {
			if (window.location.pathname !== '/login') {
				window.location.href = '/login';
			}
		}

		return Promise.resolve<AxiosResponse>(toCamelCase(error.response));
	}
);

export const get = async <T = void>(url: string): Promise<ApiResult<T>> => {
	try {
		return (await apiClient.get<ApiResult<T>>(url)).data;
	} catch (error: any) {
		return {
			isSuccess: false,
			message: error?.data?.message || "Yêu cầu thất bại.",
		};
	}
};

export const post = async <T = void>(url: string, data: any): Promise<ApiResult<T>> => {
	try {
		return (await apiClient.post<ApiResult<T>>(url, data)).data;
	} catch (error: any) {
		return {
			isSuccess: false,
			message: error?.data?.message || "Yêu cầu thất bại.",
		};
	}
};

export const postForm = async <T = void>(url: string, data: any): Promise<ApiResult<T>> => {
	try {
		return (await apiClient.post<ApiResult<T>>(url, convertToFormData(data))).data;
	} catch (error: any) {
		return {
			isSuccess: false,
			message: error?.data?.message || "Yêu cầu thất bại.",
		};
	}
};

export const putForm = async <T = void>(url: string, data: any): Promise<ApiResult<T>> => {
	try {
		return (await apiClient.put<ApiResult<T>>(url, convertToFormData(data))).data;
	} catch (error: any) {
		return {
			isSuccess: false,
			message: error?.data?.message || "Yêu cầu thất bại.",
		};
	}
};

export const put = async <T = void>(url: string, data: any): Promise<ApiResult<T>> => {
	try {
		return (await apiClient.put<ApiResult<T>>(url, data)).data;
	} catch (error: any) {
		return {
			isSuccess: false,
			message: error?.data?.message || "Yêu cầu thất bại."
		};
	}
};

export const del = async <T = void>(url: string): Promise<ApiResult<T>> => {
	try {
		return (await apiClient.delete<ApiResult<T>>(url)).data;
	} catch (error: any) {
		return {
			isSuccess: false,
			message: error?.data?.message || "Yêu cầu thất bại."
		};
	}
};