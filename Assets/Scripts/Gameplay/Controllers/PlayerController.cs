using UnityEngine;

namespace HotForgeStudio.HorrorBox
{
    public class PlayerController : IController
	{
		private IGameplayManager _gameplayManager;
		private IDataManager _dataManager;

		private CameraController _cameraController;

		public Player Player { get; private set; }

		public void Init()
		{
			_gameplayManager = GameClient.Get<IGameplayManager>();
			_dataManager = GameClient.Get<IDataManager>();

            Player = new Player(_gameplayManager.GameplayObject.transform);
        }

        public void Dispose()
        {
            Player.Dispose();
        }

        public void ResetAll()
		{
			Player.SetPosition(_gameplayManager.GameplayData.playerSpawnPosition);
        }

		public void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				Player.Kill();
		}

		public void KillPlayer()
		{
            Player.Kill();
        }
	}
}