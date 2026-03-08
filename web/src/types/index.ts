// Định nghĩa các types/interfaces chung cho ứng dụng

export interface User {
  id: number
  username: string
  email: string
  fullName?: string
}

export interface ApiResponse<T = any> {
  success: boolean
  data: T
  message?: string
  error?: string
}

export interface PaginationParams {
  page: number
  limit: number
}

export interface PaginatedResponse<T> {
  data: T[]
  total: number
  page: number
  limit: number
  totalPages: number
}
