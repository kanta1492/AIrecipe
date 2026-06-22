using System.ComponentModel.DataAnnotations;

namespace AiMealPlanner.Data;

public sealed class MealPlan
{
    public int Id { get; set; }

    [Required]
    [StringLength(450)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [StringLength(80)]
    public string Title { get; set; } = string.Empty;

    [StringLength(24)]
    public string MealType { get; set; } = "夕食";

    public DateOnly MealDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [StringLength(400)]
    public string Ingredients { get; set; } = string.Empty;

    [StringLength(800)]
    public string Steps { get; set; } = string.Empty;

    [StringLength(400)]
    public string Reason { get; set; } = string.Empty;

    [StringLength(200)]
    public string NutritionNote { get; set; } = string.Empty;

    public bool IsFavorite { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
