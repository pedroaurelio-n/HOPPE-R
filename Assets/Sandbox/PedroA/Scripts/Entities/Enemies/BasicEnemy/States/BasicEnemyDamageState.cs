using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class BasicEnemyDamageState : BasicEnemyBaseState
    {
        public BasicEnemyDamageState(BasicEnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        #region IStateMethods
        public override void Enter()
        {
            base.Enter();

            _BasicEnemy.AnimationHelper.SetAnimationBool(_BasicEnemy.AnimationData.DamageParamHash, true);

            _BasicEnemy.Agent.isStopped = true;
            _BasicEnemy.Rigidbody.isKinematic = false;
            _BasicEnemy.Agent.velocity = Vector3.zero;

            _BasicEnemy.AnimationHelper.SetAnimationBool(_BasicEnemy.AnimationData.MovingParamHash, false);
            _BasicEnemy.AnimationHelper.SetAnimationBool(_BasicEnemy.AnimationData.AttackingParamHash, false);
        }

        public override void AnimationExitEvent()
        {
            base.AnimationExitEvent();

            _BasicEnemy.Rigidbody.isKinematic = true;
            _BasicEnemy.Agent.isStopped = false;

            _StateMachine.CurrentCooldown = _BasicEnemy.Data.DamageCooldown;

            _StateMachine.ChangeState(_StateMachine.IdleState);

            _BasicEnemy.AnimationHelper.SetAnimationBool(_BasicEnemy.AnimationData.DamageParamHash, false);

        }
        #endregion
    }
}
