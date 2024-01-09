using System;
using CMCore.Behaviors.Gameplay.Player;
using CMCore.Behaviors.Object;
using CMCore.Managers;
using CMCore.Models;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CMCore.Behaviors.Gameplay.Card
{
    public class CardBehavior : PrefabBehavior
    {
        public CardModel card;
        protected PlayerBehavior RelatedTo;
        [SerializeField] protected TextMeshPro text;
        [SerializeField] protected SpriteRenderer rend;

        public override void ResetBehavior()
        {
            base.ResetBehavior();
        }

        public void SetPosition(float z)
        {
            transform.position = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), z);
        }

        public void SetPosition(Vector3 pos)
        {
            transform.position = pos;
        }

        public void SetEulerAngles(float rot)
        {
            transform.eulerAngles = new Vector3(0, rot, 0);
        }

        public void SetLayer(int lastCard)
        {
            rend.sortingOrder = lastCard;
            if (text)
                text.sortingOrder = lastCard + 1;
        }

        private void OnMouseDown()
        {
            if (RelatedTo != GameManager.LevelManager.LevelBehavior.HumanPlayer ||
                GameManager.LevelManager.LevelBehavior.HumanPlayer.CurrentState != Enums.PlayerState.Thinking ||
                !GameManager.LevelManager.LevelBehavior.HumanPlayer.Cards.Contains(this)) return;

            GameManager.LevelManager.LevelBehavior.Play(GameManager.LevelManager.LevelBehavior.HumanPlayer, this);
        }

        public void SetRelated(PlayerBehavior related)
        {
            RelatedTo = related;
        }

        public void DoMove(Vector3 pos, int delay)
        {
            transform.position = GameManager.LevelManager.LevelBehavior.DealerGameObject.transform.position;
            transform.DOMove(pos, 0.15f).SetDelay(delay * 0.05f);
        }

        public void DoMove(Vector3 from, Vector3 to, int delay, Action onComplete)
        {
            transform.position = from;
            transform.DOMove(to, 0.15f).SetDelay(delay * 0.01f).OnComplete(() => onComplete?.Invoke())
                .SetEase(Ease.InBack);
        }

        public void MoveToField(Action onComplete)
        {
            transform.DOMove(new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0), 0.2f).OnComplete(
                () => { onComplete?.Invoke(); });
        }
    }
}