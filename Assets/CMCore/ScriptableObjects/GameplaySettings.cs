using System;
using System.Collections.Generic;
using CMCore.Models;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CMCore.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameplaySettings", menuName = "CMCore/Gameplay Settings")]
    public class GameplaySettings : ScriptableObject
    {
        [field: SerializeField] public List<CardModel> Deck { get; private set; }
        [field: SerializeField] public List<CardModel> CACHE { get; private set; }


        public Enums.CardType typ;
        
        [Button]
        public void Add()
        {
            for (int i = 2; i <= 10; i++)
            {
                Deck.Add(new CardModel(typ, i.ToString()));
            }
        }
    }
    
}