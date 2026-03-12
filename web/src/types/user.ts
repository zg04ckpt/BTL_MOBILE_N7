export interface UserItem {
  key: string
  id: number
  avatar: string
  name: string
  phone: string
  level: number
  email: string
  createdAt: string
  status: 'Hoạt động' | 'Ngừng hoạt động' | 'Đang khoá'
  matches: number
}
