using CMCore.Managers;
using CMCore.Utilities;
using CMCore.Utilities.Extensions;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CMCore.Behaviors.UI.Page.Children
{
    public class LoadingUI : UIBase
    {
        private const float _closeDelay = 0.5f;

        protected override void OnHide()
        {
            KillTweens();
            textLoading.DOColor(Color.clear, FadeOutDuration).SetDelay(_closeDelay);
            foreach (var image in loadingAnimatedIcons) image.DOColor(Color.clear, FadeOutDuration).SetDelay(_closeDelay);

            foreach (var bb in bgImage)
            {
                bb.DOColor(Color.clear, FadeOutDuration).SetDelay(_closeDelay).OnComplete(() =>
                {
                    base.OnHide();
                    KillTweens();
                });
            }
        }

        protected override void OnShow()
        {
            base.OnShow();

            KillTweens();
            foreach (var bb in bgImage)
            {
                bb.color = bgImageEnabledColor;
            }


            textLoading.color = textEnabledColor;
            foreach (var image in loadingAnimatedIcons) image.color = iconEnabledColor;
            gameObject.SetActive(true);
            GameManager.Instance.DelayedAction(Hide,
               1);
        }

        private void KillTweens()
        {
            foreach (var bb in bgImage)
            {
                bb.DOKill();
            }

            textLoading.DOKill();
            foreach (var image in loadingAnimatedIcons) image.DOKill();
        }

        /////////////////////////////


        [SerializeField] private Image[] bgImage;
        [SerializeField] private TextMeshProUGUI textLoading;
        [SerializeField] private Image[] loadingAnimatedIcons;


        [SerializeField] private Color iconEnabledColor;
        [SerializeField] private Color textEnabledColor;
        [SerializeField] private Color bgImageEnabledColor;

        private const float FadeOutDuration = 0.5f;
    }
}