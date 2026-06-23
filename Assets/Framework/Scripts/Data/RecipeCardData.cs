using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    [CreateAssetMenu(menuName = "CardGame/Recipe Card", fileName = "New Recipe Card")]
    public class RecipeCardData : CardData
    {
        [SerializeField] RecipeTier tier;
        [SerializeField] List<RecipeRequirement> slots = new();
        [SerializeField] bool requiresSubRecipe;
        [SerializeField] float scoringMultiplier = 1f;
        [SerializeField] List<RecipeCardData> unlocksRecipes = new();

        public RecipeTier Tier => tier;
        public IReadOnlyList<RecipeRequirement> Slots => slots;
        public bool RequiresSubRecipe => requiresSubRecipe;
        public float ScoringMultiplier => scoringMultiplier;
        public IReadOnlyList<RecipeCardData> UnlocksRecipes => unlocksRecipes;
    }
}
