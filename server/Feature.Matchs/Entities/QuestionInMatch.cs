using Feature.Quizzes.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feature.Matchs.Entities
{
    public class QuestionInMatch
    {
        public int MatchId { get; set; }
        public int QuestionId { get; set; }
        public double CorrectRate { get; set; }

        public Match Match { get; set; }
        public Question Question { get; set; }
    }

    public class QuestionInMatchConfig : IEntityTypeConfiguration<QuestionInMatch>
    {
        public void Configure(EntityTypeBuilder<QuestionInMatch> builder)
        {
            builder.ToTable("QuestionInMatchs");
            builder.HasKey(x => new { x.MatchId, x.QuestionId });
            builder.Property(x => x.CorrectRate).IsRequired();

            builder.HasOne(x => x.Match)
                .WithMany(x => x.Questions)
                .HasForeignKey(x => x.MatchId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Question)
                .WithMany()
                .HasForeignKey(x => x.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
