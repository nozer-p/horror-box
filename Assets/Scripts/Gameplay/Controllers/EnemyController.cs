using HotForgeStudio.HorrorBox.Common;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameplayData;
using static HotForgeStudio.HorrorBox.Common.Enumerators;

namespace HotForgeStudio.HorrorBox
{
    public class EnemyController : IController
    {
        private IGameplayManager _gameplayManager;
        private IDataManager _dataManager;
        private IAppStateManager _appStateManager;

        private PlayerController _playerController;
        private MatchController _matchController;

        private List<EnemyBase> _enemies;

        private EnemyInfo _enemyInfo;

        public Transform EnemiesContainer { get; private set; }

        private float _timer;

        public void Init()
        {
            _gameplayManager = GameClient.Get<IGameplayManager>();
            _dataManager = GameClient.Get<IDataManager>();
            _appStateManager = GameClient.Get<IAppStateManager>();

            _playerController = _gameplayManager.GetController<PlayerController>();
            _matchController = GameClient.Get<IGameplayManager>().GetController<MatchController>();

            _enemies = new List<EnemyBase>();

            _enemyInfo = _gameplayManager.GameplayData.enemyInfo;

            EnemiesContainer = new GameObject("[EnemiesContainer]").transform;
            EnemiesContainer.parent = _gameplayManager.GameplayObject.transform;

            _appStateManager.AppStateChangedEvent += AppStateChangedEventHandler;
            _gameplayManager.GameplayStartedEvent += GameplayStartedEventHandler;
        }

        public void Dispose()
        {
            _appStateManager.AppStateChangedEvent -= AppStateChangedEventHandler;
            _gameplayManager.GameplayStartedEvent -= GameplayStartedEventHandler;
        }

        public void ResetAll()
        {
        }

        public void Update()
        {
            if (_gameplayManager.IsGameplayStarted && !_gameplayManager.IsGameplayPaused)
            {
                SpawnEnemy();
                _enemies?.ForEach(e => e.Update());
            }
        }

        public GameObject GetEnemy(GameObject gameObject)
        {
            EnemyBase enemy = _enemies.FirstOrDefault(e => e.SelfObject.Equals(gameObject));
            return enemy != null ? enemy.SelfObject : null;
        }

        public void KillEnemy(GameObject gameObject)
        {
            EnemyBase enemy = _enemies.FirstOrDefault(e => e.SelfObject.Equals(gameObject));
            if (enemy != null)
            {
                _enemies.Remove(enemy);
                enemy.Kill();
                enemy.Dispose();
            }
        }

        private float SpawnTime()
        {
            return _enemyInfo.cubeSpawnTime - _matchController.GameplaySeconds /
                _enemyInfo.cubeChangeDataTime * _enemyInfo.cubeSpawnTimeDecrease;
        }

        private void SpawnEnemy()
        {
            if (_enemies.FindAll(e => e.EnemyType == EnemyType.Cube).Count >= _enemyInfo.maxSpawnedCubes)
                return;

            _timer -= Time.deltaTime;

            if (!IsSpawnAvailable())
                return;
            
            _timer = SpawnTime();

            CreateEnemy(EnemyType.Cube);
        }

        private bool IsSpawnAvailable()
        {
            return _timer <= 0f;
        }

        private EnemyBase CreateEnemy(EnemyType enemyType)
        {
            EnemyBase enemy = null;
            switch (enemyType)
            {
                case EnemyType.Cube:
                    enemy = new Cube();
                    break;
                case EnemyType.Bomb:
                    enemy = new Bomb();
                    break;
            }

            if (enemy != null)
            {
                _enemies.Add(enemy);
                enemy?.Init(EnemiesContainer, GetSpawnPosition(), enemyType);
            }

            return enemy;
        }

        private Vector2 GetSpawnPosition()
        {
            float distance = Random.Range(_enemyInfo.enemySpawnMinDistanceFromPlayer,
                _enemyInfo.enemySpawnMaxDistanceFromPlayer);

            float angle = Random.Range(0f, Mathf.PI * 2);

            float offsetX = Mathf.Cos(angle) * distance;
            float offsetZ = Mathf.Sin(angle) * distance;

            Vector2 spawnPosition = new Vector3(_playerController.Player.Transform.position.x + offsetX,
                _playerController.Player.Transform.position.z + offsetZ);

            float enemySpawnPositionLimit = _enemyInfo.enemySpawnPositionLimit;
            spawnPosition.x = Mathf.Clamp(spawnPosition.x, -enemySpawnPositionLimit, enemySpawnPositionLimit);
            spawnPosition.y = Mathf.Clamp(spawnPosition.y, -enemySpawnPositionLimit, enemySpawnPositionLimit);

            return spawnPosition;
        }

        private void CleanUp()
        {
            _timer = 0f;

            _enemies.ForEach(e => e.Dispose());
            _enemies.Clear();
        }

        private void AppStateChangedEventHandler()
        {
            if (_appStateManager.AppState == Enumerators.AppState.Main ||
                _appStateManager.AppState == Enumerators.AppState.Game)
                CleanUp();
        }

        private void GameplayStartedEventHandler()
        {
            CleanUp();

            for (int i = 0; i < _enemyInfo.maxSpawnedBombs; i++)
                CreateEnemy(EnemyType.Bomb);
        }
    }
}