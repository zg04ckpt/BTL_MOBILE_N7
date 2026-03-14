using Feature.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Models.Events.Entities
{
    public class UserInEvent
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
        public DateTime LastChanged { get; set; }
        public string ProgressJsonData { get; set; }

        public User User { get; set; }
        public Event Event { get; set; }
    }

    public class UserInEventConfig : IEntityTypeConfiguration<UserInEvent>
    {
        public void Configure(EntityTypeBuilder<UserInEvent> builder)
        {
            builder.ToTable("UserInEvents");
            builder.HasKey(x => new { x.UserId, x.EventId });
            builder.Property(x => x.LastChanged).IsRequired();
            builder.Property(x => x.ProgressJsonData).HasColumnType("text").IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.EventsProgress)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Event)
                .WithMany(x => x.UserInEvents)
                .HasForeignKey(x => x.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
