using UnityEngine;

namespace HotForgeStudio.HorrorBox
{
    public class Cube : EnemyBase
    {
        private MatchController _matchController;

        public Cube() : base()
        {
            _matchController = _gameplayManager.GetController<MatchController>();
        }

        public override void Update()
        {
            base.Update();

            if (_transform != null && _playerController.Player.Transform != null)
            {
                float step = _enemyInfo.cubeDefaultSpeed * Mathf.Max((_matchController.GameplaySeconds %
                    _enemyInfo.cubeIncreaseSpeedTime) * _enemyInfo.cubeSpeedIncrease, 1f) * Time.deltaTime;

                _transform.position = Vector3.MoveTowards(_transform.position,
                    _playerController.Player.Transform.position, step);

                Vector3 targetDirection = _playerController.Player.Transform.position - _transform.position;
                float singleStep = _enemyInfo.cubeDefaultSpeed * Time.deltaTime;
                Vector3 newDirection = Vector3.RotateTowards(_transform.forward, targetDirection, singleStep, 0.0f);
                _transform.rotation = Quaternion.LookRotation(newDirection);
            }
        }
    }
}