using System.ComponentModel.DataAnnotations;

namespace AiMealPlanner.Data;

public sealed class FridgeItem
{
    public int Id { get; set; }

    [Required]
    [StringLength(450)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [StringLength(40)]
    public string Name { get; set; } = string.Empty;

    [StringLength(24)]
    public string Category { get; set; } = "その他";

    [Range(0, 999)]
    public decimal Quantity { get; set; } = 1;

    [StringLength(16)]
    public string Unit { get; set; } = "個";

    public DateOnly? ExpirationDate { get; set; }

    [StringLength(120)]
    public string Memo { get; set; } = string.Empty;

    [StringLength(16)]
    public string Source { get; set; } = "manual";

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}
