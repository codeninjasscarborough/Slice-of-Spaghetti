using UnityEngine;

namespace CardGame
{
    [CreateAssetMenu(menuName = "CardGame/Utility Card", fileName = "New Utility Card")]
    public class UtilityCardData : CardData
    {
        [SerializeField] int maxDurability = 3;
        [TextArea] [SerializeField] string effectDescription;

        public int MaxDurability => maxDurability;
        public string EffectDescription => effectDescription;
    }
}
