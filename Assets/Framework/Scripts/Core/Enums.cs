namespace CardGame
{
    public enum IngredientTier
    {
        Basic       = 0,
        Normal      = 1,
        Superb      = 2,
        Outstanding = 3,
        Legendary   = 4
    }

    public enum RecipeTier
    {
        Easy      = 0,
        Normal    = 1,
        Medium    = 2,
        Hard      = 3,
        SuperHard = 4,
        Insane    = 5
    }

    public enum CardType
    {
        Ingredient,
        Recipe,
        Utility
    }

    [System.Flags]
    public enum IngredientTag
    {
        None       = 0,
        Vegetable  = 1 << 0,
        Meat       = 1 << 1,
        Fish       = 1 << 2,
        Dairy      = 1 << 3,
        Grain      = 1 << 4,
        Spice      = 1 << 5,
        Liquid     = 1 << 6,
        Mushroom   = 1 << 7,
        Fruit      = 1 << 8,
        Luxury     = 1 << 9
    }

    public enum RequirementMatchMode
    {
        ExactCard,
        AnyInTag,
        AnyOfTier,
        AnyOfTierOrHigher,
        AnyRecipe,
        AnyUtility
    }

    public enum GamePhase
    {
        Setup,
        Draw,
        Play,
        Score,
        End
    }

    public enum PlayerSide
    {
        Local,
        Opponent
    }

    public enum CardZone
    {
        Deck,
        Hand,
        RecipeBoard,
        RecipePile,
        Discard,
        Offered
    }
}
