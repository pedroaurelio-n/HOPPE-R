using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tortoise.HOPPER
{
    public class PlayerLocomotionState : PlayerGroundedState
    {
        public PlayerLocomotionState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #region IStateMethods
        public override void Enter()
        {
            base.Enter();

            _Player.SetAnimationBool(_Player.AnimationData.SprintingParamHash, false);

            _StateMachine.SpeedModifier = 1f;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            _Player.SetAnimationFloat(_Player.AnimationData.SpeedParamHash, _StateMachine.MoveAmount);

            // if (_StateMachine.SlopeSpeedModifier == 0f && _Player.Input.PlayerActions.Jump.enabled)
            // {
            //     _Player.Input.PlayerActions.Jump.Disable();
            // }
            // else if (!_Player.Input.PlayerActions.Jump.enabled && _StateMachine.SlopeSpeedModifier != 0f)
            // {
            //     _Player.Input.PlayerActions.Jump.Enable();
            // }

            if (_StateMachine.MovementInput == Vector2.zero)
                return;
            
            if (_Player.Input.PlayerActions.Sprint.IsPressed() && _StateMachine.SlopeSpeedModifier >= _Player.Data.SprintMaxAngle)
            {
                _StateMachine.ChangeState(_StateMachine.SprintState);
                return;
            }
        }
        #endregion 

        // #region ReusableMethods
        // protected override void AddInputCallbacks()
        // {
        //     base.AddInputCallbacks();
            
        //     _Player.Input.PlayerActions.Sprint.started += OnSprintPerformed;
        // }

        // protected override void RemoveInputCallbacks()
        // {
        //     base.RemoveInputCallbacks();

        //     _Player.Input.PlayerActions.Sprint.started -= OnSprintPerformed;
        // }
        // #endregion

        // #region InputMethods
        // protected void OnSprintPerformed(InputAction.CallbackContext ctx)
        // {
        //     if (_StateMachine.MovementInput == Vector2.zero)
        //         return;

        //     _StateMachine.ChangeState(_StateMachine.SprintState);
        // }
        // #endregion
    }
}
