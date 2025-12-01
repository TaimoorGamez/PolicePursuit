using UnityEngine;
using DG.Tweening;
using Core.Events;
using Core.Plugins;
using Core.ToastMsg;
using Core.Variables;
using UnityEngine.UI;
using Core.DB.Variables;

namespace Core.Screen
{
    public class SplashScreen : UiScreens
    {
        [SerializeField] ToastManager ToastMsnger;
        [SerializeField] Initialization FirebaseInit;
        [SerializeField] DBInt FFT;
        [SerializeField] SOIntegerEvents ActiveStateEvent, DestroyStateEvent;
        [SerializeField] SOInterger MainMenuStateIndex;
        [SerializeField] Transform FillImage;
        [SerializeField] Image LogoImage;

        float _loadingTime = 2;

        private void Start()
        {
            if (FFT.Value != 1)
            {
                FFT.Value = 1;
            }
            FirebaseInit.InitPlugin();
            LogoImage.DOFillAmount(1, _loadingTime).SetEase(Ease.Linear);
            FillImage.DOScaleX(1, _loadingTime).SetEase(Ease.Linear).OnComplete(()=>
            {
                ToastMsnger.InitToastMsg();
                DestroyStateEvent.InvokeSOEvent(0);
                ActiveStateEvent.InvokeSOEvent(MainMenuStateIndex.Value);
            });
        }
    }
}
