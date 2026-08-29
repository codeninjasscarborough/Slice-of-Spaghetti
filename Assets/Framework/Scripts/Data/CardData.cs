using UnityEngine;

namespace CardGame
{
    public abstract class CardData : ScriptableObject
    {
        [SerializeField] string displayName;
        [SerializeField] Sprite art;
        [SerializeField] CardType cardType;
        [SerializeField] int basePoints;
        [SerializeField] int playCost;
        [SerializeField] int titleIndex;

        public string DisplayName => displayName;
        public Sprite Art => art;
        public CardType CardType => cardType;
        public int BasePoints => basePoints;
        public int PlayCost => playCost;

        public int TitleIndex => titleIndex;
    }
}
