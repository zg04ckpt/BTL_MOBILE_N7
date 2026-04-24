using Feature.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Models.Events.Entities
{
    public class ClaimedReward
    {
        public int UserId { get; set; }
        public int RewardId { get; set; }
        public int Value { get; set; }
        public User User { get; set; }
        public EventReward Reward { get; set; }
    }

    public class ClaimedRewardConfig : IEntityTypeConfiguration<ClaimedReward>
    {
        public void Configure(EntityTypeBuilder<ClaimedReward> builder)
        {
            builder.ToTable("ClaimedRewards");
            builder.HasKey(x => new { x.UserId, x.RewardId });
            builder.Property(x => x.Value).IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.ClaimedRewards)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Reward)
                .WithMany(x => x.ClaimedRewards)
                .HasForeignKey(x => x.RewardId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
