using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class BasicEnemyAttackState : BasicEnemyBaseState
    {
        public BasicEnemyAttackState(BasicEnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        #region IStateMethods
        public override void Enter()
        {
            base.Enter();

            _BasicEnemy.AnimationHelper.SetAnimationBool(_BasicEnemy.AnimationData.AttackingParamHash, true);

            _BasicEnemy.Agent.speed = _BasicEnemy.Data.AttackMoveSpeed;
            _BasicEnemy.Agent.angularSpeed = _BasicEnemy.Data.AttackRotationSpeed;
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            Rotate(_BasicEnemy.Target.position - _BasicEnemy.transform.position);
        }

        public override void AnimationExitEvent()
        {
            base.AnimationExitEvent();

            _BasicEnemy.AnimationHelper.SetAnimationBool(_BasicEnemy.AnimationData.AttackingParamHash, false);

            _StateMachine.CurrentCooldown = _BasicEnemy.Data.AttackCooldown;

            _StateMachine.ChangeState(_StateMachine.IdleState);
        }
        #endregion

        #region MainMethods
        private void Rotate(Vector3 direction)
        {
            var lookDirection = Quaternion.LookRotation(direction.normalized, Vector3.up);
            _BasicEnemy.transform.rotation = Quaternion.RotateTowards(_BasicEnemy.transform.rotation, lookDirection, _BasicEnemy.Data.AttackRotationSpeed);
        }
        #endregion
    }
}
