using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tortoise.HOPPER
{
    public class PlayerAttack2State : PlayerAttackState
    {
        public PlayerAttack2State(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #region IStateMethods
        public override void Enter()
        {
            _ComboStep = 2;

            base.Enter();
        }

        public override void AnimationTransitionEvent()
        {
            base.AnimationTransitionEvent();

            if (_ComboStep == 3)
            {
                _StateMachine.ChangeState(_StateMachine.Attack3State);
                return;
            }
        }
        #endregion

        #region InputMethods
        protected override void OnAttackPerformed(InputAction.CallbackContext ctx)
        {
            _ComboStep = 3;
        }
        #endregion
    }
}
