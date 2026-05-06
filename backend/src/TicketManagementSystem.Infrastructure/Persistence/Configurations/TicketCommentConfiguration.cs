using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketManagementSystem.Domain.Entities;

namespace TicketManagementSystem.Infrastructure.Persistence.Configurations;

public sealed class TicketCommentConfiguration : IEntityTypeConfiguration<TicketComment>
{
    public void Configure(EntityTypeBuilder<TicketComment> builder)
    {
        builder.ToTable("TicketComments");

        builder.HasKey(tc => tc.Id);

        builder.Property(tc => tc.Message)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(tc => tc.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(tc => tc.DeletedAtUtc);

        builder.Property(tc => tc.DeletedByUserId);

        builder.Property(tc => tc.CreatedAtUtc)
            .IsRequired();

        builder.Property(tc => tc.UpdatedAtUtc);

        builder.HasOne(tc => tc.Ticket)
            .WithMany()
            .HasForeignKey(tc => tc.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(tc => tc.User)
            .WithMany()
            .HasForeignKey(tc => tc.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(tc => tc.TicketId);

        builder.HasIndex(tc => new
        {
            tc.TicketId,
            tc.CreatedAtUtc
        });

        builder.HasIndex(tc => tc.IsDeleted);
    }
}