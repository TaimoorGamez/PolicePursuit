using Core.Store;
using UnityEngine;
using DG.Tweening;
using Core.Events;
using Core.Plugins;
using Core.ToastMsg;
using Core.Variables;
using Core.DB.Variables;

namespace Core.Screen
{
    public class SplashScreen : UiScreens
    {
        [SerializeField] ToastManager ToastMsnger;
        [SerializeField] Initialization FirebaseInit;
        [SerializeField] ItemData DefaultCap, DefaultFlame, DefaultSpray;
        [SerializeField] DBInt FFT;
        [SerializeField] SOIntegerEvents ActiveStateEvent, DestroyStateEvent;
        [SerializeField] SOInterger MainMenuStateIndex;
        [SerializeField] Transform FillImage;

        float _loadingTime = 2;

        private void Start()
        {
            if (FFT.Value != 1)
            {
                DefaultCap.IsPurchased = true;
                DefaultFlame.IsPurchased = true;
                DefaultSpray.IsPurchased = true;
                FFT.Value = 1;
            }
            FirebaseInit.InitPlugin();
            FillImage.DOScaleX(1, _loadingTime).SetEase(Ease.Linear).OnComplete(()=>
            {
                ToastMsnger.InitToastMsg();
                DestroyStateEvent.InvokeSOEvent(0);
                ActiveStateEvent.InvokeSOEvent(MainMenuStateIndex.Value);
            });
        }
    }
}
