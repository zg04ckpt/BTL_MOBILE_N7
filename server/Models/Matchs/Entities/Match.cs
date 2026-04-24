using Core.Interfaces;
using Models.Matchs.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Matchs.Enums;
using Models.Quizzes.Entities;

namespace Models.Matchs.Entities
{
    public class Match : IEntity
    {
        public int Id { get; set; }
        public int MaxSecondPerQuestion { get; set; }
        public int ScorePerCorrectAnswer { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime EndedAt { get; set; }
        public int? TopicId { get; set; }
        public BattleType BattleType { get; set; }
        public MatchStatus Status { get; set; }
        public MatchContentType ContentType { get; set; }
        public int NumberOfPlayers { get; set; }

        public Topic? Topic { get; set; }
        public List<QuestionInMatch> Questions { get; set; }
        public List<UserMatchResultHistory> Users { get; set; }
    }

    public class MatchConfig : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            builder.ToTable("Matchs");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.MaxSecondPerQuestion).IsRequired();
            builder.Property(x => x.ScorePerCorrectAnswer).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.EndedAt).IsRequired();
            builder.Property(x => x.BattleType).HasConversion<string>();
            builder.Property(x => x.ContentType).HasConversion<string>();
            builder.Property(x => x.Status).HasConversion<string>();
            builder.Property(x => x.NumberOfPlayers).IsRequired();

            builder.HasOne(x => x.Topic)
                .WithMany()
                .HasForeignKey(x => x.TopicId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
