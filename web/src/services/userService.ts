import { api } from './api'
import type { User, ApiResponse } from '@/types'

/**
 * User Service
 * Các API liên quan đến người dùng
 */
export const userService = {
  /**
   * Lấy danh sách tất cả người dùng
   */
  getAll: async () => {
    const response = await api.get<ApiResponse<User[]>>('/users')
    return response.data
  },

  /**
   * Lấy thông tin người dùng theo ID
   */
  getById: async (id: number) => {
    const response = await api.get<ApiResponse<User>>(`/users/${id}`)
    return response.data
  },

  /**
   * Tạo người dùng mới
   */
  create: async (userData: Partial<User>) => {
    const response = await api.post<ApiResponse<User>>('/users', userData)
    return response.data
  },

  /**
   * Cập nhật thông tin người dùng
   */
  update: async (id: number, userData: Partial<User>) => {
    const response = await api.put<ApiResponse<User>>(`/users/${id}`, userData)
    return response.data
  },

  /**
   * Xóa người dùng
   */
  delete: async (id: number) => {
    const response = await api.delete<ApiResponse<void>>(`/users/${id}`)
    return response.data
  }
}
