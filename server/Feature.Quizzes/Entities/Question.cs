using Core.Interfaces;
using Feature.Quizzes.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feature.Quizzes.Entities
{
    public class Question : IEntity
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public string StringContent { get; set; }
        public string ImageUrl { get; set; }
        public string AudioUrl { get; set; }
        public string VideoUrl { get; set; }
        public string AnswerJsonData { get; set; }
        public QuestionType Type { get; set; }
        public QuestionLevel Level { get; set; }
        public QuestionStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TopicId { get; set; }
        public Topic Topic { get; set; }
    }

    public class QuestionConfig : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Questions");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Slug).HasMaxLength(255).IsRequired();
            builder.Property(x => x.StringContent).HasMaxLength(1000).IsRequired();
            builder.Property(x => x.ImageUrl).HasMaxLength(500);
            builder.Property(x => x.AudioUrl).HasMaxLength(500);
            builder.Property(x => x.VideoUrl).HasMaxLength(500);
            builder.Property(x => x.AnswerJsonData).HasColumnType("text").IsRequired();
            builder.Property(x => x.Type).HasConversion<string>();
            builder.Property(x => x.Level).HasConversion<string>();
            builder.Property(x => x.Status).HasConversion<string>();
            builder.Property(x => x.CreatedAt).IsRequired();
            
            builder.HasIndex(x => x.Slug).IsUnique();
            
            builder.HasOne(x => x.Topic)
                .WithMany(x => x.Questions)
                .HasForeignKey(x => x.TopicId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

