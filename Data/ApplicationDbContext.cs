using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AiMealPlanner.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<FridgeItem> FridgeItems => Set<FridgeItem>();

    public DbSet<IngredientPhoto> IngredientPhotos => Set<IngredientPhoto>();

    public DbSet<MealPlan> MealPlans => Set<MealPlan>();

    public DbSet<AppNotification> AppNotifications => Set<AppNotification>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<FridgeItem>(entity =>
        {
            entity.HasIndex(item => new { item.UserId, item.ExpirationDate });
            entity.Property(item => item.Quantity).HasPrecision(8, 2);
        });

        builder.Entity<IngredientPhoto>()
            .HasIndex(photo => new { photo.UserId, photo.CreatedAt });

        builder.Entity<MealPlan>()
            .HasIndex(plan => new { plan.UserId, plan.MealDate });

        builder.Entity<AppNotification>()
            .HasIndex(notification => new { notification.UserId, notification.ReadAt, notification.CreatedAt });
    }
}
