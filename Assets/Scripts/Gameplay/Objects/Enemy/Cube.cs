using HotForgeStudio.HorrorBox.Common;
using UnityEngine;

namespace HotForgeStudio.HorrorBox
{
    public class Cube : EnemyBase
    {
        private MatchController _matchController;

        private CharacterController _characterController;

        public Cube() : base()
        {
            _matchController = _gameplayManager.GetController<MatchController>();
        }

        public override void Init(Transform parent, Vector2 position, Enumerators.EnemyType enemyType)
        {
            base.Init(parent, position, enemyType);

            _characterController = SelfObject.GetComponent<CharacterController>();
        }

        public override void Update()
        {
            base.Update();

            if (_transform != null && _playerController.Player.Transform != null)
            {
                float currentSpeed = _enemyInfo.cubeDefaultSpeed + 
                    _matchController.GameplaySeconds / _enemyInfo.cubeChangeDataTime *
                    _enemyInfo.cubeSpeedIncrease;
                Vector3 targetPosition = _playerController.Player.Transform.position;
                targetPosition.y = _transform.position.y;
                Vector3 moveDirection = (targetPosition - _transform.position).normalized;
                _characterController.SimpleMove(moveDirection * currentSpeed);

                Vector3 targetDirection = _playerController.Player.Transform.position - _transform.position;
                targetDirection.y = 0;
                float singleStep = currentSpeed * Time.deltaTime;
                Vector3 newDirection = Vector3.RotateTowards(_transform.forward, targetDirection, singleStep, 0.0f);
                _transform.rotation = Quaternion.LookRotation(newDirection);
            }
        }
    }
}