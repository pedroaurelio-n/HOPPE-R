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

            _BasicEnemy.Agent.speed = _BasicEnemy.Data.MoveSpeed;
            _BasicEnemy.Agent.angularSpeed = _BasicEnemy.Data.RotationSpeed;

            _BasicEnemy.AnimationHelper.SetAnimationBool(_BasicEnemy.AnimationData.ActiveParamHash, true);
            _BasicEnemy.AnimationHelper.SetAnimationBool(_BasicEnemy.AnimationData.MovingParamHash, false);

            _elapsedTime = 0f;

            _BasicEnemy.Agent.SetDestination(_BasicEnemy.transform.position);
            _BasicEnemy.Agent.isStopped = true;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (_StateMachine.CurrentCooldown > 0)
                return;

            if (CanAttack())
            {
                _StateMachine.ChangeState(_StateMachine.AttackState);
                return;
            }

            if (CanFollowTarget())
            {
                _StateMachine.ChangeState(_StateMachine.FollowState);
                return;
            }

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
