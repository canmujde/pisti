using CMCore.Managers;
using CMCore.Models;
using UnityEngine;
using UnityEngine.UI;

namespace CMCore.Behaviors.UI.Page.Children
{
    public class InGameUI : UIBase
    {
        [field: SerializeField] private Button RestartButton { get; set; }

        #region Overriding Methods

        public override void Initialize(UIManager uiManager)
        {
            base.Initialize(uiManager);
            RestartButton?.onClick.AddListener(RestartButton_OnClick);
        }
        protected override void OnShow()
        {
            base.OnShow();
        }

        protected override void OnHide()
        {
            base.OnHide();
        }

        #endregion

        private void RestartButton_OnClick()
        {
            HapticManager.Play(Enums.Haptic.H1);
            GameManager.EventManager.GameStateChanged?.Invoke(Enums.GameState.Menu);
            GameManager.EventManager.GameStateChanged?.Invoke(Enums.GameState.InGame);
        }




    }
}