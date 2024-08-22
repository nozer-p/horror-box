using HotForgeStudio.HorrorBox.Common;

namespace HotForgeStudio.HorrorBox
{
    public class PlayerController : IController
	{
		private IGameplayManager _gameplayManager;
		private IAppStateManager _appStateManager;
		private IDataManager _dataManager;

		private CameraController _cameraController;

		public Player Player { get; private set; }

		public void Init()
		{
			_gameplayManager = GameClient.Get<IGameplayManager>();
            _appStateManager = GameClient.Get<IAppStateManager>();
			_dataManager = GameClient.Get<IDataManager>();

            Player = new Player(_gameplayManager.GameplayObject.transform);

            _appStateManager.AppStateChangedEvent += AppStateChangedEventHandler;
        }

        public void Dispose()
        {
            Player.Dispose();

            _appStateManager.AppStateChangedEvent -= AppStateChangedEventHandler;
        }

        public void ResetAll()
        {
        }

		public void Update()
		{
		}

		public void KillPlayer()
		{
            Player.Kill();
        }

        private void AppStateChangedEventHandler()
        {
            if (_appStateManager.AppState == Enumerators.AppState.Main ||
                _appStateManager.AppState == Enumerators.AppState.Game)
            {
                Player.SetPosition(_gameplayManager.GameplayData.playerSpawnPosition);
                Player.SetActive(true);
            }
        }
    }
}