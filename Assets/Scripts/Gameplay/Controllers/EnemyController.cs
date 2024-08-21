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

        private List<EnemyBase> _enemies;

        private EnemyInfo _enemyInfo;

        public Transform EnemiesContainer { get; private set; }

        public void Init()
        {
            _gameplayManager = GameClient.Get<IGameplayManager>();
            _dataManager = GameClient.Get<IDataManager>();

            _enemies = new List<EnemyBase>();

            _enemyInfo = _gameplayManager.GameplayData.enemyInfo;

            EnemiesContainer = new GameObject("[EnemiesContainer]").transform;
            EnemiesContainer.parent = _gameplayManager.GameplayObject.transform;

            _gameplayManager.GameplayStartedEvent += GameplayStartedEventHandler;
        }

        public void Dispose()
        {
            _gameplayManager.GameplayStartedEvent -= GameplayStartedEventHandler;
        }

        public void ResetAll()
        {
            _enemies.ForEach(e => e.Dispose());
        }

        public void Update()
        {
            if (_gameplayManager.IsGameplayStarted && !_gameplayManager.IsGameplayPaused)
            {
                _enemies.ForEach(e => e.Update());
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
                enemy.Dispose();
            }
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
            enemy?.Init(EnemiesContainer, new Vector2(Random.Range(-2f, -15f), Random.Range(-2f, -15f)), enemyType);
            return enemy;
        }

        private void GameplayStartedEventHandler()
        {
            _enemies.Add(CreateEnemy(EnemyType.Cube));
            _enemies.Add(CreateEnemy(EnemyType.Cube));
            _enemies.Add(CreateEnemy(EnemyType.Bomb));
        }
    }
}