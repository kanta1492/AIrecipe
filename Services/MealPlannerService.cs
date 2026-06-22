using AiMealPlanner.Data;
using Microsoft.EntityFrameworkCore;

namespace AiMealPlanner.Services;

public sealed class MealPlannerService(IDbContextFactory<ApplicationDbContext> dbFactory)
{
    private static readonly string[] KnownIngredients =
    [
        "鶏肉", "鶏むね", "鶏もも", "豚肉", "牛肉", "ひき肉", "鮭", "さば", "ツナ", "卵",
        "豆腐", "納豆", "牛乳", "チーズ", "ヨーグルト", "キャベツ", "レタス", "白菜", "玉ねぎ",
        "長ねぎ", "にんじん", "じゃがいも", "さつまいも", "大根", "トマト", "きゅうり", "なす",
        "ピーマン", "ブロッコリー", "ほうれん草", "小松菜", "きのこ", "しめじ", "えのき",
        "もやし", "かぼちゃ", "米", "うどん", "パスタ", "パン"
    ];

    private static readonly RecipeTemplate[] Recipes =
    [
        new(
            "鶏むねと野菜の照り焼き",
            "夕食",
            ["鶏むね", "鶏肉", "玉ねぎ", "にんじん", "ピーマン"],
            ["しょうゆ", "みりん"],
            "食材を一口大に切る。火の通りにくい野菜から炒める。鶏肉を加えて焼き、しょうゆとみりんで照りを出す。",
            "たんぱく質をしっかり取りつつ、期限が近い野菜をまとめて使えます。",
            "主菜に向いた高たんぱく献立です。野菜を足すと食物繊維も補えます。"),
        new(
            "豚こまとキャベツのみそ炒め",
            "夕食",
            ["豚肉", "キャベツ", "もやし", "にんじん"],
            ["みそ"],
            "豚肉を炒め、野菜を加えて強火でさっと火を通す。みそ、砂糖、しょうゆ少量で味をまとめる。",
            "かさのある葉物を使い切りやすく、短時間で作れます。",
            "炭水化物はご飯で調整しやすい、満足感のある主菜です。"),
        new(
            "鮭ときのこの包み焼き",
            "夕食",
            ["鮭", "きのこ", "しめじ", "えのき", "玉ねぎ"],
            ["バター"],
            "鮭と野菜を包み、塩こしょうとバターをのせて蒸し焼きにする。仕上げにしょうゆを少し垂らす。",
            "魚と野菜を同時に調理でき、洗い物も少なく済みます。",
            "魚の脂質と野菜を組み合わせた軽めの夕食です。"),
        new(
            "卵とトマトのふんわり炒め",
            "朝食",
            ["卵", "トマト", "長ねぎ", "チーズ"],
            ["鶏がらスープ"],
            "卵を半熟に炒めて取り出す。トマトを温め、卵を戻して塩とスープ少量で整える。",
            "朝でも作りやすく、傷みやすいトマトを早めに使えます。",
            "卵のたんぱく質とトマトの酸味で軽く食べられます。"),
        new(
            "豆腐とひき肉のあんかけ丼",
            "昼食",
            ["豆腐", "ひき肉", "長ねぎ", "小松菜", "米"],
            ["片栗粉"],
            "ひき肉を炒め、豆腐と野菜を加える。だし、しょうゆで煮て、水溶き片栗粉でとろみを付ける。",
            "ご飯にかけるだけで昼食になり、豆腐を無理なく使い切れます。",
            "やわらかく食べやすい、たんぱく質中心の丼です。"),
        new(
            "冷蔵庫整理スープ",
            "朝食",
            ["キャベツ", "白菜", "玉ねぎ", "にんじん", "じゃがいも", "きのこ", "トマト"],
            ["コンソメ"],
            "余っている野菜を小さめに切る。水とコンソメで煮て、塩こしょうで整える。",
            "少量ずつ残った食材を一度に消費できます。",
            "野菜を多めに取れる軽い一品です。主食や卵を足すと朝食になります。"),
        new(
            "野菜たっぷりカレー",
            "夕食",
            ["鶏肉", "豚肉", "玉ねぎ", "にんじん", "じゃがいも", "なす", "かぼちゃ"],
            ["カレールー"],
            "肉と野菜を炒めて煮る。火を止めてルーを溶かし、再び弱火でとろみを付ける。",
            "量が多い根菜や肉をまとめて使え、翌日の昼食にも回せます。",
            "主食と主菜がまとまる献立です。副菜にサラダを添えると整います。"),
        new(
            "納豆オムレツ",
            "朝食",
            ["納豆", "卵", "チーズ", "長ねぎ"],
            ["しょうゆ"],
            "卵を溶き、納豆とねぎを混ぜる。フライパンで丸く焼き、好みでチーズを入れる。",
            "冷蔵庫に残りがちな卵と納豆で素早く作れます。",
            "朝のたんぱく質補給に向いた一皿です。"),
        new(
            "ツナと野菜のパスタ",
            "昼食",
            ["ツナ", "パスタ", "トマト", "キャベツ", "玉ねぎ", "きのこ"],
            ["にんにく"],
            "パスタを茹でる。ツナと野菜を炒め、茹で汁を少し加えて絡める。",
            "缶詰と半端な野菜で昼食を作れます。",
            "主食に野菜とたんぱく質を足した、手早い昼食です。"),
        new(
            "さば缶トマト煮",
            "夕食",
            ["さば", "トマト", "玉ねぎ", "なす", "きのこ"],
            ["にんにく"],
            "玉ねぎと野菜を炒め、さばとトマトを加えて煮る。塩こしょうで味を整える。",
            "魚を手軽に使え、トマトやなすの消費にも向きます。",
            "魚のたんぱく質と野菜をまとめて取れる献立です。")
    ];

