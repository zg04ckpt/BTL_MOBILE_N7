using Feature.Matchs.Enums;
using Feature.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feature.Matchs.Entities
{
    public class UserMatchResultHistory
    {
        public int UserId { get; set; }
        public int MatchId { get; set; }
        public decimal Duration { get; set; }
        public int Progress { get; set; }
        public int Correct { get; set; }
        public int Score { get; set; }
        public UserInMatchStatus Status { get; set; }
        public int ExpScoreGained { get; set; } // Điểm cộng thêm kinh nghiệm
        public int RankScoreGained { get; set; } // Điểm cộng thêm vào điểm xếp hạng

        public User User { get; set; }
        public Match Match { get; set; }
    }

    public class UserMatchResultHistoryConfig : IEntityTypeConfiguration<UserMatchResultHistory>
    {
        public void Configure(EntityTypeBuilder<UserMatchResultHistory> builder)
        {
            builder.ToTable("UserMatchResultHistories");
            builder.HasKey(x => new { x.UserId, x.MatchId });
            builder.Property(x => x.Duration).HasColumnType("decimal(10,3)").IsRequired();
            builder.Property(x => x.Progress).IsRequired();
            builder.Property(x => x.Correct).IsRequired();
            builder.Property(x => x.Score).IsRequired();
            builder.Property(x => x.Status).HasConversion<string>();

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Match)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.MatchId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
