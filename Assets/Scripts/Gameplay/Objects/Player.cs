using UnityEngine;
using HotForgeStudio.HorrorBox.Common;
using DG.Tweening;

namespace HotForgeStudio.HorrorBox
{
    public class Player
    {
        private IGameplayManager _gameplayManager;
        private IInputManager _inputManager;
        private IAppStateManager _appStateManager;

        private CharacterController _characterController;

        private CameraController _cameraController;
        private PlayerController _playerController;
        private ExplosionController _explosionController;

        private GameObject _selfObject;
        private GameObject _capsuleObject;

        private Transform _lightPointTransform;

        public Transform Transform => _selfObject.transform;

        private int _inputMoveIndex;

        private Vector3 _previousPosition;

        public Player(Transform spawnPosition)
        {
            _gameplayManager = GameClient.Get<IGameplayManager>();
            _inputManager = GameClient.Get<IInputManager>();
            _appStateManager = GameClient.Get<IAppStateManager>();

            _cameraController = _gameplayManager.GetController<CameraController>();
            _playerController = _gameplayManager.GetController<PlayerController>();
            _explosionController = _gameplayManager.GetController<ExplosionController>();

            _selfObject = MonoBehaviour.Instantiate(GameClient.Get<ILoadObjectsManager>().
                GetObjectByPath<GameObject>($"Prefabs/Gameplay/Player"), spawnPosition, false);

            _capsuleObject = _selfObject.transform.Find("Object").gameObject;

            _lightPointTransform = _selfObject.transform.Find("Light_Point");

            _characterController = _selfObject.GetComponent<CharacterController>();

            _cameraController.SetCameraTarget(_selfObject);
            AnimateLight();

            _inputMoveIndex = _inputManager.RegisterInputHandler(Enumerators.InputType.Joystick, 0, onInputEndParametrized: OnInputJoystickHandler);

            SetActive(false);
        }

        public void Dispose()
        {
            _lightPointTransform.DOComplete();

            if (_selfObject != null)
                MonoBehaviour.Destroy(_selfObject);

            _inputManager.UnregisterInputHandler(_inputMoveIndex);
        }

        public void SetPosition(Vector3 spawnPosition) 
        {
            _selfObject.transform.position = spawnPosition;
        }

        public void SetActive(bool value)
        {
            _capsuleObject.SetActive(value);
            _characterController.enabled = value;
        }

        public void Kill()
        {
            SetActive(false);
            _explosionController.SpawnExplosion(_selfObject.transform.position);
            _appStateManager.ChangeAppState(Enumerators.AppState.GameOver);
        }

        private void AnimateLight()
        {
            float positionY = _lightPointTransform.position.y;
            _lightPointTransform.DOMoveY(positionY + 3f, 1.5f)
                .SetEase(Ease.InOutFlash)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void OnInputJoystickHandler(object MoveDirection)
        {
            if (_gameplayManager.IsGameplayStarted && !_gameplayManager.IsGameplayPaused)
            {
                object[] param = (object[])MoveDirection;

                Move((float)param[0], (float)param[1]);
            }
        }

        private void Move(float horizontal, float vertical)
        {
            if (!_characterController.enabled)
                return;

            Vector3 joysticDirection = new Vector2(horizontal, vertical);
            Vector3 moveDirection = _cameraController.GetMovementDirection(joysticDirection);
            _characterController.SimpleMove(moveDirection * _gameplayManager.GameplayData.playerSpeed);
        }
    }
}