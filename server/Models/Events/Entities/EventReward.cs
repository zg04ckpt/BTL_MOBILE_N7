using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Events.Enums;

namespace Models.Events.Entities
{
    public class EventReward : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public EventRewardType Type { get; set; }
        public string Desc { get; set; }
        public string Unit { get; set; }

        public List<ClaimedReward> ClaimedRewards { get; set; }
    }

    public class EventRewardConfig : IEntityTypeConfiguration<EventReward>
    {
        public void Configure(EntityTypeBuilder<EventReward> builder)
        {
            builder.ToTable("EventRewards");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Unit).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Type).HasConversion<string>();
            builder.Property(x => x.Desc).HasMaxLength(1000);
        }
    }
}
