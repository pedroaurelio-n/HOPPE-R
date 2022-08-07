using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tortoise.HOPPER
{
    public class PlayerSprintState : PlayerGroundedState
    {
        public PlayerSprintState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #region IStateMethods
        public override void Enter()
        {
            base.Enter();

            _Player.SetAnimationBool(_Player.AnimationData.SprintingParamHash, true);

            _StateMachine.SpeedModifier = _Player.Data.SprintSpeedModifier;
        }

        public override void Exit()
        {
            base.Exit();
            
            // _Player.SetAnimationBool(_Player.AnimationData.SprintingParamHash, false);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (_StateMachine.MovementInput == Vector2.zero || _StateMachine.SlopeSpeedModifier < _Player.Data.SprintMaxSlopeValue)
            {
                _StateMachine.ChangeState(_StateMachine.LocomotionState);
                return;
            }
        }
        #endregion

        #region ReusableMethods
        protected override void AddInputCallbacks()
        {
            base.AddInputCallbacks();

            _Player.Input.PlayerActions.Sprint.canceled += OnSprintCanceled;
        }

        protected override void RemoveInputCallbacks()
        {
            base.RemoveInputCallbacks();

            _Player.Input.PlayerActions.Sprint.canceled -= OnSprintCanceled;
        }
        #endregion

        #region InputMethods
        private void OnSprintCanceled(InputAction.CallbackContext ctx)
        {
            _StateMachine.ChangeState(_StateMachine.LocomotionState);
        }
        #endregion
    }
}
