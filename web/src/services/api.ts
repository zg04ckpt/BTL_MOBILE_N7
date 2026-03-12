import axios, { type AxiosInstance, type AxiosRequestConfig, type AxiosResponse } from 'axios'

// Cấu hình base URL cho API
const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || '/api'

// Tạo instance của axios
const apiClient: AxiosInstance = axios.create({
  baseURL: API_BASE_URL,
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json'
  }
})

// Request interceptor để thêm token vào header
apiClient.interceptors.request.use(
  (config) => {
    // Lấy token từ localStorage nếu có
    const token = localStorage.getItem('auth_token')
    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }
    return config
  },
  (error) => {
    return Promise.reject(error)
  }
)

// Response interceptor để xử lý lỗi chung
apiClient.interceptors.response.use(
  (response: AxiosResponse) => {
    return response
  },
  (error) => {
    // Xử lý lỗi 401 (Unauthorized)
    if (error.response?.status === 401) {
      // Redirect đến trang đăng nhập hoặc xóa token
      localStorage.removeItem('auth_token')
      window.location.href = '/login'
    }
    
    // Xử lý lỗi 403 (Forbidden)
    if (error.response?.status === 403) {
      console.error('Không có quyền truy cập')
    }
    
    // Xử lý lỗi 500 (Internal Server Error)
    if (error.response?.status === 500) {
      console.error('Lỗi server')
    }
    
    return Promise.reject(error)
  }
)

// API service với các method tiện ích
export const api = {
  // GET request
  get<T = any>(url: string, config?: AxiosRequestConfig): Promise<AxiosResponse<T>> {
    return apiClient.get<T>(url, config)
  },

  // POST request
  post<T = any>(url: string, data?: any, config?: AxiosRequestConfig): Promise<AxiosResponse<T>> {
    return apiClient.post<T>(url, data, config)
  },

  // PUT request
  put<T = any>(url: string, data?: any, config?: AxiosRequestConfig): Promise<AxiosResponse<T>> {
    return apiClient.put<T>(url, data, config)
  },

  // PATCH request
  patch<T = any>(url: string, data?: any, config?: AxiosRequestConfig): Promise<AxiosResponse<T>> {
    return apiClient.patch<T>(url, data, config)
  },

  // DELETE request
  delete<T = any>(url: string, config?: AxiosRequestConfig): Promise<AxiosResponse<T>> {
    return apiClient.delete<T>(url, config)
  }
}

// Export axios instance để có thể sử dụng trực tiếp nếu cần
export default apiClient
