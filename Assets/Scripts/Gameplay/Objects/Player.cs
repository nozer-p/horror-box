using UnityEngine;
using HotForgeStudio.HorrorBox.Common;

namespace HotForgeStudio.HorrorBox
{
    public class Player
    {
        private GameObject _selfObject;

        private IGameplayManager _gameplayManager;
        private IInputManager _inputManager;
        private IAppStateManager _appStateManager;

        private CharacterController _characterController;

        private CameraController _cameraController;
        private PlayerController _playerController;

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

            _selfObject = MonoBehaviour.Instantiate(GameClient.Get<ILoadObjectsManager>().
                GetObjectByPath<GameObject>($"Prefabs/Gameplay/Player"), spawnPosition, false);

            _characterController = _selfObject.GetComponent<CharacterController>();

            _cameraController.SetCameraTarget(_selfObject);

            _inputMoveIndex = _inputManager.RegisterInputHandler(Enumerators.InputType.Joystick, 0, onInputEndParametrized: OnInputJoystickHandler);
        }

        public void Dispose()
        {
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
            _selfObject.SetActive(value);
        }

        public void Kill()
        {
            //SetActive(false);
            _appStateManager.ChangeAppState(Enumerators.AppState.GameOver);
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
            Vector3 joysticDirection = new Vector2(horizontal, vertical);
            Vector3 moveDirection = _cameraController.GetMovementDirection(joysticDirection);

            _characterController.SimpleMove(moveDirection * _gameplayManager.GameplayData.playerSpeed);
        }
    }
}