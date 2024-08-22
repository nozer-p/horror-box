using HotForgeStudio.HorrorBox.Common;
using UnityEngine;

namespace HotForgeStudio.HorrorBox
{
    public class ExplosionController : IController
    {
        private IAppStateManager _appStateManager;
        private ISoundManager _soundManager;

        private Transform _particleContainer;

        private GameObject _particleObjectPrefab;
        private GameObject _particleObject;

        public void Init()
        {
            _appStateManager = GameClient.Get<IAppStateManager>();
            _soundManager = GameClient.Get<ISoundManager>();

            _particleContainer = new GameObject("[ParticleContainer]").transform;
            _particleContainer.parent = MainApp.Instance.transform;

            _particleObjectPrefab = GameClient.Get<ILoadObjectsManager>()
                .GetObjectByPath<GameObject>("Prefabs/Gameplay/Explosion");

            _appStateManager.AppStateChangedEvent += AppStateChangedEventHandler;
        }

        public void Dispose()
        {
            _appStateManager.AppStateChangedEvent -= AppStateChangedEventHandler;

            CleanUp();
        }

        public void ResetAll()
        {
        }

        public void Update()
        {
        }

        public void SpawnExplosion(Vector3 position)
        {
            _soundManager.PlaySound(Enumerators.SoundType.Explosion);

            _particleObject = MonoBehaviour.Instantiate(_particleObjectPrefab,
                position, Quaternion.identity, _particleContainer);
        }

        private void CleanUp()
        {
            if (_particleObject != null)
                MonoBehaviour.Destroy(_particleObject);
        }

        private void AppStateChangedEventHandler()
        {
            if (_appStateManager.AppState == Enumerators.AppState.Main ||
                _appStateManager.AppState == Enumerators.AppState.Game)
                CleanUp();
        }
    }
}