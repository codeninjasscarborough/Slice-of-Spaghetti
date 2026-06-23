using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class GuideEntry
    {
        public RecipeCardData Recipe { get; }
        public string GuideText { get; }
        public Sprite Icon { get; }
        public bool IsCraftable { get; }
        public bool IsDiscovered { get; }

        public GuideEntry(RecipeCatalog.RecipeCatalogEntry catalogEntry, bool isCraftable, bool isDiscovered)
        {
            Recipe = catalogEntry.recipe;
            GuideText = catalogEntry.guideText;
            Icon = catalogEntry.icon;
            IsCraftable = isCraftable;
            IsDiscovered = isDiscovered;
        }
    }

    public class GuideGroup
    {
        public RecipeTier Tier { get; }
        public List<GuideEntry> Entries { get; } = new();

        public GuideGroup(RecipeTier tier) => Tier = tier;
    }

    public class RecipeGuideManager
    {
        readonly RecipeCatalog _catalog;

        // Runtime discovery state â€” stored here, not on the ScriptableObject,
        // so it persists across sessions via PlayerPrefs without dirtying assets.
        readonly HashSet<string> _discovered = new();
        const string PlayerPrefsKey = "CGF_Discovered";

        public RecipeGuideManager(RecipeCatalog catalog)
        {
            _catalog = catalog;
            LoadDiscovery();
        }

        // Returns all visible recipes grouped by tier.
        public List<GuideGroup> GetGuideGroups(Hand hand)
        {
            var groups = new Dictionary<RecipeTier, GuideGroup>();

            foreach (var entry in _catalog.Entries)
            {
                if (!entry.visible) continue;

                if (!groups.TryGetValue(entry.recipe.Tier, out var group))
                {
                    group = new GuideGroup(entry.recipe.Tier);
                    groups[entry.recipe.Tier] = group;
                }

                bool craftable = CanCraft(entry.recipe, hand, null);
                group.Entries.Add(new GuideEntry(entry, craftable, IsDiscovered(entry)));
            }

            return new List<GuideGroup>(groups.Values);
        }

        // Returns only recipes the player can complete right now.
        public List<GuideEntry> GetCraftableRecipes(Hand hand, RecipePile completedPile)
        {
            var result = new List<GuideEntry>();

            foreach (var entry in _catalog.Entries)
            {
                if (!entry.visible) continue;
                if (!CanCraft(entry.recipe, hand, completedPile)) continue;

                result.Add(new GuideEntry(entry, true, IsDiscovered(entry)));
            }

            return result;
        }

        public bool IsDiscovered(RecipeCatalog.RecipeCatalogEntry entry) =>
            entry.startDiscovered || _discovered.Contains(entry.recipe.name);

        public void MarkDiscovered(RecipeCardData recipe)
        {
            if (_discovered.Add(recipe.name))
            {
                SaveDiscovery();
                GameEventBus.Publish(new RecipeDiscoveredEvent { Recipe = recipe });
            }
        }

        public void ResetDiscovery()
        {
            _discovered.Clear();
            PlayerPrefs.DeleteKey(PlayerPrefsKey);
        }

        void SaveDiscovery()
        {
            PlayerPrefs.SetString(PlayerPrefsKey, string.Join(",", _discovered));
            PlayerPrefs.Save();
        }

        void LoadDiscovery()
        {
            string saved = PlayerPrefs.GetString(PlayerPrefsKey, "");
            if (string.IsNullOrEmpty(saved)) return;
            foreach (var name in saved.Split(','))
                if (!string.IsNullOrEmpty(name))
                    _discovered.Add(name);
        }

        bool CanCraft(RecipeCardData recipe, Hand hand, RecipePile completedPile)
        {
            var available = new List<CardData>();
            foreach (var c in hand.Cards) available.Add(c.Data);
            if (completedPile != null)
                foreach (var c in completedPile.Cards) available.Add(c.Data);

            var used = new bool[available.Count];

            foreach (var slot in recipe.Slots)
            {
                bool matched = false;
                for (int i = 0; i < available.Count; i++)
                {
                    if (!used[i] && slot.IsSatisfiedBy(available[i]))
                    {
                        used[i] = true;
                        matched = true;
                        break;
                    }
                }
                if (!matched) return false;
            }

            return true;
        }
    }
}
