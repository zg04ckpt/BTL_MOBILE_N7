using Core.Interfaces;
using Feature.Quizzes.Entities;
using Feature.Quizzes.Models;
using Feature.Quizzes.Models.Requests;

namespace Feature.Quizzes.Interfaces
{
    public interface IQuestionService
        : ICrudWithPagingService<Question, CreateQuestionRequest, UpdateQuestionRequest, QuestionListItemDto, QuestionDetailDto, SearchQuestionRequest>
    {
    }
}
