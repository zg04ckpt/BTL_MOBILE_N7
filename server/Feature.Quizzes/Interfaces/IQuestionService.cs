using Core.Interfaces;
using Core.Models;
using Models.Quizzes.DTOs;
using Models.Quizzes.Entities;
using Models.Quizzes.Requests;

namespace Feature.Quizzes.Interfaces
{
    public interface IQuestionService
        : ICrudWithPagingService<Question, CreateQuestionRequest, UpdateQuestionRequest, QuestionListItemDto, QuestionDetailDto, SearchQuestionRequest>
    {
        Task<IEnumerable<ChangedResponse>> CreateManyAsync(List<CreateQuestionRequest> requests);
        Task<IEnumerable<ChangedResponse>> DeleteManyAsync(List<int> ids);
    }
}
