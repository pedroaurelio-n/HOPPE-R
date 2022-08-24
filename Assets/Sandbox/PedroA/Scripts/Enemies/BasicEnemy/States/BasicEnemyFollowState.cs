using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class BasicEnemyFollowState : BasicEnemyBaseState
    {
        public BasicEnemyFollowState(BasicEnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        #region IStateMethods
        public override void Enter()
        {
            base.Enter();

            _BasicEnemy.Agent.isStopped = false;

            _BasicEnemy.Agent.SetDestination(_BasicEnemy.Target.position);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            _BasicEnemy.Agent.SetDestination(_BasicEnemy.Target.position);

            if (ShouldStopFollow())
            {
                _StateMachine.StopFollow = true;
                _StateMachine.ChangeState(_StateMachine.IdleState);
                return;
            }
        }
        #endregion

        #region MainMethods
        private bool ShouldStopFollow()
        {
            var isFarFromTarget = Vector3.Distance(_BasicEnemy.transform.position, _BasicEnemy.Target.position) > _BasicEnemy.Data.MaxDistanceFromTarget;
            var isFarFromArea = Vector3.Distance(_BasicEnemy.transform.position, _BasicEnemy.MoveArea.bounds.center) > _BasicEnemy.Data.MaxDistanceFromArea;
            var isVerticallyDistant = Mathf.Abs(_BasicEnemy.Target.position.y - _BasicEnemy.transform.position.y) > _BasicEnemy.Data.MaxYDifference;

            return isFarFromArea || isFarFromTarget || isVerticallyDistant;
        }
        #endregion
    }
}
