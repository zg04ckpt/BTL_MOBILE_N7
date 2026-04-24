using Core.Base;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Core.Utilities;
using Feature.Quizzes.Interfaces;
using Models.Quizzes.DTOs;
using Models.Quizzes.Entities;
using Models.Quizzes.Requests;
using System.Linq.Expressions;

namespace Feature.Quizzes.Services
{
    public class TopicService
        : BaseService, ITopicService
    {
        public TopicService(IUnitOfWork uow) : base(uow)
        {
        }

        public async Task<IEnumerable<TopicListItemDto>> GetAllAsync()
        {
            var entities = await _uow.Repository<Topic>().GetAllAsync(
                predicate: t => true,
                includes: t => t.Questions);

            return entities.Select(MapToListItemDto);
        }

        public async Task<TopicDetailDto?> GetByIdAsync(int id)
        {
            var entity = await _uow.Repository<Topic>().GetFirstAsync(
                predicate: e => e.Id == id,
                includes: e => e.Questions)
                ?? throw new NotFoundException("Topic not found");

            return MapToDetailDto(entity);
        }

        public async Task<Paginated<TopicListItemDto>> GetPagingAsync(SearchTopicRequest request)
        {
            var entities = await _uow.Repository<Topic>().GetAllAsync(
                predicate: BuildFilter(request),
                pageIndex: request.PageIndex,
                pageSize: request.PageSize,
                orderBy: request.OrderBy ?? nameof(Topic.Name),
                asc: request.IsAsc,
                includes: e => e.Questions);

            var totalItems = await _uow.Repository<Topic>().CountAsync(BuildFilter(request));

            return new Paginated<TopicListItemDto>
            {
                Items = entities.Select(MapToListItemDto),
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalItems = totalItems
            };
        }

        public async Task<ChangedResponse> CreateAsync(CreateTopicRequest request)
        {
            await ConfirmValidCreateDataAsync(request);

            var topic = new Topic
            {
                Name = request.Name,
                Slug = StringUtil.ToSlug(request.Name)
            };

            await _uow.Repository<Topic>().AddAsync(topic);
            await _uow.SaveChangesAsync();

            return ChangedResponse.FromEntity(topic);
        }

        public async Task<ChangedResponse> UpdateAsync(int id, UpdateTopicRequest request)
        {
            var entity = await GetEntityAsync<Topic>(id);
            await ConfirmValidUpdateDataAsync(entity, request);

            entity.Name = request.Name;
            entity.Slug = StringUtil.ToSlug(request.Name);

            await _uow.Repository<Topic>().UpdateAsync(entity);
            await _uow.SaveChangesAsync();

            return ChangedResponse.FromEntity(entity);
        }

        public async Task<ChangedResponse> DeleteAsync(int id)
        {
            if (await _uow.Repository<Question>().ExistsAsync(e => e.TopicId == id))
            {
                throw new ForbiddenException("Topic is already in use");
            }

            var entity = await GetEntityAsync<Topic>(id);
            await _uow.Repository<Topic>().DeleteAsync(entity);
            await _uow.SaveChangesAsync();

            return ChangedResponse.FromEntity(entity);
        }

        private async Task ConfirmValidCreateDataAsync(CreateTopicRequest request)
        {
            var topicRepository = _uow.Repository<Topic>();

            if (await topicRepository.ExistsAsync(t => t.Name == request.Name))
            {
                throw new BadRequestException("Topic name already exists");
            }
        }

        private async Task ConfirmValidUpdateDataAsync(Topic entity, UpdateTopicRequest request)
        {
            var topicRepository = _uow.Repository<Topic>();

            if (entity.Name != request.Name 
                && await topicRepository.ExistsAsync(t => t.Id != entity.Id && t.Name == request.Name))
            {
                throw new BadRequestException("Topic name already exists");
            }
        }

        private static Expression<Func<Topic, bool>> BuildFilter(SearchTopicRequest request)
        {
            return t => string.IsNullOrEmpty(request.Name) || t.Name.Contains(request.Name);
        }

        private static TopicListItemDto MapToListItemDto(Topic entity)
        {
            return new TopicListItemDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Slug = entity.Slug,
                QuestionCount = entity.Questions.Count
            };
        }

        private static TopicDetailDto MapToDetailDto(Topic entity)
        {
            return new TopicDetailDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Slug = entity.Slug,
                QuestionCount = entity.Questions.Count
            };
        }
    }
}
