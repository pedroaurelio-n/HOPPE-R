using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tortoise.HOPPER
{
    public class PlayerAttackState : PlayerGroundedState
    {
        protected int _ComboStep;

        public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #region IStateMethods
        public override void Enter()
        {
            base.Enter();

            _Player.AnimationHelper.SetAnimationBool(_Player.AnimationData.AttackingParamHash, true);

            _Player.AnimationHelper.SetAnimationInt(_Player.AnimationData.ComboStepParamHash, _ComboStep);

            _StateMachine.SpeedModifier = 0f;

            _Player.AnimationHelper.SetRootMotion(true);
        }

        public override void Exit()
        {
            base.Exit();

            _Player.AnimationHelper.SetAnimationBool(_Player.AnimationData.AttackingParamHash, false);

            _Player.AnimationHelper.SetRootMotion(false);

            _ComboStep = 0;
        }

        public override void AnimationExitEvent()
        {
            base.AnimationExitEvent();

            if (_Player.Input.PlayerActions.Sprint.IsPressed())
            {
                _StateMachine.ChangeState(_StateMachine.SprintState);
                return;
            }
            _StateMachine.ChangeState(_StateMachine.LocomotionState);
        }
        #endregion

        #region ReusableMethods
        protected override void AddInputCallbacks()
        {
            base.AddInputCallbacks();

            _Player.Input.PlayerActions.Attack.performed += OnAttackPerformed;
        }

        protected override void RemoveInputCallbacks()
        {
            base.RemoveInputCallbacks();

            _Player.Input.PlayerActions.Attack.performed -= OnAttackPerformed;
        }
        #endregion

        #region InputMethods
	    protected override void OnJumpPerformed(InputAction.CallbackContext ctx)
        {
        }
        #endregion
    }
}