    public IReadOnlyList<string> ExtractIngredientCandidates(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return [];
        }

        var normalized = text.Replace('、', ',').Replace('，', ',').Replace('　', ' ');
        var words = normalized
            .Split([',', '\n', '\r', '/', ' ', '・'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(word => word.Trim())
            .Where(word => word.Length is >= 1 and <= 20)
            .ToList();

        var knownMatches = KnownIngredients
            .Where(ingredient => normalized.Contains(ingredient, StringComparison.OrdinalIgnoreCase));

        return knownMatches.Concat(words)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Take(16)
            .ToList();
    }

    public async Task<IReadOnlyList<RecipeSuggestion>> BuildSuggestionsAsync(
        string userId,
        string mealType,
        string mood,
        int servingCount)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var items = await db.FridgeItems
            .Where(item => item.UserId == userId)
            .OrderBy(item => item.ExpirationDate ?? DateOnly.MaxValue)
            .ThenBy(item => item.Name)
            .ToListAsync();

        var today = DateOnly.FromDateTime(DateTime.Today);
        var itemNames = items.Select(item => item.Name).ToList();
        var expiringSoon = items
            .Where(item => item.ExpirationDate is not null && item.ExpirationDate <= today.AddDays(2))
            .Select(item => item.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var scored = Recipes
            .Where(recipe => mealType == "おまかせ" || recipe.MealType == mealType)
            .Select(recipe =>
            {
                var matched = recipe.Ingredients
                    .Where(ingredient => itemNames.Any(item => IsMatch(item, ingredient)))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();
                var missing = recipe.Ingredients
                    .Where(ingredient => matched.All(match => !IsMatch(match, ingredient)))
                    .Take(3)
                    .ToList();
                var expiryBoost = matched.Count(match => expiringSoon.Contains(match)) * 12;
                var moodBoost = mood switch
                {
                    "時短" when recipe.Steps.Length < 80 => 14,
                    "節約" when recipe.Title.Contains("整理") || recipe.Title.Contains("丼") => 12,
                    "ヘルシー" when recipe.NutritionNote.Contains("野菜") || recipe.Title.Contains("魚") => 12,
                    _ => 0
                };
                var score = Math.Min(98, 35 + matched.Count * 15 + expiryBoost + moodBoost - missing.Count * 4);

                var reason = matched.Count == 0
                    ? "買い足し少なめの定番献立です。"
                    : $"優先食材: {string.Join("、", matched)}。";
                if (expiryBoost > 0)
                {
                    reason += " 期限が近い食材を先に使います。";
                }
                if (servingCount > 1)
                {
                    reason += $" {servingCount}人分。";
                }

                return new RecipeSuggestion(
                    recipe.Title,
                    recipe.MealType,
                    matched,
                    missing.Concat(recipe.PantryItems).Distinct(StringComparer.OrdinalIgnoreCase).Take(4).ToList(),
                    recipe.Steps,
                    reason,
                    recipe.NutritionNote,
                    score);
            })
            .OrderByDescending(suggestion => suggestion.Confidence)
            .ThenBy(suggestion => suggestion.MissingIngredients.Count)
            .Take(5)
            .ToList();

        return scored.Count > 0 ? scored : [CreateFallbackSuggestion(items, servingCount)];
    }

    public async Task<MealPlan> SaveMealPlanAsync(string userId, RecipeSuggestion suggestion, DateOnly mealDate)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var plan = new MealPlan
        {
            UserId = userId,
            Title = suggestion.Title,
            MealType = suggestion.MealType,
            MealDate = mealDate,
            Ingredients = string.Join("、", suggestion.MatchedIngredients.DefaultIfEmpty("登録食材なし")),
            Steps = suggestion.Steps,
            Reason = suggestion.Reason,
            NutritionNote = suggestion.NutritionNote
        };

        db.MealPlans.Add(plan);
        db.AppNotifications.Add(new AppNotification
        {
            UserId = userId,
            Title = "献立を記録しました",
            Message = $"{plan.MealDate:yyyy/MM/dd}の{plan.MealType}に「{plan.Title}」を保存しました。",
            Kind = "success",
            ActionUrl = "/history"
        });

        await db.SaveChangesAsync();
        return plan;
    }

