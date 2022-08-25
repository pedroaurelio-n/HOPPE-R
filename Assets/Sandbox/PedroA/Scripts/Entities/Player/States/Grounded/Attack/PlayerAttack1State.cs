using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tortoise.HOPPER
{
    public class PlayerAttack1State : PlayerAttackState
    {
        public PlayerAttack1State(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #region IStateMethods
        public override void Enter()
        {
            _ComboStep = 1;

            base.Enter();
        }

        public override void AnimationTransitionEvent()
        {
            base.AnimationTransitionEvent();

            if (_ComboStep == 2)
            {
                _StateMachine.ChangeState(_StateMachine.Attack2State);
                return;
            }
        }
        #endregion

        #region InputMethods
        protected override void OnAttackPerformed(InputAction.CallbackContext ctx)
        {
            _ComboStep = 2;
        }
        #endregion
    }
}
