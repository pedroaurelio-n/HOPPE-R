using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class BasicEnemyIdleState : BasicEnemyBaseState
    {
        private float _elapsedTime;

        public BasicEnemyIdleState(BasicEnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        #region IStateMethods
        public override void Enter()
        {
            base.Enter();

            _elapsedTime = 0f;

            _BasicEnemy.Agent.SetDestination(_BasicEnemy.transform.position);
            _BasicEnemy.Agent.isStopped = true;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= _BasicEnemy.Data.IdleDuration)
            {
                _StateMachine.ChangeState(_StateMachine.WanderState);
            }
        }
        #endregion
    }
}
