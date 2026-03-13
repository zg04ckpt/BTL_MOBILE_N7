using CNLib.Services.Logs;
using Core.Base;
using Core.Exceptions;
using Core.Interfaces;
using Core.Utilities;
using Feature.Quizzes.Interfaces;
using Models.Quizzes.DTOs;
using Models.Quizzes.Entities;
using Models.Quizzes.Enums;
using Models.Quizzes.Requests;
using System.Linq.Expressions;
using System.Text.Json;

namespace Feature.Quizzes.Services
{
    public class QuestionService
        : CrudWithPagingService<Question, CreateQuestionRequest, UpdateQuestionRequest, QuestionListItemDto, QuestionDetailDto, SearchQuestionRequest>, IQuestionService
    {
        private readonly IStorageService _storageService;
        private readonly ILogService<QuestionService> _logService;

        public QuestionService(IUnitOfWork uow, IStorageService storageService, ILogService<QuestionService> logService) : base(uow)
        {
            _storageService = storageService;
            _logService = logService;
        }

        protected override async Task ConfirmValidCreateDataAsync(CreateQuestionRequest request)
        {
            var topicRepository = _uow.Repository<Topic>();
            
            if (!await topicRepository.ExistsAsync(t => t.Id == request.TopicId))
            {
                _logService.LogError($"Create question failed: Topic #{request.TopicId} not found");
                throw new BadRequestException("Topic does not exist");
            }

            ValidateAnswers(request.Type, request.CorrectAnswers, request.StringAnswers);
        }

        protected override async Task ConfirmValidUpdateDataAsync(Question entity, UpdateQuestionRequest request)
        {
            var topicRepository = _uow.Repository<Topic>();
            
            if (!await topicRepository.ExistsAsync(t => t.Id == request.TopicId))
            {
                _logService.LogError($"Update question failed: Topic #{request.TopicId} not found");
                throw new BadRequestException("Topic does not exist");
            }

            ValidateAnswers(request.Type, request.CorrectAnswers, request.StringAnswers);
        }

        protected override Expression<Func<Question, object>> GetOrderBy(string? orderBy)
        {
            return orderBy?.ToLower() switch
            {
                "createdat" => q => q.CreatedAt,
                "level" => q => q.Level,
                "type" => q => q.Type,
                "status" => q => q.Status,
                _ => q => q.Id
            };
        }

        protected override Expression<Func<Question, bool>> GetPagingFilter(SearchQuestionRequest request)
        {
            return q =>
                (string.IsNullOrEmpty(request.StringContent) || q.StringContent.Contains(request.StringContent)) &&
                (!request.TopicId.HasValue || q.TopicId == request.TopicId.Value) &&
                (string.IsNullOrEmpty(request.Type) || q.Type.ToString() == request.Type) &&
                (string.IsNullOrEmpty(request.Level) || q.Level.ToString() == request.Level) &&
                (string.IsNullOrEmpty(request.Status) || q.Status.ToString() == request.Status);
        }

        protected override async Task<Question> MapFromCreateToEntityAsync(CreateQuestionRequest request)
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

        protected override QuestionDetailDto MapFromEntityToDetail(Question entity)
        {
            var answers = JsonSerializer.Deserialize<AnswersDto>(entity.AnswerJsonData);

            return new QuestionDetailDto
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
                TopicName = entity.Topic?.Name ?? string.Empty,
                CreatedAt = entity.CreatedAt,
                CorrectAnswers = answers?.CorrectAnswers ?? new List<string>(),
                StringAnswers = answers?.StringAnswers ?? new List<string>()
            };
        }

        protected override QuestionListItemDto MapFromEntityToListItem(Question entity)
        {
            return new QuestionListItemDto
            {
                Id = entity.Id,
                Slug = entity.Slug,
                StringContent = entity.StringContent,
                Type = entity.Type,
                Level = entity.Level,
                Status = entity.Status,
                TopicName = entity.Topic?.Name ?? string.Empty,
                CreatedAt = entity.CreatedAt
            };
        }

        protected override async Task UpdateEntityAsync(Question entity, UpdateQuestionRequest request)
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
                        _logService.LogError($"Validate failed: TrueFalse needs 2 answers, got {stringAnswers.Count}");
                        throw new BadRequestException("True/False question must have exactly 2 answers");
                    }
                    if (correctAnswers.Count != 1)
                    {
                        _logService.LogError($"Validate failed: TrueFalse needs 1 correct, got {correctAnswers.Count}");
                        throw new BadRequestException("True/False question must have exactly 1 correct answer");
                    }
                    break;

                case QuestionType.SingleChoice:
                    if (stringAnswers.Count < 2)
                    {
                        _logService.LogError($"Validate failed: SingleChoice needs 2+ answers, got {stringAnswers.Count}");
                        throw new BadRequestException("Single Choice question must have at least 2 answers");
                    }
                    if (correctAnswers.Count != 1)
                    {
                        _logService.LogError($"Validate failed: SingleChoice needs 1 correct, got {correctAnswers.Count}");
                        throw new BadRequestException("Single Choice question must have exactly 1 correct answer");
                    }
                    break;

                case QuestionType.MultipleChoice:
                    if (stringAnswers.Count < 2)
                    {
                        _logService.LogError($"Validate failed: MultipleChoice needs 2+ answers, got {stringAnswers.Count}");
                        throw new BadRequestException("Multiple Choice question must have at least 2 answers");
                    }
                    if (correctAnswers.Count < 1)
                    {
                        _logService.LogError($"Validate failed: MultipleChoice needs 1+ correct, got {correctAnswers.Count}");
                        throw new BadRequestException("Multiple Choice question must have at least 1 correct answer");
                    }
                    if (correctAnswers.Count >= stringAnswers.Count)
                    {
                        _logService.LogError($"Validate failed: Correct ({correctAnswers.Count}) >= Total ({stringAnswers.Count})");
                        throw new BadRequestException("Number of correct answers must be less than total answers");
                    }
                    break;
            }

            foreach (var correctAnswer in correctAnswers)
            {
                if (!stringAnswers.Contains(correctAnswer))
                {
                    _logService.LogError($"Validate failed: Correct answer '{correctAnswer}' not in list");
                    throw new BadRequestException($"Correct answer '{correctAnswer}' is not in the answer list");
                }
            }
        }
    }
}
