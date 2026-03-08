using CNLib.Services.Logs;
using Core.Base;
using Core.Exceptions;
using Core.Interfaces;
using Core.Utilities;
using Feature.Quizzes.Entities;
using Feature.Quizzes.Interfaces;
using Feature.Quizzes.Models;
using System.Linq.Expressions;

namespace Feature.Quizzes.Services
{
    public class TopicService
        : CrudWithPagingService<Topic, CreateTopicRequest, UpdateTopicRequest, TopicListItemDto, TopicDetailDto, SearchTopicRequest>, ITopicService
    {
        private readonly ILogService<TopicService> _logService;

        public TopicService(IUnitOfWork uow, ILogService<TopicService> logService) : base(uow)
        {
            _logService = logService;
        }

        protected override async Task ConfirmValidCreateDataAsync(CreateTopicRequest request)
        {
            var topicRepository = _uow.Repository<Topic>();

            if (await topicRepository.ExistsAsync(t => t.Name == request.Name))
            {
                _logService.LogError($"Create topic failed: Name exists - {request.Name}");
                throw new BadRequestException("Topic name already exists");
            }
        }

        protected override async Task ConfirmValidUpdateDataAsync(Topic entity, UpdateTopicRequest request)
        {
            var topicRepository = _uow.Repository<Topic>();

            if (entity.Name != request.Name 
                && await topicRepository.ExistsAsync(t => t.Id != entity.Id && t.Name == request.Name))
            {
                _logService.LogError($"Update topic failed: Name exists - {request.Name}");
                throw new BadRequestException("Topic name already exists");
            }
        }

        protected override Expression<Func<Topic, object>> GetOrderBy(string? orderBy)
        {
            return orderBy?.ToLower() switch
            {
                "name" => t => t.Name,
                _ => t => t.Id
            };
        }

        protected override Expression<Func<Topic, bool>> GetPagingFilter(SearchTopicRequest request)
        {
            return t => string.IsNullOrEmpty(request.Name) || t.Name.Contains(request.Name);
        }

        protected override Task<Topic> MapFromCreateToEntityAsync(CreateTopicRequest request)
        {
            var topic = new Topic
            {
                Name = request.Name,
                Slug = StringUtil.ToSlug(request.Name)
            };
            
            return Task.FromResult(topic);
        }

        protected override TopicDetailDto MapFromEntityToDetail(Topic entity)
        {
            return new TopicDetailDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Slug = entity.Slug,
                QuestionCount = entity.Questions?.Count ?? 0
            };
        }

        protected override TopicListItemDto MapFromEntityToListItem(Topic entity)
        {
            return new TopicListItemDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Slug = entity.Slug,
                QuestionCount = entity.Questions?.Count ?? 0
            };
        }

        protected override Task UpdateEntityAsync(Topic entity, UpdateTopicRequest request)
        {
            entity.Name = request.Name;
            entity.Slug = StringUtil.ToSlug(request.Name);
            
            return Task.CompletedTask;
        }
    }
}
