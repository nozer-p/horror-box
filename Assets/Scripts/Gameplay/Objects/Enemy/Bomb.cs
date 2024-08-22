using UnityEngine;

namespace HotForgeStudio.HorrorBox
{
    public class Bomb : EnemyBase
    {
        private EnemyController _enemyController;

        public Bomb() : base()
        {
            _enemyController = _gameplayManager.GetController<EnemyController>();
        }

        protected override void OnBehaviourHandlerTriggerEnteredHandler(Collider collider)
        {
            base.OnBehaviourHandlerTriggerEnteredHandler(collider);

            return;

            if (collider.gameObject.Equals(_enemyController.GetEnemy(collider.gameObject)))
            {
                _enemyController.KillEnemy(collider.gameObject);
                _enemyController.KillEnemy(SelfObject);
            }
        }
    }
}