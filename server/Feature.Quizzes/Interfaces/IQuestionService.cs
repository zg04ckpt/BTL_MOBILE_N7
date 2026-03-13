using Core.Interfaces;
using Models.Quizzes.DTOs;
using Models.Quizzes.Entities;
using Models.Quizzes.Requests;

namespace Feature.Quizzes.Interfaces
{
    public interface IQuestionService
        : ICrudWithPagingService<Question, CreateQuestionRequest, UpdateQuestionRequest, QuestionListItemDto, QuestionDetailDto, SearchQuestionRequest>
    {
    }
}
