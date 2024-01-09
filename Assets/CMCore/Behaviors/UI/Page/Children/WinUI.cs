using CMCore.Managers;
using CMCore.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace CMCore.Behaviors.UI.Page.Children
{
    public class WinUI : UIBase
    {
        public int playerPoints;
        public int opponentPoints;

        [SerializeField] private Button nextButton;
        [SerializeField] private TextMeshProUGUI playerPointsText;
        [SerializeField] private TextMeshProUGUI aiPointsText;

        private void NextButton_OnClick()
        {
            GameManager.EventManager.GameStateChanged?.Invoke(Enums.GameState.Menu);
            GameManager.EventManager.GameStateChanged?.Invoke(Enums.GameState.InGame);
            HapticManager.Play(Enums.Haptic.H1);
        }

        public override void Initialize(UIManager uiManager)
        {
            base.Initialize(uiManager);
            nextButton.onClick.AddListener(NextButton_OnClick);
        }

        protected override void OnShow()
        {
            base.OnShow();
            playerPointsText.text = playerPoints.ToString();
            aiPointsText.text = opponentPoints.ToString();
        }
        

        protected override void OnHide()
        {
            base.OnHide();
        }
    }
}