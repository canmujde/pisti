using System.Collections.Generic;
using System.Linq;
using CMCore.Behaviors.Gameplay.Card;
using CMCore.Behaviors.Object;
using CMCore.Contracts;
using CMCore.Managers;
using CMCore.Models;
using UnityEngine;

namespace CMCore.Behaviors.Gameplay.Player
{
    public abstract class PlayerBehavior : PrefabBehavior, IPlayer
    {
        
        [field: SerializeField]public Enums.PlayerState CurrentState { get; set; }
        [field: SerializeField]public List<CardBehavior> Cards { get; set; }
        [field: SerializeField]public List<CardBehavior> CollectedCards { get; set; }
        [field: SerializeField]public int PistiCount { get; set; }

        public virtual void DoWaitForTurn()
        {
            CurrentState = Enums.PlayerState.WaitingForTurn;
        }

        public virtual void DoPlay()
        {
            CurrentState = Enums.PlayerState.Playing;
        }

        public virtual void DoThink()
        {
            CurrentState = Enums.PlayerState.Thinking;
        }

        public override void ResetBehavior()
        {
            base.ResetBehavior();
            Cards = new List<CardBehavior>();
            CollectedCards = new List<CardBehavior>();
            PistiCount = 0;
        }

        public void CollectCards(List<CardBehavior> playedCards)
        {
            var i = 0;
            foreach (var card in playedCards)
            {
                var pos = this == GameManager.LevelManager.LevelBehavior.AIPlayer
                    ? transform.position + Vector3.up
                    : transform.position + Vector3.down;
                card.DoMove(card.transform.position, pos,i, () =>
                {
                    PoolManager.Return(card);
                });

                i++;
            }
            
            CollectedCards.AddRange(playedCards.ToList());
        }
    }
}