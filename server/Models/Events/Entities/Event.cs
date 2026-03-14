using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Events.Enums;

namespace Models.Events.Entities
{
    public class Event : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public EventTimeType EventTimeType { get; set; }
        public EventType EventType { get; set; }
        public string EventConfigJsonData { get; set; }
        public string Desc { get; set; }
        public bool IsLocked { get; set; }

        public List<UserInEvent> UserInEvents { get; set; }
    }

    public class EventConfig : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("Events");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.StartTime).IsRequired();
            builder.Property(x => x.EventTimeType).HasConversion<string>();
            builder.Property(x => x.EventType).HasConversion<string>();
            builder.Property(x => x.EventConfigJsonData).HasColumnType("text").IsRequired();
            builder.Property(x => x.Desc).HasMaxLength(1000);
        }
    }
}
