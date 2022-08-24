using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class BasicEnemyBaseState : IState
    {
        protected BasicEnemyStateMachine _StateMachine;
        protected BasicEnemy _BasicEnemy;

        public BasicEnemyBaseState(BasicEnemyStateMachine stateMachine)
        {
            _StateMachine = stateMachine;
            _BasicEnemy = _StateMachine.BasicEnemy;
        }

        #region IStateMethods
        public virtual void Enter()
        {
        }

        public virtual void Exit()
        {
        }

        public virtual void HandleInput()
        {
        }

        public virtual void LogicUpdate()
        {
        }

        public virtual void PhysicsUpdate()
        {
        }

        public virtual void EnterTrigger(Collider collider)
        {
        }

        public virtual void ExitTrigger(Collider collider)
        {
        }

        public virtual void AnimationEnterEvent()
        {
        }

        public virtual void AnimationExitEvent()
        {
        }

        public virtual void AnimationTransitionEvent()
        {
        }
        #endregion
    }
}
