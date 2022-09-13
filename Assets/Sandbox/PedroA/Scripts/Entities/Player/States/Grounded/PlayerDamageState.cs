using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
 
namespace Tortoise.HOPPER
{
    public class PlayerDamageState : PlayerGroundedState
    {
        public PlayerDamageState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #region IStateMethods
        public override void Enter()
        {
            base.Enter();

            _StateMachine.SpeedModifier = 0f;

            _Player.AnimationHelper.SetAnimationBool(_Player.AnimationData.AttackingParamHash, false);
            _Player.AnimationHelper.SetAnimationBool(_Player.AnimationData.DamageParamHash, true);
        }

        public override void Exit()
        {
            base.Exit();

            _Player.AnimationHelper.SetAnimationBool(_Player.AnimationData.DamageParamHash, false);
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
        protected override void ReadMovementInput()
        {
        }
        #endregion

        #region InputMethods
	    protected override void OnJumpPerformed(InputAction.CallbackContext ctx)
        {
        }
        #endregion
    }
}