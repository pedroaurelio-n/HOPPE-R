using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinController
{
    public class PlayerSprintingState : PlayerMovingState
    {
        private PlayerSprintData _sprintData;

        private float _startTime;

        private bool _keepSprinting;
        private bool _shouldResetSprintingState;

        public PlayerSprintingState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
            _sprintData = MovementData.SprintData;
        }

        #region IStateMethods
        public override void Enter()
        {
            StateMachine.ReusableData.MovementSpeedModifier = _sprintData.SpeedModifier;

            base.Enter();

            StartAnimation(StateMachine.Player.AnimationData.SprintParameterHash);

            StateMachine.ReusableData.CurrentJumpForce = AirborneData.JumpData.StrongForce;

            _shouldResetSprintingState = true;

            _startTime = Time.time;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(StateMachine.Player.AnimationData.SprintParameterHash);

            if (_shouldResetSprintingState)
            {
                _keepSprinting = false;
                StateMachine.ReusableData.ShouldSprint = false;
            }
        }

        public override void Update()
        {
            base.Update();

            if (_keepSprinting)
                return;

            if (Time.time < _startTime + _sprintData.SprintToRunTime)
                return;

            StopSprinting();
        }
        #endregion

        #region MainMethods
        private void StopSprinting()
        {
            if (StateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                StateMachine.ChangeState(StateMachine.IdlingState);
                return;
            }

            StateMachine.ChangeState(StateMachine.RunningState);
        }
        #endregion

        #region ReusableMethods
        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();

            StateMachine.Player.Input.PlayerActions.Sprint.performed += OnSprintPerformed;
        }

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();

            StateMachine.Player.Input.PlayerActions.Sprint.performed -= OnSprintPerformed;
        }

        protected override void OnFall()
        {
            _shouldResetSprintingState = false;
            
            base.OnFall();
        }
        #endregion

        #region InputMethods
        protected override void OnMovementCanceled(InputAction.CallbackContext ctx)
        {
            StateMachine.ChangeState(StateMachine.HardStoppingState);

            base.OnMovementCanceled(ctx);
        }

        protected override void OnJumpStarted(InputAction.CallbackContext ctx)
        {
            _shouldResetSprintingState = false;
            
            base.OnJumpStarted(ctx);
        }

        private void OnSprintPerformed(InputAction.CallbackContext ctx)
        {
            _keepSprinting = true;
            StateMachine.ReusableData.ShouldSprint = true;
        }
        #endregion
    }
}
