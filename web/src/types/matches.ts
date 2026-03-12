export interface MatchItem {
  id: number
  startTime: string
  endTime: string | null
  type: 'Solo' | 'MVP'
  numberOfPlayers: number
  status: 'Đang diễn ra' | 'Kết thúc'
  topic: string
}
