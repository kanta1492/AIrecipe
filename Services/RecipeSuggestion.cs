namespace AiMealPlanner.Services;

public sealed record RecipeSuggestion(
    string Title,
    string MealType,
    IReadOnlyList<string> MatchedIngredients,
    IReadOnlyList<string> MissingIngredients,
    string Steps,
    string Reason,
    string NutritionNote,
    int Confidence);
