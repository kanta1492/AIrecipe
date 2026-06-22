using System.ComponentModel.DataAnnotations;

namespace AiMealPlanner.Data;

public sealed class AppNotification
{
    public int Id { get; set; }

    [Required]
    [StringLength(450)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [StringLength(80)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(240)]
    public string Message { get; set; } = string.Empty;

    [StringLength(24)]
    public string Kind { get; set; } = "info";

    [StringLength(120)]
    public string ActionUrl { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? ReadAt { get; set; }
}
