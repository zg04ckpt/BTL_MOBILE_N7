using Core.Base;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Core.Utilities;
using Feature.Quizzes.Interfaces;
using LinqKit;
using Models.Quizzes.DTOs;
using Models.Quizzes.Entities;
using Models.Quizzes.Enums;
using Models.Quizzes.Requests;
using System.Linq.Expressions;
using System.Text.Json;
using static Grpc.Core.Metadata;

namespace Feature.Quizzes.Services
{
    public class QuestionService
        : BaseService, IQuestionService, ICrudWithPagingService<Question, CreateQuestionRequest, UpdateQuestionRequest, QuestionListItemDto, QuestionDetailDto, SearchQuestionRequest>
    {
        private readonly IStorageService _storageService;

        public QuestionService(IUnitOfWork uow, IStorageService storageService) : base(uow)
        {
            _storageService = storageService;
        }

        public async Task<IEnumerable<QuestionListItemDto>> GetAllAsync()
        {
            var entities = await _uow.Repository<Question>().GetAllAsync(
                predicate: q => true,
                selector: entity => new QuestionListItemDto
                {
                    Id = entity.Id,
                    Slug = entity.Slug,
                    StringContent = entity.StringContent,
                    Type = entity.Type,
                    Level = entity.Level,
                    Status = entity.Status,
                    TopicName = entity.Topic.Name,
                    CreatedAt = entity.CreatedAt
                });

            return entities;
        }

        public async Task<QuestionDetailDto?> GetByIdAsync(int id)
        {
            var question = await _uow.Repository<Question>().GetFirstAsync(
                predicate: q => q.Id == id,
                selector: entity => new
                {
                    Data = new QuestionDetailDto
                    {
                        Id = entity.Id,
                        Slug = entity.Slug,
                        StringContent = entity.StringContent,
                        ImageUrl = entity.ImageUrl,
                        AudioUrl = entity.AudioUrl,
                        VideoUrl = entity.VideoUrl,
                        Type = entity.Type,
                        Level = entity.Level,
                        Status = entity.Status,
                        TopicId = entity.TopicId,
                        TopicName = entity.Topic.Name,
                        CreatedAt = entity.CreatedAt,
                    },
                    entity.AnswerJsonData,
                })
                ?? throw new NotFoundException("Question not found");

            var answers = JsonSerializer.Deserialize<AnswersDto>(question.AnswerJsonData)!;
            question.Data.CorrectAnswers = answers.CorrectAnswers;
            question.Data.StringAnswers = answers.StringAnswers;

            return question.Data;
        }

        public async Task<Paginated<QuestionListItemDto>> GetPagingAsync(SearchQuestionRequest request)
        {
            var filter = PredicateBuilder.New<Question>();
            if (!string.IsNullOrEmpty(request.StringContent))
            {
                filter = filter.And(e => e.StringContent.Contains(request.StringContent));
            }
            if (request.TopicId != null)
            {
                filter = filter.And(e => e.TopicId == request.TopicId);
            }
            if (request.Type != null)
            {
                filter = filter.And(e => e.Type == request.Type);
            }
            if (request.Level != null)
            {
                filter = filter.And(e => e.Level == request.Level);
            }
            if (request.Status != null)
            {
                filter = filter.And(e => e.Status == request.Status);
            }

            var entities = await _uow.Repository<Question>().GetPagingAsync(
                predicate: filter,
                pageIndex: request.PageIndex,
                pageSize: request.PageSize,
                orderBy: request.OrderBy ?? nameof(Question.CreatedAt),
                asc: request.IsAsc,
                selector: entity => new QuestionListItemDto
                {
                    Id = entity.Id,
                    Slug = entity.Slug,
                    StringContent = entity.StringContent,
                    Type = entity.Type,
                    Level = entity.Level,
                    Status = entity.Status,
                    TopicName = entity.Topic.Name,
                    CreatedAt = entity.CreatedAt
                });

            return entities;
        }

        public async Task<ChangedResponse> CreateAsync(CreateQuestionRequest request)
        {
            await ConfirmValidCreateDataAsync(request);

            var entity = await MapFromCreateRequestAsync(request);
            await _uow.Repository<Question>().AddAsync(entity);
            await _uow.SaveChangesAsync();

            return ChangedResponse.FromEntity(entity);
        }

        public async Task<ChangedResponse> UpdateAsync(int id, UpdateQuestionRequest request)
        {
            var entity = await GetEntityAsync<Question>(id);
            await ConfirmValidUpdateDataAsync(request);

            await UpdateEntityAsync(entity, request);
            await _uow.Repository<Question>().UpdateAsync(entity);
            await _uow.SaveChangesAsync();

            return ChangedResponse.FromEntity(entity);
        }

        public async Task<ChangedResponse> DeleteAsync(int id)
        {
            var entity = await GetEntityAsync<Question>(id);
            await _uow.Repository<Question>().DeleteAsync(entity);
            await _uow.SaveChangesAsync();

            return ChangedResponse.FromEntity(entity);
        }

        private async Task ConfirmValidCreateDataAsync(CreateQuestionRequest request)
        {            
            if (!await _uow.Repository<Topic>().ExistsAsync(t => t.Id == request.TopicId))
            {
                throw new BadRequestException("Topic does not exist");
            }

            var slug = StringUtil.ToSlug(request.StringContent);
            if (await _uow.Repository<Question>().ExistsAsync(q => q.Slug == slug))
            {
                throw new BadRequestException("Question content is duplicated");
            }

            ValidateAnswers(request.Type, request.CorrectAnswers, request.StringAnswers);
        }

        private async Task ConfirmValidUpdateDataAsync(UpdateQuestionRequest request)
        {
            var topicRepository = _uow.Repository<Topic>();
            
            if (!await topicRepository.ExistsAsync(t => t.Id == request.TopicId))
            {
                throw new BadRequestException("Topic does not exist");
            }

            ValidateAnswers(request.Type, request.CorrectAnswers, request.StringAnswers);
        }

        private async Task<Question> MapFromCreateRequestAsync(CreateQuestionRequest request)
        {
            var answers = new AnswersDto
            {
                CorrectAnswers = request.CorrectAnswers,
                StringAnswers = request.StringAnswers
            };

            var imageUrl = request.Image != null ? await _storageService.SaveAsync(request.Image) : string.Empty;
            var audioUrl = request.Audio != null ? await _storageService.SaveAsync(request.Audio) : string.Empty;
            var videoUrl = request.Video != null ? await _storageService.SaveAsync(request.Video) : string.Empty;

            return new Question
            {
                Slug = StringUtil.ToSlug(request.StringContent),
                StringContent = request.StringContent,
                ImageUrl = imageUrl,
                AudioUrl = audioUrl,
                VideoUrl = videoUrl,
                AnswerJsonData = JsonSerializer.Serialize(answers),
                Type = request.Type,
                Level = request.Level,
                Status = QuestionStatus.Draft,
                CreatedAt = DateTime.UtcNow,
                TopicId = request.TopicId
            };
        }

        private async Task UpdateEntityAsync(Question entity, UpdateQuestionRequest request)
        {
            var answers = new AnswersDto
            {
                CorrectAnswers = request.CorrectAnswers,
                StringAnswers = request.StringAnswers
            };

            entity.Slug = StringUtil.ToSlug(request.StringContent);
            entity.StringContent = request.StringContent;
            entity.AnswerJsonData = JsonSerializer.Serialize(answers);
            entity.Type = request.Type;
            entity.Level = request.Level;
            entity.Status = request.Status;
            entity.TopicId = request.TopicId;

            if (request.Image != null)
            {
                if (!string.IsNullOrEmpty(entity.ImageUrl))
                {
                    await _storageService.DeleteAsync(entity.ImageUrl);
                }
                entity.ImageUrl = await _storageService.SaveAsync(request.Image);
            }

            if (request.Audio != null)
            {
                if (!string.IsNullOrEmpty(entity.AudioUrl))
                {
                    await _storageService.DeleteAsync(entity.AudioUrl);
                }
                entity.AudioUrl = await _storageService.SaveAsync(request.Audio);
            }

            if (request.Video != null)
            {
                if (!string.IsNullOrEmpty(entity.VideoUrl))
                {
                    await _storageService.DeleteAsync(entity.VideoUrl);
                }
                entity.VideoUrl = await _storageService.SaveAsync(request.Video);
            }
        }

        private void ValidateAnswers(QuestionType type, List<string> correctAnswers, List<string> stringAnswers)
        {
            switch (type)
            {
                case QuestionType.TrueFalse:
                    if (stringAnswers.Count != 2)
                    {
                        throw new BadRequestException("True/False question must have exactly 2 answers");
                    }
                    if (correctAnswers.Count != 1)
                    {
                        throw new BadRequestException("True/False question must have exactly 1 correct answer");
                    }
                    break;

                case QuestionType.SingleChoice:
                    if (stringAnswers.Count < 2)
                    {
                        throw new BadRequestException("Single Choice question must have at least 2 answers");
                    }
                    if (correctAnswers.Count != 1)
                    {
                        throw new BadRequestException("Single Choice question must have exactly 1 correct answer");
                    }
                    break;

                case QuestionType.MultipleChoice:
                    if (stringAnswers.Count < 2)
                    {
                        throw new BadRequestException("Multiple Choice question must have at least 2 answers");
                    }
                    if (correctAnswers.Count < 1)
                    {
                        throw new BadRequestException("Multiple Choice question must have at least 1 correct answer");
                    }
                    if (correctAnswers.Count >= stringAnswers.Count)
                    {
                        throw new BadRequestException("Number of correct answers must be less than total answers");
                    }
                    break;
            }

            foreach (var correctAnswer in correctAnswers)
            {
                if (!stringAnswers.Contains(correctAnswer))
                {
                    throw new BadRequestException($"Correct answer '{correctAnswer}' is not in the answer list");
                }
            }
        }
    }
}
