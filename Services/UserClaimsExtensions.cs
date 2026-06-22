using System.Security.Claims;

namespace AiMealPlanner.Services;

public static class UserClaimsExtensions
{
    public static string GetRequiredUserId(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("ログインユーザーを確認できませんでした。");
    }
}
