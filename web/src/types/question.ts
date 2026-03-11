export interface QuestionItem {
  id: number
  content: string
  topic: string
  level: string
  answers: AnswerItem[]
  status: string
}

export interface AnswerItem {
  id: number
  content: string
  isCorrect: boolean
}
