using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Infrastructure.Options;

public sealed class DatabaseOptions
{
    public const string SectionName = "Database";

    [Required]
    public string ConnectionString { get; init; } = string.Empty;
}