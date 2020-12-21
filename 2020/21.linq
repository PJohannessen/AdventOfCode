<Query Kind="Program">
  <RuntimeVersion>5.0</RuntimeVersion>
</Query>

#load ".\shared"

void Main()
{
    var inputStrings = Utils.ParseStrings("21.txt");
    var recipes = Parse(inputStrings);
    Dictionary<string, string> mapping = new();
    var remainingAllergens = recipes.SelectMany(l => l.Allergens).Distinct().ToList();
    var remainingIngredients = recipes.SelectMany(l => l.Ingredients).Distinct().ToList();
    
    while (remainingAllergens.Count > 0)
    {
        foreach (var allergen in remainingAllergens.ToArray())
        {
            var matchingRecipes = recipes.Where(r => r.Allergens.Contains(allergen));
            var matchingIngredients = remainingIngredients.Where(i => matchingRecipes.All(mr => mr.Ingredients.Contains(i)));
            if (matchingIngredients.Count() == 1)
            {
                mapping.Add(allergen, matchingIngredients.Single());
                remainingAllergens.Remove(allergen);
                remainingIngredients.Remove(matchingIngredients.Single());
            }
        }
    }
    
    int safeIngredients = recipes.SelectMany(r => r.Ingredients).Where(i => !mapping.Values.Contains(i)).Count();
    var list = mapping.Select(kvp => kvp).OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).ToArray();
    string dangerousIngredients = string.Join(',', list);

    $"P1: {safeIngredients}".Dump();
    $"P2: {dangerousIngredients}".Dump();
}

List<(int Id, List<string> Ingredients, List<string> Allergens)> Parse(string[] inputStrings)
{
    List<(int Id, List<string> Ingridients, List<string> Allergens)> list = new();
    int id = 1;
    foreach (var s in inputStrings)
    {
        int allergiesStartIndex = s.IndexOf('(');
        List<string> allergies = new();
        List<string> ingredients = new();
        if (allergiesStartIndex > 0)
        {
            allergies = s[allergiesStartIndex..].Split(new[] { "(contains ", ")", ", " }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
        else
        {
            allergiesStartIndex = s.Length;
        }
        ingredients = s[..allergiesStartIndex].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();

        list.Add((id, ingredients, allergies));
        id++;
    }
    return list;
}