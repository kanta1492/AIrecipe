using System.ComponentModel.DataAnnotations;

namespace AiMealPlanner.Data;

public sealed class IngredientPhoto
{
    public int Id { get; set; }

    [Required]
    [StringLength(450)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [StringLength(240)]
    public string StoredPath { get; set; } = string.Empty;

    [Required]
    [StringLength(80)]
    public string ContentType { get; set; } = "image/jpeg";

    [StringLength(120)]
    public string OriginalFileName { get; set; } = string.Empty;

    [StringLength(500)]
    public string Note { get; set; } = string.Empty;

    [StringLength(500)]
    public string ConfirmedIngredients { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
