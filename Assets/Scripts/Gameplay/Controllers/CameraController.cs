using UnityEngine;

namespace HotForgeStudio.HorrorBox
{
    public class CameraController : IController
	{
		private IGameplayManager _gameplayManager;

		private GameObject _cameraObject,
						   _cameraTarget;

		private Vector3 _cameraOffset;

		public float CurrentCameraDistance { get; private set; }

		public Camera GameplayCamera { get; private set; }

		private Vector3 _forward, _right;

		public void Init()
		{
			_gameplayManager = GameClient.Get<IGameplayManager>();

            _cameraObject = MainApp.Instance.transform.Find("Camera").gameObject;
            GameplayCamera = _cameraObject.GetComponent<Camera>();
            _forward = _cameraObject.transform.forward;
            _forward.y = 0;
            _forward = Vector3.Normalize(_forward);
            _right = Quaternion.Euler(new Vector3(0, 90, 0)) * _forward;

            SetCameraOffset(new Vector3(0, 1, 0));
            SetCameraDistance(_gameplayManager.GameplayData.cameraDistance);
		}

        public void Dispose()
        {
        }

        public void ResetAll()
		{

		}

		public void Update()
		{
			if (_cameraTarget != null)
            {
				UpdateCameraPosition();
			}
		}

		public void SetCameraTarget(GameObject cameraTarget)
        {
			_cameraTarget = cameraTarget;
		}

        public Vector3 GetMovementDirection(Vector2 direction)
        {
            Vector3 rightMove = _right * direction.x;
            Vector3 upMove = _forward * direction.y;
            return rightMove + upMove;
        }

        private void SetCameraOffset(Vector3 cameraOffset)
        {
			_cameraOffset = cameraOffset;
		}

		private void SetCameraDistance(float distance)
        {
			CurrentCameraDistance = distance;
        }

		private void UpdateCameraPosition()
        {
			var newPosition = _cameraTarget.transform.position + _cameraOffset;
			newPosition -= _cameraObject.transform.forward * CurrentCameraDistance;

			_cameraObject.transform.position = 
				Vector3.Lerp(_cameraObject.transform.position,
					newPosition, Time.deltaTime * _gameplayManager.GameplayData.cameraMovementSpeed);
		}
	}
}