using System;
using CMCore.Managers;
using CMCore.Models;
using TMPro;
using UnityEngine;

namespace CMCore.Behaviors.Gameplay.Card
{
    public class GenericCard : CardBehavior
    {
        private Color _blackColor = new Color(0.03052688f, 0.03876059f, 0.1320755f);
        private Color _redColor = new Color(0.6603774f, 0.08410467f, 0.3436471f);
        public void Set(CardModel model)
        {
            card = model;

            text.text = card.CardValue;
            rend.sprite = Array.Find(GameManager.Instance.Core.Assets.Sprites,
                sprite => sprite.name == card.CardType.ToString());

            if (model.CardType == Enums.CardType.Club || model.CardType == Enums.CardType.Spade)
            {
                text.color = _blackColor;
            }
            else
            {
                text.color = _redColor;
            }

        }
        
    }
}
