using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class BasicEnemyWanderState : BasicEnemyBaseState
    {
        public BasicEnemyWanderState(BasicEnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        #region IStateMethods
        public override void Enter()
        {
            base.Enter();

            _BasicEnemy.Agent.isStopped = false;
            _StateMachine.StopFollow = false;

            var areaExtents = _BasicEnemy.MoveArea.size * 0.5f;
            var randomPoint = new Vector3(
                Random.Range(-areaExtents.x, areaExtents.x),
                Random.Range(-areaExtents.y, areaExtents.y),
                Random.Range(-areaExtents.z, areaExtents.z)
            ) + _BasicEnemy.MoveArea.center;

            var worldPoint = _BasicEnemy.MoveArea.transform.TransformPoint(randomPoint);

            _BasicEnemy.Agent.SetDestination(worldPoint);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            CheckForFollowTarget();

            if (_BasicEnemy.Agent.remainingDistance < _BasicEnemy.Data.MinPointDistance)
            {
                _StateMachine.ChangeState(_StateMachine.IdleState);
                return;
            }
        }
        #endregion
    }
}
