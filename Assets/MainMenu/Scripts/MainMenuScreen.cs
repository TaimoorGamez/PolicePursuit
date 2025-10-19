using UnityEngine;
using Core.Events;
using Core.Plugins;
using Core.Variables;

namespace Core.Screen
{
    public class MainMenuScreen : MonoBehaviour
    {
        [SerializeField] Initialization FirebaseInit, AdmobInit;
        [SerializeField] SOEvents InitLevelEvent;
        [SerializeField] SOInterger MainMenuStateIndex, GamePlayStateIndex, SettingStateIndex, IsFirebaseInit;
        [SerializeField] SOIntegerEvents ActiveStateEvent, DestroyStateEvent;

        public void OnclickSettingBtn()
        {
            ActiveStateEvent.InvokeSOEvent(SettingStateIndex.Value);
        }

        private void Start()
        {
            if(IsFirebaseInit.Value == 1)
            {
                AdmobInit.InitPlugin();
            }
            else
            {
                FirebaseInit.InitPlugin();
            }
        }

        public void OnClickPlayButton()
        {
            InitLevelEvent.InvokeSOEvent();
            ActiveStateEvent.InvokeSOEvent(GamePlayStateIndex.Value);
            DestroyStateEvent.InvokeSOEvent(MainMenuStateIndex.Value);
        }
    }
}
