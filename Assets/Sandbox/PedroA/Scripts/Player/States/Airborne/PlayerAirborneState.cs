using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tortoise.HOPPER
{
    public class PlayerAirborneState : PlayerBaseState
    {
        public PlayerAirborneState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #region IStateMethods
        public override void Enter()
        {
            base.Enter();

            _StateMachine.SlopeSpeedModifier = 1f;
            
            _Player.AnimationHelper.SetAnimationBool(_Player.AnimationData.GroundedParamHash, false);
            _Player.AnimationHelper.SetAnimationBool(_Player.AnimationData.AirborneParamHash, true);
        }
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();

            _StateMachine.SpeedModifier = Mathf.MoveTowards(_StateMachine.SpeedModifier, 1f, _Player.Data.AirCounterSprint * Time.deltaTime);
        }

        public override void EnterTrigger(Collider collider)
        {
            base.EnterTrigger(collider);

            if (_Player.Input.PlayerActions.Sprint.IsPressed())
            {
                _StateMachine.ChangeState(_StateMachine.SprintState);
                return;
            }
            _StateMachine.ChangeState(_StateMachine.LocomotionState);
        }
        #endregion

        #region InputMethods
        protected override void OnJumpPerformed(InputAction.CallbackContext ctx)
        {
            if (_StateMachine.AdditionalJumps <= 0)
            {
                _StateMachine.ChangeState(_StateMachine.GlideState);
                return;
            }

            _StateMachine.AdditionalJumps--;

            ResetVelocityY();

            _StateMachine.ChangeState(_StateMachine.JumpState);
        }
        #endregion
    }
}
