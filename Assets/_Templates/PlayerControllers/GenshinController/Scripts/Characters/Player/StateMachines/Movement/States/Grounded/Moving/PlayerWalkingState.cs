using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinController
{
    public class PlayerWalkingState : PlayerMovingState
    {
        private PlayerWalkData _walkData;

        public PlayerWalkingState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
            _walkData = MovementData.WalkData;
        }

        #region IStateMethods
        public override void Enter()
        {
            StateMachine.ReusableData.MovementSpeedModifier = MovementData.WalkData.SpeedModifier;

            StateMachine.ReusableData.BackwardsCameraRecenteringData = _walkData.BackwardsCameraRecenteringData;

            base.Enter();

            StartAnimation(StateMachine.Player.AnimationData.WalkParameterHash);

            StateMachine.ReusableData.CurrentJumpForce = AirborneData.JumpData.WeakForce;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(StateMachine.Player.AnimationData.WalkParameterHash);

            SetBaseCameraRecenteringData();
        }
        #endregion

        #region InputMethods
        protected override void OnMovementCanceled(InputAction.CallbackContext ctx)
        {
            StateMachine.ChangeState(StateMachine.LightStoppingState);

            base.OnMovementCanceled(ctx);
        }

        protected override void OnWalkToggleStarted(InputAction.CallbackContext ctx)
        {
            base.OnWalkToggleStarted(ctx);

            StateMachine.ChangeState(StateMachine.RunningState);
        }
        #endregion
    }
}