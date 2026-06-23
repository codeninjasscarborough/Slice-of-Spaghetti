using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace CardGame
{
    [Serializable]
    class CardJsonEntry
    {
        public string name;
        public string type;       // "ingredient" or "recipe"
        public string tier;
        public int basePoints;
        public int playCost;
        // Ingredient fields
        public string[] tags;
        public float scoreMultiplierContribution = 1f;
        public int bonusFlatPoints;
        // Recipe fields
        public bool requiresSubRecipe;
        public float scoringMultiplier = 1f;
    }

    [Serializable]
    class CardJsonList { public CardJsonEntry[] cards; }

    [Serializable]
    class RecipeJsonEntry
    {
        public string name;
        public string tier;
        public int basePoints;
        public float scoringMultiplier = 1f;
        public bool requiresSubRecipe;
        public RecipeSlotJson[] slots;
    }

    [Serializable]
    class RecipeSlotJson
    {
        public string matchMode;   // ExactCard | AnyInTag | AnyOfTier | AnyOfTierOrHigher | AnyRecipe
        public string requiredTier;
        public string[] requiredTags;
    }

    [Serializable]
    class RecipeJsonList { public RecipeJsonEntry[] recipes; }

    public class DataLoader : MonoBehaviour
    {
        static readonly string BasePath =
            Path.Combine(Application.streamingAssetsPath, "CardGame");

        // Unified entry point. Calls onComplete with raw JSON strings for cards and recipes.
        // Use the JSON strings to patch or extend your ScriptableObject databases at runtime.
        public void Load(Action<string, string> onComplete)
        {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WSA
            string cardsJson   = LoadSync("cards.json");
            string recipesJson = LoadSync("recipes.json");
            onComplete?.Invoke(cardsJson, recipesJson);
#else
            StartCoroutine(LoadAsync(onComplete));
#endif
        }

        static string LoadSync(string fileName)
        {
            string path = Path.Combine(BasePath, fileName);
            if (!File.Exists(path))
            {
                Debug.LogWarning($"[DataLoader] File not found: {path}");
                return null;
            }
            return File.ReadAllText(path);
        }

        IEnumerator LoadAsync(Action<string, string> onComplete)
        {
            string cardsJson = null, recipesJson = null;

            yield return FetchUrl(GetUrl("cards.json"),   json => cardsJson   = json);
            yield return FetchUrl(GetUrl("recipes.json"), json => recipesJson = json);

            onComplete?.Invoke(cardsJson, recipesJson);
        }

        IEnumerator FetchUrl(string url, Action<string> onDone)
        {
            using var req = UnityWebRequest.Get(url);
            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
                onDone(req.downloadHandler.text);
            else
                Debug.LogWarning($"[DataLoader] Failed to load {url}: {req.error}");
        }

        static string GetUrl(string fileName) =>
            Path.Combine(Application.streamingAssetsPath, "CardGame", fileName);

#if UNITY_EDITOR
        [UnityEditor.MenuItem("CardGame/Generate Default cards.json")]
        static void WriteDefaultCardsTemplate()
        {
            string json = JsonUtility.ToJson(new CardJsonList
            {
                cards = new[]
                {
                    new CardJsonEntry { name = "Tomato",  type = "ingredient", tier = "Common", basePoints = 2, tags = new[]{"Vegetable"} },
                    new CardJsonEntry { name = "Broth",   type = "ingredient", tier = "Common", basePoints = 3, tags = new[]{"Liquid"} },
                    new CardJsonEntry { name = "Beef",    type = "ingredient", tier = "Uncommon", basePoints = 6, tags = new[]{"Meat"} },
                    new CardJsonEntry { name = "Truffle", type = "ingredient", tier = "Legendary", basePoints = 20, tags = new[]{"Mushroom","Luxury"}, scoreMultiplierContribution = 1.5f }
                }
            }, true);
            WriteFile("cards.json", json);
        }

        [UnityEditor.MenuItem("CardGame/Generate Default recipes.json")]
        static void WriteDefaultRecipesTemplate()
        {
            string json = JsonUtility.ToJson(new RecipeJsonList
            {
                recipes = new[]
                {
                    new RecipeJsonEntry
                    {
                        name = "Tomato Soup", tier = "Common", basePoints = 10, scoringMultiplier = 1f,
                        slots = new[]
                        {
                            new RecipeSlotJson { matchMode = "AnyInTag", requiredTags = new[]{"Vegetable"} },
                            new RecipeSlotJson { matchMode = "AnyInTag", requiredTags = new[]{"Liquid"} }
                        }
                    }
                }
            }, true);
            WriteFile("recipes.json", json);
        }

        static void WriteFile(string fileName, string content)
        {
            string dir = Path.Combine(Application.streamingAssetsPath, "CardGame");
            Directory.CreateDirectory(dir);
            string path = Path.Combine(dir, fileName);
            File.WriteAllText(path, content);
            UnityEditor.AssetDatabase.Refresh();
            Debug.Log($"[DataLoader] Written: {path}");
        }
#endif
    }
}
