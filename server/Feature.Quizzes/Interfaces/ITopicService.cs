using Core.Interfaces;
using Models.Quizzes.DTOs;
using Models.Quizzes.Entities;
using Models.Quizzes.Requests;

namespace Feature.Quizzes.Interfaces
{
    public interface ITopicService
        : ICrudWithPagingService<Topic, CreateTopicRequest, UpdateTopicRequest, TopicListItemDto, TopicDetailDto, SearchTopicRequest>
    {
    }
}
