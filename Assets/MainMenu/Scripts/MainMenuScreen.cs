using UnityEngine;
using Core.Events;
using Core.Plugins;
using Core.Purchase;
using Core.Variables;

namespace Core.Screen
{
    public class MainMenuScreen : UiScreens
    {
        [SerializeField] Initialization FirebaseInit, AdmobInit;
        [SerializeField] SOPurchase SoStore;
        [SerializeField] SOEvents InitLevelEvent;
        [SerializeField] SOInterger MainMenuStateIndex, GamePlayStateIndex, SettingStateIndex, IsFirebaseInit;
        [SerializeField] SOIntegerEvents ActiveStateEvent, DestroyStateEvent;


        private void Start()
        {
            if(IsFirebaseInit.Value == 1)
            {
                SoStore.InitializePurchasing();
                Invoke(nameof(InitializeAds), 2f);
            }
            else
            {
                FirebaseInit.InitPlugin();
            }
        }

        public void OnClickPlayButton()
        {
            ActiveStateEvent.InvokeSOEvent(GamePlayStateIndex.Value);
            DestroyStateEvent.InvokeSOEvent(MainMenuStateIndex.Value);
            InitLevelEvent.InvokeSOEvent();
        }
        public void OnclickSettingBtn()
        {
            ActiveStateEvent.InvokeSOEvent(SettingStateIndex.Value);
        }
        public override void OnClose()
        {
            
        }

        void InitializeAds()
        {
            AdmobInit.InitPlugin();
        }
    }
}
