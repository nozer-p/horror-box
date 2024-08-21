using System;
using UnityEngine;
using HotForgeStudio.HorrorBox.Common;

namespace HotForgeStudio.HorrorBox
{
    public class MainApp : MonoBehaviour
    {
        public event Action LateUpdateEvent;
        public event Action FixedUpdateEvent;

        private bool _lastOnApplicationPauseState;

        private static MainApp _Instance;
        public static MainApp Instance
        {
            get { return _Instance; }
            private set { _Instance = value; }
        }

        private void Awake()
        {
            _lastOnApplicationPauseState = false;

            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        private void Start()
        {
            if (Instance == this)
            {
                GameClient.Instance.InitServices();

                GameClient.Get<IAppStateManager>().ChangeAppState(Enumerators.AppState.AppStart);
                GameClient.Get<IAppStateManager>().ChangeAppState(Enumerators.AppState.Main);
            }
        }

        private void Update()
        {
            if (Instance == this)
            {
                GameClient.Instance.Update();
            }
        }

        private void LateUpdate()
        {
            if (Instance == this)
            {
                if (LateUpdateEvent != null)
                    LateUpdateEvent();
            }
        }

        private void FixedUpdate()
        {
            if (Instance == this)
            {
                if (FixedUpdateEvent != null)
                    FixedUpdateEvent();
            }
        }

        private void OnApplicationPause(bool pause)
        {
            if (!pause && _lastOnApplicationPauseState)
            {
                GameClient.Instance.GetService<IGameplayManager>().SetPauseStatusOfGameplay(pause);
            }

            _lastOnApplicationPauseState = pause;
        }

        private void OnDestroy()
        {
            if (Instance == this)
                GameClient.Instance.Dispose();

            Screen.sleepTimeout = SleepTimeout.SystemSetting;
        }
    }
}