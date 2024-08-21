using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static GameplayData;
using static HotForgeStudio.HorrorBox.Common.Enumerators;

namespace HotForgeStudio.HorrorBox
{
    public abstract class EnemyBase
    {
        public GameObject SelfObject { get; protected set; }

        protected IGameplayManager _gameplayManager;

        protected Transform _transform => SelfObject != null ? SelfObject.transform : null;

        private OnBehaviourHandler _onBehaviourHandler;

        protected PlayerController _playerController;

        protected EnemyInfo _enemyInfo;

        private readonly Dictionary<EnemyType, string> _prefabsName = new Dictionary<EnemyType, string>()
        {
            { EnemyType.Cube, "Cube" },
            { EnemyType.Bomb, "Bomb" },
        };

        public EnemyBase()
        {
            _gameplayManager = GameClient.Get<IGameplayManager>();

            _playerController = _gameplayManager.GetController<PlayerController>();

            _enemyInfo = _gameplayManager.GameplayData.enemyInfo;
        }

        public void Init(Transform parent, Vector2 position, EnemyType enemyType)
        {
            GameObject gameObject = GameClient.Get<ILoadObjectsManager>().
                GetObjectByPath<GameObject>($"Prefabs/Gameplay/{_prefabsName[enemyType]}");
            SelfObject = MonoBehaviour.Instantiate(gameObject, new Vector3(position.x,
                gameObject.transform.position.y, position.y), Quaternion.identity, parent);

            _onBehaviourHandler = SelfObject.GetComponent<OnBehaviourHandler>();

            _onBehaviourHandler.TriggerEntered += OnBehaviourHandlerTriggerEnteredHandler;
        }

        public void Dispose()
        {
            if (SelfObject != null)
                MonoBehaviour.Destroy(SelfObject);

            _onBehaviourHandler.TriggerEntered -= OnBehaviourHandlerTriggerEnteredHandler;
        }

        public void Kill()
        {
        }

        public virtual void Update()
        {
        }

        protected virtual void OnBehaviourHandlerTriggerEnteredHandler(Collider collider)
        {
            if (collider.gameObject.Equals(_playerController.Player.Transform.gameObject))
            {
                _playerController.KillPlayer();
            }
        }
    }
}