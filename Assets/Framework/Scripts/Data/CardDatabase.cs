using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    [CreateAssetMenu(menuName = "CardGame/Card Database", fileName = "CardDatabase")]
    public class CardDatabase : ScriptableObject
    {
        [Serializable]
        public class CardEntry
        {
            public CardData data;
            [Min(1)] public int copies = 1;
        }

        [SerializeField] List<CardEntry> entries = new();

        public IReadOnlyList<CardEntry> Entries => entries;
    }
}
