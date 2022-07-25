using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace GenshinController
{
    public class PlayerRunningState : PlayerMovingState
    {
        private PlayerSprintData _sprintData;

        private float _startTime;

        public PlayerRunningState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
            _sprintData = MovementData.SprintData;
        }

        #region IStateMethods
        public override void Enter()
        {
            StateMachine.ReusableData.MovementSpeedModifier = MovementData.RunData.SpeedModifier;

            base.Enter();

            StartAnimation(StateMachine.Player.AnimationData.RunParameterHash);

            StateMachine.ReusableData.CurrentJumpForce = AirborneData.JumpData.MediumForce;

            _startTime = Time.time;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(StateMachine.Player.AnimationData.RunParameterHash);
        }

        public override void Update()
        {
            base.Update();

            if (!StateMachine.ReusableData.ShouldWalk)
                return;

            if (Time.time < _startTime + _sprintData.RunToWalkTime)
                return;

            StopRunning();
        }
        #endregion

        #region MainMethods
        private void StopRunning()
        {
            if (StateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                StateMachine.ChangeState(StateMachine.IdlingState);
                return;
            }

            StateMachine.ChangeState(StateMachine.WalkingState);
        }
        #endregion

        #region InputMethods
        protected override void OnMovementCanceled(InputAction.CallbackContext ctx)
        {
            StateMachine.ChangeState(StateMachine.MediumStoppingState);

            base.OnMovementCanceled(ctx);
        }

        protected override void OnWalkToggleStarted(InputAction.CallbackContext ctx)
        {
            base.OnWalkToggleStarted(ctx);

            StateMachine.ChangeState(StateMachine.WalkingState);
        }
        #endregion
    }
}
