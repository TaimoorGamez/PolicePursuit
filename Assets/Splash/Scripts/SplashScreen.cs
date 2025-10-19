using UnityEngine;
using DG.Tweening;
using Core.Events;
using Core.Plugins;
using Core.Purchase;
using Core.ToastMsg;
using Core.Variables;
using Core.DB.Variables;

namespace Core.Screen
{
    public class SplashScreen : MonoBehaviour
    {
        [SerializeField] ToastManager ToastMsnger;
        [SerializeField] Initialization FirebaseInit;
        [SerializeField] SOPurchase SoStore;
        [SerializeField] DBInt FFT;
        [SerializeField] SOIntegerEvents ActiveStateEvent, DestroyStateEvent;
        [SerializeField] SOInterger MainMenuStateIndex;
        [SerializeField] Transform FillImage;

        float _loadingTime = 2;

        private void Start()
        {
            if (FFT.Value != 1)
            {
                FFT.Value = 1;
            }
            FirebaseInit.InitPlugin();
            FillImage.DOScaleX(1, _loadingTime).SetEase(Ease.Linear).OnComplete(() =>
            {
                ToastMsnger.InitToastMsg();
                ActiveStateEvent.InvokeSOEvent(MainMenuStateIndex.Value);
                DestroyStateEvent.InvokeSOEvent(0);
                SoStore.InitializePurchasing();
            });
        }
    }
}
