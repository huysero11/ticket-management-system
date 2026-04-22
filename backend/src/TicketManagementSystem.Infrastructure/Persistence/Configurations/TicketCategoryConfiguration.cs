using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketManagementSystem.Domain.Entities;

namespace TickketManagementSystem.Infrastructure.Persistence.Configurations;

public sealed class TicketCategoryConfiguration : IEntityTypeConfiguration<TicketCategory>
{
    public void Configure(EntityTypeBuilder<TicketCategory> builder)
    {
        builder.ToTable("TicketCategories");

        builder.HasKey(tc => tc.Id);

        builder.Property(tc => tc.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(tc => tc.Description)
            .HasMaxLength(255);

        builder.Property(tc => tc.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(tc => tc.CreatedAtUtc)
            .IsRequired();

        builder.Property(tc => tc.UpdatedAtUtc)
            .IsRequired(false);

        builder.HasIndex(tc => tc.Name)
            .IsUnique();
    }
}