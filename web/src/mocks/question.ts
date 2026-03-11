import { QuestionItem } from './../types/question'

export const questionData: QuestionItem[] = [
  {
    id: 1,
    content: 'Thủ đô của Việt Nam là gì?',
    topic: 'Xã hội',
    level: 'Dễ',
    status: 'Hoạt động',
    answers: [
      { id: 101, content: 'Hà Nội', isCorrect: true },
      { id: 102, content: 'TP. Hồ Chí Minh', isCorrect: false },
      { id: 103, content: 'Đà Nẵng', isCorrect: false },
      { id: 104, content: 'Huế', isCorrect: false }
    ]
  },
  {
    id: 2,
    content: 'Phép toán 5 + 7 bằng bao nhiêu?',
    topic: 'Toán học',
    level: 'Dễ',
    status: 'Chờ duyệt',
    answers: [
      { id: 201, content: '10', isCorrect: false },
      { id: 202, content: '11', isCorrect: false },
      { id: 203, content: '12', isCorrect: true },
      { id: 204, content: '13', isCorrect: false }
    ]
  },
  {
    id: 3,
    content: 'Ngôn ngữ nào dưới đây là ngôn ngữ lập trình?',
    topic: 'Tin học',
    level: 'Trung bình',
    status: 'Hoạt động',
    answers: [
      { id: 301, content: 'HTML', isCorrect: false },
      { id: 302, content: 'CSS', isCorrect: false },
      { id: 303, content: 'TypeScript', isCorrect: true },
      { id: 304, content: 'JSON', isCorrect: false }
    ]
  },
  {
    id: 4,
    content: 'Chiến dịch Điện Biên Phủ kết thúc vào năm nào?',
    topic: 'Lịch sử',
    level: 'Khó',
    status: 'Hoạt động',
    answers: [
      { id: 401, content: '1945', isCorrect: false },
      { id: 402, content: '1954', isCorrect: true },
      { id: 403, content: '1975', isCorrect: false },
      { id: 404, content: '1930', isCorrect: false }
    ]
  },
  {
    id: 5,
    content: 'Đâu là một framework của JavaScript?',
    topic: 'Tin học',
    level: 'Dễ',
    status: 'Hoạt động',
    answers: [
      { id: 501, content: 'VueJS', isCorrect: true },
      { id: 502, content: 'Django', isCorrect: false },
      { id: 503, content: 'Laravel', isCorrect: false },
      { id: 504, content: 'Flutter', isCorrect: false }
    ]
  },
  {
    id: 6,
    content: 'Sông nào dài nhất thế giới?',
    topic: 'Xã hội',
    level: 'Trung bình',
    status: 'Hoạt động',
    answers: [
      { id: 601, content: 'Sông Mê Kông', isCorrect: false },
      { id: 602, content: 'Sông Amazon', isCorrect: false },
      { id: 603, content: 'Sông Nile', isCorrect: true },
      { id: 604, content: 'Sông Hồng', isCorrect: false }
    ]
  },
  {
    id: 7,
    content: 'Căn bậc hai của 144 là?',
    topic: 'Toán học',
    level: 'Dễ',
    status: 'Ngừng hoạt động',
    answers: [
      { id: 701, content: '10', isCorrect: false },
      { id: 702, content: '11', isCorrect: false },
      { id: 703, content: '12', isCorrect: true },
      { id: 704, content: '14', isCorrect: false }
    ]
  },
  {
    id: 8,
    content: 'Ai là người tìm ra châu Mỹ?',
    topic: 'Lịch sử',
    level: 'Trung bình',
    status: 'Hoạt động',
    answers: [
      { id: 801, content: 'Isaac Newton', isCorrect: false },
      { id: 802, content: 'Christopher Columbus', isCorrect: true },
      { id: 803, content: 'Albert Einstein', isCorrect: false },
      { id: 804, content: 'Marco Polo', isCorrect: false }
    ]
  },
  {
    id: 9,
    content: 'Giao thức truyền tải siêu văn bản là gì?',
    topic: 'Tin học',
    level: 'Khó',
    status: 'Chờ duyệt',
    answers: [
      { id: 901, content: 'FTP', isCorrect: false },
      { id: 902, content: 'SMTP', isCorrect: false },
      { id: 903, content: 'HTTP', isCorrect: true },
      { id: 904, content: 'TCP/IP', isCorrect: false }
    ]
  },
  {
    id: 10,
    content: 'Số nguyên tố nhỏ nhất là số nào?',
    topic: 'Toán học',
    level: 'Trung bình',
    status: 'Hoạt động',
    answers: [
      { id: 1001, content: '0', isCorrect: false },
      { id: 1002, content: '1', isCorrect: false },
      { id: 1003, content: '2', isCorrect: true },
      { id: 1004, content: '3', isCorrect: false }
    ]
  }
]
