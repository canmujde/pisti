using System;
using UnityEngine;

namespace CMCore.Models
{
    
    [Serializable]
    public class CardModel
    {
        public CardModel(Enums.CardType cardType, string cardValue)
        {
            CardType = cardType;
            CardValue = cardValue;
        }

        [field: SerializeField] public Enums.CardType CardType { get; private set; }
        [field: SerializeField] public string CardValue { get; private set; }
    }
}
