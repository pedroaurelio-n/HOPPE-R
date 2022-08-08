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

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            _Player.SetAnimationFloat(_Player.AnimationData.SpeedParamHash, _StateMachine.MoveAmount);

            if (_StateMachine.MovementInput == Vector2.zero)
                return;
            
            if (_Player.Input.PlayerActions.Sprint.IsPressed() && _StateMachine.SlopeSpeedModifier >= _Player.Data.SprintMaxSlopeValue)
            {
                _StateMachine.ChangeState(_StateMachine.SprintState);
                return;
            }
        }
        #endregion
    }
}
