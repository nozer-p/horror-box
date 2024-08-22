using System;
using UnityEngine;

namespace HotForgeStudio.HorrorBox
{
    public class MatchController : IController
    {
        public Action GameplaySecondsUpdatedEvent;

        private IGameplayManager _gameplayManager;
        private IDataManager _dataManager;

        public int GameplaySeconds { get; private set; }

        private float _timer;

        private int _previousGameplaySeconds;

        public void Init()
        {
            _gameplayManager = GameClient.Get<IGameplayManager>();
            _dataManager = GameClient.Get<IDataManager>();

            _gameplayManager.GameplayStartedEvent += GameplayStartedEventHandler;
            _gameplayManager.GameplayEndedEvent += GameplayEndedEventHandler;
        }

        public void Dispose()
        {
            _gameplayManager.GameplayStartedEvent -= GameplayStartedEventHandler;
            _gameplayManager.GameplayEndedEvent -= GameplayEndedEventHandler;
        }

        public void ResetAll()
        {
        }

        public void Update()
        {
            if (_gameplayManager.IsGameplayStarted && !_gameplayManager.IsGameplayPaused)
            {
                _timer += Time.deltaTime;

                int gameplaySeconds = Mathf.CeilToInt(_timer);

                if (gameplaySeconds > _previousGameplaySeconds)
                {
                    GameplaySeconds = gameplaySeconds;
                    _previousGameplaySeconds = gameplaySeconds;

                    GameplaySecondsUpdatedEvent?.Invoke();
                }
            }
        }

        private void GameplayStartedEventHandler()
        {
            _timer = 0f;
            _previousGameplaySeconds = 0;
            GameplaySeconds = 0;
        }

        private void GameplayEndedEventHandler()
        {
            GameplaySeconds = Mathf.CeilToInt(_timer);

            if (GameplaySeconds > _dataManager.CachedUserLocalData.bestTimeSurvived)
            {
                _dataManager.CachedUserLocalData.bestTimeSurvived = GameplaySeconds;
                _dataManager.SaveAllData();
            }
        }
    }
}