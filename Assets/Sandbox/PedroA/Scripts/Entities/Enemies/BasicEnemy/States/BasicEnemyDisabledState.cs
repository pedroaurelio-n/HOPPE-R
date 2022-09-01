using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class BasicEnemyDisabledState : BasicEnemyBaseState
    {
        public BasicEnemyDisabledState(BasicEnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        #region IStateMethods
        public override void Enter()
        {
            base.Enter();

            _BasicEnemy.AnimationHelper.SetAnimationBool(_BasicEnemy.AnimationData.ActiveParamHash, false);
            _BasicEnemy.AnimationHelper.SetAnimationBool(_BasicEnemy.AnimationData.MovingParamHash, false);

            _BasicEnemy.Rigidbody.velocity = Vector3.zero;
            _BasicEnemy.Rigidbody.isKinematic = true;

            _BasicEnemy.Agent.SetDestination(_BasicEnemy.transform.position);
            _BasicEnemy.Agent.isStopped = true;
        }
        #endregion
    }
}
