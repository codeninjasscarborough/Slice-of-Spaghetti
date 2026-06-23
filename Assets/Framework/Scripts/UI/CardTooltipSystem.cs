using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CardGame.UI
{
    public class CardTooltipSystem : MonoBehaviour
    {
        public static CardTooltipSystem Instance { get; private set; }

        [Header("Timing")]
        [SerializeField] float hoverDelay = 0.75f;

        [Header("Panel References")]
        [SerializeField] RectTransform tooltipPanel;
        [SerializeField] TextMeshProUGUI titleLabel;
        [SerializeField] TextMeshProUGUI tierTypeLabel;
        [SerializeField] TextMeshProUGUI pointsLabel;
        [SerializeField] TextMeshProUGUI detailLabel;
        [SerializeField] Transform slotsContainer;
        [SerializeField] TextMeshProUGUI slotLinePrefab;

        Coroutine _pending;

        void Awake()
        {
            Instance = this;
            tooltipPanel.gameObject.SetActive(false);
        }

        public void BeginHover(Card card, Vector2 screenPosition)
        {
            StopPending();
            _pending = StartCoroutine(ShowAfterDelay(card, screenPosition));
        }

        public void EndHover()
        {
            StopPending();
            tooltipPanel.gameObject.SetActive(false);
        }

        void StopPending()
        {
            if (_pending != null) StopCoroutine(_pending);
            _pending = null;
        }

        IEnumerator ShowAfterDelay(Card card, Vector2 screenPos)
        {
            yield return new WaitForSeconds(hoverDelay);
            Populate(card);
            PositionPanel(screenPos);
            tooltipPanel.gameObject.SetActive(true);
        }

        void Populate(Card card)
        {
            var data = card.Data;
            titleLabel.text = data.DisplayName;
            tierTypeLabel.text = data switch
            {
                IngredientCardData i => $"{i.Tier} Â· Ingredient",
                RecipeCardData r     => $"{r.Tier} Â· Recipe",
                _                    => data.CardType.ToString()
            };
            pointsLabel.text = $"Base Points: {data.BasePoints}   Play Cost: {data.PlayCost}";

            foreach (Transform child in slotsContainer)
                Destroy(child.gameObject);
            slotsContainer.gameObject.SetActive(false);

            if (data is IngredientCardData ing)
            {
                var tagNames = new List<string>();
                foreach (IngredientTag tag in System.Enum.GetValues(typeof(IngredientTag)))
                    if (tag != IngredientTag.None && (ing.Tags & tag) != 0)
                        tagNames.Add(tag.ToString());

                detailLabel.text =
                    $"Tags: {(tagNames.Count > 0 ? string.Join(", ", tagNames) : "None")}\n" +
                    $"Score Multiplier: Ã—{ing.ScoreMultiplierContribution:F2}   Bonus Flat: +{ing.BonusFlatPoints}";
            }
            else if (data is RecipeCardData recipe)
            {
                detailLabel.text =
                    $"Scoring Multiplier: Ã—{recipe.ScoringMultiplier:F2}\n" +
                    $"Requires Sub-Recipe: {(recipe.RequiresSubRecipe ? "Yes" : "No")}";

                slotsContainer.gameObject.SetActive(true);
                foreach (var slot in recipe.Slots)
                {
                    var line = Instantiate(slotLinePrefab, slotsContainer);
                    line.text = FormatSlot(slot);
                }
            }
            else if (data is UtilityCardData utility)
            {
                detailLabel.text =
                    $"Durability: {card.CurrentDurability}/{utility.MaxDurability}\n" +
                    (string.IsNullOrEmpty(utility.EffectDescription) ? "" : utility.EffectDescription);
            }
            else
            {
                detailLabel.text = string.Empty;
            }
        }

        static string FormatSlot(RecipeRequirement req) => req.MatchMode switch
        {
            RequirementMatchMode.ExactCard         => $"Exact: {req.ExactCard?.DisplayName ?? "?"}",
            RequirementMatchMode.AnyInTag          => $"Any {req.RequiredTags}",
            RequirementMatchMode.AnyOfTier         => $"Any {req.RequiredTier}",
            RequirementMatchMode.AnyOfTierOrHigher => $"{req.RequiredTier}+",
            RequirementMatchMode.AnyRecipe         => "Any Completed Recipe",
            _                                      => req.MatchMode.ToString()
        };

        void PositionPanel(Vector2 screenPos)
        {
            var canvas = tooltipPanel.GetComponentInParent<Canvas>();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.GetComponent<RectTransform>(),
                screenPos,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                out var localPos);

            tooltipPanel.anchoredPosition = localPos + new Vector2(20f, -20f);

            var canvasRect = canvas.GetComponent<RectTransform>().rect;
            var pos = tooltipPanel.anchoredPosition;
            pos.x = Mathf.Clamp(pos.x,
                canvasRect.xMin + tooltipPanel.rect.width  * 0.5f,
                canvasRect.xMax - tooltipPanel.rect.width  * 0.5f);
            pos.y = Mathf.Clamp(pos.y,
                canvasRect.yMin + tooltipPanel.rect.height * 0.5f,
                canvasRect.yMax - tooltipPanel.rect.height * 0.5f);
            tooltipPanel.anchoredPosition = pos;
        }
    }
}
