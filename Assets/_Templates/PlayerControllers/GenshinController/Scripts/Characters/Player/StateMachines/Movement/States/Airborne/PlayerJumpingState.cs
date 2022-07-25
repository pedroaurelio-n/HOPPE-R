using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinController
{
    public class PlayerJumpingState : PlayerAirborneState
    {
        private PlayerJumpData _jumpData;

        private bool _shouldKeepRotating;
        private bool _canStartFalling;

        public PlayerJumpingState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
            _jumpData = AirborneData.JumpData;
        }

        #region IStateMethods
        public override void Enter()
        {
            base.Enter();

            StateMachine.ReusableData.MovementSpeedModifier = 0f;

            StateMachine.ReusableData.MovementDecelerationForce = _jumpData.DecelerationForce;

            StateMachine.ReusableData.RotationData = _jumpData.RotationData;

            _shouldKeepRotating = StateMachine.ReusableData.MovementInput != Vector2.zero;

            Jump();
        }

        public override void Exit()
        {
            base.Exit();

            SetBaseRotationData();

            _canStartFalling = false;
        }

        public override void Update()
        {
            base.Update();

            if (!_canStartFalling && IsMovingUp(0f))
            {
                _canStartFalling = true;
            }

            if (!_canStartFalling || GetVerticalVelocity().y > 0)
                return;

            StateMachine.ChangeState(StateMachine.FallingState);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (_shouldKeepRotating)
                RotateTowardsTargetRotation();

            if (IsMovingUp())
            {
                DecelerateVertically();
            }
        }
        #endregion

        #region MainMethods
        private void Jump()
        {
            var jumpForce = StateMachine.ReusableData.CurrentJumpForce;

            var jumpDirection = StateMachine.Player.transform.forward;

            if (_shouldKeepRotating)
            {
                UpdateTargetRotation(GetMovementDirection());
                
                jumpDirection = GetTargetRotationDirection(StateMachine.ReusableData.CurrentTargetRotation.y);
            }

            jumpForce.x *= jumpDirection.x;
            jumpForce.z *= jumpDirection.z;

            var capsuleColliderCenterInWorldSpace = StateMachine.Player.ColliderUtility.CapsuleColliderData.Collider.bounds.center;
            Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

            if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, _jumpData.JumpToGroundRayDistance,
                                StateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
            {
                var groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

                if (IsMovingUp())
                {
                    var forceModifier = _jumpData.JumpForceModifierOnSlopeUpwards.Evaluate(groundAngle);

                    jumpForce.x *= forceModifier;
                    jumpForce.z *= forceModifier;
                }

                if (IsMovingDown())
                {
                    var forceModifier = _jumpData.JumpForceModifierOnSlopeDownwards.Evaluate(groundAngle);

                    jumpForce.y *= forceModifier;
                }
            }

            ResetVelocity();

            StateMachine.Player.Rigidbody.AddForce(jumpForce, ForceMode.VelocityChange);
        }
        #endregion

        #region ReusableMethods
        protected override void ResetSprintState()
        {
        }
        #endregion

        #region  InputMethods
        protected override void OnMovementCanceled(InputAction.CallbackContext ctx)
        {
        }
        #endregion
    }
}
