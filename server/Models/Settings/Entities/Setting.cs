using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Settings.Enums;

namespace Models.Settings.Entities
{
    public class Setting : IEntity
    {
        public int Id { get; set; }
        public ConfigurationKey Key { get; set; }
        public string Value { get; set; }
        public string? Description { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class SystemConfigurationConfig : IEntityTypeConfiguration<Setting>
    {
        public void Configure(EntityTypeBuilder<Setting> builder)
        {
            builder.ToTable("Settings");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Key).HasConversion<string>().IsRequired();
            builder.Property(x => x.Value).HasMaxLength(1000).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.UpdatedAt).IsRequired();
            
            builder.HasIndex(x => x.Key).IsUnique();
        }
    }
}