    public async Task RefreshExpiryNotificationsAsync(string userId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var today = DateOnly.FromDateTime(DateTime.Today);
        var soon = today.AddDays(2);
        var items = await db.FridgeItems
            .Where(item => item.UserId == userId && item.ExpirationDate != null && item.ExpirationDate <= soon)
            .ToListAsync();

        foreach (var item in items)
        {
            var title = item.ExpirationDate < today
                ? $"{item.Name}の期限が過ぎています"
                : $"{item.Name}の期限が近いです";
            var exists = await db.AppNotifications
                .AnyAsync(notification => notification.UserId == userId
                    && notification.Title == title
                    && notification.ReadAt == null);
            if (exists)
            {
                continue;
            }

            db.AppNotifications.Add(new AppNotification
            {
                UserId = userId,
                Title = title,
                Message = item.ExpirationDate < today
                    ? $"{item.ExpirationDate:yyyy/MM/dd}までの予定でした。状態を確認してください。"
                    : $"{item.ExpirationDate:yyyy/MM/dd}までに使うと安心です。",
                Kind = item.ExpirationDate < today ? "danger" : "warning",
                ActionUrl = "/fridge"
            });
        }

        await db.SaveChangesAsync();
    }

    private static RecipeSuggestion CreateFallbackSuggestion(IReadOnlyList<FridgeItem> items, int servingCount)
    {
        var names = items.Take(4).Select(item => item.Name).ToList();
        var ingredientText = names.Count == 0 ? "登録した食材" : string.Join("、", names);
        return new RecipeSuggestion(
            "冷蔵庫整理プレート",
            "夕食",
            names,
            ["卵", "調味料"],
            "使いたい食材を食べやすく切る。火の通りにくいものから炒め、塩こしょうとしょうゆで味を整える。",
            $"{ingredientText}を中心に使い切る献立です。{servingCount}人分。",
            "主菜に卵や豆腐を足すとたんぱく質を補いやすくなります。",
            42);
    }

    private static bool IsMatch(string itemName, string recipeIngredient)
    {
        return itemName.Contains(recipeIngredient, StringComparison.OrdinalIgnoreCase)
            || recipeIngredient.Contains(itemName, StringComparison.OrdinalIgnoreCase);
    }

    private sealed record RecipeTemplate(
        string Title,
        string MealType,
        IReadOnlyList<string> Ingredients,
        IReadOnlyList<string> PantryItems,
        string Steps,
        string Reason,
        string NutritionNote);
}
