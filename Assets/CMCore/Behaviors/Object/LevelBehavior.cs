using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMCore.Behaviors.Gameplay.Card;
using CMCore.Behaviors.Gameplay.Player;
using CMCore.Managers;
using CMCore.Models;
using CMCore.Utilities;
using CMCore.Utilities.Extensions;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CMCore.Behaviors.Object
{
    public class LevelBehavior : PrefabBehavior
    {
        private List<CardModel> _deck;
        private int _lastCard;
        private AIBehavior _aiPlayer;
        private HumanBehavior _humanPlayer;
        private List<CardBehavior> _playedCards;
        [SerializeField] private GameObject cardDealerBackGameObject;
        [SerializeField] private TextMeshPro cardDealerBackText;

        public GameObject DealerGameObject => cardDealerBackGameObject;
        public List<CardBehavior> PlayedCards => _playedCards;
        public AIBehavior AIPlayer => _aiPlayer;
        public HumanBehavior HumanPlayer => _humanPlayer;

        public override void ResetBehavior()
        {
            base.ResetBehavior();
            _playedCards = new List<CardBehavior>();
            _humanPlayer = PoolManager.Retrieve("Human").GetComponent<HumanBehavior>();
            _aiPlayer = PoolManager.Retrieve("AI").GetComponent<AIBehavior>();

            _humanPlayer.ResetBehavior();
            _aiPlayer.ResetBehavior();


            _lastCard = 0;

            _deck = this.Settings().Deck.ToList();
            _deck.Shuffle();


            for (int i = 0; i < 4; i++)
            {
                var last = _deck.Last();
                var cardPrefab = GetCardFromPool(last);
                _deck.Remove(last);

                cardPrefab.SetPosition(i <= 2 ? 0.1f : 0);
                cardPrefab.SetEulerAngles(i <= 2 ? 180 : 0);
                cardPrefab.SetLayer(_lastCard);
                _playedCards.Add(cardPrefab);
                _lastCard += 2;
            }


            var isHumanTurn = RandomExtensions.NextBool();


            DealCards(true, true);

            if (isHumanTurn)
            {
                _aiPlayer.CurrentState = Enums.PlayerState.WaitingForTurn;
                _humanPlayer.CurrentState = Enums.PlayerState.Thinking;
            }
            else
            {
                _aiPlayer.CurrentState = Enums.PlayerState.Thinking;
                _humanPlayer.CurrentState = Enums.PlayerState.WaitingForTurn;
                Play(_aiPlayer, _aiPlayer.ChooseCardToPlay());
            }

            cardDealerBackGameObject.SetActive(true);
            cardDealerBackText.text = _deck.Count.ToString();
        }

        private void DealCards(bool human, bool ai)
        {
            if (human)
            {
                for (int i = 0; i < 4; i++)
                {
                    var card = GetCardFromPool(_deck.Last());

                    _deck.Remove(_deck.Last());

                    var i1 = i;
                    this.DelayedAction(
                        () => { card.DoMove(new Vector3(i1 - 1.5f, _humanPlayer.transform.position.y, 0), i1); },
                        new WaitUntil(() => !GameManager.UIManager.LoadingUI.gameObject.activeSelf));

                    card.SetEulerAngles(0);
                    card.SetLayer(i + 1);
                    card.SetRelated(_humanPlayer);
                    _humanPlayer.Cards.Add(card);
                }
            }

            if (ai)
            {
                for (int i = 0; i < 4; i++)
                {
                    var card = GetCardFromPool(_deck.Last());

                    _deck.Remove(_deck.Last());

                    var i1 = i;
                    this.DelayedAction(
                        () => { card.DoMove(new Vector3(i1 - 1.5f, _aiPlayer.transform.position.y, 0), i1); },
                        new WaitUntil(() => !GameManager.UIManager.LoadingUI.gameObject.activeSelf));

                    card.SetEulerAngles(180);
                    card.SetRelated(_aiPlayer);
                    _aiPlayer.Cards.Add(card);
                }
            }

            cardDealerBackGameObject.SetActive(_deck.Count > 0);
            cardDealerBackText.text = _deck.Count.ToString();
        }

        private CardBehavior GetCardFromPool(CardModel model)
        {
            if (model.CardValue != "K" && model.CardValue != "J" && model.CardValue != "Q")
            {
                var cardBehavior = this.GetFromPool("GenericCard").GetComponent<CardBehavior>();
                var asGeneric = cardBehavior.GetComponent<GenericCard>();

                asGeneric.ResetBehavior();
                asGeneric.Set(model);

                return cardBehavior;
            }

            switch (model.CardType)
            {
                case Enums.CardType.Club:
                    switch (model.CardValue)
                    {
                        case "K":
                            return this.GetFromPool("KingClub").GetComponent<CardBehavior>();
                        case "J":
                            return this.GetFromPool("JackClub").GetComponent<CardBehavior>();
                        case "Q":
                            return this.GetFromPool("QueenClub").GetComponent<CardBehavior>();
                    }

                    break;
                case Enums.CardType.Diamond:
                    switch (model.CardValue)
                    {
                        case "K":
                            return this.GetFromPool("KingDiamond").GetComponent<CardBehavior>();
                        case "J":
                            return this.GetFromPool("JackDiamond").GetComponent<CardBehavior>();
                        case "Q":
                            return this.GetFromPool("QueenDiamond").GetComponent<CardBehavior>();
                    }

                    break;
                case Enums.CardType.Heart:
                    switch (model.CardValue)
                    {
                        case "K":
                            return this.GetFromPool("KingHeart").GetComponent<CardBehavior>();
                        case "J":
                            return this.GetFromPool("JackHeart").GetComponent<CardBehavior>();
                        case "Q":
                            return this.GetFromPool("QueenHeart").GetComponent<CardBehavior>();
                    }

                    break;
                case Enums.CardType.Spade:
                    switch (model.CardValue)
                    {
                        case "K":
                            return this.GetFromPool("KingSpade").GetComponent<CardBehavior>();
                        case "J":
                            return this.GetFromPool("JackSpade").GetComponent<CardBehavior>();
                        case "Q":
                            return this.GetFromPool("QueenSpade").GetComponent<CardBehavior>();
                    }

                    break;
            }

            return null;
        }

        public async void Play(PlayerBehavior player, CardBehavior card)
        {
            var loaded = GameManager.UIManager.LoadingUI.gameObject.activeSelf;

            while (GameManager.UIManager.LoadingUI.gameObject.activeSelf)
            {
                await Task.Delay(1);
            }

            if (loaded)
                await Task.Delay(1000);

            if (player == _aiPlayer && !_aiPlayer.Cards.Any() && !_deck.Any()) return;

            _lastCard += 2;
            card.SetLayer(_lastCard);
            card.SetEulerAngles(0);

            card.MoveToField(() =>
            {
                _playedCards.Add(card);


                if ((_playedCards.Count >= 2 && card.card.CardValue == _playedCards[^2].card.CardValue) ||
                    (card.card.CardValue == "J" && _playedCards.Count > 1))
                {
                    player.CollectCards(_playedCards.ToList());

                    Debug.Log("collector: " + player.name);

                    if (_playedCards.Count == 2 && _playedCards[^1].card.CardValue == _playedCards[^2].card.CardValue)
                        player.PistiCount++;

                    _playedCards = new List<CardBehavior>();
                }

                if (player == _humanPlayer)
                {
                    _aiPlayer.CurrentState = Enums.PlayerState.Thinking;
                    _humanPlayer.CurrentState = Enums.PlayerState.WaitingForTurn;
                    _humanPlayer.Cards.Remove(card);
                    this.DelayedAction(() => { Play(_aiPlayer, _aiPlayer.ChooseCardToPlay()); }, 1.5f);
                }
                else
                {
                    _humanPlayer.CurrentState = Enums.PlayerState.Thinking;
                    _aiPlayer.CurrentState = Enums.PlayerState.WaitingForTurn;
                    _aiPlayer.Cards.Remove(card);
                }

                if (!_aiPlayer.Cards.Any() && !_humanPlayer.Cards.Any())
                {
                    if (_deck.Any())
                    {
                        DealCards(true, true);
                    }

                    else
                    {
                        ProceedToResult();
                    }
                }
            });
        }

        private void ProceedToResult()
        {
            var humanPoints = 0;
            var aiPoints = 0;


            foreach (var collectedCard in _humanPlayer.CollectedCards)
            {
                if (collectedCard.card.CardValue == "A")
                {
                    humanPoints += 1;
                }
                else if (collectedCard.card.CardValue == "2" && collectedCard.card.CardType == Enums.CardType.Club)
                {
                    humanPoints += 2;
                }
                else if (collectedCard.card.CardValue == "10" && collectedCard.card.CardType == Enums.CardType.Diamond)
                {
                    humanPoints += 3;
                }
                else if (collectedCard.card.CardValue == "J")
                {
                    humanPoints += 1;
                }
            }

            foreach (var collectedCard in _aiPlayer.CollectedCards)
            {
                if (collectedCard.card.CardValue == "A")
                {
                    aiPoints += 1;
                }
                else if (collectedCard.card.CardValue == "2" && collectedCard.card.CardType == Enums.CardType.Club)
                {
                    aiPoints += 2;
                }
                else if (collectedCard.card.CardValue == "10" && collectedCard.card.CardType == Enums.CardType.Diamond)
                {
                    aiPoints += 3;
                }
                else if (collectedCard.card.CardValue == "J")
                {
                    aiPoints += 1;
                }
            }

            if (_aiPlayer.CollectedCards.Count > _humanPlayer.CollectedCards.Count) aiPoints += 3;
            if (_humanPlayer.CollectedCards.Count > _aiPlayer.CollectedCards.Count) humanPoints += 3;

            humanPoints += _humanPlayer.PistiCount * 10;
            aiPoints += _aiPlayer.PistiCount * 10;

            GameManager.UIManager.WinUI.opponentPoints = aiPoints;
            GameManager.UIManager.WinUI.playerPoints = humanPoints;

            GameManager.UIManager.FailUI.opponentPoints = aiPoints;
            GameManager.UIManager.FailUI.playerPoints = humanPoints;


            GameManager.EventManager.GameStateChanged?.Invoke(humanPoints > aiPoints
                ? Enums.GameState.Win
                : Enums.GameState.Fail);
        }
    }
}