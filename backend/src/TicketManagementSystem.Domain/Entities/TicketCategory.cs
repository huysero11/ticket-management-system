using TicketManagementSystem.Domain.Common;

namespace TicketManagementSystem.Domain.Entities;

public sealed class TicketCategory : AuditableEntity
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }

    private TicketCategory()
    {
    }

    private TicketCategory(string name, string? description)
    {
        Name = name;
        Description = description;
        IsActive = true;
    }

    public static TicketCategory Create(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Category name is required.");
        }

        var normalizedName = name.Trim();

        if (normalizedName.Length > 100)
        {
            throw new ArgumentException("Category name must not exceed 100 characters.");
        }

        string? normalizedDescription = null;

        if (!string.IsNullOrWhiteSpace(description))
        {
            normalizedDescription = description.Trim();

            if (normalizedDescription.Length > 255)
            {
                throw new ArgumentException("Description must not exceed 255 characters.");
            }
        }

        return new TicketCategory(normalizedName, normalizedDescription);
    }

    public void Update(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Category name is required.");
        }

        var normalizedName = name.Trim();

        if (normalizedName.Length > 100)
        {
            throw new ArgumentException("Category name must not exceed 100 characters.");
        }

        string? normalizedDescription = null;

        if (!string.IsNullOrWhiteSpace(description))
        {
            normalizedDescription = description.Trim();

            if (normalizedDescription.Length > 255)
            {
                throw new ArgumentException("Description must not exceed 255 characters.");
            }
        }

        Name = normalizedName;
        Description = normalizedDescription;
        SetUpdated();
    }

    public void Deactivate()
    {
        if (!IsActive)
        {
            return;
        }

        IsActive = false;
        SetUpdated();
    }
}