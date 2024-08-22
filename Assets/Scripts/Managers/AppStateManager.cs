using HotForgeStudio.HorrorBox.Common;
using System;

namespace HotForgeStudio.HorrorBox
{
    public sealed class AppStateManager : IService, IAppStateManager
    {
        public event Action AppStateChangedEvent;

        private IUIManager _uiManager;

        public Enumerators.AppState AppState { get; private set; } = Enumerators.AppState.Unknown;

        public void Init()
        {
            _uiManager = GameClient.Get<IUIManager>();
        }

        public void Dispose()
        {
        }

        public void Update()
        {
        }

        public void ChangeAppState(Enumerators.AppState stateTo)
        {
            if (AppState == stateTo)
                return;

            AppState = stateTo;

            AppStateChangedEvent?.Invoke();

            switch (stateTo)
            {
                case Enumerators.AppState.Main:
                    GameClient.Get<IGameplayManager>().StopGameplay();
                    _uiManager.SetPage<MainPage>();
                    break;
                case Enumerators.AppState.Game:
                    GameClient.Get<IGameplayManager>().StartGameplay();
                    _uiManager.SetPage<GamePage>();
                    break;
                case Enumerators.AppState.GameOver:
                    GameClient.Get<IGameplayManager>().StopGameplay();
                    _uiManager.DrawPopup<ResultsPopup>();
                    break;
            }
        }
    }
}