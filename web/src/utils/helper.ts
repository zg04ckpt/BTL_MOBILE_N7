import { QuestionItem } from '@/types/question'
import { UserItem } from '@/types/user'

export const getStatusColor = (status: UserItem['status']): string => {
  const colors: Record<(UserItem | QuestionItem)['status'], string> = {
    'Hoạt động': 'success',
    'Ngừng hoạt động': 'default',
    'Đang khoá': 'error',
    'Chờ duyệt': 'warning'
  }
  return colors[status] || 'processing'
}
