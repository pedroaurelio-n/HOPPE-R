using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinController
{
    public class PlayerDashingState : PlayerGroundedState
    {
        private PlayerDashData _dashData;

        private float _startTime;
        private int _consecutiveDashesUsed;

        private bool _shouldKeepRotating;

        public PlayerDashingState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
            _dashData = MovementData.DashData;
        }

        #region IStateMethods
        public override void Enter()
        {
            StateMachine.ReusableData.MovementSpeedModifier = _dashData.SpeedModifier;

            base.Enter();

            StartAnimation(StateMachine.Player.AnimationData.DashParameterHash);

            StateMachine.ReusableData.CurrentJumpForce = AirborneData.JumpData.StrongForce;

            StateMachine.ReusableData.RotationData = _dashData.RotationData;

            Dash();

            _shouldKeepRotating = StateMachine.ReusableData.MovementInput != Vector2.zero;

            UpdateConsecutiveDashes();

            _startTime = Time.time;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(StateMachine.Player.AnimationData.DashParameterHash);

            SetBaseRotationData();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (!_shouldKeepRotating)
                return;

            RotateTowardsTargetRotation();
        }

        public override void OnAnimationTransitionEvent()
        {
            if (StateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                StateMachine.ChangeState(StateMachine.HardStoppingState);
                return;
            }

            StateMachine.ChangeState(StateMachine.SprintingState);
        }
        #endregion

        #region MainMethods
        private void Dash()
        {
            var dashDirection = StateMachine.Player.transform.forward;

            dashDirection.y = 0f;

            UpdateTargetRotation(dashDirection, false);

            if (StateMachine.ReusableData.MovementInput != Vector2.zero)
            {
                UpdateTargetRotation(GetMovementDirection());

                dashDirection = GetTargetRotationDirection(StateMachine.ReusableData.CurrentTargetRotation.y);
            }

            StateMachine.Player.Rigidbody.velocity = dashDirection * GetMovementSpeed(false);
        }

        private void UpdateConsecutiveDashes()
        {
            if (!IsConsecutive())
                _consecutiveDashesUsed = 0;

            ++_consecutiveDashesUsed;

            if (_consecutiveDashesUsed == _dashData.ConsecutiveDashesLimitAmount)
            {
                _consecutiveDashesUsed = 0;

                StateMachine.Player.Input.DisableActionFor(StateMachine.Player.Input.PlayerActions.Dash, _dashData.DashLimitReachedCooldown);
            }
        }

        private bool IsConsecutive()
        {
            return Time.time < _startTime + _dashData.TimeToBeConsideredConsecutive;
        }
        #endregion

        #region ReusableMethods
        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();

            StateMachine.Player.Input.PlayerActions.Move.performed += OnMovementPerformed;
        }

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();

            StateMachine.Player.Input.PlayerActions.Move.performed -= OnMovementPerformed;
        }
        #endregion

        #region InputMethods
        private void OnMovementPerformed(InputAction.CallbackContext ctx)
        {
            _shouldKeepRotating = true;
        }
        
        protected override void OnDashStarted(InputAction.CallbackContext ctx)
        {
        }
        #endregion
    }
}
