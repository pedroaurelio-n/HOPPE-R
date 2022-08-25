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

            _BasicEnemy.AnimationHelper.SetAnimationBool(_BasicEnemy.AnimationData.ActiveParamHash, true);
            _BasicEnemy.AnimationHelper.SetAnimationBool(_BasicEnemy.AnimationData.MovingParamHash, false);

            _elapsedTime = 0f;

            _BasicEnemy.Agent.SetDestination(_BasicEnemy.transform.position);
            _BasicEnemy.Agent.isStopped = true;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            CheckForFollowTarget();

            if (_BasicEnemy.MoveArea == null)
                return;

            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= _BasicEnemy.Data.IdleDuration)
            {
                _StateMachine.ChangeState(_StateMachine.WanderState);
                return;
            }
        }
        #endregion
    }
}
