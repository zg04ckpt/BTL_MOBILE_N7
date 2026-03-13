using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Models.Settings.Entities
{
    public class SystemConfiguration : IEntity
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string? Description { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class SystemConfigurationConfig : IEntityTypeConfiguration<SystemConfiguration>
    {
        public void Configure(EntityTypeBuilder<SystemConfiguration> builder)
        {
            builder.ToTable("SystemConfigurations");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Key).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Value).HasMaxLength(1000).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.UpdatedAt).IsRequired();
            
            builder.HasIndex(x => x.Key).IsUnique();
        }
    }
}
