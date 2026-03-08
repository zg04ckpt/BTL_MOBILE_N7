using Feature.Quizzes.Interfaces;
using Feature.Quizzes.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Feature.Quizzes
{
    public static class DI
    {
        public static IServiceCollection AddQuizzesFeature(this IServiceCollection services)
        {
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<ITopicService, TopicService>();
            
            return services;
        }
    }
}

