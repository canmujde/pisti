using System.Linq;
using CMCore.Behaviors.Gameplay.Card;
using CMCore.Managers;
using CMCore.Utilities.Extensions;
using UnityEngine;

namespace CMCore.Behaviors.Gameplay.Player
{
    public class AIBehavior : PlayerBehavior
    {
        public override void DoWaitForTurn()
        {
            base.DoWaitForTurn();
        }

        public override void DoPlay()
        {
            base.DoPlay();
        }

        public override void DoThink()
        {
            base.DoThink();
        }

        public CardBehavior ChooseCardToPlay()
        {
            if (GameManager.LevelManager.LevelBehavior != null &&
                GameManager.LevelManager.LevelBehavior.PlayedCards != null &&
                GameManager.LevelManager.LevelBehavior.PlayedCards.Any())
            {
                var lastPlayed = GameManager.LevelManager.LevelBehavior.PlayedCards.Last();
                var isThereAnySameCard = Cards.Any(behavior => behavior.card.CardValue == lastPlayed.card.CardValue);

                if (isThereAnySameCard)
                    return Cards.Find(behavior => behavior.card.CardValue == lastPlayed.card.CardValue);


                return Cards.Any() ? Cards.Random() : null;
            }
            else
            {
                return Cards.Any() ? Cards.Random() : null;
            }
        }
    }
}