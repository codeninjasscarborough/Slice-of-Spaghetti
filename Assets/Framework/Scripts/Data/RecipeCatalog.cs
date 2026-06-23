using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    [CreateAssetMenu(menuName = "CardGame/Recipe Catalog", fileName = "RecipeCatalog")]
    public class RecipeCatalog : ScriptableObject
    {
        [Serializable]
        public class RecipeCatalogEntry
        {
            public RecipeCardData recipe;
            [TextArea] public string guideText;
            public Sprite icon;
            public bool visible = true;
            // Set in the Inspector to make a recipe start as already discovered.
            // Runtime discovery state is tracked separately in RecipeGuideManager.
            public bool startDiscovered;
        }

        [SerializeField] List<RecipeCatalogEntry> entries = new();

        public IReadOnlyList<RecipeCatalogEntry> Entries => entries;

        public RecipeCatalogEntry FindEntry(RecipeCardData recipe)
        {
            foreach (var e in entries)
                if (e.recipe == recipe) return e;
            return null;
        }

    }
}
