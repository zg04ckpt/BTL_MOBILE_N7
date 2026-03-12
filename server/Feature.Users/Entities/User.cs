using Core.Interfaces;
using Feature.Users.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feature.Users.Entities
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string AvatarUrl { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public AccountStatus Status { get; set; }
        public int Level { get; set; }
        public int Rank { get; set; }
        public int RankScore { get; set; }
        public int Exp { get; set; }
        public DateTime CreatedAt { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }

    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(50);
            builder.Property(x => x.AvatarUrl).HasMaxLength(255);
            builder.Property(x => x.PhoneNumber).HasMaxLength(15);
            builder.Property(x => x.PasswordHash).HasMaxLength(255);
            builder.Property(x => x.Status).HasConversion<string>();
            builder.Property(x => x.PhoneNumber).HasMaxLength(15);
            builder.HasOne(x => x.Role)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
